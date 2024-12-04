namespace ClassAssignmentApp.Models
{
    [Serializable]
    public class RECData
    {
        public string ErrorMessage { get; set; }
        public int BuildingNumber { get; set; }
        public int RoomNumber { get; set; }
        public int Capacity { get; set; }
        public string Unavailable { get; set; }
    }
}
