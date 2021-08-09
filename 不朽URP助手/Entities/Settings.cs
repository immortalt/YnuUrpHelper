using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 不朽URP助手.ViewModel;

namespace 不朽URP助手.Entities
{
    public class UrpSettings
    {
        public bool savepwd { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<SelectClassVM> SelectCourseCar { get; set; }
    }
}
