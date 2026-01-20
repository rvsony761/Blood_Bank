using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sample_Rohit_Project.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        @*-- Here this Code is For Bind/Make the DropDown *@
        private void Binddropdown(Bill_detail bill)
        {
            string qry = "Select cast(TariffId as varchar(10))[Value],TariffPlan[Text] from tblTariffs";
            bill.TariffList = db.Database.SqlQuery<SelectListItem>(qry).ToList();
            string qry2 = "Select cast(CustomerId as varchar(10))[Value],CustomerName[Text] from tblCustomers";
            bill.CustomerList = db.Database.SqlQuery<SelectListItem>(qry2).ToList();

        }

        @* CasCading DropDown bnane ke liye *@
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
                record.BloodBankList = new List<SelectListItem>();
            }

            @* Ye DataBase Se Static DropDown Create krne ke liye h List m 
               dropdown ko bhejne ke liye is you stuck somewhere
             *@
            record.CustomerList = new List<SelectListItem>
            {
                new SelectListItem{Value="Employee",Text="Employee"},
                 new SelectListItem{Value="Individual",Text="Individual"}
            };

        }


        @*
            Always Use these type of Variable for getting the data from the other tables also
        *@
        var group = db.BloodGroups.Where(x => x.BloodgroupCode == record.BloodgroupCode).FirstOrDefault();
        var bank = db.BloodBanks.Where(x => x.BloodbankCode == record.BloodbankCode).FirstOrDefault();


        @* 
        Here Two main Concept are Involved like 
        1. user Quantity will not Exceed the Total Quantity
        2. Here Apply concept 

        Remaining=Total_Quantity-Used_Quantity

        3. And don't Forot to Apply the Sql Parameter Concept while Sending the Parameter to the 
        Sql Query
         *@
        string qry4 = "Select TotalQuantityAvailable from BloodBanks where BloodbankCode=@BloodbankCode AND BloodgroupCode=@Bloodgroupcode";
        int TotalQuantity = db.Database.SqlQuery<int>(qry4, new SqlParameter("@BloodbankCode", record.BloodbankCode), new SqlParameter("@Bloodgroupcode", record.BloodgroupCode)).FirstOrDefault();
        string qry3 = "Select IsNull(sum(Quantity),0) from BloodRecords where BloodbankCode=@BloodbankCode AND BloodgroupCode=@Bloodgroupcode";
        int usedquantity = db.Database.SqlQuery<int>(qry3, new SqlParameter("@BloodbankCode", record.BloodbankCode), new SqlParameter("@Bloodgroupcode", record.BloodgroupCode)).FirstOrDefault();
        Remaining=TotalQuantity-usedQuantity;

        if (record.Quantity > Remaining)
        {
            TempData["message"] = $"Present Quantity is only {Remaining}";
            Binddropdown(record);
            return RedirectToAction("Create");
        }

            @*
            Save krne ka code Db me 
            *@
            db.TicketBookings.Add(booking);
            int result = db.SaveChanges();
            if(result>0)
            {
                TempData["message"] = $"Seats are Booked Successfully with and Amount is {booking.PayableAmount}";
                
            }
}

}
}

@*
    Layout m Render ke upper ka code jisse alert show hoyega
*@
@if(TempData["message"] != null)
{
    < div class= "alert alert-success" >
        @TempData["message"]
    </ div >
}

    @*
        Important Date Function Related to Sql 
    *@
    model.model_name.Month  => to extract the month in number from the date sent by the user
    Select getdate()

SELECT DATEADD(year, 1, '2017/08/25') AS After_one_year;
Select DATEADD(MONTH,2,'2026/01/23') as After_Two_Month
Select DATEADD(DAY,10,'2026/01/23') as after_10_daye

Select DATEDIFF(YEAR,'2026-10-23','2024-01-23') => -2
Select DATEDIFF(YEAR,'2024-10-23','2026-01-23') => 0
Select DATEDIFF(MONTH,'2026-01-23','2026-03-23')
Select DATEDIFF(DAY,'2026-01-23','2026-03-23')

Select DATEPART(year,'2026-01-23')
Select DATEPART(MONTH,'2026-01-23')
Select Datepart(Day,'2026-01-23')

Select DATENAME(year,'2026-01-23')
Select DATENAME(MONTH,'2026-01-23') => Give month name from the Date
Select DATENAME(DAY,'2026-01-23')

Select ISNULL('',0)

<-- This is the code for create the multiple column from the single columns -->
Select Housetype,
       count(case when HouseStatus = 'Occupied' then 1 end) as No_of_House_on_Rent,
       count(case when HouseStatus = 'Available' then 1 end) as No_of_House_Available 
from housetbls
group by Housetype