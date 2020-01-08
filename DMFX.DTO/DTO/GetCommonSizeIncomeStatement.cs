using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetCommonSizeIncomeStatement/{RegulatorCode}/{CompanyCode}/{FilingName}/{SessionToken}", "GET")]
    [Route("/GetCommonSizeIncomeStatement", "POST")]
    public class GetCommonSizeIncomeStatement : RequestBase, IReturn<GetCommonSizeIncomeStatementResponse>
    {
        public GetCommonSizeIncomeStatement()
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

    public class GetCommonSizeIncomeStatementResponse : ResponseBase
    {
        public class ResponsePayload
        {
            public ResponsePayload()
            {
                IncomeStatementData = new List<FilingRecordBase>();
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

            public List<FilingRecordBase> IncomeStatementData
            {
                get;
                set;
            }
        }

        public GetCommonSizeIncomeStatementResponse()
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
