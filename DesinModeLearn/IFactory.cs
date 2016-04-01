using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesinModeLearn
{
    public interface IFactory
    {
         MyOperater CreateOperator();
    }
}
