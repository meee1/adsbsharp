using MathNet.Filtering;
using MathNet.Filtering.FIR;
using NAudio.Wave;
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
                //test = "8D86D5E058135037C0A9112B72B7";
                test = "8D7C62ABBFAD4000000000BB2FBC";

                test = "A140" + test;

                var by = hex2binary(test);

                foreach (var bit in by) { 
                    _decoder.ProcessSample((bit - '0')* 200);
                }

                foreach (var bit in by)
                {
                    _decoder.ProcessSample((bit - '0') * 200);
                }

                var t=0;
                /*
                WaveFileReader reader = new WaveFileReader(@"C:\SDRSharp\SDRSharp_20210816_221955Z_1090000000Hz_IQ.wav");
                var st = File.OpenWrite("test.wav");
                st.SetLength(0);
                var writer = new WaveFileWriter(st, new WaveFormat(2000000, 16, 2));

                    _frameSink.Start(hostnameTb.Text, (int)portNumericUpDown.Value);

                OnlineFilter bandpass = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, 2000000, 900000, 2000000);
                OnlineFilter bandpass2 = OnlineFirFilter.CreateBandpass(ImpulseResponse.Finite, 2000000, 900000, 2000000);

                double[] I = new double[2000000];
                double[] Q = new double[2000000];
                int a = 0;

                while (reader.CurrentTime < reader.TotalTime) {
                    var sample = reader.ReadNextSampleFrame();

                    var real=(int) (sample[0] * 10000);
                    var imag =(int) (sample[1] * 10000);

                    I[a]= sample[0];
                    Q[a]= sample[1];

                    a++;

                    if(a >= 2000000)
                    {
                        a=0;

                        var real2 = bandpass.ProcessSamples(I);
                        var imag2 = bandpass2.ProcessSamples(Q);

                        for(int b=0; b < 2000000; b++) {

                            var mag = real2[b] * real2[b] + imag2[b] * imag2[b];

                            writer.WriteSample((float)real2[b]*3);
                            writer.WriteSample((float)imag2[b]*3);

                            _decoder.ProcessSample((int)(mag*int.MaxValue));
                        }

                        writer.Flush();
                    }
               
                }
            */



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
        }

        #endregion

        #region Samples Callback

        private void rtl_SamplesAvailable(object sender, Complex* buf, int length)
        {
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
