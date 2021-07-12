using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.staffEntity
{
    public class StudentMarkEntity
    {
        public int Student_Roll_no { get; set; }
        public int Tamil { get; set; }
        public int English { get; set; }
        public int Science { get; set; }
        public int Maths { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }
        public string Subject { get; set; }
        public DateTime? ScheduledTime { get; set; }
    }
}
