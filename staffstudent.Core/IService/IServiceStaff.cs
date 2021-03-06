using Microsoft.AspNetCore.Http;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.IService
{
    public interface IServiceStaff
    {
        public StudentMarkEntity Studentcheck(int roll, string password);
        public List<StudentInformationEntity> Getstudentlist();
        public string UploadExclel(Fileupload uploadexcel);
        public void Addstudentdetail(StudentInformationEntity studentdetail);
        public StudentInformationEntity Getstudentbyrollno(int rollno);
        public void Deletestudentbyrollno(int rollno);

        public List<StudentMarkEntity> GetstudentMarkList();
        public void ScheduleMail(StudentMarkEntity ScheduleMail);
    }
}
