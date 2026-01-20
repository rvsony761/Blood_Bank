using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Course.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public string StudentMobile { get; set; }

    }
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public string CourseType { get; set; }
        public decimal CourseFees { get; set; }
    }

    public class Payment_detail
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentMethodName { get; set; }
        public decimal Payment_Discunt { get; set; }
    }
    public class Enrollment_detail
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int PaymentId { get; set; }
        public DateTime? Enrollment_Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal GST { get; set; }
        public decimal Total_Payable { get; set; }

        [ForeignKey("PaymentId")] 
        public virtual Payment_detail Paymentdetailinfo { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Courseinfo { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Studentinfo { get; set; }

        [NotMapped]
        public List<SelectListItem> StudentList { get; set; }
        
        [NotMapped]
        public List<SelectListItem> CourseList { get; set; }
        
        [NotMapped]
        public List<SelectListItem> PaymentMethodList { get; set; }
        
        [NotMapped]
        public List<int> SelectedCoursesIds { get; set; }
    }
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base("cn")
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment_detail> Enrollment_details { get; set; }
        public DbSet<Payment_detail> Payment_details { get; set; }

    }
    public class spreport1
    {
        public string CourseName { get; set; }
        public string Duration { get; set; }
        public int Total_Students { get; set; }
        public decimal Total_Payable { get; set; }

    }
}