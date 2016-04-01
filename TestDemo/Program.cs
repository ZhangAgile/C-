using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestDemo
{
    class ThreadClass
    {
        public static ManualResetEvent mre = new ManualResetEvent(false);
        public static void trmain()
        {
            Thread tr = Thread.CurrentThread;
            Console.WriteLine("thread:waiting for an event");
            mre.WaitOne();
            Console.WriteLine("thread:Got an event");
            for (int x = 0; x < 10; x++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(tr.Name+":"+x);
            }
        }
        static void Main(string[] args)
        {
            Thread thrd1 = new Thread(new ThreadStart(trmain));
            thrd1.Name = "Thread1";
            thrd1.Start();
            for (int x = 0; x < 10; x++)
            {
                Thread.Sleep(900);
                Console.WriteLine("Main    :" + x);
                if (5 == x)
                {
                    mre.Reset();
                }
                if (8 == x)
                {
                    mre.Set();
                }
            }
            while (thrd1.IsAlive)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Main:Waiting for thread to stop...");
            }

            Console.ReadKey();
        }
    }
}
