using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetCompanyFilingsInfo/{RegulatorCode}/{CompanyCode}/{SessionToken}", "GET")]
    [Route("/GetCompanyFilingsInfo/{RegulatorCode}/{CompanyCode}/{Types}/{SessionToken}", "GET")]
    [Route("/GetCompanyFilingsInfo/{RegulatorCode}/{CompanyCode}/{PeriodStart}/{PeriodEnd}/{SessionToken}", "GET")]
    [Route("/GetCompanyFilingsInfo/{RegulatorCode}/{CompanyCode}/{Types}/{PeriodStart}/{PeriodEnd}/{SessionToken}", "GET")]
    [Route("/GetCompanyFilingsInfo", "POST")]
    public class GetCompanyFilingsInfo : RequestBase, IReturn<GetCompanyFilingsInfoResponse>
    {
        public GetCompanyFilingsInfo()
        {
            Types = new List<string>();
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

        public DateTime? PeriodStart
        {
            get;
            set;
        }

        public DateTime? PeriodEnd
        {
            get;
            set;
        }

        public List<string> Types
        {
            get;
            set;
        }

    }

    public class CompanyFilingInfo
    {
        public string Name
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
    }

    public class GetCompanyFilingsInfoResponse : ResponseBase
    {
        public GetCompanyFilingsInfoResponse()
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
                Filings = new List<CompanyFilingInfo>();
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

            public List<CompanyFilingInfo> Filings
            {
                get;
                set;
            }
        }
    }
}
