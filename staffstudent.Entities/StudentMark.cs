﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace staffstudent.Entities
{
    public partial class StudentMark
    {
        [Key]
        [Column("Student_Roll_no")]
        public int StudentRollNo { get; set; }
        public int Tamil { get; set; }
        public int English { get; set; }
        public int Science { get; set; }
        public int Maths { get; set; }
        public int Total { get; set; }
        public double Average { get; set; }
        [StringLength(10)]
        public string Subject { get; set; }
        [Column("Scheduled_time", TypeName = "datetime")]
        public DateTime? ScheduledTime { get; set; }
        [Column("Is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("Created_time_stamp", TypeName = "datetime")]
        public DateTime CreatedTimeStamp { get; set; }
        [Column("Updated_time_stamp", TypeName = "datetime")]
        public DateTime UpdatedTimeStamp { get; set; }
    }
}