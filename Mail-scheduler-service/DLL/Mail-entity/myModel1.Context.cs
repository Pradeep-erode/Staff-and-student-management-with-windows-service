﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mail_scheduler_service.DLL.Mail_entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StaffmanagementEntitiesss : DbContext
    {
        public StaffmanagementEntitiesss()
            : base("name=StaffmanagementEntitiesss")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Student_Information> Student_Information { get; set; }
        public virtual DbSet<StudentMark> StudentMarks { get; set; }
    }
}