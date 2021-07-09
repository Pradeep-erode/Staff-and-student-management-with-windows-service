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
        private readonly IServiceStaff _Iservicestaff;
        public StaffAPIController(IServiceStaff staffservice)
        {
            _Iservicestaff = staffservice;
        }
        [HttpGet]
        public ActionResult Studentcheck(int roll,string password)
        {
            var check = _Iservicestaff.Studentcheck(roll, password);
            if (check !=null)
            {
                return Ok(check);
            }
            else
            {
                return Forbid();
                //return StatusCode(403); 
            }
        }
        [HttpGet]
        public ActionResult Getstudentlist()
        {
            var studentlist = _Iservicestaff.Getstudentlist();
            return Ok(studentlist);
        }

        #region Excel upload file
        [HttpPost]
        public ActionResult UploadExclel(Fileupload fileupload)
        {
            if (fileupload.filebyte != null)
            {
                Fileupload getexcel = new();
                getexcel.Filename = fileupload.Filename;
                getexcel.contenttype = fileupload.contenttype;

                byte[] bytefile = fileupload.filebyte;

                //for converting byte[] to IForm file
                var streama = new MemoryStream(bytefile);
                IFormFile file = new FormFile(streama, 0, bytefile.Length, "name", "fileName");

                var Issuccess = _Iservicestaff.UploadExclel(file, getexcel);
                if (Issuccess == 1)
                {
                    return Ok();
                }
                else if (Issuccess == 2)
                {
                    return StatusCode(417);
                }
                else if (Issuccess == 3)
                {
                    return StatusCode(409);
                }
                else if (Issuccess > 3)
                {
                    return Ok(Issuccess);
                }
            }
            return NotFound();

        }

        #endregion

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
        [HttpGet]
        public ActionResult GetstudentMarkList()
        {
            var listofmark=_Iservicestaff.GetstudentMarkList();
            return Ok(listofmark);
        }

    }
}
