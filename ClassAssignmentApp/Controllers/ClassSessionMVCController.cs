using Azure;
using ClassAssignmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ClassAssignmentApp.Controllers
{
    public class ClassSessionMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");

        private readonly HttpClient _client;

        public ClassSessionMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult ClassSessionsIndex(int? InstructorID, int? ClassRoomID, int? CourseID, string? WeekDay,
            int? BuildingNumber, string? ScreenMessage, string? ErrorMessage)
        {
            ClassSessionMVMaint ClassSessionMV = new ClassSessionMVMaint();
            if (InstructorID != null) ClassSessionMV.InstructorID = (int)InstructorID;
            if (ClassRoomID != null) ClassSessionMV.ClassRoomID = (int)ClassRoomID;
            if (CourseID != null) ClassSessionMV.CourseID = (int)CourseID;
            if (WeekDay != null) ClassSessionMV.WeekDay = (string)WeekDay;
            if (BuildingNumber != null) ClassSessionMV.BuildingNumber = (int)BuildingNumber;

            GetAllLists(ClassSessionMV);
            ClassSessionMV.InstructorSessions = ReturnBlankWorkingSchedule();
            ClassSessionMV.ClassRoomSessions = ReturnBlankWorkingSchedule();

            if (InstructorID != null && ClassRoomID != null && CourseID != null
                && WeekDay != null && BuildingNumber != null)
            {
                RefreshScreen(ClassSessionMV);
            }

            if (ErrorMessage != null)
            {
                ClassSessionMV.ErrorMessage = (string)ErrorMessage;
                ClassSessionMV.ErrorCondition = true;
                ClassSessionMV.ShowForInput = true;
                ClassSessionMV.ShowInstructorSchedule = true;
                ClassSessionMV.ShowRoomSchedule = true;
                return View(ClassSessionMV);
            }

            return View(ClassSessionMV);
        }

        [HttpGet]
        public IActionResult StudentSessionsIndex(int? StudentID, int? CourseID,string? ScreenMessage, string? ErrorMessage)
        {
            StudentSessionMVMaint StudentSessionMV = new StudentSessionMVMaint();

            if (ScreenMessage != null && ScreenMessage == "Course Added")
            {
                CourseID = 0;
                StudentSessionMV.CourseID = 0;
                StudentSessionMV.ShowCourseSchedule = false;
            }

            if (StudentID != null) StudentSessionMV.StudentID = (int)StudentID;
            if (CourseID != null) StudentSessionMV.CourseID = (int)CourseID;

            GetAllStudentLists(StudentSessionMV);

            StudentSessionMV.StudentSessions = ReturnBlankWorkingSchedule();
            StudentSessionMV.CourseSessions = ReturnBlankWorkingSchedule();
            StudentSessionMV.CourseOfferings = ReturnBlankCourseOfferings();

            //if (StudentID != null && CourseID != null)
            //    RefreshStudentScreen(StudentSessionMV);

            if (StudentID != null)
                RefreshStudentScreen(StudentSessionMV);

            if (ErrorMessage != null)
            {
                StudentSessionMV.ErrorMessage = (string)ErrorMessage;
                StudentSessionMV.ShowScreenMessage = true;
            }

            return View(StudentSessionMV);
        }

        [HttpPost]
        public IActionResult ClassSessionsIndex(ClassSessionMVMaint ClassSessionMV, string? command)
        {
            ClassSessionMV.ShowScreenMessage = false;
            RefreshScreen(ClassSessionMV);

            if (command != null)
            {
                string FullCommand = command;
            }

            switch (command)
            {
                case "NewEntry":
                    if (ClassSessionMV.StartTime == null || ClassSessionMV.EndTime == null)
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ErrorMessage = "Start and End Times are required";
                        return View(ClassSessionMV);
                    }

                    if (ClassSessionMV.StartTime > ClassSessionMV.EndTime)
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ErrorMessage = "Start Time must be before End Time";
                        return View(ClassSessionMV);
                    }

                    if (ClassSessionMV.InstructorID == null || ClassSessionMV.InstructorID < 1)
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ErrorMessage = "Instructor ID is missing";
                        return View(ClassSessionMV);
                    }

                    if (ClassSessionMV.WeekDay == null || ClassSessionMV.WeekDay == "")
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ErrorMessage = "WeekDay is missing";
                        return View(ClassSessionMV);
                    }

                    string InstructorCallSequence = _client.BaseAddress + "/ClassSession/"
                        + ClassSessionMV.InstructorID.ToString() + "/"
                        + ClassSessionMV.WeekDay + "/"
                        + ClassSessionMV.StartTime.ToString() + "/"
                        + ClassSessionMV.EndTime.ToString() + "/Instructor";
                    HttpResponseMessage InstructorConflicts = _client.GetAsync(InstructorCallSequence).Result;

                    string ClassRoomCallSequence = _client.BaseAddress + "/ClassSession/"
                        + ClassSessionMV.ClassRoomID.ToString() + "/"
                        + ClassSessionMV.WeekDay + "/"
                        + ClassSessionMV.StartTime.ToString() + "/"
                        + ClassSessionMV.EndTime.ToString() + "/ClassRoom";
                    HttpResponseMessage ClassRoomConflicts = _client.GetAsync(ClassRoomCallSequence).Result;


                    if (InstructorConflicts.IsSuccessStatusCode && ClassRoomConflicts.IsSuccessStatusCode)
                    {
                        //var dataList = InstructorConflicts.Content.ReadAsAsync<IList<WorkingDailyConflicts>>();
                        //List<WorkingDailyConflicts> ConflictList = dataList.Result.ToList();

                        //if (ConflictList.Count == 1)
                        //{
                        //    ClassSessionMV.ErrorCondition = true;
                        //    ClassSessionMV.ErrorMessage = "Conflicts with instructor session: "
                        //        + ConflictList[0].StartTime.ToString() + " - " +
                        //        ConflictList[0].EndTime.ToString();
                        //    return View(ClassSessionMV);
                        //}
                        //else if (ConflictList.Count > 1)
                        //    {
                        //    ClassSessionMV.ErrorCondition = true;
                        //    ClassSessionMV.ErrorMessage = "Multiple session conflicts";
                        //     return View(ClassSessionMV);
                        //}
                        //----------------------------------------------------------------------
                        var InstructorDataList = InstructorConflicts.Content.ReadAsAsync<IList<WorkingDailyConflicts>>();
                        List<WorkingDailyConflicts> InstructorConflictList = InstructorDataList.Result.ToList();

                        var ClassRoomDataList = ClassRoomConflicts.Content.ReadAsAsync<IList<WorkingDailyConflicts>>();
                        List<WorkingDailyConflicts> ClassRoomConflictList = ClassRoomDataList.Result.ToList();

                        if (InstructorConflictList.Count > 0 || ClassRoomConflictList.Count > 0)
                        { //At least one conflict - either Instructor or Classroom
                            if (InstructorConflictList.Count == 1 && ClassRoomConflictList.Count == 0)
                            { //Just one conflict with Instructor schedule
                                ClassSessionMV.ErrorCondition = true;
                                ClassSessionMV.ErrorMessage = "Conflicts with instructor session: "
                                    + InstructorConflictList[0].StartTime.ToString() + " - " +
                                    InstructorConflictList[0].EndTime.ToString();
                                return View(ClassSessionMV);
                            }
                            else if (InstructorConflictList.Count == 0 && ClassRoomConflictList.Count == 1)
                            { //Just one conflict with Classroom schedule
                                ClassSessionMV.ErrorCondition = true;
                                ClassSessionMV.ErrorMessage = "Conflicts with class room session: "
                                    + ClassRoomConflictList[0].StartTime.ToString() + " - " +
                                    ClassRoomConflictList[0].EndTime.ToString();
                                return View(ClassSessionMV);
                            }
                            else
                            { //Multiple schedule conflicts for Instructor and/or Classroom
                                ClassSessionMV.ErrorCondition = true;
                                ClassSessionMV.ErrorMessage = "Multiple session conflicts";
                                return View(ClassSessionMV);
                            }
                        }
                    }
                    else
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ErrorMessage = "System Error: Unable to validate entry";
                        return View(ClassSessionMV);
                    }

                    ClassSession REC1 = new ClassSession();
                    ClassSession2 REC = new ClassSession2();

                    REC.WeekDay = ClassSessionMV.WeekDay;
                    //REC.StartTime = (TimeOnly)ClassSessionMV.StartTime;
                    //REC.EndTime = (TimeOnly)ClassSessionMV.EndTime;
                    string sStartTime = ClassSessionMV.StartTime.ToString();
                    string sEndTime = ClassSessionMV.EndTime.ToString();
                    string sConvertedStartTime = ConvertTODtoString((TimeOnly)ClassSessionMV.StartTime);
                    string sConvertedEndTime = ConvertTODtoString((TimeOnly)ClassSessionMV.EndTime);

                    //REC.StartTime = "09:00:00";
                    //REC.EndTime = "09:50:00";
                    REC.StartTime = sConvertedStartTime;
                    REC.EndTime = sConvertedEndTime;
                    REC.InstructorID = ClassSessionMV.InstructorID;
                    REC.ClassRoomID = ClassSessionMV.ClassRoomID;
                    REC.CourseID = ClassSessionMV.CourseID;
                    HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/ClassSession/", REC).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        ClassSessionMV.ErrorCondition = false;
                        ClassSessionMV.ShowScreenMessage = true;
                        ClassSessionMV.ErrorMessage = "";
                        ClassSessionMV.ScreenMessage = "Session Added!";
                        return RedirectToAction("ClassSessionsIndex", new
                        {
                            InstructorID = (int)ClassSessionMV.InstructorID,
                            ClassRoomID = (int)ClassSessionMV.ClassRoomID,
                            CourseID = (int)ClassSessionMV.CourseID,
                            WeekDay = (string)ClassSessionMV.WeekDay,
                            BuildingNumber = (int)ClassSessionMV.BuildingNumber,
                            ScreenMessage = (string)ClassSessionMV.ScreenMessage
                        });
                    }
                    else
                    {
                        ClassSessionMV.ErrorCondition = true;
                        ClassSessionMV.ShowScreenMessage = false;
                        ClassSessionMV.ErrorMessage = "Error: Unable to add session!";
                        ClassSessionMV.ScreenMessage = "";
                    }

                    return View(ClassSessionMV);

                default:
                    break;
            }

            return View(ClassSessionMV);
        }

        [HttpPost]
        public IActionResult StudentSessionsIndex(StudentSessionMVMaint StudentSessionMV, string? command)
        {
            RefreshStudentScreen(StudentSessionMV);

            //if (StudentSessionMV.StudentID > 0)
            //{
            //    for (int i = 0; i < StudentSessionMV.StudentSelectList.Count; i++)
            //    {
            //        if (StudentSessionMV.StudentSelectList[i].Value == StudentSessionMV.StudentID.ToString())
            //        {
            //            StudentSessionMV.StudentName = StudentSessionMV.StudentSelectList[i].Text;
            //            break;
            //        }
            //    }
            //}

            //if (StudentSessionMV.CourseID > 0)
            //{
            //    for (int i = 0; i < StudentSessionMV.CourseSelectList.Count; i++)
            //    {
            //        if (StudentSessionMV.CourseSelectList[i].Value == StudentSessionMV.CourseID.ToString())
            //        {
            //            StudentSessionMV.CourseName = StudentSessionMV.CourseSelectList[i].Text;
            //            break;
            //        }
            //    }
            //}
            StudentSessionMV.ShowScreenMessage = false;
            return View(StudentSessionMV);
        }

        private string ConvertTODtoString(TimeOnly TOD)
        {
            string TODasString = "";
            string sTOD = TOD.ToString();
            string[] TimeArray = sTOD.Split(':', ' ');
            int HH = Convert.ToInt32(TimeArray[0]);
            int MM = Convert.ToInt32(TimeArray[1]);
            string AM_PM = TimeArray[TimeArray.Length - 1];

            if (AM_PM == "PM" && HH < 12) HH += 12;
            TODasString = String.Format("{0:00}:{1:00}:00", HH, MM);
            return TODasString;
        }


        [HttpGet]
        public IActionResult FileUpdate(int? InstructorID, int? ClassRoomID, int? CourseID, string? WeekDay,
            int? BuildingNumber, int? id)
        {
            //First see if any students have been assigned to this session before doing the Delete
            string StudentListSEQ = _client.BaseAddress + "/StudentClassSession/" + id.ToString()
                + "/StudentList/AAA/BBB";
            HttpResponseMessage MessageResponse = _client.GetAsync(StudentListSEQ).Result;

            if (MessageResponse.IsSuccessStatusCode)
            {
                var StudentData = MessageResponse.Content.ReadAsAsync<IList<StudentClassSession>>();
                List<StudentClassSession> DataList = StudentData.Result.ToList();
                if (DataList.Count > 0)
                {
                    string ReturnMessage = "Cannot delete - Students Already Assigned!";
                    return RedirectToAction("ClassSessionsIndex", new
                    {
                        InstructorID = (int)InstructorID,
                        ClassRoomID = (int)ClassRoomID,
                        CourseID = (int)CourseID,
                        WeekDay = (string)WeekDay,
                        BuildingNumber = (int)BuildingNumber,
                        ErrorMessage = ReturnMessage
                    });
                }
            }
            else
            {
                return NotFound();
            }

            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/ClassSession/" + id.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ClassSessionsIndex", new
                {
                    InstructorID = (int)InstructorID,
                    ClassRoomID = (int)ClassRoomID,
                    CourseID = (int)CourseID,
                    WeekDay = (string)WeekDay,
                    BuildingNumber = (int)BuildingNumber
                });
            }
            else
            {
                //return NotFound();
                return RedirectToAction("Index",
                 new RouteValueDictionary(new
                 {
                     controller = "ErrorMessage",
                     action = "Index",
                     ErrorMessage = "Unsuccessful Delete Record Attempt",
                     AddressDescription = "at Class Session Maintenance"
                 }));
            }
        }

        [HttpGet]
        public IActionResult RemoveCourseAssignment(int? StudentClassSessionID, int? StudentID)
        {
            //string InstructorID = ClassSessionMV.InstructorID.ToString();

            HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/StudentClassSession/" + StudentClassSessionID.ToString()).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("StudentSessionsIndex", new
                {
                    StudentID = (int)StudentID,
                    CourseID = 0,
                    ScreenMessage = "Course Removed"
                });
            }
            else
            {
                return RedirectToAction("Index",
                 new RouteValueDictionary(new
                 {
                     controller = "ErrorMessage",
                     action = "Index",
                     ErrorMessage = "Unsuccessful Remove Course Attempt",
                     AddressDescription = "at Class Session Maintenance"
                 }));

                //return NotFound(); 
            }

        }

        [HttpGet]
        public IActionResult RegisterStudent(int? StudentID, int? CourseID, int? ClassSessionID)
        {
            if (StudentID != null && CourseID != null && ClassSessionID != null)
            {
                StudentID = (int)StudentID;
                CourseID = (int)CourseID;
                StudentClassSession obj = new StudentClassSession();
                obj.StudentID = (int)StudentID;
                obj.ClassSessionID = (int)ClassSessionID;

                HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/StudentClassSession/", obj).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("StudentSessionsIndex", new
                    {
                        StudentID = (int)StudentID,
                        CourseID = 0,
                        ScreenMessage = "Course Added"
                    });
                }
                else
                {
                    return RedirectToAction("Index",
                     new RouteValueDictionary(new
                     {
                         controller = "ErrorMessage",
                         action = "Index",
                         ErrorMessage = "Unsuccessful Student Registration Attempt",
                         AddressDescription = "at Class Session Maintenance"
                     }));

                    //return NotFound(); 
                }
            }
            else
            {
                return RedirectToAction("Index",
                 new RouteValueDictionary(new
                 {
                     controller = "ErrorMessage",
                     action = "Index",
                     ErrorMessage = "Unsuccessful Student Registration Attempt",
                     AddressDescription = "at Class Session Maintenance"
                 }));

                //return NotFound();
            }
        }

        private static List<WorkingDailySchedule> ReturnBlankWorkingSchedule()
        {
            List<WorkingDailySchedule> InstSchedBlankList = new List<WorkingDailySchedule>();
            WorkingDailySchedule ScheduleREC = new WorkingDailySchedule();
            ScheduleREC.InstructorID = 0;
            ScheduleREC.RoomNumber = 0;
            ScheduleREC.LastName = "";
            ScheduleREC.FirstName = "";
            ScheduleREC.MiddleName = "";
            ScheduleREC.BuildingNumber = 0;
            ScheduleREC.StartTime = TimeOnly.MinValue;
            ScheduleREC.EndTime = TimeOnly.MinValue;
            ScheduleREC.DayOfWeek = "";
            ScheduleREC.Title = "";
            InstSchedBlankList.Add(ScheduleREC);

            return InstSchedBlankList;
        }

        private static List<CourseOfferings> ReturnBlankCourseOfferings()
        {
            List<CourseOfferings> OfferingsList = new List<CourseOfferings>();
            CourseOfferings REC = new CourseOfferings();
            REC.ClassRoomID = 0;
            REC.CourseID = 0;
            REC.CourseName = "";
            REC.ClassRoomID = 0;
            REC.BuildingNumber = 0;
            REC.RoomNumber= 0;
            REC.DayOfWeek = "";
            REC.StartTime = TimeOnly.MinValue;
            REC.EndTime = TimeOnly.MinValue;
            REC.RoomCapacity = 0;
            REC.DayNumber = 0;
            REC.AvailabilityStatus = "";
            REC.ClosedOut = "";
            REC.InstructorName = "";
            OfferingsList.Add(REC);

            return OfferingsList;
        }

        /*
        [HttpPost]
        public IActionResult ClassSessionsIndex(ClassSessionMVMaint ClassSessionMV,
            Microsoft.AspNetCore.Http.IFormCollection form, string? command)

         */

        private void RefreshStudentScreen(StudentSessionMVMaint StudentSessionMV)
        {
            List<WorkingDailySchedule> StudentSessions = new List<WorkingDailySchedule>();
            List<CourseOfferings> OfferingsList = new List<CourseOfferings>();
            StudentSessions.Clear();
            StudentSessionMV.ShowScreenMessage = false;

            int? StudentID = StudentSessionMV.StudentID;
            int? CourseID = StudentSessionMV.CourseID;

            GetAllStudentLists(StudentSessionMV);

            StudentSessionMV.ShowForInput = true;
            StudentSessionMV.ShowStudentSchedule = true;
            StudentSessionMV.ShowCourseSchedule = true;

            if (StudentSessionMV.StudentID < 1)
            {
                StudentSessionMV.ShowForInput = false;
                StudentSessionMV.ShowStudentSchedule = false;
            }
                
            if (StudentSessionMV.CourseID < 1)
                StudentSessionMV.ShowCourseSchedule = false;

            StudentSessionMV.StudentSessions = ReturnBlankWorkingSchedule();
            StudentSessionMV.CourseSessions = ReturnBlankWorkingSchedule();
            StudentSessionMV.CourseOfferings = ReturnBlankCourseOfferings();

            if (StudentSessionMV.StudentID > 0)
            {
                for (int i = 0; i < StudentSessionMV.StudentSelectList.Count; i++)
                {
                    if (StudentSessionMV.StudentSelectList[i].Value == StudentSessionMV.StudentID.ToString())
                    {
                        StudentSessionMV.StudentName = StudentSessionMV.StudentSelectList[i].Text;
                        break;
                    }
                }
            }

            if (StudentSessionMV.CourseID > 0)
            {
                for (int i = 0; i < StudentSessionMV.CourseSelectList.Count; i++)
                {
                    if (StudentSessionMV.CourseSelectList[i].Value == StudentSessionMV.CourseID.ToString())
                    {
                        StudentSessionMV.CourseName = StudentSessionMV.CourseSelectList[i].Text;
                        break;
                    }
                }
            }

            if (StudentSessionMV.ShowForInput == true)
            {
                string CallSequence = _client.BaseAddress + "/StudentClassSession/" + StudentID.ToString()
                    + "/CourseSchedule";
                HttpResponseMessage StudentSchedule = _client.GetAsync(CallSequence).Result;

                if (StudentSchedule.IsSuccessStatusCode)
                {
                    var dataList = StudentSchedule.Content.ReadAsAsync<IList<WorkingDailySchedule>>();
                    //InstructorSessions = dataList.Result.ToList();
                    StudentSessions.Clear();

                    foreach (var item in dataList.Result)
                    {
                        WorkingDailySchedule REC = new WorkingDailySchedule();
                        REC.StudentClassSessionID = item.StudentClassSessionID;
                        REC.ClassSessionID = item.ClassSessionID;
                        REC.InstructorID = item.InstructorID;
                        REC.RoomNumber = item.RoomNumber;
                        REC.BuildingNumber = item.BuildingNumber;
                        REC.FirstName = item.FirstName;
                        REC.MiddleName = item.MiddleName;
                        REC.LastName = item.LastName;
                        REC.Title = item.Title;
                        REC.StartTime = item.StartTime;
                        REC.EndTime = item.EndTime;
                        REC.DayOfWeek = item.DayOfWeek;
                        StudentSessions.Add(REC);

                        if (StudentID != null && StudentID > 0)
                        {

                        }
                    }

                    StudentSessionMV.StudentSessions = StudentSessions;

                    //----------------------------------------------------------------------------------------
                    //Course Offerings
                    if (CourseID != null && CourseID > 0 && StudentID != null && StudentID > 0)
                    {
                        string OfferingsSeq = _client.BaseAddress + "/StudentClassSession/" + CourseID.ToString()
                        + "/" + StudentID.ToString() + "/1";
                        HttpResponseMessage OfferingResponse = _client.GetAsync(OfferingsSeq).Result;

                        if (OfferingResponse.IsSuccessStatusCode)
                        {
                            var COFDataList = OfferingResponse.Content.ReadAsAsync<IList<CourseOfferings>>();
                            OfferingsList.Clear();

                            foreach (var item in COFDataList.Result)
                            {
                                CourseOfferings COFREC = new CourseOfferings();
                                COFREC.ClassSessionID = item.ClassSessionID;
                                COFREC.CourseID = item.CourseID;
                                COFREC.CourseName = item.CourseName;
                                COFREC.ClassRoomID = item.ClassRoomID;
                                COFREC.BuildingNumber = item.BuildingNumber;
                                COFREC.RoomNumber = item.RoomNumber;
                                COFREC.DayOfWeek = item.DayOfWeek;
                                COFREC.StartTime = item.StartTime;
                                COFREC.EndTime = item.EndTime;
                                COFREC.RoomCapacity = item.RoomCapacity;
                                COFREC.DayNumber = item.DayNumber;
                                COFREC.AvailabilityStatus = item.AvailabilityStatus;
                                COFREC.ClosedOut = item.ClosedOut;
                                COFREC.InstructorName = item.InstructorName;
                                OfferingsList.Add(COFREC);
                            }
                            StudentSessionMV.CourseOfferings = OfferingsList;
                        }
                    }
                }

            }
        }

        private void RefreshScreen(ClassSessionMVMaint ClassSessionMV)
        {
            List<WorkingDailySchedule> InstructorSessions = new List<WorkingDailySchedule>();
            InstructorSessions.Clear();

            int? InstructorID = ClassSessionMV.InstructorID;
            int? ClassRoomID = ClassSessionMV.ClassRoomID;
            int? CourseID = ClassSessionMV.CourseID;
            string? WeekDay = ClassSessionMV.WeekDay;
            int? BuildingNumber = ClassSessionMV.BuildingNumber;

            ViewBag.BuildingNumber = ClassSessionMV.BuildingNumber;

            //ClassSessionMV.LastBuildingNumber = ClassSessionMV.BuildingNumber;

            GetAllLists(ClassSessionMV);

            ClassSessionMV.ShowForInput = true;
            ClassSessionMV.ShowInstructorSchedule = false;
            ClassSessionMV.ShowRoomSchedule = false;
            ClassSessionMV.ShowScreenMessage = false;
            if (ClassSessionMV.InstructorID < 1) ClassSessionMV.ShowForInput = false;
            if (ClassSessionMV.ClassRoomID < 1) ClassSessionMV.ShowForInput = false;
            if (ClassSessionMV.CourseID < 1) ClassSessionMV.ShowForInput = false;
            if (ClassSessionMV.WeekDay == "NO SELECTED VALUE") ClassSessionMV.ShowForInput = false;

            ClassSessionMV.LastBuildingNumber = BuildingNumber;
            ClassSessionMV.InstructorSessions = ReturnBlankWorkingSchedule();
            ClassSessionMV.ClassRoomSessions = ReturnBlankWorkingSchedule();

            if (ClassSessionMV.ShowForInput == true)
            {
                string CallSequence = _client.BaseAddress + "/ClassSession/" + InstructorID.ToString()
                    + "/" + WeekDay + "/Instructor";
                HttpResponseMessage InstructorSchedule = _client.GetAsync(CallSequence).Result;

                if (InstructorSchedule.IsSuccessStatusCode)
                {
                    var dataList = InstructorSchedule.Content.ReadAsAsync<IList<WorkingDailySchedule>>();
                    //InstructorSessions = dataList.Result.ToList();
                    InstructorSessions.Clear();

                    foreach (var item in dataList.Result)
                    {
                        WorkingDailySchedule REC = new WorkingDailySchedule();
                        REC.ClassSessionID = item.ClassSessionID;
                        REC.InstructorID = item.InstructorID;
                        REC.RoomNumber = item.RoomNumber;
                        REC.BuildingNumber = item.BuildingNumber;
                        REC.FirstName = item.FirstName;
                        REC.MiddleName = item.MiddleName;
                        REC.LastName = item.LastName;
                        REC.Title = item.Title;
                        REC.StartTime = item.StartTime;
                        REC.EndTime = item.EndTime;
                        InstructorSessions.Add(REC);
                    }

                    ClassSessionMV.InstructorSessions = InstructorSessions;

                    //----------------------------------------------------------------------------------------
                    //Class Room Usage
                    List<WorkingDailySchedule> ClassRoomSessions = new List<WorkingDailySchedule>();
                    //ClassRoomSessions.Clear();

                    string CallSequence2 = _client.BaseAddress + "/ClassSession/" + ClassRoomID.ToString()
                        + "/" + WeekDay + "/ClassRoom";
                    HttpResponseMessage ClassRoomSchedule = _client.GetAsync(CallSequence2).Result;

                    if (ClassRoomSchedule.IsSuccessStatusCode)
                    {
                        var dataList2 = ClassRoomSchedule.Content.ReadAsAsync<IList<WorkingDailySchedule>>();
                        //InstructorSessions = dataList.Result.ToList();
                        ClassRoomSessions.Clear();

                        foreach (var item in dataList2.Result)
                        {
                            WorkingDailySchedule REC = new WorkingDailySchedule();
                            REC.ClassSessionID = item.ClassSessionID;
                            REC.InstructorID = item.InstructorID;
                            REC.RoomNumber = item.RoomNumber;
                            REC.BuildingNumber = item.BuildingNumber;
                            REC.FirstName = item.FirstName;
                            REC.MiddleName = item.MiddleName;
                            REC.LastName = item.LastName;
                            REC.Title = item.Title;
                            REC.StartTime = item.StartTime;
                            REC.EndTime = item.EndTime;
                            ClassRoomSessions.Add(REC);
                        }

                        ClassSessionMV.ClassRoomSessions = ClassRoomSessions;
                        ClassSessionMV.ShowInstructorSchedule = true;
                    }
                }

                ClassSessionMV.ErrorCondition = false;

                if (ClassSessionMV.ScreenMessage == null || ClassSessionMV.ScreenMessage.Length == 0)
                {
                    ClassSessionMV.ShowScreenMessage = false;
                }
            }
        }




























        private StudentSessionMVMaint GetAllStudentLists(StudentSessionMVMaint obj)
        {
            List<Student> StudentList = new List<Student>();
            List<Course> CourseList = new List<Course>();
            StudentList.Clear();
            CourseList.Clear();

            HttpResponseMessage response = null;
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

            response = _client.GetAsync(_client.BaseAddress + "/Course").Result;

            if (response.IsSuccessStatusCode)
            {
                var dataCourse = response.Content.ReadAsAsync<IList<Course>>();
                CourseList = dataCourse.Result.ToList();
                List<SelectListItem> CourseSelectList = new List<SelectListItem>();

                CourseSelectList.Add(new SelectListItem { Text = "Select Course", Value = "0" });

                foreach (var item in CourseList)
                {
                    CourseSelectList.Add(new SelectListItem { Text = item.Title, Value = item.CourseID.ToString() });
                }
                obj.CourseSelectList = CourseSelectList;
            }

            return obj;
        }


        private ClassSessionMVMaint GetAllLists(ClassSessionMVMaint obj)
        {
            //ClassSessionMVMaint obj = new ClassSessionMVMaint();
            List<Instructor> InstructorList = new List<Instructor>();
            List<ClassRoom> ClassRoomList = new List<ClassRoom>();
            List<Course> CourseList = new List<Course>();
            List<SelectListItem> WeekDayList = new List<SelectListItem>();

            InstructorList.Clear();
            ClassRoomList.Clear();
            CourseList.Clear();

            //InstructorMVMaint InstructorMV = new InstructorMVMaint();
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

            response = _client.GetAsync(_client.BaseAddress + "/ClassRoom").Result;

            if (response.IsSuccessStatusCode)
            {
                var dataClassRoom = response.Content.ReadAsAsync<IList<ClassRoom>>();
                ClassRoomList = dataClassRoom.Result.ToList()
            .OrderBy(x => x.BuildingNumber)
            .ThenBy(x => x.RoomNumber).ToList();

                List<SelectListItem> ClassRoomSelectList = new List<SelectListItem>();
                List<SelectListItem> BuildingSelectList = new List<SelectListItem>();
                List<SelectListItem> RoomSelectList = new List<SelectListItem>();

                foreach (var item in ClassRoomList)
                {
                    string ClassRoomName = "Building " + item.BuildingNumber.ToString();
                    ClassRoomName += ", Room " + item.RoomNumber.ToString();
                    ClassRoomSelectList.Add(new SelectListItem { Text = ClassRoomName, Value = item.ClassRoomID.ToString() });
                }
                obj.ClassRoomSelectList = ClassRoomSelectList;

                int BuildingNumberSave = -1;
                foreach (var item in ClassRoomList)
                {
                    if (item.BuildingNumber != BuildingNumberSave)
                    {
                        string BuildingName = "Building " + item.BuildingNumber.ToString();
                        BuildingSelectList.Add(new SelectListItem { Text = BuildingName, Value = item.BuildingNumber.ToString() });
                        BuildingNumberSave = (item.BuildingNumber == null ? 0 : (int)item.BuildingNumber);
                    }
                }
                obj.BuildingSelectList = BuildingSelectList;

                if (obj.BuildingNumber != null && obj.BuildingNumber > 0)
                {
                    foreach (var item in ClassRoomList)
                    {
                        if (item.BuildingNumber == obj.BuildingNumber)
                        {
                            string RoomName = "Room " + item.RoomNumber.ToString();
                            RoomSelectList.Add(new SelectListItem { Text = RoomName, Value = item.ClassRoomID.ToString() });
                        }
                    }
                    obj.RoomSelectList = RoomSelectList;
                }

            }

            response = _client.GetAsync(_client.BaseAddress + "/Course").Result;

            if (response.IsSuccessStatusCode)
            {
                var dataCourse = response.Content.ReadAsAsync<IList<Course>>();
                CourseList = dataCourse.Result.ToList();
                List<SelectListItem> CourseSelectList = new List<SelectListItem>();

                foreach (var item in CourseList)
                {
                    CourseSelectList.Add(new SelectListItem { Text = item.Title, Value = item.CourseID.ToString() });
                }
                obj.CourseSelectList = CourseSelectList;
            }

            WeekDayList.Clear();
            WeekDayList.Add(new SelectListItem { Text = "Monday", Value = "Monday" });
            WeekDayList.Add(new SelectListItem { Text = "Tuesday", Value = "Tuesday" });
            WeekDayList.Add(new SelectListItem { Text = "Wednesday", Value = "Wednesday" });
            WeekDayList.Add(new SelectListItem { Text = "Thursday", Value = "Thursday" });
            WeekDayList.Add(new SelectListItem { Text = "Friday", Value = "Friday" });

            obj.WeekDaySelectList = WeekDayList;
            return obj;
        }
    }
}
