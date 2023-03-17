namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class amount_clmn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CollectRequests", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CollectRequests", "Amount");
        }
    }
}
