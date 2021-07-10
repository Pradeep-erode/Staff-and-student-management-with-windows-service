using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Staff_and_student.Controllers
{
    public class StaffmanagementController : Controller
    {

        #region  all login pages
        public IActionResult Mainpage()
        {
            return View();
        }
        public IActionResult Stafflogin()
        {
            return View();
        }
        public IActionResult Studentlogin()
        {
            return View();
        }
        #endregion

        #region staff login post 

        [HttpPost]
        public IActionResult Stafflogin(Staffcheck staffcheck)
        {
            if (staffcheck.Name == "pradeep" && staffcheck.password == 1234)
            {
                return RedirectToAction("Studentdashboard");
            }
            else
            {
                ViewBag.Credentials = "Please Enter correct credentials";
                return View();
            }
        }
        #endregion

        #region Student login post

        [HttpPost]
        public IActionResult Studentlogin(studentcheck studencheck)
        {
            if (ModelState.IsValid)
            {
                StudentMarkEntity markdata = new StudentMarkEntity();
                using (var client = new HttpClient())
                {
                    int roll = studencheck.StudentRollNo;
                    string password= studencheck.Password;

                    client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/Studentcheck");
                    var getindividual = client.GetAsync("?roll=" + roll + "&password=" + password);
                    getindividual.Wait();
                    var result = getindividual.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var individualdata = result.Content.ReadAsAsync<StudentMarkEntity>();
                        individualdata.Wait();

                        markdata = individualdata.Result;
                        return RedirectToAction("singlestudentmark", markdata);
                    }
                    //client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/Studentcheck/");
                    //var Posttask = client.PostAsJsonAsync(client.BaseAddress, studencheck);
                    //Posttask.Wait();

                    //var checkresult = Posttask.Result;
                    //if (checkresult.IsSuccessStatusCode)
                    //{
                    //    return RedirectToAction("StudentmarkShow");
                    //}
                    else
                    {
                        TempData["Wrongcredential"] = "Wrong credentials";
                        return RedirectToAction("Studentlogin");
                    }
                }
            }
            else
            {
                TempData["Wrongcredential"] = "Enter valid credentials";
                return RedirectToAction("Studentlogin");
            }
        }
        #endregion

        #region student list dashhboard
        public ActionResult Studentdashboard()
        {
            using (var client = new HttpClient())
            {
                List<StudentInformationEntity> studentlist = new List<StudentInformationEntity>();
                client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/");
                var gettask = client.GetAsync("Getstudentlist");
                gettask.Wait();

                var result = gettask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var studentdetail = result.Content.ReadAsStringAsync().Result;
                    studentlist = JsonConvert.DeserializeObject<List<StudentInformationEntity>>(studentdetail);
                    return View(studentlist);
                }
                else
                {
                    return View();
                }
            }
        }
        #endregion

        #region Excel upload post

        [HttpPost]
        public ActionResult Excelupload()
        {
            IFormFile docs = Request.Form.Files["UploadedFile"];

            if (docs != null)
            {
                Fileupload fileupload = new Fileupload();

                string filename = Path.GetFileNameWithoutExtension(docs.FileName);
                string extension = Path.GetExtension(docs.FileName);
                fileupload.Filename = filename + extension;
                using (var stream = new MemoryStream())
                {
                    docs.CopyToAsync(stream);
                    fileupload.filebyte = stream.ToArray(); 
                }
                fileupload.contenttype = docs.ContentType;

                if (fileupload.Filename.EndsWith(".xls") || fileupload.Filename.EndsWith(".xlsx"))
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/UploadExclel");
                        var Posttask = client.PostAsJsonAsync(client.BaseAddress, fileupload);
                        Posttask.Wait();
                        var checkresult = Posttask.Result;
                        if (checkresult.IsSuccessStatusCode)
                        {
                            return RedirectToAction("StudentmarkShow");
                        }
                        else if (checkresult.ReasonPhrase.Equals("Expectation Failed"))
                        {
                            TempData["ExcelNotify"] = "Some of the student Is not in student detail please update student detail first...";
                            return RedirectToAction("Studentdashboard");
                        }
                        
                        else if (checkresult.ReasonPhrase.Equals("Conflict"))
                        {
                            TempData["ExcelNotify"] = "Please check Excel file..Column should be not null...";
                            return RedirectToAction("Studentdashboard");
                        }
                        else if (checkresult.ReasonPhrase.Equals("Not Found"))
                        {
                            TempData["ExcelNotify"] = "connection To API failed...";
                            return RedirectToAction("Studentdashboard");
                        }
                    }

                }
                TempData["ExcelNotify"] = "select Excel File only";
                return RedirectToAction("Studentdashboard");
            }
            TempData["ExcelNotify"] = "Please select file to upload";
            return RedirectToAction("Studentdashboard");
        }
        #endregion

        #region student detail add and Edit page
        public ActionResult StudentAddandEdit(int rollno)
        {
            if (rollno > 0)
            {
               
                using (var client = new HttpClient())
                {
                    StudentInformationEntity studentdata = new StudentInformationEntity();

                    client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/Getstudentbyrollno/");
                    var getforedit = client.GetAsync("?rollno=" + rollno);
                    getforedit.Wait();
                    var result = getforedit.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var editdata = result.Content.ReadAsAsync<StudentInformationEntity>();
                        editdata.Wait();

                        studentdata = editdata.Result;
                        return View(studentdata);
                    }
                    else
                    {
                        TempData["nodataforEdit"] = "No data for Edit";
                        return RedirectToAction("Studentdashboard");
                    }
                }

            }
            return View();
        }
        #endregion

        #region student detail post logic
        [HttpPost]
        public ActionResult StudentdetailAdd(StudentInformationEntity studentinfo)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/Addstudentdetail/");
                var Posttask = client.PostAsJsonAsync(client.BaseAddress, studentinfo);
                Posttask.Wait();
                var checkresult = Posttask.Result;
                if (checkresult.IsSuccessStatusCode)
                {
                    return RedirectToAction("Studentdashboard");
                }
                else
                {
                    TempData["nodata"] = "please fill the form to continue";
                    return RedirectToAction("StudentAddandEdit");
                }
            }

        }
        #endregion

        #region all Student mark list page
        public ActionResult StudentmarkShow()
        {
            using (var client = new HttpClient())
            {
                List<StudentMarkEntity> markadd = new List<StudentMarkEntity>();

                client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/");
                var getforedit = client.GetAsync("GetstudentMarkList");
                getforedit.Wait();
                var checkresult = getforedit.Result;
                if (checkresult.IsSuccessStatusCode)
                {
                    var getmark = checkresult.Content.ReadAsStringAsync().Result;
                    markadd = JsonConvert.DeserializeObject<List<StudentMarkEntity>>(getmark);
                    return View(markadd);
                }
                else
                {
                    TempData["nomarklist"] = "Please Contact Admin...";
                    return RedirectToAction("Studentlogin");
                }
            }
        }

        #endregion

        #region Individual mark
        public ActionResult singlestudentmark(StudentMarkEntity markdata)
        {
            return View(markdata);
        }
        #endregion

        #region  student mark and information delete logic
        public ActionResult StudentDelete(int rollno)
        {
            if (rollno == 0)
            {
                return RedirectToAction("Studentdashboard");
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/Deletestudentbyrollno/");
                    var deletetask = client.DeleteAsync("?rollno=" + rollno);
                    deletetask.Wait();

                    var result = deletetask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Studentdashboard");
                    }
                    else
                    {
                        ViewBag.APIerror = true;
                        return RedirectToAction("Studentdashboard");
                    }
                }
            }
        }
        #endregion

    }
}
