using BloodBank_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBank_System.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db=new ApplicationDbContext();
        private void Binddropdown(BloodRecord record)
        {
            string qry2 = "Select cast(BloodgroupCode as varchar(10))[Value],BloodgroupName as [Text] from BloodGroups ";
            record.BloodGroupList = db.Database.SqlQuery<SelectListItem>(qry2).ToList();
            if (record.BloodgroupCode > 0)
            {
                string qry1 = "Select cast(BloodbankCode as varchar(10))[Value],BloodbankName as [Text]  from BloodBanks where BloodgroupCode=@Bloodcode";
                record.BloodBankList = db.Database.SqlQuery<SelectListItem>(qry1, new SqlParameter("@Bloodcode", record.BloodgroupCode)).ToList();
            }
            else
            {
                record.BloodBankList=new List<SelectListItem>();
            }
            record.CustomerList = new List<SelectListItem>
            {
                new SelectListItem{Value="Employee",Text="Employee"},
                 new SelectListItem{Value="Individual",Text="Individual"}
            };
             
        }
        public ActionResult Create()
        {
            BloodRecord record=new BloodRecord();
            Binddropdown(record);
            return View(record);
        }
        [HttpPost]
        public ActionResult Create(BloodRecord record)
        {
            if(!ModelState.IsValid)
            {
                Binddropdown(record);
                return View(record);
            }
            var group = db.BloodGroups.Where(x => x.BloodgroupCode == record.BloodgroupCode).FirstOrDefault();
            var bank=db.BloodBanks.Where(x=>x.BloodbankCode == record.BloodbankCode).FirstOrDefault();

            string qry4 = "Select TotalQuantityAvailable from BloodBanks where BloodbankCode=@BloodbankCode AND BloodgroupCode=@Bloodgroupcode";
            int TotalQuantity = db.Database.SqlQuery<int>(qry4, new SqlParameter("@BloodbankCode", record.BloodbankCode), new SqlParameter("@Bloodgroupcode", record.BloodgroupCode)).FirstOrDefault();
            string qry3 = "Select IsNull(sum(Quantity),0) from BloodRecords where BloodbankCode=@BloodbankCode AND BloodgroupCode=@Bloodgroupcode";
            int usedquantity = db.Database.SqlQuery<int>(qry3, new SqlParameter("@BloodbankCode",record.BloodbankCode), new SqlParameter("@Bloodgroupcode", record.BloodgroupCode)).FirstOrDefault();
            int available = TotalQuantity - usedquantity;
            if (record.Quantity > available)
            {
                TempData["message"] = $"Present Quantity is only {available}";
                Binddropdown(record);
                return RedirectToAction("Create");
            }
            if (record.CustomerName == "Employee")
            {
                record.Cost = record.Quantity * bank.Priceperunit;
                record.Discount = (record.Cost * 20) / 100;
                record.AmountPayable = record.Cost - record.Discount;
            }
            else if (record.CustomerName == "Individual")
            {
                record.Cost = record.Quantity * bank.Priceperunit;
                record.Discount = (record.Cost * 10) / 100;
                record.AmountPayable = record.Cost - record.Discount;
            }
            db.BloodRecords.Add(record);
            int result=db.SaveChanges();
            if(result>0)
            {
                TempData["message"] = "Data Saves Successfully";
            }
            return RedirectToAction("Create");
        }
        public ActionResult Report1()
        {
            List<spreport1> rp1 = new List<spreport1>();
            rp1 = db.Database.SqlQuery<spreport1>("Exec spreport1").ToList();
            return View(rp1);
        }

        [HttpGet]
        public ActionResult Report2()
        {
            List< spreport2 > blank=new List<spreport2>();
            
            return View(blank);
        }
        [HttpPost]
        public ActionResult Report2(DateTime StartDate,DateTime EndDate)
        {
            List<spreport2> rp2 = new List<spreport2>();
            rp2 = db.Database.SqlQuery<spreport2>("Exec spreport2 @StartDate,@EndDate", new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate)).ToList();
            return View(rp2);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}