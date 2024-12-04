using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ClassAssignmentApp.Models
{
    public class StudentMVMaint
    {
        [DisplayName("LastNameSelection")]
        public string? LastNameSelection { get; set; }

        public bool ShowDetailsForm { get; set; }
        public string? OperationMode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? ErrorCondition { get; set; }

        public List<Student>? StudentList { get; set; }

        public int StudentID { get; set; }

        [Required(ErrorMessage = "Please enter Students First Name")]
        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Please enter Students Last Name")]
        public string? LastName { get; set; }

        public int? ClassRank { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public List<SelectListItem>? StateSelectList { get; set; }
    }
}
