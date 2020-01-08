using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    public abstract class FilingRecordBase
    {
        public string Code
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

        public string FactId
        {
            get;
            set;
        }
 
    }
    public class FilingNumRecord : FilingRecordBase
    {
         public decimal Value
        {
            get;
            set;
        }

        public string UnitName
        {
            get;
            set;
        }
    }

    public class FilingStrRecord : FilingRecordBase
    {
 
        public string Value
        {
            get;
            set;
        }

    }

    public class FilingDttmRecord : FilingRecordBase
    {

        public DateTime Value
        {
            get;
            set;
        }

    }
}
