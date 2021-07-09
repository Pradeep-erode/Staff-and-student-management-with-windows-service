using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Core.staffEntity
{
    public class StudentInformationEntity
    {
        [Required]
        public int StudentRollNo { get; set; }
        [Required]
        [StringLength(40)]
        public string StudentFirstName { get; set; }
        [Required]
        [StringLength(40)]
        public string StudentLastName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }
        [Required]
        [StringLength(40)]
        public string FatherFirstName { get; set; }
        [Required]
        [StringLength(40)]
        public string FatherLastName { get; set; }
        [Required]
        [StringLength(40)]
        public string MotherFirstName { get; set; }
        [Required]
        [StringLength(40)]
        public string MotherLastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public long StudentContactNo { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public long FatherSContactNo { get; set; }
        [Required]
        [StringLength(30)]
        public string FatherSOccupation { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime UpdatedTimeStamp { get; set; }
        [Required]
        public string Password { get; set; }

        public bool IsMarkadded { get; set; }
        public IFormFile Excel { get; set; }
    }
    public class studentcheck
    {
        public int StudentRollNo { get; set; }
        public string Password { get; set; }
    }

    public class Fileupload
    {
        public IFormFile excelfile { get; set; }
        public string Filename { get; set; }
        public byte[] filebyte { get; set; }
        public string contenttype { get; set; }
    }
}
