using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using staffstudent.Core.IService;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace staff_student_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StaffAPIController : ControllerBase
    {

        #region Injection

        private readonly IServiceStaff _Iservicestaff;
        public StaffAPIController(IServiceStaff staffservice)
        {
            _Iservicestaff = staffservice;
        }
        #endregion

        #region Student login checking and get individual mark list 

        [HttpGet]
        public ActionResult Studentcheck(int roll, string password)
        {
            var check = _Iservicestaff.Studentcheck(roll, password);
            if (check != null)
            {
                return Ok(check);
            }
            else
            {
                return Forbid();
                //return StatusCode(403); 
            }
        }

        #endregion

        #region Get student list dashboard
        [HttpGet]
        public ActionResult Getstudentlist()
        {
            var studentlist = _Iservicestaff.Getstudentlist();
            return Ok(studentlist);
        }
        #endregion

        #region Excel upload file
        [HttpPost]
        public ActionResult UploadExclel(Fileupload fileupload)
        {
            if (fileupload.filebyte != null)
            {
                Fileupload uploadexcel = new();

                //transform fileupload data into getexcel object
                uploadexcel.Filename = fileupload.Filename;
                uploadexcel.contenttype = fileupload.contenttype;

                //as byte[] data not transfering to service and repository flow
                //we convert byte[] to IForm file here...
                byte[] bytefile = fileupload.filebyte;
                var streama = new MemoryStream(bytefile);
                IFormFile file = new FormFile(streama, 0, bytefile.Length, "name", "fileName");
                uploadexcel.excelfile = file;

                var Issuccess = _Iservicestaff.UploadExclel(uploadexcel);
                if (Issuccess != null)
                {
                    return Ok(Issuccess);
                }

                #region For returning differrent success code

                //else if (Issuccess == 2)
                //{
                //    return StatusCode(417);
                //}
                //else if (Issuccess == 3)
                //{
                //    return StatusCode(409);
                //}
                //else if (Issuccess == 4)
                //{
                //    return StatusCode(400);
                //}
                //else if (Issuccess == 5)
                //{
                //    return StatusCode(403);
                //}
                //else if (Issuccess == 6)
                //{
                //    return StatusCode(406);
                //}
                #endregion

            }
            return NotFound();

        }

        #endregion

        #region new student Add And Update student detail
        [HttpPost]
        public ActionResult Addstudentdetail(StudentInformationEntity studentinfo)
        {
            if (studentinfo != null)
            {
                _Iservicestaff.Addstudentdetail(studentinfo);
                return Ok();
            }
            return NotFound();
        }
        #endregion

        #region get student detail for edit
        [HttpGet]
        public ActionResult Getstudentbyrollno(int rollno)
        {
            if (rollno > 0)
            {
                var studentlist = _Iservicestaff.Getstudentbyrollno(rollno);
                return Ok(studentlist);
            }
            return NotFound();
        }
        #endregion

        #region Delete student detail and mark both
        [HttpDelete]
        public ActionResult Deletestudentbyrollno(int rollno)
        {
            if (rollno > 0)
            {
                _Iservicestaff.Deletestudentbyrollno(rollno);
                return Ok();
            }
            return NotFound();
        }
        #endregion

        #region get all student mark list
        [HttpGet]
        public ActionResult GetstudentMarkList()
        {
            var listofmark = _Iservicestaff.GetstudentMarkList();
            return Ok(listofmark);
        }
        #endregion

        #region Schedule Mail Post method

        [HttpPost]
        public ActionResult ScheduleMail(StudentMarkEntity ScheduleMail)
        {
            _Iservicestaff.ScheduleMail(ScheduleMail);
            return Ok();
        }

        #endregion

    }
}
