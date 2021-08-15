using iio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ADSBSharp
{
    public class Pluto
    {
        public void Start(SamplesReadyDelegate callback)
        {
            Context ctx = new Context("ip:192.168.2.1");
            if (ctx == null)
            {
                Console.WriteLine("Unable to create IIO context");
                return;
            }
            Console.WriteLine("IIO context created: " + ctx.name);
            Console.WriteLine("IIO context description: " + ctx.description);
            Console.WriteLine("IIO context has " + ctx.devices.Count + " devices:");
            foreach (Device dev in ctx.devices)
            {
                Console.WriteLine("\t" + dev.id + ": " + dev.name);
                if (dev is Trigger)
                {
                    Console.WriteLine("Found trigger! Rate=" + ((Trigger)dev).get_rate());
                }
                Console.WriteLine("\t\t" + dev.channels.Count + " channels found:");
                foreach (Channel chn in dev.channels)
                {
                    string type = "input";
                    if (chn.output)
                    {
                        type = "output";
                    }
                    Console.WriteLine("\t\t\t" + chn.id + ": " + chn.name + " (" + type + ")");
                    if (chn.attrs.Count == 0)
                    {
                        continue;
                    }
                    Console.WriteLine("\t\t\t" + chn.attrs.Count + " channel-specific attributes found:");
                    foreach (Attr attr in chn.attrs)
                    {
                        Console.WriteLine("\t\t\t\t" + dev.name + " " + attr.name);
                        //if (attr.name.CompareTo("frequency") == 0)
                        try
                        {
                            Console.WriteLine("\t\t\t\t" + "Attribute content: " + attr.read());
                        } catch { }
                    }

                }
                /* If we find cf-ad9361-lpc, try to read a few bytes from the first channel */
                if (dev.name.CompareTo("cf-ad9361-lpc") == 0)
                {      

                    /*
    // RX stream config
    rxcfg.bw_hz = MHZ(2);   // 2 MHz rf bandwidth
    rxcfg.fs_hz = MHZ(2.5);   // 2.5 MS/s rx sample rate
    rxcfg.lo_hz = GHZ(2.5); // 2.5 GHz rf frequency
    rxcfg.rfport = "A_BALANCED"; // port A (select for rf freq.)
                     */
  


                    Task.Run(()=>{

                        Thread.Sleep(500);

                        var phy = ctx.get_device("ad9361-phy");

                        var gainmode = phy.get_channel("voltage0").find_attribute("gain_control_mode");
                        gainmode.write( "slow_attack");
                        //manual

                        var port = phy.get_channel("voltage0").find_attribute("rf_port_select");
                        var rfbw = phy.get_channel("voltage0").find_attribute("rf_bandwidth");
                        var samplehz = phy.get_channel("voltage0").find_attribute("sampling_frequency");
                        var freq = phy.get_channel("altvoltage0").find_attribute("frequency");

                        var gain = phy.get_channel("voltage0").find_attribute("hardwaregain");

                        var _rx0_i = dev.get_channel("voltage0");
                        var _rx0_q = dev.get_channel("voltage1");

                        _rx0_i.enable();
                        _rx0_q.enable();

                        IOBuffer buf = new IOBuffer(dev, 2000000 / 20);

                        //gain.write(40);

                        rfbw.write(2000000);
                        
                        samplehz.write((long)2000000);

                        freq.write(1090000000);

                        float scale = 1.0f / 32768.0f;

                        float[] lut = new float[0x10000];
                        for (UInt16 i = 0x0000; i < 0xFFFF; i++)
                        {
                            lut[i] = ((((i & 0xFFFF) + 32768) % 65536) - 32768) * scale;
                        }

                        var sampleCount = buf.samples_count;

                        unsafe
                        {
                            UnsafeBuffer unsafeBuffer = UnsafeBuffer.Create((int)sampleCount, sizeof(Complex));
                        

                        while (true) { 
                            buf.refill();
                           
                            var samplesI = _rx0_i.read(buf);
                            var samplesQ = _rx0_q.read(buf);

                                var ptrIq = (Complex*)unsafeBuffer.Address;

                                    for (int i = 0; i < samplesI.Length/2; i++)
                                    {
                                        int sampleOffset = (i * 2);

                                        UInt16 sampleI = (UInt16)((samplesI[sampleOffset + 1] << 8) + samplesI[sampleOffset]);
                                        UInt16 sampleQ = (UInt16)((samplesQ[sampleOffset + 1] << 8) + samplesQ[sampleOffset]);

                                        ptrIq->Real = ((((sampleI & 0xFFFF) + 32768) % 65536) -32768);
                    ptrIq->Imag = ((((sampleQ & 0xFFFF) +32768) % 65536) -32768);
                    ptrIq++;
                                    }

                                    callback(this, (Complex*)unsafeBuffer, (int)buf.samples_count);

                            }
                        }
                    });

                    //Console.WriteLine("Read " + chn.read(buf).Length + " bytes from hardware");
                    //buf.Dispose();
                }
                if (dev.attrs.Count == 0)
                {
                    continue;
                }
                Console.WriteLine("\t\t" + dev.attrs.Count + " device-specific attributes found:");
                foreach (Attr attr in dev.attrs)
                {
                    Console.WriteLine("\t\t\t" + attr.name);
                }
            }
        }

        public static int Size = 100; 

        public static Complex[][][] ButterworthBandpassFilter(Complex[][][] frequencies, double d0, int n, double W)
        {
            Complex[][][] filtered = frequencies;
            for (int i = 0; i < 3; i++)
            {
                for (int u = 0; u < Size; u++)
                {
                    for (int v = 0; v < Size; v++)
                    {
                        double d = Math.Sqrt((Math.Pow(u - Size / 2, 2) + Math.Pow(v - Size / 2, 2)));
                        filtered[i][u][v] *= 1f - (float)(1f / (1 + Math.Pow(d * W / (d * d - d0 * d0), 2 * n)));
                    }
                }
            }

            return filtered;
        }
    }
}
