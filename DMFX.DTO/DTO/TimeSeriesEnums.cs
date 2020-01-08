using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.QuotesInterfaces
{
    public enum ETimeFrame
    {
        Undefined = 0,
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Annually,
        SemiAnnual
    }

    public enum EUnit
    {
        Undefined = 1,

        USD = 2,
        EUR = 3,
        Percent = 4,

        Value = 5
    }

    public enum ETimeSeriesType
    {
        Price = 1,
        Indicator = 2
    }
}
