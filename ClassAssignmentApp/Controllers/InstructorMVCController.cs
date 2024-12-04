using Azure;
using ClassAssignmentApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassAssignmentApp.Controllers
{
    public class InstructorMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");

        private readonly HttpClient _client;

        public InstructorMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            InstructorMVMaint InstructorMV = new InstructorMVMaint();

            InstructorMV = CommonIndex("");

            return View(InstructorMV);
        }

        [HttpPost]
        public IActionResult Index(string? LastNameSelection)
        {
            string SelectBy = (LastNameSelection == null ? "" :  LastNameSelection);
            InstructorMVMaint InstructorMV = CommonIndex(SelectBy);

            return View(InstructorMV);
        }

        private InstructorMVMaint CommonIndex(string? LastNameSelection)
        {
            InstructorMVMaint InstructorMV = new InstructorMVMaint();
            List<Instructor> InstructorList = new List<Instructor>();


            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Instructor").Result;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IList<Instructor>>();
                InstructorMV.InstructorList = data.Result.ToList();

                if (LastNameSelection != null && LastNameSelection != "")
                {
                    var SelectedInstructorData = data.Result.Where(x => x.LastName.Contains(LastNameSelection));
                    InstructorMV.InstructorList = SelectedInstructorData.ToList();
                }
            }

            return InstructorMV;
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
 

            //DiaryEntry? diaryEntry = _db.DiaryEntries.Find(id);

            //if (diaryEntry == null)
            //{
            //    return NotFound();
            //}

            InstructorMVMaint InstructorMV = new InstructorMVMaint();

            if (id == null || id == 0)
            {
                //InstructorMV.InstructorID = instructor.InstructorID;
                InstructorMV.FirstName = "";
                InstructorMV.MiddleName = "";
                InstructorMV.LastName = "";
                InstructorMV.Title = "";
                InstructorMV.PhoneNumber = "";
                InstructorMV.AddressLine1 = "";
                InstructorMV.AddressLine2 = "";
                InstructorMV.City = "";
                InstructorMV.State = "";
                InstructorMV.ZipCode = "";
                InstructorMV.OperationMode = "ADD";

                List<StateREC> stateList = FillStateList();

                //var stateData = stateList;
                InstructorMV.StateSelectList = new List<SelectListItem>();

                foreach (var state in stateList)
                {
                    InstructorMV.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                }

                return View(InstructorMV);
            }

            HttpResponseMessage editRecord = _client.GetAsync(_client.BaseAddress + "/Instructor/" + id.ToString()).Result;

            if (editRecord.IsSuccessStatusCode)
            {
                var instructor = (Instructor?)editRecord.Content.ReadAsAsync<Instructor>().Result;

                if (!(instructor == null))
                {
                    InstructorMV.InstructorID = instructor.InstructorID;
                    InstructorMV.FirstName = instructor.FirstName;
                    InstructorMV.MiddleName = instructor.MiddleName;
                    InstructorMV.LastName = instructor.LastName;
                    InstructorMV.Title = instructor.Title;
                    InstructorMV.PhoneNumber = instructor.PhoneNumber;
                    InstructorMV.AddressLine1 = instructor.AddressLine1;
                    InstructorMV.AddressLine2 = instructor.AddressLine2;
                    InstructorMV.City = instructor.City;
                    InstructorMV.State = instructor.State;
                    InstructorMV.ZipCode = instructor.ZipCode;
                    InstructorMV.OperationMode = "CHANGEDELETE";

                    //var dbStateList = new List<StateREC>();
                    //var states = dbStateList.Select(x => new SelectListItem
                    //{
                    //    Value = x.StateID.ToString(),
                    //    Text = x.StateAbbrev
                    //});

                    //obj.StateList = dbStateList;

                    //InstructorMV.StateList = stateList;
                    //var collection = InstructorMV.StateList.Select(a => new SelectListItem() { Text = a.StateAbbrev,Value = a.StateID});
                    //InstructorMV.StateSelectList = (SelectList?)collection;

                    List<StateREC> stateList = FillStateList();
                    InstructorMV.StateSelectList = new List<SelectListItem>();

                    foreach (var state in stateList)
                    {
                        InstructorMV.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                    }
                }
            }

            return View(InstructorMV);
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
        public IActionResult Edit(InstructorMVMaint obj, string command)
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

            if (command == "Create")
                obj.OperationMode = "ADD";
            else
                obj.OperationMode = "CHANGEDELETE";

            if (obj == null || obj.Title == null || obj.Title.Length < 3)
            {
                ModelState.AddModelError("Title", "Title too short");
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
                        response = _client.PostAsJsonAsync(_client.BaseAddress + "/Instructor/", obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }

                    case "Edit":
                        response = _client.PutAsJsonAsync(_client.BaseAddress + "/Instructor/" + obj.InstructorID.ToString(), obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }


                    case "Delete":
                        response = _client.DeleteAsync(_client.BaseAddress + "/Instructor/" + obj.InstructorID.ToString()).Result;

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

                    default:
                        List<StateREC> stateList = FillStateList();
                        obj.StateSelectList = new List<SelectListItem>();

                        foreach (var state in stateList)
                        {
                            obj.StateSelectList.Add(new SelectListItem { Text = state.StateName, Value = state.StateAbbreviation });
                        }

                        obj.State = command;
                        command = null;
                        return View(obj);



                        //break;
                }

                return NotFound();

            }
            else
            {
                return View(obj);
            }
        }
    }
}
