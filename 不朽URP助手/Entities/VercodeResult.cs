using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 不朽URP助手.Entities
{
    public class VercodeResult
    {
        public string resultcode { get; set; }
        public string reason { get; set; }
        public object result { get; set; }
        public int error_code { get; set; }
    }
}
