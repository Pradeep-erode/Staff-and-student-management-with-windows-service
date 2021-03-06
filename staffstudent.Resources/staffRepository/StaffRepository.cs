using Dapper;
using LinqToExcel;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using staffstudent.Core.IRepository;
using staffstudent.Core.staffEntity;
using staffstudent.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Resources.staffRepository
{
    public class StaffRepository : IRepositoryStaff
    {
        //As it inherits IRepositoryStaff its take appsettings.json in API through Dependency Injection

        private readonly IConfiguration _config;
        public StaffRepository(IConfiguration configuration)
        {
            _config = configuration;
        }

        #region Student login check
        public StudentMarkEntity Studentcheck(int roll, string password)
        {
            StudentMarkEntity markadd = new StudentMarkEntity();
            using (var _context = new StaffmanagementContext())
            {
                var studetcheck = _context.StudentInformation.Where(m => m.StudentRollNo == roll && m.Password == password).SingleOrDefault();
                var marktest = _context.StudentMark.Where(m => m.StudentRollNo == roll).SingleOrDefault();
                if (studetcheck != null && marktest != null)
                {
                    markadd.Student_Roll_no = marktest.StudentRollNo;
                    markadd.Tamil = marktest.Tamil;
                    markadd.English = marktest.English;
                    markadd.Science = marktest.Science;
                    markadd.Maths = marktest.Maths;
                    markadd.Total = marktest.Total;
                    markadd.Average = marktest.Average;
                }
                return markadd;
            }
        }
        #endregion

        #region Getstudentlist for dashboard and dapper method
        public List<StudentInformationEntity> Getstudentlist()
        {
            #region Dapper

            var connectionString = _config.GetConnectionString("NorthwindDatabase");
           // also we use like this==> var connectionStringa = _config["ConnectionStrings:NorthwindDatabase"];

            string query = "SELECT * FROM Student_Information";
            SqlConnection con;
            con = new SqlConnection(connectionString);
            con.Open();
            IList <StudentInformationEntity> EmpList = SqlMapper.Query<StudentInformationEntity>(
                                  con, query).ToList();
            con.Close();
            //In stored procedure,
            // IList<StudentInformationEntity> EmpList = SqlMapper.Query<StudentInformationEntity>(
            //                    con, "GetEmployees").ToList();


            //IDbConnection db2 = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServerConnString"].ConnectionString);
            //using (var db = new SqlConnection(connectionString))
            //    return db.Query<StudentInformationEntity>(query).AsList();

            #endregion

            List<StudentInformationEntity> listofstudent = new();
            using (var _context = new StaffmanagementContext())
            {
                var list = _context.StudentInformation.Where(m => m.IsDeleted == false).ToList();
                foreach (var context in list)
                {
                    StudentInformationEntity studentlist = new();
                    studentlist.StudentRollNo = context.StudentRollNo;
                    studentlist.StudentFirstName = context.StudentFirstName + " " + context.StudentLastName;
                    studentlist.Gender = context.Gender;
                    studentlist.Dob = context.Dob;
                    studentlist.FatherFirstName = context.FatherFirstName + " " + context.FatherLastName;
                    studentlist.MotherFirstName = context.MotherFirstName + " " + context.MotherLastName;
                    studentlist.Email = context.Email;
                    studentlist.StudentContactNo = context.StudentContactNo;
                    studentlist.FatherSContactNo = context.FatherSContactNo;
                    studentlist.FatherSOccupation = context.FatherSOccupation;
                    studentlist.Password = context.Password;

                    var checkformark = _context.StudentMark.Where(m => m.StudentRollNo == context.StudentRollNo).SingleOrDefault();
                    if (checkformark != null)
                    {
                        studentlist.IsMarkadded = true;
                    }
                    else { studentlist.IsMarkadded = false; }
                    listofstudent.Add(studentlist);
                }
            }

            return listofstudent;
        }
        #endregion

        #region Add new student and update
        public void Addstudentdetail(StudentInformationEntity studentdetail)
        {

            using (var _context = new StaffmanagementContext())
            {
                var updatetask = _context.StudentInformation.Where(m => m.StudentRollNo == studentdetail.StudentRollNo).SingleOrDefault();
                if (updatetask != null)
                {
                    updatetask.StudentRollNo = studentdetail.StudentRollNo;
                    updatetask.StudentFirstName = studentdetail.StudentFirstName;
                    updatetask.StudentLastName = studentdetail.StudentLastName;
                    updatetask.Gender = studentdetail.Gender;
                    updatetask.Dob = studentdetail.Dob;
                    updatetask.FatherFirstName = studentdetail.FatherFirstName;
                    updatetask.FatherLastName = studentdetail.FatherLastName;
                    updatetask.MotherFirstName = studentdetail.MotherFirstName;
                    updatetask.MotherLastName = studentdetail.MotherLastName;
                    updatetask.Email = studentdetail.Email;
                    updatetask.StudentContactNo = studentdetail.StudentContactNo;
                    updatetask.FatherSContactNo = studentdetail.FatherSContactNo;
                    updatetask.FatherSOccupation = studentdetail.FatherSOccupation;
                    updatetask.UpdatedTimeStamp = DateTime.Now;
                    _context.SaveChanges();
                }
                else
                {
                    StudentInformation addtask = new();

                    addtask.StudentRollNo = studentdetail.StudentRollNo;
                    addtask.StudentFirstName = studentdetail.StudentFirstName;
                    addtask.StudentLastName = studentdetail.StudentLastName;
                    addtask.Gender = studentdetail.Gender;
                    addtask.Dob = studentdetail.Dob;
                    addtask.FatherFirstName = studentdetail.FatherFirstName;
                    addtask.FatherLastName = studentdetail.FatherLastName;
                    addtask.MotherFirstName = studentdetail.MotherFirstName;
                    addtask.MotherLastName = studentdetail.MotherLastName;
                    addtask.Email = studentdetail.Email;
                    addtask.StudentContactNo = studentdetail.StudentContactNo;
                    addtask.FatherSContactNo = studentdetail.FatherSContactNo;
                    addtask.FatherSOccupation = studentdetail.FatherSOccupation;
                    addtask.Password = studentdetail.Password;
                    _context.StudentInformation.Add(addtask);
                    _context.SaveChanges();
                }

            }
        }
        #endregion 

        #region Excel upload
        //for oldb error
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string UploadExclel(List<StudentMarkEntity> lists)
        {
            using (var _context = new StaffmanagementContext())
            {
                foreach (var mark in lists)
                {
                    StudentMark markupload = new StudentMark();

                    //Update logic for mark if roll nummber exists in our databse

                    var checkforexist = _context.StudentMark.Where(m => m.StudentRollNo == mark.Student_Roll_no).SingleOrDefault();
                    if (checkforexist != null)
                    {
                        checkforexist.Tamil = mark.Tamil;
                        checkforexist.English = mark.English;
                        checkforexist.Science = mark.Science;
                        checkforexist.Maths = mark.Maths;
                        checkforexist.Total = mark.Total;
                        checkforexist.Average = mark.Average;
                        _context.SaveChanges();
                    }
                    else
                    {
                        //Add logic for mark if roll number not exists in mark databse and check weather StudentInformation has that roll number
                        // if StudentInformation does not containt that roll number mark is not added,otherwise mark is added.

                        var checkfordetail = _context.StudentInformation.Where(m => m.StudentRollNo == mark.Student_Roll_no).SingleOrDefault();
                        if (checkfordetail != null)
                        {
                            markupload.StudentRollNo = mark.Student_Roll_no;
                            markupload.Tamil = mark.Tamil;
                            markupload.English = mark.English;
                            markupload.Science = mark.Science;
                            markupload.Maths = mark.Maths;
                            markupload.Total = mark.Total;
                            markupload.Average = mark.Average;
                            _context.Add(markupload);
                            _context.SaveChanges();
                        }
                        else
                        {
                            return "Some of the student Is not in student detail please update student detail first... ";
                        }
                    }
                }
                return "updated successfully...";
            }
           
        }

        #endregion

        #region Getstudent detail for Edit

        public StudentInformationEntity Getstudentbyrollno(int rollno)
        {
            StudentInformationEntity studentforedit = new StudentInformationEntity();
            using (var _context = new StaffmanagementContext())
            {
                var getforedit = _context.StudentInformation.Where(m => m.StudentRollNo == rollno && m.IsDeleted == false).SingleOrDefault();

                studentforedit.StudentRollNo = getforedit.StudentRollNo;
                studentforedit.StudentFirstName = getforedit.StudentFirstName;
                studentforedit.StudentLastName = getforedit.StudentLastName;
                studentforedit.Gender = getforedit.Gender;
                studentforedit.Dob = getforedit.Dob;
                studentforedit.FatherFirstName = getforedit.FatherFirstName;
                studentforedit.FatherLastName = getforedit.FatherLastName;
                studentforedit.MotherFirstName = getforedit.MotherFirstName;
                studentforedit.MotherLastName = getforedit.MotherLastName;
                studentforedit.Email = getforedit.Email;
                studentforedit.StudentContactNo = getforedit.StudentContactNo;
                studentforedit.FatherSContactNo = getforedit.FatherSContactNo;
                studentforedit.FatherSOccupation = getforedit.FatherSOccupation;
                studentforedit.Password = getforedit.Password;
            }

            return studentforedit;
        }
        #endregion

        #region Delete student detail and mark both
        public void Deletestudentbyrollno(int rollno)
        {
            using (var _context = new StaffmanagementContext())
            {
                //deleting detail
                var deletedetail = _context.StudentInformation.Where(m => m.StudentRollNo == rollno).SingleOrDefault();
                //deleting marks
                var deletedmarks = _context.StudentMark.Where(m => m.StudentRollNo == rollno).SingleOrDefault();
                deletedetail.IsDeleted = true;
                deletedmarks.IsDeleted = true;
                deletedetail.UpdatedTimeStamp = DateTime.Now;
                deletedmarks.UpdatedTimeStamp = DateTime.Now;
                _context.SaveChanges();
            }
        }
        #endregion

        #region Get mark list

        public List<StudentMarkEntity> GetstudentMarkList()
        {
            List<StudentMarkEntity> marklist = new();
            using (var _context = new StaffmanagementContext())
            {
                var getmark = _context.StudentMark.Where(m => m.IsDeleted == false).ToList();
                foreach (var mark in getmark)
                {
                    StudentMarkEntity get = new();
                    get.Student_Roll_no = mark.StudentRollNo;
                    get.Tamil = mark.Tamil;
                    get.English = mark.English;
                    get.Science = mark.Science;
                    get.Maths = mark.Maths;
                    get.Total = mark.Total;
                    get.Average = mark.Average;
                    marklist.Add(get);
                }
            }
            return marklist;
        }

        #endregion

        #region Schedule mail for test

        public void ScheduleMail(StudentMarkEntity ScheduleMail)
        {
            try
            {
                using (var _context = new StaffmanagementContext())
                {
                    var getrollno = _context.StudentMark.Where(m => m.IsDeleted == false && m.StudentRollNo == ScheduleMail.Student_Roll_no).FirstOrDefault();
                    getrollno.Subject = ScheduleMail.Subject;
                    getrollno.ScheduledTime = ScheduleMail.ScheduledTime;
                    _context.SaveChanges();
                }
            }
            catch
            {
                
            }
        }


        #endregion

    }
}
