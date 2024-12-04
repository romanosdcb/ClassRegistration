using ClassAssignmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassAssignmentApp.Controllers
{
    public class StudentMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api"); 
        private readonly HttpClient _client;

        public StudentMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            StudentMVMaint StudentMV = new StudentMVMaint();

            StudentMV = CommonIndex("");

            return View(StudentMV);
        }

        [HttpPost]
        public IActionResult Index(string? LastNameSelection)
        {
            string SelectBy = (LastNameSelection == null ? "" : LastNameSelection);
            StudentMVMaint StudentMV = CommonIndex(SelectBy);

            return View(StudentMV);
        }

        private StudentMVMaint CommonIndex(string? LastNameSelection)
        {
            StudentMVMaint StudentMV = new StudentMVMaint();
            List<Student> StudentList = new List<Student>();


            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Student").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IList<Student>>();
                StudentMV.StudentList = data.Result.ToList();

                if (LastNameSelection != null && LastNameSelection != "")
                {
                    var SelectedStudentData = data.Result.Where(x => x.LastName.Contains(LastNameSelection));
                    StudentMV.StudentList = SelectedStudentData.ToList();
                }
            }

            return StudentMV;
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            StudentMVMaint StudentMV = new StudentMVMaint();

            if (id == null || id == 0)
            {
                //StudentMV.StudentID = Student.StudentID;
                StudentMV.FirstName = "";
                StudentMV.MiddleName = "";
                StudentMV.LastName = "";
                StudentMV.ClassRank = 0;
                StudentMV.PhoneNumber = "";
                StudentMV.AddressLine1 = "";
                StudentMV.AddressLine2 = "";
                StudentMV.City = "";
                StudentMV.State = "";
                StudentMV.ZipCode = "";
                StudentMV.OperationMode = "ADD";

                List<StateREC> stateList = FillStateList();

                StudentMV.StateSelectList = new List<SelectListItem>();

                foreach (var state in stateList)
                {
                    StudentMV.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                }

                return View(StudentMV);
            }

            HttpResponseMessage editRecord = _client.GetAsync(_client.BaseAddress + "/Student/" + id.ToString()).Result;

            if (editRecord.IsSuccessStatusCode)
            {
                var Student = (Student?)editRecord.Content.ReadAsAsync<Student>().Result;

                if (!(Student == null))
                {
                    StudentMV.StudentID = Student.StudentID;
                    StudentMV.FirstName = Student.FirstName;
                    StudentMV.MiddleName = Student.MiddleName;
                    StudentMV.LastName = Student.LastName;
                    StudentMV.ClassRank = Student.ClassRank;
                    StudentMV.PhoneNumber = Student.PhoneNumber;
                    StudentMV.AddressLine1 = Student.AddressLine1;
                    StudentMV.AddressLine2 = Student.AddressLine2;
                    StudentMV.City = Student.City;
                    StudentMV.State = Student.State;
                    StudentMV.ZipCode = Student.ZipCode;
                    StudentMV.OperationMode = "CHANGEDELETE";

                    //var dbStateList = new List<StateREC>();
                    //var states = dbStateList.Select(x => new SelectListItem
                    //{
                    //    Value = x.StateID.ToString(),
                    //    Text = x.StateAbbrev
                    //});

                    //obj.StateList = dbStateList;

                    //StudentMV.StateList = stateList;
                    //var collection = StudentMV.StateList.Select(a => new SelectListItem() { Text = a.StateAbbrev,Value = a.StateID});
                    //StudentMV.StateSelectList = (SelectList?)collection;

                    List<StateREC> stateList = FillStateList();
                    StudentMV.StateSelectList = new List<SelectListItem>();

                    foreach (var state in stateList)
                    {
                        StudentMV.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                    }
                }
            }

            return View(StudentMV);
        }

        private List<StateREC> FillStateList()
        {
            List<StateREC> stateList = new List<StateREC>();

            stateList.Clear();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/UnitedState").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IList<StateREC>>();
                stateList = data.Result.ToList();
            }

            return stateList;
        }

        [HttpPost]
        public IActionResult Edit(StudentMVMaint obj, string command)
        {
            bool ValidRecord = true;

            //var dbStateList = new List<StateREC>();
            //var states = dbStateList.Select(x => new SelectListItem
            //{
            //    Value = x.StateID.ToString(),
            //    Text = x.StateAbbrev
            //});

            //obj.StateList = dbStateList;
            //obj.StateList = stateList;

            //List<SelectListItem> CurrentList = new List<SelectListItem>();
            //if (obj.StateSelected != null)
            //{
            //    CurrentList = obj.StateSelected;
            //}


            //CurrentList = obj.StateSelected;

            //if (obj.StateSelected != null )
            //{
            //    SelectListItem SLI = new SelectListItem();
            //    SLI.Text = "T";
            //    SLI.Value = "V";
            //    List<SelectListItem> EmptyList = new List<SelectListItem>();
            //    EmptyList.Add( SLI );
            //    obj.StateSelected = EmptyList;
            //}

            //if (command == null)
            //{
            //    command = (obj.State == null ? "AA" : obj.State);
            //}

            if (command == "Create")
                obj.OperationMode = "ADD";
            else
                obj.OperationMode = "CHANGEDELETE";

            if (obj == null || obj.ClassRank == null || obj.ClassRank < 1 || obj.ClassRank > 4)
            {
                ModelState.AddModelError("Class Rank", "Class Rank: 1 to 4");
                ValidRecord = false;

            }

            if (obj.State != null && obj.State.Length > 2)
            {
                obj.State = null;
            }

            if (ModelState.IsValid && ValidRecord)
            {
                //_db.DiaryEntries.Add(obj);
                //_db.SaveChanges();
                HttpResponseMessage response;

                switch (command)
                {
                    case "Create":
                        response = _client.PostAsJsonAsync(_client.BaseAddress + "/Student/", obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }

                    case "Edit":
                        response = _client.PutAsJsonAsync(_client.BaseAddress + "/Student/" + obj.StudentID.ToString(), obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }


                    case "Delete":
                        response = _client.DeleteAsync(_client.BaseAddress + "/Student/" + obj.StudentID.ToString()).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }

                    case "Cancel":
                        return RedirectToAction("Index");

                    case "DropDown":
                        return View(obj);

                    default:
                        //break;
                        List<StateREC> stateList = FillStateList();
                        obj.StateSelectList = new List<SelectListItem>();

                        foreach (var state in stateList)
                        {
                            obj.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                        }

                        obj.State = command;
                        command = null;
                        return View(obj);
                }

                return NotFound();

            }
            else
            {
                //if (command == null)
                //{
                //    command = "DropDown";
                //    RedirectToAction("Index");
                //}
                return View(obj);

            }
        }
    }
}
