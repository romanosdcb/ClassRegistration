using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    [Serializable]
    public class CourseMVMaint
    {
        //public int ViewMode { get; set; }
        public string? DepatmentSelection { get; set; }

        public bool ShowDetailsForm { get; set; }
        public string? OperationMode { get; set; }
        public string? SelectedDepartment { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? ErrorCondition { get; set; }

        public List<Course>? CourseList { get; set; }

        public int CourseID { get; set; }

        //[Required(ErrorMessage = "Please enter a Title")]
        public string? Title { get; set; }
        public string? Department { get; set; }

        //[Required(ErrorMessage = "Please enter Credit Hours")]
        public int? CreditHours { get; set; }

        public int? PrerequisiteCourseID { get; set; }
    }
}
