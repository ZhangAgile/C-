using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesinModeLearn
{
    class SimpleFactory
    {
        public MyOperater obj;
        public static MyOperater CrateOperator(string x)
        {
            MyOperater obj = null;
            if (string.IsNullOrWhiteSpace(x))
            {
                return null;
            }
            switch (x)
            {
                case "+":
                    {
                        obj = new AddOpretor();
                    };
                    break;
                case "-":
                    {
                        obj = new AddOpretor();
                    };
                    break;
                case "*":
                    {
                        obj = new AddOpretor();
                    };
                    break;
                case "/":
                    {
                        obj = new AddOpretor();
                    };
                    break;
                default: break;
            }
            return obj;
        }
    }
}
