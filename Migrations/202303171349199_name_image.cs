namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name_image : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CollectRequests", "Name", c => c.String());
            AddColumn("dbo.CollectRequests", "Image", c => c.String());
        }
        
        public override void Down()
        {
            AddColumn("dbo.CollectRequests", "Name", c => c.String());
            DropColumn("dbo.CollectRequests", "Image");
        }
    }
}
