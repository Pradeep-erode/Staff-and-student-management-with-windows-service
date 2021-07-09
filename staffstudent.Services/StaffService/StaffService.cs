using Microsoft.AspNetCore.Http;
using staffstudent.Core.IRepository;
using staffstudent.Core.IService;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Services.StaffService
{
    public class StaffService : IServiceStaff
    {
        private readonly IRepositoryStaff _Irepositorystaff;
        public StaffService(IRepositoryStaff repositorystaff)
        {
            _Irepositorystaff = repositorystaff;
        }
        public StudentMarkEntity Studentcheck(int roll, string password)
        {
            return _Irepositorystaff.Studentcheck(roll,password);
        }
        public List<StudentInformationEntity> Getstudentlist()
        {
            return _Irepositorystaff.Getstudentlist();
        }
        public void Addstudentdetail(StudentInformationEntity studentdetail)
        {
            _Irepositorystaff.Addstudentdetail(studentdetail);
        }
        public StudentInformationEntity Getstudentbyrollno(int rollno)
        {
            return _Irepositorystaff.Getstudentbyrollno(rollno);
        }
        public void Deletestudentbyrollno(int rollno)
        {
            _Irepositorystaff.Deletestudentbyrollno(rollno);
        }
        public int UploadExclel(IFormFile docs, Fileupload getexcel)
        {
            var count= _Irepositorystaff.UploadExclel(docs, getexcel);
            return count;
        }
        public List<StudentMarkEntity> GetstudentMarkList()
        {
            return _Irepositorystaff.GetstudentMarkList();
        }
    }
}
