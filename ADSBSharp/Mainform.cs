using MathNet.Filtering;
using MathNet.Filtering.FIR;
using NAudio.Wave;
using SDRSharp.Radio;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ADSBSharp
{
    public unsafe partial class MainForm : Form
    {        
        private IFrameSink _frameSink = new SimpleTcpServer();
        private readonly AdsbBitDecoder _decoder = new AdsbBitDecoder();     
        private bool _isDecoding;        
        private bool _initialized;
        private int _frameCount;
        private float _avgFps;
        private Pluto _pluto;

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string hex2binary(string hexvalue)
        {
            // Convert.ToUInt32 this is an unsigned int
            // so no negative numbers but it gives you one more bit
            // it much of a muchness 
            // Uint MAX is 4,294,967,295 and MIN is 0
            // this padds to 4 bits so 0x5 = "0101"
            return String.Join(String.Empty, hexvalue.Select(c => Convert.ToString(Convert.ToUInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public MainForm()
        {
            InitializeComponent();

            Text = "ADSB# v" + Assembly.GetExecutingAssembly().GetName().Version;

            _decoder.FrameReceived += delegate(byte[] frame, int length)
                                          {
                                              Interlocked.Increment(ref _frameCount);
                                              _frameSink?.FrameReady(frame, length);
                                          };

            portNumericUpDown_ValueChanged(null, null);
            confidenceNumericUpDown_ValueChanged(null, null);
            timeoutNumericUpDown_ValueChanged(null, null);

            _initialized = true;
            try
            {
                //https://drwxr.org/2016/09/automatic-dependent-surveillance-broadcast-ads-b-part-i/
                //https://wiki.analog.com/resources/eval/user-guides/picozed_sdr/tutorials/adsb
                /*
Transmit Frequency: 1090 MHz
Modulation: Pulse Position Modulation (PPM)
Data Rate: 1 Mbit/s
Message Length: 56 μsec or 112 μsec
24-bit CRC checksum
                 */
                string test = "8D4840D6202CC371C32CE0576098";
                //test = "8D7C5E42991068264004111A1869";
                //test = "8D7C5E42F80000060049A089E732";
                //test = "8D7C5E42587006D70AEFF68D8151";
                //test = "8D7C5E425870026A6FBD9EECEE64";


                var by = hex2binary(test);
                by = by.Select(a=>a=='0' ? "01": "10").Aggregate((a,b) => { return a + b; });

                var pre = hex2binary("A140");

                _decoder.ConfidenceLevel = 1;
                _decoder.ProcessSample(5);
                _decoder.ProcessSample(5);

                foreach (var bit in pre)
                {
                    _decoder.ProcessSample(((bit - '0') * 200) + 1);
                }

                foreach (var bit in by) 
                {
                    _decoder.ProcessSample(((bit - '0')* 200) + 1);
                }

                _decoder.ConfidenceLevel = 3;

                
                //var file = @"C:\Users\mich1\Downloads\sdrpp_windows_x64\recordings\baseband_1090000000Hz_15-08-49_30-08-2024.wav";
                //var file = @"C:\Users\mich1\Downloads\sdrpp_windows_x64\recordings\baseband_1090000000Hz_14-16-32_21-11-2024.wav";
                var file = @"C:\SDRSharp\bin\SDRSharp_20241121_221419Z_1090199996Hz_IQ.wav";
                WaveFileReader reader = new WaveFileReader(file);
                reader.CurrentTime = new TimeSpan(0, 0, 0);
                var st = File.OpenWrite("test.wav");
                st.SetLength(0);
                var writer = new WaveFileWriter(st, new WaveFormat(2000000, 16, 2));

                    _frameSink.Start("", (int)portNumericUpDown.Value);
                _decoder.ConfidenceLevel = 3;

                OnlineFilter bandpass = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, 2000000, 1000);
                OnlineFilter bandpass2 = OnlineFirFilter.CreateHighpass(ImpulseResponse.Finite, 2000000, 1000);

                int buffersize = 1000000;
                double[] I = new double[buffersize];
                double[] Q = new double[buffersize];
                int a = 0;
                float mag2 = 0;
                DateTime start = DateTime.Now;

                _decoder.FrameReceived += delegate (byte[] frame, int length)
                {
                    Console.WriteLine(reader.CurrentTime);
                    writer.Flush();
                };

                while (reader.CurrentTime < reader.TotalTime) {
                    var sample = reader.ReadNextSampleFrame();

                    writer.WriteSample((float)sample[0]);

                    //sample[0] = (float)bandpass.ProcessSample(sample[0]);
                    //sample[1] = (float)bandpass2.ProcessSample(sample[1]);

                    //writer.WriteSample((float)sample[0]);

                    mag2 = ((sample[0] * sample[0] + sample[1] * sample[1]));

                    mag2 = mag2 * mag2;

                    writer.WriteSample((float)(mag2));

                    _decoder.ProcessSample((int)(mag2 * int.MaxValue));
                }

                return;


                /* _rtlDevice.Open();

                 var devices = DeviceDisplay.GetActiveDevices();
                 deviceComboBox.Items.Clear();
                 deviceComboBox.Items.AddRange(devices);

                 //_initialized = true;
                 deviceComboBox.SelectedIndex = 0;
                 deviceComboBox_SelectedIndexChanged(null, null);  */
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }                        
        }

        #region GUI Controls

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!_isDecoding)
            {
                StartDecoding();
            }
            else
            {
                StopDecoding();
            }

            startBtn.Text = _isDecoding ? "Stop" : "Start";
            //deviceComboBox.Enabled = !_rtlDevice.Device.IsStreaming;
            //portNumericUpDown.Enabled = !_rtlDevice.Device.IsStreaming;
            //shareCb.Enabled = !_rtlDevice.Device.IsStreaming;
            //hostnameTb.Enabled = !_rtlDevice.Device.IsStreaming && shareCb.Checked;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDecoding)
            {
                StopDecoding();
            }
        }
       
  



        #endregion

        #region Private Methods

        private void ConfigureGUI()
        {
            startBtn.Enabled = _initialized;
            if (!_initialized)
            {
                return;
            }
         
        }


        private void StartDecoding()
        {                        
            try
            {
     
                {
                    _frameSink = new SimpleTcpServer();
                }
                
                _frameSink.Start("",(int) portNumericUpDown.Value);

                //waterfall.BandType = SDRSharp.PanView.BandType.Center;
                waterfall.CenterFrequency = 1090000000;
                waterfall.SpectrumWidth = 2000000;
                //waterfall.Frequency = 1090000000;
            }
            catch (Exception e)
            {
                StopDecoding();
                MessageBox.Show("Unable to start networking\n" + e.Message);
                return;
            }

            try
            {
                try
                {
                    _pluto = new Pluto();
                    _pluto.Start(rtl_SamplesAvailable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    MessageBox.Show("Unable to start Pluto device\n" + ex.Message);
                }


              //  _rtlDevice.Start(rtl_SamplesAvailable);
            }
            catch (Exception e)
            {
                StopDecoding();
                MessageBox.Show("Unable to start RTL device\n" + e.Message);
                return;
            }

            _isDecoding = true;
        }

        private void StopDecoding()
        {
            //_rtlDevice.Stop();
            _frameSink.Stop();
            _frameSink = null;
            _isDecoding = false;
            _avgFps = 0f;
            _frameCount = 0;
            Pluto.run = false;
        }

        #endregion

        #region Samples Callback

        private void rtl_SamplesAvailable(object sender, Complex* buf, int length)
        {
            //Console.Write(".");
            float[] power = new float[1024];

            Span<Complex> comp = new Span<Complex>(buf, 1024);

            var iqarray = comp.ToArray().Select(a => new SDRSharp.Radio.Complex(a.Real*100, a.Imag*100)).ToArray();

            fixed (SDRSharp.Radio.Complex* iq = iqarray)
            {
                Fourier.ForwardTransform(iq, 1024);

                Fourier.SpectrumPower(iqarray, power, 1024, 0);
            }

            var max = power.Max();
            var min = power.Min();
            var avg = power.Average();
            waterfall.DisplayRange = 60;//(int)(max - min);
            waterfall.DisplayOffset = 130;//(int)max + 9;
            waterfall.Attack = 1;
            waterfall.Decay = 2;
            //waterfall.UseSmoothing = true;

            if (!this.IsDisposed && !this.Disposing && Pluto.run)
                BeginInvoke((Action)delegate
                {
                    fixed (float* ptr_f = power.Reverse().ToArray())
                    {
                        if (!waterfall.IsDisposed)
                            waterfall.Render(ptr_f, 1024);
                    }
                });
            

            for (var i = 0; i < length; i++)
            {
                var real = buf[i].Real;
                var imag = buf[i].Imag;

                //Console.WriteLine($"{real}  {imag}");

                var mag = real * real + imag * imag;

                _decoder.ProcessSample(mag);
            }
        }

        #endregion

        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            var fps = (_frameCount) * 1000.0f / fpsTimer.Interval;
            _frameCount = 0;

            _avgFps = 0.9f * _avgFps + 0.1f * fps;

            fpsLabel.Text = ((int) _avgFps).ToString();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void portNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            notifyIcon.Text = Text + " on port " + portNumericUpDown.Value;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
            }
            else
            {
                ShowInTaskbar = true;
            }
        }

        private void confidenceNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _decoder.ConfidenceLevel = (int) confidenceNumericUpDown.Value;
        }

        private void timeoutNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            _decoder.Timeout = (int) timeoutNumericUpDown.Value;
        }

    }

    public class DeviceDisplay
    {
        public uint Index { get; private set; }
        public string Name { get; set; }

        public static DeviceDisplay[] GetActiveDevices()
        {
            var count = NativeMethods.rtlsdr_get_device_count();
            var result = new DeviceDisplay[count];

            for (var i = 0u; i < count; i++)
            {
                var name = NativeMethods.rtlsdr_get_device_name(i);
                result[i] = new DeviceDisplay { Index = i, Name = name };
            }

            return result;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
