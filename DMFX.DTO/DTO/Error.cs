using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Interfaces
{
    public class Error
    {
        public Error()
        {
            Timestamp = DateTime.UtcNow;
        }

        public EErrorCodes Code
        {
            get;
            set;
        }

        public EErrorType Type
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public DateTime Timestamp
        {
            get;
            set;
        }
    }
}
