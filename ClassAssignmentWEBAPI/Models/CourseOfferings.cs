using Microsoft.EntityFrameworkCore;

namespace ClassAssignmentWEBAPI.Models
{
    [Keyless]
    public class CourseOfferings
    {
        public int ClassSessionID { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int ClassRoomID { get; set; }
        public int BuildingNumber { get; set; }
        public int RoomNumber { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int RoomCapacity { get; set; }
        public int DayNumber { get; set; }
        public string AvailabilityStatus { get; set; }
        public string ClosedOut { get; set; }
        public string InstructorName { get; set; }
    }
}
