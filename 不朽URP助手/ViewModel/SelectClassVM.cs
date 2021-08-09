using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace 不朽URP助手.ViewModel
{
    [DataContract]
    public class SelectClassVM
    {
        [DataMember]
        public string TeachClassId { get; set; }
        [DataMember]
        public string CourseName { get; set; }
        [DataMember]
        public string CourseNatureName { get; set; }
        [DataMember]
        public int TotalCredit { get; set; }
        [DataMember]
        public string Mon { get; set; }
        [DataMember]
        public string Tue { get; set; }
        [DataMember]
        public string Wed { get; set; }
        [DataMember]
        public string Thu { get; set; }
        [DataMember]
        public string Fri { get; set; }
        [DataMember]
        public string Sat { get; set; }
        [DataMember]
        public string Sun { get; set; }
        [DataMember]
        public string PeopleCount { get; set; }
        [IgnoreDataMember]
        public SolidColorBrush PeopleCountColor { get; set; }
        [DataMember]
        public string TeacherName { get; set; }
        [DataMember]
        public string TeacherTitle { get; set; }
        [DataMember]
        public string TestDate { get; set; }
        [DataMember]
        public string TestPeroid { get; set; }
        [DataMember]
        public string SelectStatus { get; set; }
    }
}
