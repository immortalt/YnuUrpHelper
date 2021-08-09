using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 不朽URP助手.Entities
{
    public class Semester
    {
        public string Id { get; set; }
        public string Year { get; set; }
        public string SeasonCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public object TestDate { get; set; }
        public object TestPeroid { get; set; }
        public object TestPeroidMemo { get; set; }
        public int ClassPeroidNum { get; set; }
        public object Status { get; set; }
        public object Memo { get; set; }
        public object CourseSemesterStatus { get; set; }
        public string SeasonName { get; set; }
    }

    public class SurveyItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }

    public class Survey
    {
        public Semester Semester { get; set; }
        public List<SurveyItem> SurveyItemList { get; set; }
    }

    public class TeachClass
    {
        public string TeachClassId { get; set; }
        public string CourseName { get; set; }
        public string CourseNature { get; set; }
        public string TeacherName { get; set; }
    }

    public class EvaluationResult
    {
        public string TeachClassId { get; set; }
        public double Score { get; set; }
        public string SurveyAnswerStr { get; set; }
        public string Comment { get; set; }
    }
    public class TeachEvaluation
    {
        public Survey Survey { get; set; }
        public List<TeachClass> TeachClassList { get; set; }
        public List<EvaluationResult> EvaluationResultList { get; set; }
    }
}
