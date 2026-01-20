using Student_Course.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Course.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db=new ApplicationDbContext();

        private void Binddropdown(Enrollment_detail record)
        {
            string qry = "Select cast(StudentId as varchar(10))[Value],StudentName[Text] from Students";
            record.StudentList = db.Database.SqlQuery<SelectListItem>(qry).ToList();
            string qry2 = "Select cast(CourseId as varchar(10))[Value],[CourseName][Text] from Courses";
            record.CourseList = db.Database.SqlQuery<SelectListItem>(qry2).ToList();
            record.SelectedCoursesIds = new List<int>();
            string qry3 = "Select cast(PaymentId as varchar(10))[Value],PaymentMethodName[Text] from Payment_detail";
            record.PaymentMethodList = db.Database.SqlQuery<SelectListItem>(qry3).ToList();
        }
        public ActionResult Create()
        {
            Enrollment_detail record=new Enrollment_detail();
            Binddropdown(record);
            return View(record);
        }
        [HttpPost]
        public ActionResult Create(Enrollment_detail record)
        {
            
            if(!ModelState.IsValid)
            {
                Binddropdown(record);
                return View(record);
            }
            int results=0;
            foreach (var item in record.SelectedCoursesIds)
            {
                Enrollment_detail obj = new Enrollment_detail();
                var student=db.Students.Where(x=>x.StudentId==record.StudentId).FirstOrDefault();
                var course=db.Courses.Where(x=>x.CourseId==item).FirstOrDefault();
                var payment = db.Payment_details.Where(x => x.PaymentId == record.PaymentId).FirstOrDefault();
                if(course.CourseType=="Online")
                {
                    obj.Amount = course.CourseFees+(course.CourseFees*5)/100;
                }
                else
                {
                    obj.Amount = course.CourseFees;    
                }
                obj.GST = (obj.Amount * 12) / 100;
                obj.Discount = ((obj.Amount + obj.GST) * payment.Payment_Discunt) / 100;
                obj.Total_Payable = (obj.Amount + obj.GST) - obj.Discount;
                obj.StudentId=record.StudentId;
                obj.CourseId = item;
                obj.PaymentId = record.PaymentId;
                obj.Enrollment_Date= record.Enrollment_Date;
                db.Enrollment_details.Add(obj);    
            }
            results = db.SaveChanges();
            if(results>0)
            {
                TempData["message"]="Data Saves Successfully";
            }
            return RedirectToAction("Create");
        }

        public ActionResult Report1()
        {
            List<spreport1> report=new List<spreport1>();

            return View(report);
        }
        [HttpPost]
        public ActionResult Report1(DateTime StartDate,DateTime EndDate)
        {
            List<spreport1> report = new List<spreport1>();
            report=db.Database.SqlQuery<spreport1>("Exec spreport1 @StartDate,@EndDate",new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate)).ToList();
            return View(report);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}