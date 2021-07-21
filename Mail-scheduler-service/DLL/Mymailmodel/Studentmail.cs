using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_scheduler_service.DLL.Mymailmodel
{
    public class Studentdata
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public DateTime? Date { get; set; }
    }
}
