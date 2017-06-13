using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 不朽URP助手.Entities
{
    public class TeachClassModel
    {
        public string TeachClassId { get; set; }
        public string CourseName { get; set; }
        public string CourseNatureCode { get; set; }
        public string CourseNatureName { get; set; }
        public int TotalCredit { get; set; }
        public string Mon { get; set; }
        public string Tue { get; set; }
        public string Wed { get; set; }
        public string Thu { get; set; }
        public string Fri { get; set; }
        public string Sat { get; set; }
        public string Sun { get; set; }
        public int Capacity { get; set; }
        public int SelectedCount { get; set; }
        public string TeacherName { get; set; }
        public object TeacherNameSy { get; set; }
        public string ClassRoom { get; set; }
        public string TestDate { get; set; }
        public string TestPeroid { get; set; }
        public object Memo { get; set; }
        public string TeachMethod { get; set; }
        public string TeacherTitle { get; set; }
    }
}
