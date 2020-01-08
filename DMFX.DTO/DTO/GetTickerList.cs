using DMFX.QuotesInterfaces;
using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetTickerList/{CountryCode}/{Type}/{SessionToken}", "GET")]
    [Route("/GetTickerList/{CountryCode}/{Type}/{Agency}/{SessionToken}", "GET")]
    [Route("/GetTickerList", "POST")]
    public class GetTickerList : RequestBase , IReturn<GetTickerListResponse>
    {
        public string CountryCode
        {
            get;
            set;
        }

        public ETimeSeriesType Type
        {
            get;
            set;
        }

        public string Agency
        {
            get;
            set;
        }
    }

    public class TickerListItem
    {
        public string Ticker
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string CountryCode
        {
            get;
            set;
        }        

        public EUnit Unit
        {
            get;
            set;
        }

        public ETimeSeriesType Type
        {
            get;
            set;
        }

    }

    public class GetTickerListResponse : ResponseBase
    {
        public GetTickerListResponse()
        {
            Payload = new ResponsePayload();
        }

        public ResponsePayload Payload
        {
            get;
            set;
        }

        public class ResponsePayload
        {
            public ResponsePayload()
            {
                Tickers = new List<TickerListItem>();
            }
            public string CountryCode
            {
                get;
                set;
            }

            public string Agency
            {
                get;
                set;
            }

            public IList<TickerListItem> Tickers
            {
                get;
                set;
            }
        }
    }
}
