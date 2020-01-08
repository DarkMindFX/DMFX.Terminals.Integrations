using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    public class TickerQuotes
    {        

        public TickerQuotes()
        {
            TimePeriod = QuotesInterfaces.ETimeFrame.Daily;
            Quotes = new List<QuoteRecord>();
        }

        public string Code
        {
            get;
            set;
        }

        public QuotesInterfaces.ETimeFrame TimePeriod
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

        public List<QuoteRecord> Quotes
        {
            get;
            set;
        }
    }

    public class QuoteRecord
    {
        public QuoteRecord(DateTime t , IList<decimal> values)
        {
            Time = t;
            Values = new List<decimal>(values);
        }

        public DateTime Time
        {
            get;
            set;
        }

        public List<decimal> Values
        {
            get;
            set;
        }
    }
}
