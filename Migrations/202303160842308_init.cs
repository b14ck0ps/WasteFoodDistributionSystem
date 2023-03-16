namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CollectRequests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        RestaurantId = c.Int(nullable: false),
                        EmployeeId = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                        MaximumPreservationTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .ForeignKey("dbo.Restaurants", t => t.RestaurantId, cascadeDelete: true)
                .Index(t => t.RestaurantId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        ContactNumber = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 255),
                        AssignedArea = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        RestaurantId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Address = c.String(nullable: false, maxLength: 255),
                        ContactNumber = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.RestaurantId);
            
            CreateTable(
                "dbo.FoodDistributions",
                c => new
                    {
                        DistributionId = c.Int(nullable: false, identity: true),
                        RequestId = c.Int(nullable: false),
                        DistributedAt = c.DateTime(nullable: false),
                        RecipientCount = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.DistributionId)
                .ForeignKey("dbo.CollectRequests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.RequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodDistributions", "RequestId", "dbo.CollectRequests");
            DropForeignKey("dbo.CollectRequests", "RestaurantId", "dbo.Restaurants");
            DropForeignKey("dbo.CollectRequests", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.FoodDistributions", new[] { "RequestId" });
            DropIndex("dbo.CollectRequests", new[] { "EmployeeId" });
            DropIndex("dbo.CollectRequests", new[] { "RestaurantId" });
            DropTable("dbo.FoodDistributions");
            DropTable("dbo.Restaurants");
            DropTable("dbo.Employees");
            DropTable("dbo.CollectRequests");
        }
    }
}
