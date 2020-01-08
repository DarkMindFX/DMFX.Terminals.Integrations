using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    public class TickerInfo
    {
        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public DateTime? LastUpdate
        {
            get;
            set;
        }

        public DateTime PeriodStart
        {
            get;
            set;
        }

        public DateTime PeriodEnd
        {
            get;
            set;
        }
    }
}
