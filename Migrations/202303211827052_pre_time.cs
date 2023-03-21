namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pre_time : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CollectRequests", "MaximumPreservationTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CollectRequests", "MaximumPreservationTime", c => c.Int(nullable: false));
        }
    }
}
