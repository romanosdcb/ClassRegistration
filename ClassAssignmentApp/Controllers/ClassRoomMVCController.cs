using Microsoft.AspNetCore.Mvc;
using ClassAssignmentApp.Models;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http.Json;
using ClassAssignmentApp.Data;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

namespace ClassAssignmentApp.Controllers
{
    public class ClassRoomMVCController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7008/api");
        private readonly HttpClient _client;
        
        public ClassRoomMVCController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index(int? Id, int? SelectedBuildingID)
        {
            ClassRoomMVMaint ClassRoomMV = new ClassRoomMVMaint();
            List<ClassRoom> classRoomList = new List<ClassRoom>();
            ClassRoomData ClassRoomDataOBJ = new ClassRoomData();

            if (TempData["ClassRoomREC"] != null)
            {
                string MyData = TempData["ClassRoomREC"].ToString();
                ClassRoomData CDobj = new ClassRoomData();
                CDobj = JsonConvert.DeserializeObject<ClassRoomData>(MyData);
                ClassRoomDataOBJ.ClassRoomID = CDobj.ClassRoomID;
                ClassRoomDataOBJ.SelectedBuildingNo = CDobj.SelectedBuildingNo;
                ClassRoomDataOBJ.ErrorMessage = CDobj.ErrorMessage;
                ClassRoomDataOBJ.ErrorMessage = CDobj.ErrorMessage;
                ClassRoomDataOBJ.BuildingNumber = CDobj.BuildingNumber;
                ClassRoomDataOBJ.RoomNumber = CDobj.RoomNumber;
                ClassRoomDataOBJ.Capacity = CDobj.Capacity;
                ClassRoomDataOBJ.Unavailable = CDobj.Unavailable;
                ClassRoomDataOBJ.Unavail = CDobj.Unavail;
                ClassRoomMV.OperationMode = CDobj.OperationMode;
                ClassRoomMV.ClassRoomID = CDobj.ClassRoomID;
            }
            else
            {
                ClassRoomMV.ClassRoomID = 0;
                ClassRoomDataOBJ.ErrorMessage = "";
                ClassRoomDataOBJ.ErrorCondition = false;
            }

            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/ClassRoom").Result;
            ClassRoomMV.BuildingNumber = 0;
            ClassRoomMV.RoomNumber = 0;
            ClassRoomMV.Capacity = 0;
            ClassRoomMV.Unavailable = "";

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsAsync<IList<ClassRoom>>();
                ClassRoomMV.ClassRoomList = data.Result.ToList();

                if (SelectedBuildingID != null && SelectedBuildingID > 0)
                {
                    var SelectedBuildingData = data.Result.Where(x => x.BuildingNumber == SelectedBuildingID);
                    ClassRoomMV.ClassRoomList = SelectedBuildingData.ToList();
                }
            }

            if (ClassRoomDataOBJ != null && ClassRoomDataOBJ.ErrorMessage != null && ClassRoomDataOBJ.ErrorMessage.Length > 0)
            {
                ClassRoomMV.ErrorMessage = ClassRoomDataOBJ.ErrorMessage;
                ClassRoomMV.ErrorCondition = true;
                ClassRoomMV.RoomNumber = ClassRoomDataOBJ.RoomNumber;
                ClassRoomMV.BuildingNumber = ClassRoomDataOBJ.BuildingNumber;
                ClassRoomMV.Capacity = ClassRoomDataOBJ.Capacity;
                ClassRoomMV.Unavailable = ClassRoomDataOBJ.Unavailable;
                ClassRoomMV.ShowDetailsForm = true;
                return View(ClassRoomMV);
            }
            else
                ClassRoomMV.ErrorCondition = false;

            if (Id == null || Id == 0)
            {
                ClassRoomMV.OperationMode = "LIST";
                ClassRoomMV.BuildingNumberSelection = 0;
                ClassRoomMV.ShowDetailsForm = false;
            }
            else
            {
                if (Id == -1)
                {
                    ClassRoomMV.ShowDetailsForm = true;
                    ClassRoomMV.OperationMode = "ADD";
                }
                else
                {
                    HttpResponseMessage editRecord = _client.GetAsync(_client.BaseAddress + "/ClassRoom/" + Id.ToString()).Result;

                    if (editRecord.IsSuccessStatusCode)
                    {
                        var classRoom = (ClassRoom?)editRecord.Content.ReadAsAsync<ClassRoom>().Result;

                        if (!(classRoom == null))
                        {
                            ClassRoomMV.ClassRoomID = classRoom.ClassRoomID;
                            ClassRoomMV.BuildingNumber = classRoom.BuildingNumber;
                            ClassRoomMV.RoomNumber = classRoom.RoomNumber;
                            ClassRoomMV.Capacity = classRoom.Capacity;
                            ClassRoomMV.Unavailable = (classRoom.Unavailable == null ? "" : classRoom.Unavailable);
                            ClassRoomMV.Unavail = (classRoom.Unavailable == "T" ? true : false);
                            ClassRoomMV.ShowDetailsForm = true;
                            ClassRoomMV.OperationMode = "CHANGEDELETE";
                        }
                    }
                }
            }

            return View(ClassRoomMV);
        }

        [HttpGet]
        public IActionResult Create(ClassRoomMVMaint obj)
        {
            obj.ShowDetailsForm = true;
            obj.ClassRoomID = 0;
            obj.BuildingNumber = 0;
            obj.RoomNumber = 0;
            obj.Capacity = 0;
            obj.Unavailable = "F";

            ClassRoomMVMaint ModelObj = new ClassRoomMVMaint();
            ModelObj.ShowDetailsForm = true;
            return View(obj);
        }

        [HttpPost]
        public IActionResult Create(ClassRoom? obj)
        {
            if (obj == null || obj.ClassRoomID == 0 || obj.BuildingNumber == 0 || obj.Capacity == 0)
            {
                ModelState.AddModelError("Class Room", "Invalid data");
            }

            if (ModelState.IsValid)
            {
                //_db.DiaryEntries.Add(obj);
                //_db.SaveChanges();
                HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/ClassRoom/", obj).Result;

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

        private ClassRoomData LoadClassRoomDataOBJ(ClassRoom obj, string sOperationMode)
        {
            bool ErrorCondition = false;
            int ErrorCount = 0;
            ClassRoomData DataOBJ = new ClassRoomData();
            DataOBJ.ClassRoomID = obj.ClassRoomID;
            DataOBJ.ErrorMessage = "Please Enter Required Fields: ";
            DataOBJ.ErrorCondition = true;
            DataOBJ.BuildingNumber = (obj.BuildingNumber == null ? 0 : (int)obj.BuildingNumber);
            DataOBJ.RoomNumber = (obj.RoomNumber == null ? 0 : (int)obj.RoomNumber);
            DataOBJ.Capacity = (obj.Capacity == null ? 0 : (int)obj.Capacity);
            DataOBJ.Unavailable = (obj.Unavailable == null ? "F" : obj.Unavailable);
            DataOBJ.OperationMode = sOperationMode;

            if (obj.BuildingNumber == null || obj.BuildingNumber == 0)
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Building Number";
                else
                    DataOBJ.ErrorMessage += ", Building Number";    //Plus sign characters (+) will be replaced by comma (,)
            }

            if (obj.RoomNumber == null || obj.RoomNumber == 0)
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Room Number";
                else
                    DataOBJ.ErrorMessage += ", Room Number";
            }

            if (obj.Capacity == null || obj.Capacity == 0)
            {
                ErrorCount++;
                if (ErrorCount < 2)
                    DataOBJ.ErrorMessage += "Max Capacity";
                else
                    DataOBJ.ErrorMessage += ", Max Capacity";
            }

            if (obj.BuildingNumber != null && obj.BuildingNumber > 500)
            {
                DataOBJ.ErrorMessage = "Building Number cannot be greater than 500!";
                DataOBJ.ErrorCondition = false;
            }

            return DataOBJ;
        }

        [HttpPost]
        public IActionResult FileUpdate(int id, ClassRoomMVMaint MVobj, string command)
        {
            bool ErrorCondition = false;
            ClassRoom obj = new ClassRoom();
            ClassRoomData DataOBJ = new ClassRoomData();

            if (MVobj.ClassRoomID != null) obj.ClassRoomID = MVobj.ClassRoomID;

            obj.BuildingNumber = MVobj.BuildingNumber;
            obj.RoomNumber = MVobj.RoomNumber;
            obj.Capacity = MVobj.Capacity;
            MVobj.ErrorCondition = false;

            if (MVobj.Unavail == true)
                obj.Unavailable = "T";
            else
                obj.Unavailable = "F";

            if (obj.BuildingNumber == null || obj.BuildingNumber == 0 || obj.BuildingNumber > 500) ErrorCondition = true;
            if (obj.RoomNumber == null || obj.RoomNumber == 0) ErrorCondition = true;
            if (obj.Capacity == null || obj.Capacity == 0) ErrorCondition = true;

            int ErrorCount = 0;

            switch (command)
            {
                case "Create":
                    if (ErrorCondition)
                    {
                        DataOBJ = LoadClassRoomDataOBJ(obj, "ADD");
                        string SER = JsonConvert.SerializeObject(DataOBJ);  //Conversion tool uses "," to separate fields and ":" to separate field/value pairs
                        TempData["ClassRoomREC"] = SER; // Store in session value
                        return RedirectToAction("Index", new { Id = 0, SelectedBuildingID = 0, pClassRoomDataOBJ = DataOBJ });
                    }

                    if (ModelState.IsValid)
                    {
                        //_db.DiaryEntries.Add(obj);
                        //_db.SaveChanges();
                        HttpResponseMessage response = _client.PostAsJsonAsync(_client.BaseAddress + "/ClassRoom/", obj).Result;

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
                        ModelState.AddModelError("ClassRoom", "Invalid data");
                        DataOBJ = LoadClassRoomDataOBJ(obj, "CHANGEDELETE");
                        string SER = JsonConvert.SerializeObject(DataOBJ);
                        TempData["ClassRoomREC"] = SER; // Store in session value
                        return RedirectToAction("Index", new { Id = DataOBJ.ClassRoomID, SelectedBuildingID = 0, pClassRoomDataOBJ = DataOBJ });
                    }

                    if (ModelState.IsValid)
                    {
                        if (obj.ClassRoomID < 1) obj.ClassRoomID = MVobj.ClassRoomID;
                        HttpResponseMessage response = _client.PutAsJsonAsync(_client.BaseAddress + "/ClassRoom/" + obj.ClassRoomID.ToString(), obj).Result;

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
                    if (ModelState.IsValid && obj.ClassRoomID > 0)
                    {
                        //_db.DiaryEntries.Remove(obj);
                        //_db.SaveChanges();
                        HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "/ClassRoom/" + obj.ClassRoomID.ToString()).Result;

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
                     return RedirectToAction("Index", new { id = 0, SelectedBuildingID = MVobj.SelectedBuildingNo });

                case "Cancel":
                    MVobj.ClassRoomID = 1;
                    MVobj.RoomNumber = 1;
                    MVobj.BuildingNumber = 1;
                    MVobj.Capacity = 1;
                    return RedirectToAction("Index");

                default:
                    return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Delete(ClassRoomMVMaint obj)
        {
            return View();
        }
    }
}
