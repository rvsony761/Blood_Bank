using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBank_System.Models
{
    public class BloodGroup
    {
        [Key]
        public int BloodgroupCode { get; set; }
        public string BloodgroupName { get; set; }
        public ICollection<BloodBank> BloodBank {  get; set; }
        public ICollection<BloodRecord> BloodRecord { get; set; }

    }
    public class BloodBank
    {
        [Key]
        public int BloodbankCode { get; set; }
        public string BloodbankName { get; set; }
        public int BloodgroupCode { get; set; }
        public int Priceperunit { get; set; }
        public int TotalQuantityAvailable { get; set; }

        [ForeignKey("BloodgroupCode")]
        public virtual BloodGroup Bloodgroup { get; set; }
        public ICollection<BloodRecord> BloodRecord { get; set; }


    }
    public class BloodRecord
    {
        [Key]
        public int Transactionid{ get; set; }
        public int BloodbankCode { get; set; }
        public int BloodgroupCode { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateofTransaction { get; set; }
        public int Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
        public decimal AmountPayable { get; set; }

        [ForeignKey("BloodbankCode")]
        public virtual BloodBank BloodBank { get; set; }
        
        [ForeignKey("BloodgroupCode")]
        public virtual BloodGroup Bloodgroup { get; set; }
          
        [NotMapped]
        public List<SelectListItem> BloodBankList { get; set; }
        [NotMapped]
        public List<SelectListItem> BloodGroupList { get; set; }
        
        [NotMapped] 
        public List<SelectListItem> CustomerList { get;set;}
        }
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext():base("cn")
        {

        }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<BloodBank> BloodBanks { get; set;}
        public DbSet<BloodRecord> BloodRecords { get; set; }

    }
    public class spreport1
    {
        public string BloodbankName { get; set; }
        public string BloodgroupName { get; set; }
        public string CustomerName { get; set; }
        public int QuantitySold { get; set; }
    }
    public class spreport2
    {
        public DateTime DateofTransaction { get; set; }
        public string CustomerName { get; set; }
        public decimal Discount { get; set; }
    }

}