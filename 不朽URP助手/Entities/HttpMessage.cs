using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace 不朽URP助手.Entities
{
    public class HttpMessage
    {
        public HttpStatusCode statusCode { get; set; }
        public object data { get; set; }
    }
}
