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
                Fileupload getexcel = new();

                //transform fileupload data into getexcel object
                getexcel.Filename = fileupload.Filename;
                getexcel.contenttype = fileupload.contenttype;

                //as byte[] data not transfering to service and repository flow
                //we convert byte[] to IForm file here...
                byte[] bytefile = fileupload.filebyte;
                var streama = new MemoryStream(bytefile);
                IFormFile file = new FormFile(streama, 0, bytefile.Length, "name", "fileName");

                //here we send IFormFile and Fileupload instanse as arguments...
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
            var listofmark=_Iservicestaff.GetstudentMarkList();
            return Ok(listofmark);
        }
	#endregion
       
    }
}
