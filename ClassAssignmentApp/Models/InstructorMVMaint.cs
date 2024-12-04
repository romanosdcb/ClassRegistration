using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentApp.Models
{
    public class InstructorMVMaint
    {
        [DisplayName("LastNameSelection")]
        public string? LastNameSelection { get; set; }

        public bool ShowDetailsForm { get; set; }
        public string? OperationMode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? ErrorCondition { get; set; }

        public List<Instructor>? InstructorList { get; set; }

        public int InstructorID { get; set; }

        [Required(ErrorMessage = "Please enter Instructors First Name")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Instructors Last Name")]
        public string? LastName { get; set; }

        public string? Title { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public List<SelectListItem>? StateSelectList { get; set; }
    }


}
