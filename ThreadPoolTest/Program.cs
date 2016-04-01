using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolTest
{
    class Program
    {
        delegate string MyDelegate(string name);
        static void Main(string[] args)
        {
            //int workerThreads;
            //int completionPortThreads;
            //ThreadPool.GetMaxThreads(out workerThreads,out completionPortThreads);
            //Console.WriteLine(string.Format("workerThreads:{0},completionPortThreads:{1}", workerThreads, completionPortThreads));
            //Console.ReadKey();

            //string[] strArgs = System.Environment.GetCommandLineArgs();
            //string[] tmpStrArr = strArgs[0].Split('\\');

            //Console.WriteLine(tmpStrArr.ToString());

            //System.Collections.Hashtable hasTable = new System.Collections.Hashtable();
            //hasTable.Add(1,tmpStrArr[0]);
            //hasTable.Add(2, tmpStrArr[2]);
            //Console.WriteLine(hasTable[1].ToString()+"\\"+hasTable[2].ToString());

            //Console.WriteLine(string.Format("ThreadID:{0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            //ThreadPool.QueueUserWorkItem(new WaitCallback(ShowMessage),"Hello");
            //Console.WriteLine(string.Format("ThreadID:{0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
            //System.Threading.Thread.CurrentThread.Join();

            //委托类启动线程
            ThreadMessage("Main Thread");
            MyDelegate mydel = new MyDelegate(Hello);
            IAsyncResult result = mydel.BeginInvoke("zhangsan", new AsyncCallback(Completed), new Pearson() { Name="张三丰",Age=26});
            
            //while (!result.IsCompleted)
            //{
            //    Console.WriteLine(string.Format("HaHa:{0}", Thread.CurrentThread.ManagedThreadId));
            //}

            //while (!result.AsyncWaitHandle.WaitOne(200))
            //{
            //    Console.WriteLine("Sing a song,OK?");
            //    Console.WriteLine("Watch a Movie,OK?");
            //}

            //string data = mydel.EndInvoke(result);
            //Console.WriteLine(data);
            int a = 0;
            do{
                Console.WriteLine("Sing a song,OK?");
                Console.WriteLine("Watch a Movie,OK?");
                a++;
            }while(a<6);
            Console.ReadKey();

        }
        static string Hello(string name)
        {
            ThreadMessage("Async Thread--------");
            Thread.Sleep(1000);
            return "Hello" + name;
        }
        private static void ShowMessage(object state)
        {
            if (!string.IsNullOrWhiteSpace(state.ToString()))
            {
                Console.WriteLine(state.ToString());
            }
            Console.WriteLine(string.Format("ThreadID:{0}", System.Threading.Thread.CurrentThread.ManagedThreadId));
        }
        static void ThreadMessage(string data)
        {
            string messgae = string.Format("{0} \n ThreadID is {1}", data, Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine(messgae);
        }
        static void Completed(IAsyncResult result)
        {
            ThreadMessage("Async Thread--------");
            System.Runtime.Remoting.Messaging.AsyncResult _result = (System.Runtime.Remoting.Messaging.AsyncResult)result;
            MyDelegate mydel = (MyDelegate)_result.AsyncDelegate;
            string data = mydel.EndInvoke(result);
            Pearson p = (Pearson)_result.AsyncState;
            string message = p.Name + "'s age is " + p.Age;
            Console.WriteLine(data);
            Console.WriteLine(message);
        }
    }
}
