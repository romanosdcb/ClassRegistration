using ClassAssignmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ClassAssignmentApp.Controllers
{
    public class ScheduleMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");

        private readonly HttpClient _client;

        public ScheduleMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult ScheduleIndex()
        {
            ScheduleMVMaint scheduleMVMaint = new ScheduleMVMaint();

            scheduleMVMaint.InstructorSessions = ReturnBlankInstructorSchedule();
            scheduleMVMaint.StudentSessions = ReturnBlankStudentSchedule();

            return View(scheduleMVMaint);
        }

        [HttpPost]
        public IActionResult ScheduleIndex(ScheduleMVMaint obj)
        {
            GetAllLists(obj);
            obj.InstructorSessions = ReturnBlankInstructorSchedule();
            obj.StudentSessions = ReturnBlankStudentSchedule();

            switch (obj.RequestType)
            {
                case "I":
                    obj.ShowInstructor = true;
                    obj.ShowStudent = false;

                    if (obj.InstructorID != null && obj.InstructorID > 0)
                    {
                        obj.ShowInstructorData = true;
                        obj = DisplayInstructorSchedule(obj);
                    }
                    else
                        obj.ShowInstructorData = false;
                    break;

                case "S":
                    obj.ShowInstructor = false;
                    obj.ShowStudent = true;

                    if (obj.StudentID != null && obj.StudentID > 0)
                    {
                        obj.ShowStudentData = true;
                        obj = DisplayStudentSchedule(obj);
                    }
                    else
                        obj.ShowStudentData = false;
                    break;

                default:
                    break;
            }

            return View(obj);
        }

        private ScheduleMVMaint DisplayInstructorSchedule(ScheduleMVMaint obj)
        {
            string CallSequence = _client.BaseAddress + "/Session/" + obj.InstructorID.ToString() + "/Instructor";
            HttpResponseMessage InstSchedule = _client.GetAsync(CallSequence).Result;
            List<InstructorSchedule> IScheduleList = new List<InstructorSchedule>();

            if (InstSchedule.IsSuccessStatusCode)
            {
                var schedule = (Schedule?)InstSchedule.Content.ReadAsAsync<Schedule>().Result;
                IScheduleList.Clear();

                foreach (var item in schedule.InstructorSessions)
                {
                    InstructorSchedule REC = new InstructorSchedule();
                    REC.ClassSessionID = item.ClassSessionID;
                    REC.InstructorID = item.InstructorID;
                    REC.ClassRoomID = item.ClassRoomID;
                    REC.CourseID = item.CourseID;
                    REC.InstructorName = item.InstructorName;
                    REC.BuildingRoom = item.BuildingRoom;
                    REC.CourseName = item.CourseName;
                    REC.StartTime = item.StartTime;
                    REC.EndTime = item.EndTime;
                    REC.WeekDay = item.WeekDay;
                    REC.DayNumber = item.DayNumber;
                    REC.StudentCount = item.StudentCount;
                    IScheduleList.Add(REC);
                }
                obj.InstructorSessions = IScheduleList;
            }
            else
            {
                
            }

            return obj;
        }

        private ScheduleMVMaint DisplayStudentSchedule(ScheduleMVMaint obj)
        {
            string CallSequence = _client.BaseAddress + "/Session/" + obj.StudentID.ToString() + "/Student";
            HttpResponseMessage StudentSchedule = _client.GetAsync(CallSequence).Result;
            List<StudentSchedule> SScheduleList = new List<StudentSchedule>();

            if (StudentSchedule.IsSuccessStatusCode)
            {
                var schedule = (Schedule?)StudentSchedule.Content.ReadAsAsync<Schedule>().Result;
                SScheduleList.Clear();

                foreach (var item in schedule.StudentSessions)
                {
                    StudentSchedule REC = new StudentSchedule();
                    REC.ClassSessionID = item.ClassSessionID;
                    REC.InstructorID = item.InstructorID;
                    //REC.ClassRoomID = item.ClassRoomID;
                    REC.CourseID = item.CourseID;
                    REC.InstructorName = item.InstructorName;
                    REC.BuildingRoom = item.BuildingRoom;
                    REC.CourseName = item.CourseName;
                    REC.StartTime = item.StartTime;
                    REC.EndTime = item.EndTime;
                    REC.WeekDay = item.WeekDay;
                    REC.DayNumber = item.DayNumber;
                    SScheduleList.Add(REC);
                }
                obj.StudentSessions = SScheduleList;
            }
            else
            {

            }

            return obj;
        }

        private ScheduleMVMaint GetAllLists(ScheduleMVMaint obj)
        {
            List<Instructor> InstructorList = new List<Instructor>();
            List<Student> StudentList = new List<Student>();

            InstructorList.Clear();
            StudentList.Clear();

            HttpResponseMessage response = null;

            response = _client.GetAsync(_client.BaseAddress + "/Instructor").Result;

            if (response.IsSuccessStatusCode)
            {
                var dataInst = response.Content.ReadAsAsync<IList<Instructor>>();
                InstructorList = dataInst.Result.ToList();
                List<SelectListItem> InstructorSelectList = new List<SelectListItem>();

                foreach (var item in InstructorList)
                {
                    string FullName = item.LastName + ", " + item.FirstName;
                    InstructorSelectList.Add(new SelectListItem { Text = FullName, Value = item.InstructorID.ToString() });
                }
                obj.InstructorSelectList = InstructorSelectList;
            }

            response = _client.GetAsync(_client.BaseAddress + "/Student").Result;

            if (response.IsSuccessStatusCode)
            {
                var dataInst = response.Content.ReadAsAsync<IList<Student>>();
                StudentList = dataInst.Result.ToList();
                List<SelectListItem> StudentSelectList = new List<SelectListItem>();

                foreach (var item in StudentList)
                {
                    string FullName = item.LastName + ", " + item.FirstName;
                    StudentSelectList.Add(new SelectListItem { Text = FullName, Value = item.StudentID.ToString() });
                }
                obj.StudentSelectList = StudentSelectList;
            }

            return obj;
        }

        private static List<InstructorSchedule> ReturnBlankInstructorSchedule()
        {
            List<InstructorSchedule> InstSchedBlankList = new List<InstructorSchedule>();
            InstructorSchedule REC = new InstructorSchedule();
            REC.ClassSessionID = 0;
            REC.InstructorID = 0;
            REC.ClassRoomID = 0;
            REC.CourseID = 0;
            REC.InstructorName = "";
            REC.BuildingRoom = "";
            REC.CourseName = "";
            REC.StartTime = TimeOnly.MinValue;
            REC.EndTime = TimeOnly.MinValue;
            REC.DayNumber = 0;
            REC.StudentCount = 0;
            InstSchedBlankList.Add(REC);

            return InstSchedBlankList;
        }

        private static List<StudentSchedule> ReturnBlankStudentSchedule()
        {
            List<StudentSchedule> StudentSchedBlankList = new List<StudentSchedule>();
            StudentSchedule REC = new StudentSchedule();
            REC.ClassSessionID = 0;
            REC.StudentID = 0;
            REC.CourseID = 0;
            REC.InstructorID = 0;
            REC.DayNumber = 0;
            REC.WeekDay = "";
            REC.StartTime = TimeOnly.MinValue;
            REC.EndTime = TimeOnly.MinValue;
            REC.BuildingRoom = "";
            REC.InstructorName = "";

            StudentSchedBlankList.Add(REC) ;
            return StudentSchedBlankList ;
        }
    }
}
