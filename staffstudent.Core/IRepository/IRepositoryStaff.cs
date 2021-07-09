using Microsoft.AspNetCore.Http;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.IRepository
{
    public interface IRepositoryStaff
    {
        public StudentMarkEntity Studentcheck(int roll, string password);
        public List<StudentInformationEntity> Getstudentlist();
        public int UploadExclel(IFormFile docs, Fileupload getexcel);
        public void Addstudentdetail(StudentInformationEntity studentdetail);
        public StudentInformationEntity Getstudentbyrollno(int rollno);
        public void Deletestudentbyrollno(int rollno);

        public List<StudentMarkEntity> GetstudentMarkList();
    }
}
