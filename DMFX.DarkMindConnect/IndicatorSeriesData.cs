using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.DarkMindConnect
{
    class IndicatorSeriesData
    {
        public string Name
        {
            get;
            set;
        }

        public Dictionary<DateTime, Decimal> Values
        {
            get;
            set;
        }

        public IndicatorSeriesData()
        {
            Values = new Dictionary<DateTime, Decimal>();
        }
    }
   
}
