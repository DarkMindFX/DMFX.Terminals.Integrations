using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetCommonSizeBalanceSheet/{RegulatorCode}/{CompanyCode}/{FilingName}/{SessionToken}", "GET")]
    [Route("/GetCommonSizeBalanceSheet", "POST")]
    public class GetCommonSizeBalanceSheet : RequestBase, IReturn<GetCommonSizeBalanceSheetResponse>
    {
        public GetCommonSizeBalanceSheet()
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

    public class GetCommonSizeBalanceSheetResponse : ResponseBase
    {
        public class ResponsePayload
        {
            public ResponsePayload()
            {
                BalanceSheetData = new List<FilingRecordBase>();
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

            public List<FilingRecordBase> BalanceSheetData
            {
                get;
                set;
            }
        }

        public GetCommonSizeBalanceSheetResponse()
        {
            Payload = new ResponsePayload();
        }

        public ResponsePayload Payload
        {
            get;
            set;
        }
    }
}
