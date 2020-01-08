using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/GetCompanies/{RegulatorCode}/{SessionToken}", "GET")]
    [Route("/GetCompanies", "POST")]
    public class GetCompanies : RequestBase, IReturn<GetCompaniesResponse>
    {
        public string RegulatorCode
        {
            get;
            set;
        }
    }

    public class CompanyInfo
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
        
    }

    public class GetCompaniesResponse : ResponseBase
    {
        public GetCompaniesResponse()
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
                Companies = new List<CompanyInfo>();
            }
            public string RegulatorCode
            {
                get;
                set;
            }
            public List<CompanyInfo> Companies
            {
                get;
                set;
            }
        }
    }
}
