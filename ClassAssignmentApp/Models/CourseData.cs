namespace ClassAssignmentApp.Models
{
    [Serializable]
    public class CourseData
    {
        public int CourseID { get; set; }
        public string SelectedDepartment { get; set; }
        public string OperationMode { get; set; }
        public string ErrorMessage { get; set; }
        public bool ErrorCondition { get; set; }
        public string Title { get; set; }

        //[Required(ErrorMessage = "Please enter a Room Number")]
        public string Department { get; set; }

        //[Required(ErrorMessage = "Please enter Maximum Capacity")]
        public int CreditHours { get; set; }

        public int? PrerequisiteCourseID { get; set; }
    }
}
