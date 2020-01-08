using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    public abstract class RequestBase
    {
        public RequestBase()
        {
            RequestID = Guid.NewGuid().ToString();
        }

        public string RequestID
        {
            get; set;
        }

        public string SessionToken
        {
            get; set;
        }

    }
}
