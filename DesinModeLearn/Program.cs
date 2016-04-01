using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesinModeLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 简单工厂客户端实现
            //MyOperater obj =SimpleFactory.CrateOperator("+");
            //obj.a = 5;
            //obj.b = 6;
            //Console.WriteLine(obj.Operater(obj.a, obj.b)); 
            #endregion

            #region 工厂模式客户端实现
            IFactory fac = new AddFactory();
            MyOperater oper = fac.CreateOperator();
            oper.a = 5;
            oper.b = 6;
            Console.WriteLine(oper.Operater(oper.a, oper.b)); 
            #endregion
            Console.ReadKey();
        }
       
    }
}
