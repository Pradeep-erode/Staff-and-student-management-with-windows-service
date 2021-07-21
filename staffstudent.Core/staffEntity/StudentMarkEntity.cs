using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.staffEntity
{
    public class StudentMarkEntity
    {
        public int Student_Roll_no { get; set; }
        public string Student_Name { get; set; }
        public int Tamil { get; set; }
        public int English { get; set; }
        public int Science { get; set; }
        public int Maths { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }
        public string Subject { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ScheduledTime { get; set; }
    }
    class InvalidStudentNameException : Exception
    {
        public InvalidStudentNameException() { }

        public InvalidStudentNameException(string name)
            : base(String.Format("Invalid Student Name: {0}", name))
        {

        }
    }

}
