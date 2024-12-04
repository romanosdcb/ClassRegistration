using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public int CreditHours { get; set; }

        public int? PrerequisiteCourseID { get; set; }
    }
}
