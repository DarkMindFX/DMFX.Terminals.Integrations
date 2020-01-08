using DMFX.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.Service.DTO
{
    public abstract class ResponseBase
    {
        public ResponseBase()
        {
            Errors = new List<Error>();
            PayloadFormat = "json";
        }

        public string RequestID
        {
            get; set;
        }

        public bool Success
        {
            get; set;
        }

        public List<Error> Errors
        {
            get; set;
        }

        public string SessionToken
        {
            get;
            set;
        }

        public string PayloadFormat
        {
            get;
            set;
        }
    }
}
