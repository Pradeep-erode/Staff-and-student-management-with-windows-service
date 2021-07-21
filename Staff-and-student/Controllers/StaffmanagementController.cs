using LamarCodeGeneration.Frames;
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
using System.Threading;
using System.Threading.Tasks;

namespace Staff_and_student.Controllers
{
    public class StaffmanagementController : Controller
    {
        public object Schedular { get; private set; }

        #region  all login pages
        public IActionResult Mainpage()
        {
           TempData[" logout"] = "logout not nessesary";
           return View();
        }
        public IActionResult Stafflogin()
        {
            //DateTime scheduledTime = DateTime.MinValue;
            //scheduledTime = scheduledTime.AddDays(1);
            //scheduledTime = DateTime.Now.AddMinutes(1);
            //TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
            //var times = timeSpan;
            //string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            //var msg = schedule;
            //int dueTime = Convert.ToInt32(timeSpan.Milliseconds);
            //var lio = Timeout.Infinite;

            TempData[" logout"] = "logout not nessesary";
          return View();
        }
        public IActionResult Studentlogin()
        {
            //var timeString = "07:00";
            //DateTime t = DateTime.Parse(timeString);
            //TimeSpan ts = new TimeSpan();
            //ts = t - DateTime.Now;
            //if (ts.TotalMilliseconds < 0)
            //{
            //    ts = t.AddDays(1) - DateTime.Now;
            //}

            return View();
        }
        #endregion

        #region staff login post 

        [HttpPost]
        public IActionResult Stafflogin(Staffcheck staffcheck)
        {
            if (staffcheck.Name == "pradeep" && staffcheck.password == 1234)
            {
                HttpContext.Session.SetString("credential", staffcheck.Name);
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

                        HttpContext.Session.SetString("cred", "Student");
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
            if (HttpContext.Session.GetString("credential") !=null)
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
            else { return RedirectToAction("Stafflogin"); }
        }
        #endregion

        #region schedule Email
        public ActionResult ScheduleTest(int rollno)
        {
            if (HttpContext.Session.GetString("credential") != null)
            {
                StudentMarkEntity markmodel = new StudentMarkEntity();
                markmodel.Student_Roll_no = rollno;
                return View(markmodel);
            }
            else
            {
                return RedirectToAction("Stafflogin");
            }
        }

        #endregion

        #region Schedule mail post method

        [HttpPost]
        public ActionResult ScheduleTestformail(StudentMarkEntity mailschedule)
        {
            if (mailschedule.ScheduledTime > DateTime.Now.AddMinutes(10))
            {
                if (HttpContext.Session.GetString("credential") != null)
                {


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("https://localhost:44322/api/StaffAPI/ScheduleMail/");
                        var Posttask = client.PostAsJsonAsync(client.BaseAddress, mailschedule);
                        Posttask.Wait();
                        var checkresult = Posttask.Result;
                        if (checkresult.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Studentdashboard");
                        }
                        else
                        {
                            TempData["nodata"] = "Not Scheduled correctly...";
                            return RedirectToAction("ScheduleTest");
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Stafflogin");
                }
            }
            else
            {
                TempData["nodata"] = "Schedule minimum of 10 minutes from time NOW...";
                return View("ScheduleTest");
            }
        }

        #endregion

        #region Excel upload post

        [HttpPost]
        public ActionResult Excelupload()
        {
            if (HttpContext.Session.GetString("credential") != null)
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
                            //normal post method
                            //var Posttask = client.PostAsJsonAsync(client.BaseAddress, fileupload);

                            //for getting value from post method
                            var Posttask = client.PostAsJsonAsync(client.BaseAddress, fileupload).Result;
                            //for getting int value
                            //var getint = Posttask.Content.ReadAsAsync<int>();

                            //for getting string value
                            var getint = Posttask.Content.ReadAsStringAsync();
                           
                            var errormessage=getint.Result;

                            TempData["ExcelNotify"] = errormessage;
                            return RedirectToAction("Studentdashboard");

                            //for checking response code depend on its response phrases...

                            #region response phrases check

                            //if (checkresult.IsSuccessStatusCode)
                            //{
                            //    return RedirectToAction("StudentmarkShow");
                            //}
                            //else if (checkresult.ReasonPhrase.Equals("Expectation Failed"))
                            //{
                            //    TempData["ExcelNotify"] = "Some of the student Is not in student detail please update student detail first...";
                            //    return RedirectToAction("Studentdashboard");
                            //}

                            //else if (checkresult.ReasonPhrase.Equals("Conflict"))
                            //{
                            //    TempData["ExcelNotify"] = "Linq to Excel package not intalled correctly...";
                            //    return RedirectToAction("Studentdashboard");
                            //}
                            //else if (checkresult.ReasonPhrase.Equals("Not Found"))
                            //{
                            //    TempData["ExcelNotify"] = "connection To API failed...";
                            //    return RedirectToAction("Studentdashboard");
                            //}

                            //else if (checkresult.ReasonPhrase.Equals("Bad Request"))
                            //{
                            //    TempData["ExcelNotifhy"] = "Table column should not be null...";
                            //    return RedirectToAction("Studentdashboard");
                            //}
                            //else if (checkresult.ReasonPhrase.Equals("Forbidden"))
                            //{
                            //    TempData["ExcelNotifhy"] = "use only numbers in mark field...";
                            //    return RedirectToAction("Studentdashboard");
                            //}
                            //else if (checkresult.ReasonPhrase.Equals("Not Acceptable"))
                            //{
                            //    TempData["ExcelNotifhy"] = "student roll should be 4 digit and mark should be 3 digits only allowed...";
                            //    return RedirectToAction("Studentdashboard");
                            //}

                            #endregion

                        }

                    }
                    TempData["ExcelNotify"] = "select Excel File only";
                    return RedirectToAction("Studentdashboard");
                }
                TempData["ExcelNotify"] = "Please select file to upload";
                return RedirectToAction("Studentdashboard");
            }
            else
            {
                return RedirectToAction("Stafflogin");
            }
        }
        #endregion

        #region student detail add and Edit page
        public ActionResult StudentAddandEdit(int rollno)
        {
            if (HttpContext.Session.GetString("credential") != null)
            {
                StudentInformationEntity studentdata = new StudentInformationEntity();
                if (rollno > 0)
                {

                    using (var client = new HttpClient())
                    {


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
                return View(studentdata);
            }
            else
            {
                return RedirectToAction("Stafflogin");
            }
        }
        #endregion

        #region student detail post logic
        [HttpPost]
        public ActionResult StudentdetailAdd(StudentInformationEntity studentinfo)
        {
            if (HttpContext.Session.GetString("credential") != null)
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
            else
            {
                return RedirectToAction("Stafflogin");
            }

        }
        #endregion

        #region all Student mark list page
        public ActionResult StudentmarkShow()
        {
            if (HttpContext.Session.GetString("credential") != null)
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
            else
            {
                return RedirectToAction("Stafflogin");
            }
        }

        #endregion

        #region Individual mark
        public ActionResult singlestudentmark(StudentMarkEntity markdata)
        {
            if (HttpContext.Session.GetString("cred") != null)
            {
                return View(markdata);
            }
            else
            {
                return RedirectToAction("Stafflogin");
            }
            
        }
        #endregion

        #region  student mark and information delete logic
        public ActionResult StudentDelete(int rollno)
        {
            if (HttpContext.Session.GetString("credential") != null)
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
            else
            {
                return RedirectToAction("Stafflogin");
            }
        }
        #endregion

        #region Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Mainpage");
        }
        #endregion

    }
}
