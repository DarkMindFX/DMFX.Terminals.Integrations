using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetFilingRatios/{RegulatorCode}/{CompanyCode}/{FilingName}/{SessionToken}", "GET")]
    [Route("/GetFilingRatios/{RegulatorCode}/{CompanyCode}/{FilingName}/{RatioCodes}/{SessionToken}", "GET")]
    [Route("/GetFilingRatios", "POST")]
    public class GetFilingRatios : RequestBase, IReturn<GetFilingRatiosResponse>
    {
        public GetFilingRatios()
        {
            RatioCodes = new List<string>();
        }

        public string RegulatorCode
        {
            get;
            set;
        }

        public string CompanyCode
        {
            get;
            set;
        }

        public string FilingName
        {
            get;
            set;
        }

        public List<string> RatioCodes
        {
            get;
            set;
        }
    }

    public class RatioRecord
    {
        public string Code
        {
            get;
            set;
        }

        public decimal Value
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

        public string UnitName
        {
            get;
            set;
        }
    }

    public class GetFilingRatiosResponse : ResponseBase
    {
        public GetFilingRatiosResponse()
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
                Ratios = new List<RatioRecord>();
            }

            public string RegulatorCode
            {
                get;
                set;
            }

            public string CompanyCode
            {
                get;
                set;
            }

            public string FilingName
            {
                get;
                set;
            }

            public List<RatioRecord> Ratios
            {
                get;
                set;
            }
        }
    }
}
