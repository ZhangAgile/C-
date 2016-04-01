using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICEClient
{
    class Client
    {
        static void Main(string[] args)
        {
            int status = 0;
            Ice.Communicator ic = Ice.Util.initialize(ref args);
            try
            {
                Ice.ObjectPrx obj = ic.stringToProxy("SimplePrinter:default -p 10000");
                ICETest.PrinterPrx prx = ICETest.PrinterPrxHelper.checkedCast(obj);
                if (prx == null)
                    throw new ApplicationException("Invalid Proxy!");
                prx.printString("Hello World!");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                status = 1;
            }
            if(ic!=null)
            {
                try
                {
                    ic.destroy();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    status = 1;
                }
            }
            Environment.Exit(status);
        }
    }
}
