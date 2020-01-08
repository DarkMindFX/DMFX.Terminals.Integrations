using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetFilingData/{RegulatorCode}/{CompanyCode}/{FilingName}/{SessionToken}", "GET")]
    [Route("/GetFilingData/{RegulatorCode}/{CompanyCode}/{FilingName}/{Values}/{SessionToken}", "GET")]
    [Route("/GetFilingData", "POST")]
    public class GetFilingData : RequestBase, IReturn<GetFilingDataResponse>
    {
        public GetFilingData()
        {
            Values = new List<string>();
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

        public List<string> Values
        {
            get;
            set;
        }
    }

    public class GetFilingDataResponse : ResponseBase
    {
        public GetFilingDataResponse()
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
                FilingData = new List<FilingNumRecord>();
                FilingStrData = new List<FilingStrRecord>();
                FilingDttmData = new List<FilingDttmRecord>();
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

            public string Type
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

            public DateTime Submitted
            {
                get;
                set;
            }

            public List<FilingNumRecord> FilingData
            {
                get;
                set;
            }

            public List<FilingStrRecord> FilingStrData
            {
                get;
                set;
            }

            public List<FilingDttmRecord> FilingDttmData
            {
                get;
                set;
            }
        }
    }
}
