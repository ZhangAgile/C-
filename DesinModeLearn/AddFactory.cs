using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesinModeLearn
{
    class AddFactory:IFactory
    {
        public MyOperater CreateOperator()
        {
            return new AddOpretor();
        }
    }
}
