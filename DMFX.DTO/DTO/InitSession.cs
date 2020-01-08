using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/InitSession/{AccountKey}", "GET")]
    [Route("/InitSession", "POST")]
    public class InitSession : RequestBase, IReturn<InitSessionResponse>
    {
        public string AccountKey
        {
            get; set;
        }
    }

    public class InitSessionResponse : ResponseBase
    {
    }
}
