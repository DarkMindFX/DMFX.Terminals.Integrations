using ServiceStack;
using ServiceStack.ServiceHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    [Route("/CloseSession/{SessionToken}", "GET")]
    [Route("/CloseSession", "POST")]
    public class CloseSession : RequestBase, IReturn<CloseSessionResponse>
    {
    }

    public class CloseSessionResponse : ResponseBase
    {
    }
}
