using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICETest
{
    public class PrinterI : PrinterDisp_
    {
        public PrinterI()
        { }

        public override void printString(string s, Ice.Current current__)
        {
            System.Console.WriteLine(s);
        }
    }
}
