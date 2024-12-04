using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentApp.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        //[Required]
        [Display(Name = "Title")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Department Name")]
        [StringLength(100)]
        public string Department { get; set; } = string.Empty;

        //[Required]
        [Display(Name = "Credit Hours")]
        public int CreditHours { get; set; } = Int32.MinValue;

        [Display(Name = "Prerequisite ID")]
        public int? PrerequisiteCourseID { get; set; } = Int32.MinValue;
    }
}
