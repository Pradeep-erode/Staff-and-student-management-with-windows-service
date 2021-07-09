using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.staffEntity
{
    public class Staffcheck
    {
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public int password { get; set; }
    }
}
