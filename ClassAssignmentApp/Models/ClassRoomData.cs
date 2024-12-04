namespace ClassAssignmentApp.Models
{
    [Serializable]
    public class ClassRoomData
    {
        public int ClassRoomID { get; set; }
        public int SelectedBuildingNo { get; set; }
        public string OperationMode { get; set; }
        public string ErrorMessage { get; set; }
        public bool ErrorCondition { get; set; }
        public int BuildingNumber { get; set; }

        //[Required(ErrorMessage = "Please enter a Room Number")]
        public int RoomNumber { get; set; }

        //[Required(ErrorMessage = "Please enter Maximum Capacity")]
        public int Capacity { get; set; }
        public string Unavailable { get; set; }
        public bool Unavail { get; set; }
    }
}
