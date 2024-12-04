using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentWEBAPI.Models
{
    public class Instructor
    {
        [Key]
        public int InstructorID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Middle Name")]
        [StringLength(50)]
        public string? MiddleName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Title")]
        [StringLength(50)]
        public string? Title { get; set; } = string.Empty;

        [Display(Name = "Phone No")]
        [StringLength(20)]
        public string? PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Address Line 1")]
        [StringLength(100)]
        public string? AddressLine1 { get; set; } = string.Empty;


        [Display(Name = "Address Line 2")]
        [StringLength(50)]
        public string? AddressLine2 { get; set; } = string.Empty;

        [Display(Name = "City")]
        [StringLength(100)]
        public string? City { get; set; } = string.Empty;

        [Display(Name = "State")]
        [StringLength(2)]
        public string? State { get; set; } = string.Empty;

        [Display(Name = "Zip code")]
        [StringLength(10)]
        public string? ZipCode { get; set; } = string.Empty;
    }
}
