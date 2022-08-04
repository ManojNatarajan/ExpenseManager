using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenEagles.Logger
{
    public class Serilog_Logger : Logger_Base
    {
        public Serilog_Logger(string categoryName) : base(categoryName)
        {
            //Default implementation in base class!
        }
    }
}
