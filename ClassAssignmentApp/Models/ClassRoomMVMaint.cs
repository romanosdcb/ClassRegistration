using System.ComponentModel.DataAnnotations;

namespace ClassAssignmentApp.Models
{
    [Serializable]
    public class ClassRoomMVMaint
    {
        //public int ViewMode { get; set; }
        public int BuildingNumberSelection { get; set; }

        public bool ShowDetailsForm { get; set; }
        public string? OperationMode { get; set; }
        public int? SelectedBuildingNo { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? ErrorCondition { get; set; }

        public List<ClassRoom>? ClassRoomList { get; set; }

        public int ClassRoomID { get; set; }

        //[Required(ErrorMessage = "Please enter a Building Number")]
        public int? BuildingNumber { get; set; }

        //[Required(ErrorMessage = "Please enter a Room Number")]
        public int? RoomNumber { get; set; }

        //[Required(ErrorMessage = "Please enter Maximum Capacity")]
        public int? Capacity { get; set; }

        public string? Unavailable { get; set; }
        public bool Unavail { get; set; }

    }

   
}
