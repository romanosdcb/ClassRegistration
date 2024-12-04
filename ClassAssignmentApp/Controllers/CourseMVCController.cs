using Microsoft.AspNetCore.Mvc;
using ClassAssignmentApp.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http.Json;
using ClassAssignmentApp.Data;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace ClassAssignmentApp.Controllers
{
    public class CourseMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");

        private readonly HttpClient _client;

        public CourseMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(int? Id, string? SelectedDepartment)
        {
            CourseMVMaint CourseMV = new CourseMVMaint();
            List<Course> courseList = new List<Course>();
            CourseData CourseDataOBJ = new CourseData();

            if (TempData["CourseREC"] != null)
            {
                string MyData = TempData["CourseREC"].ToString();
                CourseData CDobj = new CourseData();
                CDobj = JsonConvert.DeserializeObject<CourseData>(MyData);
                CourseDataOBJ.CourseID = CDobj.CourseID;
                CourseDataOBJ.SelectedDepartment = CDobj.SelectedDepartment;
                CourseDataOBJ.OperationMode = CDobj.OperationMode;
                CourseDataOBJ.ErrorMessage = CDobj.ErrorMessage;
                CourseDataOBJ.ErrorCondition = CDobj.ErrorCondition;
                CourseDataOBJ.Title = CDobj.Title;
                CourseDataOBJ.Department = CDobj.Department;
                CourseDataOBJ.CreditHours = CDobj.CreditHours;
                CourseDataOBJ.PrerequisiteCourseID = CDobj.PrerequisiteCourseID;
                CourseMV.OperationMode = CourseDataOBJ.OperationMode;
                CourseMV.CourseID = CDobj.CourseID;
            }
            else
            {
                CourseMV.CourseID = 0;
                CourseDataOBJ.ErrorMessage = "";
                CourseDataOBJ.ErrorCondition = false;
            }

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/Course").Result;
            CourseMV.Title = "";
            CourseMV.Department = "";
            CourseMV.CreditHours = 0;
            CourseMV.PrerequisiteCourseID = 0;

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IList<Course>>();
                CourseMV.CourseList = data.Result.ToList();

                if (SelectedDepartment != null && SelectedDepartment != "")
                {
                    var SelectedCourseData = data.Result.Where(x => x.Department == SelectedDepartment);
                    CourseMV.CourseList = SelectedCourseData.ToList();
                }
             }

            if (CourseDataOBJ != null && CourseDataOBJ.ErrorMessage != null && CourseDataOBJ.ErrorMessage.Length > 0)
            {
                CourseMV.ErrorMessage = CourseDataOBJ.ErrorMessage;
                CourseMV.ErrorCondition = true;
                CourseMV.Title = CourseDataOBJ.Title;
                CourseMV.Department = CourseDataOBJ.Department;
                CourseMV.CreditHours = CourseDataOBJ.CreditHours;
                CourseMV.PrerequisiteCourseID = CourseDataOBJ.PrerequisiteCourseID;
                CourseMV.ShowDetailsForm = true;
                return View(CourseMV);
            }
            else
                CourseMV.ErrorCondition = false;

            if (Id == null || Id == 0)
            {
                CourseMV.OperationMode = "LIST";
                CourseMV.DepatmentSelection = "";
                CourseMV.ShowDetailsForm = false;
            }
            else
            {
                if (Id == -1)
                {
                    CourseMV.ShowDetailsForm = true;
                    CourseMV.OperationMode = "ADD";
                }
                else
                {
                    HttpResponseMessage editRecord = _client.GetAsync(_client.BaseAddress + "/Course/" + Id.ToString()).Result;

                    if (editRecord.IsSuccessStatusCode)
                    {
                        var course = (Course?)editRecord.Content.ReadAsAsync<Course>().Result;

                        if (!(course == null))
                        {
                            CourseMV.CourseID = course.CourseID;
                            CourseMV.Title = course.Title;
                            CourseMV.Department = course.Department;
                            CourseMV.CreditHours = course.CreditHours;
                            CourseMV.PrerequisiteCourseID = (course.PrerequisiteCourseID == null ? 0 : course.PrerequisiteCourseID);
                            CourseMV.ShowDetailsForm = true;
                            CourseMV.OperationMode = "CHANGEDELETE";
                        }
                    }
                }
            }

            return View(CourseMV);
        }

        [HttpGet]
        public IActionResult Create(CourseMVMaint obj)
        {
            obj.ShowDetailsForm = true;
            obj.CourseID = 0;
            obj.Title = "";
            obj.Department = "";
            obj.CreditHours = 0;
            obj.PrerequisiteCourseID = 0;
            CourseMVMaint ModelObj = new CourseMVMaint();
            ModelObj.ShowDetailsForm = true;
            return View(obj);
        }

        [HttpPost]
        public IActionResult Create(Course? obj)
        {
            if (obj == null || obj.CourseID == 0 || obj.Title == "" || obj.Department == "" || obj.CreditHours == 0)
            {
                ModelState.AddModelError("Course", "Invalid data");
            }

            if (ModelState.IsValid)
            {
                //_db.DiaryEntries.Add(obj);
                //_db.SaveChanges();
                HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/Course/", obj).Result;

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View(obj);
            }
        }

        private CourseData LoadCourseDataOBJ(Course obj, string sOperationMode)
        {
            bool ErrorCondition = false;
            int ErrorCount = 0;
            CourseData DataOBJ = new CourseData();
            DataOBJ.CourseID = obj.CourseID;
            DataOBJ.ErrorMessage = "Please Enter Required Fields: ";
            DataOBJ.ErrorCondition = true;
            DataOBJ.Title = (obj.Title == null ? string.Empty : obj.Title);
            DataOBJ.Department = (obj.Department == null ? string.Empty : obj.Department);
            DataOBJ.CreditHours = (obj.CreditHours == null ? 0 : (int)obj.CreditHours);
            DataOBJ.PrerequisiteCourseID = (obj.PrerequisiteCourseID == null ? 0 : (int)obj.PrerequisiteCourseID);
            DataOBJ.OperationMode = sOperationMode;

            if (obj.Title == null || obj.Title == "")
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Title";
                else
                    DataOBJ.ErrorMessage += ", Title";    //Plus sign characters (+) will be replaced by comma (,)
            }

            if (obj.Department == null || obj.Department == "")
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Department";
                else
                    DataOBJ.ErrorMessage += ", Department";
            }

            if (obj.CreditHours == null || obj.CreditHours == 0)
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Credit Hours";
                else
                    DataOBJ.ErrorMessage += ", Credit Hours";
            }

            if (obj.Title != null && obj.Title.Length < 5)
            {
                DataOBJ.ErrorMessage = "Title must be at least 5 characters long!";
                DataOBJ.ErrorCondition = false;
            }

            return DataOBJ;
        }

        [HttpPost]
        public IActionResult FileUpdate(int id, CourseMVMaint MVobj, string command)
        {
            bool ErrorCondition = false;
            Course obj = new Course();
            CourseData DataOBJ = new CourseData();
            if (MVobj.CourseID != null) obj.CourseID = MVobj.CourseID;
            obj.Title = MVobj.Title;
            obj.Department = MVobj.Department;
            obj.CreditHours = (MVobj.CreditHours == null? 0 : (int)MVobj.CreditHours);
            obj.PrerequisiteCourseID = MVobj.PrerequisiteCourseID;
            MVobj.ErrorCondition = false;
            if (obj.Title == null || obj.Title == string.Empty || obj.Title.Length < 5) ErrorCondition = true;
            if (obj.Department == null || obj.Department == string.Empty) ErrorCondition = true;
            if (obj.CreditHours == null || obj.CreditHours == 0) ErrorCondition = true;
            int ErrorCount = 0;

            switch (command)
            {
                case "Create":
                    if (ErrorCondition)
                    {
                        DataOBJ = LoadCourseDataOBJ(obj, "ADD");
                        string SER = JsonConvert.SerializeObject(DataOBJ);  //Conversion tool uses "," to separate fields and ":" to separate field/value pairs
                        TempData["CourseREC"] = SER; // Store in session value
                        return RedirectToAction("Index", new { Id = 0, SelectedDepartment = "", pCourseDataOBJ = DataOBJ });
                    }

                    if (ModelState.IsValid)
                    {
                        //_db.DiaryEntries.Add(obj);
                        //_db.SaveChanges();
                        HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/Course/", obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return View(MVobj);
                    }

                case "Edit":
                    if (ErrorCondition)
                    {
                        ModelState.AddModelError("Course", "Invalid data");
                        DataOBJ = LoadCourseDataOBJ(obj, "CHANGEDELETE");
                        string SER = JsonConvert.SerializeObject(DataOBJ);
                        TempData["CourseREC"] = SER; // Store in session value
                        return RedirectToAction("Index", new { Id = DataOBJ.CourseID, SelectedDepartment = "", pCourseDataOBJ = DataOBJ });
                    }

                    if (ModelState.IsValid)
                    {
                        if (obj.CourseID < 1) obj.CourseID = MVobj.CourseID;
                        HttpResponseMessage response = _client.PutAsJsonAsync(_client.BaseAddress + "/Course/" + obj.CourseID.ToString(), obj).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return View(MVobj);
                    }

                case "Delete":
                    if (ModelState.IsValid && obj.CourseID > 0)
                    {
                        //_db.DiaryEntries.Remove(obj);
                        //_db.SaveChanges();
                        //HttpResponseMessage response = _client.PutAsJsonAsync(_client.BaseAddress + "/DiaryEntries/" + id.ToString(), obj).Result;
                        HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/Course/" + obj.CourseID.ToString()).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                    else
                    {
                        return View(MVobj);
                    }

                case "Redisplay":
                    return RedirectToAction("Index", new { id = 0, SelectedDepartment = MVobj.SelectedDepartment });

                case "Cancel":
                    MVobj.Title = "1";
                    MVobj.Department = "1";
                    MVobj.CreditHours = 1;
                    MVobj.PrerequisiteCourseID = 1;
                    return RedirectToAction("Index");

                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Delete(CourseMVMaint obj)
        {
            return View();
        }
    }
}
