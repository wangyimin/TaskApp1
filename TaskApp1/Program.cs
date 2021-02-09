using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime start = DateTime.Now;
            DateTime endP = start.AddSeconds(5);
            DateTime endC = start.AddSeconds(8);

            using (PC<string> _pc = new PC<string>())
            {
                Task _p = Task.Factory.StartNew(
                    () =>
                    {
                        while (DateTime.Now < endP)
                        {
                            Task t = _pc.Produce(GetDateTime(), el => { Trace.WriteLine($"Produce ({el}) is coming"); });
                            Thread.Sleep(new Random().Next(1000));
                        }
                    });

                Task _c = Task.Factory.StartNew(
                    () =>
                    {
                        while (DateTime.Now < endC || _pc.Length > 0)
                        {
                            Thread.Sleep(800);
                            Task t = _pc.Consume(el => { Trace.WriteLine($"Consume ({el}) is completed at {GetDateTime()}"); });
                        }
                    });

                Task.WaitAll(_p, _c);
            }
        }

        static string GetDateTime() => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
    }
}
