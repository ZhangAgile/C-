using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerializeTest
{
    class Program
    {
        [Serializable]
        internal class Pearson
        {
            public string Name { get; set; }
            [NonSerialized]
            private bool _Sex=true;
            public bool Sex
            {
                get { return _Sex; } 
                set {_Sex=value;}
            }
            public int Age { get; set; }
        }
        public static EventWaitHandle autoEvent = new EventWaitHandle(true, EventResetMode.AutoReset, "MyEvent");
        static void Main(string[] args)
        {
            #region 序列化
            //Pearson p1 = new Pearson() { Name = "张三", Age = 20, Sex = true };
            //Pearson p2 = new Pearson() { Name = "李四", Age = 21, Sex = false };
            //MemoryStream stream = new MemoryStream();
            //BinaryFormatter formatter = new BinaryFormatter();
            //formatter.Serialize(stream, p1);
            //p1 = null;
            //stream.Position = 0;
            //p1 = (Pearson)formatter.Deserialize(stream);
            //Console.WriteLine("Name:{0},Sex:{1},Age:{2}", p1.Name, p1.Sex, p1.Age); 
            #endregion

            #region 线程同步测试 Interlocked
            //int x = 0;
            //const int number = 5000000;//500万
            //Stopwatch watch = new Stopwatch();
            //watch.Start();
            //for (int i = 0; i < number; i++)
            //{
            //    x++;
            //}
            //Console.WriteLine("使用时间:{0}", watch.ElapsedMilliseconds);
            //watch.Restart();
            //for (int i = 0; i < number; i++)
            //{
            //    Interlocked.Increment(ref x);
            //}
            //Console.WriteLine("使用时间:{0}", watch.ElapsedMilliseconds);
            #endregion

            #region 句柄测试EventWaitHandle
            Console.WriteLine("Main Thread Start run at: " + DateTime.Now.ToLongTimeString());
            Thread t = new Thread(TestMethod);

            // 为了有时间启动另外一个线程
            Thread.Sleep(2000);
            t.Start();
            Console.Read();
            #endregion

            Console.ReadKey();

        }
        public static void TestMethod()
        {
            // 进程一：显示的时间间隔为2秒
            // 进程二中显示的时间间隔为3秒
            // 因为进程二中AutoResetEvent的初始状态为非终止的
            // 因为在进程一中通过WaitOne方法的调用已经把AutoResetEvent的初始状态返回为非终止状态了
            autoEvent.WaitOne(1000);
            Console.WriteLine("Method start at : " + DateTime.Now.ToLongTimeString());
        }
    }
}
