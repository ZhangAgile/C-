using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICETest
{
    class Server
    {
        public Server()
        { }
        static void Main(string[] args)
        {
            int status = 0;
            Ice.Communicator ic = null;
            try
            {
                ic = Ice.Util.initialize(ref args);//初始化主句柄
                Ice.ObjectAdapter adapter= ic.createObjectAdapterWithEndpoints("SimplePrinter","default -p 10000");
                Ice.Object obj = new PrinterI();
                adapter.add(obj, Ice.Util.stringToIdentity("SimplePrinter"));
                adapter.activate();
                ic.waitForShutdown();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                status = 1;
            }
            if(ic!=null)
            {
                //clean up
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
        }
    }
}
