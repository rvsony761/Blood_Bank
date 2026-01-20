namespace BloodBank_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BloodBanks",
                c => new
                    {
                        BloodbankCode = c.Int(nullable: false, identity: true),
                        BloodbankName = c.String(),
                        BloodgroupCode = c.Int(nullable: false),
                        Priceperunit = c.Int(nullable: false),
                        TotalQuantityAvailable = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BloodbankCode)
                .ForeignKey("dbo.BloodGroups", t => t.BloodgroupCode, cascadeDelete: true)
                .Index(t => t.BloodgroupCode);
            
            CreateTable(
                "dbo.BloodGroups",
                c => new
                    {
                        BloodgroupCode = c.Int(nullable: false, identity: true),
                        BloodgroupName = c.String(),
                    })
                .PrimaryKey(t => t.BloodgroupCode);
            
            CreateTable(
                "dbo.BloodRecords",
                c => new
                    {
                        Transactionid = c.Int(nullable: false, identity: true),
                        BloodbankCode = c.Int(nullable: false),
                        BloodgroupCode = c.Int(nullable: false),
                        CustomerName = c.String(),
                        DateofTransaction = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountPayable = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Transactionid)
                .ForeignKey("dbo.BloodBanks", t => t.BloodbankCode, cascadeDelete: true)
                .ForeignKey("dbo.BloodGroups", t => t.BloodgroupCode, cascadeDelete: false)
                .Index(t => t.BloodbankCode)
                .Index(t => t.BloodgroupCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BloodRecords", "BloodgroupCode", "dbo.BloodGroups");
            DropForeignKey("dbo.BloodRecords", "BloodbankCode", "dbo.BloodBanks");
            DropForeignKey("dbo.BloodBanks", "BloodgroupCode", "dbo.BloodGroups");
            DropIndex("dbo.BloodRecords", new[] { "BloodgroupCode" });
            DropIndex("dbo.BloodRecords", new[] { "BloodbankCode" });
            DropIndex("dbo.BloodBanks", new[] { "BloodgroupCode" });
            DropTable("dbo.BloodRecords");
            DropTable("dbo.BloodGroups");
            DropTable("dbo.BloodBanks");
        }
    }
}
