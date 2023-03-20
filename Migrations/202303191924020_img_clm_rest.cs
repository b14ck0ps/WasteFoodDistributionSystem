namespace WasteFoodDistributionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class img_clm_rest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Restaurants", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restaurants", "Image");
        }
    }
}
