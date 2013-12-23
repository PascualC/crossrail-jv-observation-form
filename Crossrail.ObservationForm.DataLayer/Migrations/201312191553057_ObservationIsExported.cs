namespace Crossrail.ObservationForm.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservationIsExported : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Observation", "IsExported", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Observation", "IsExported");
        }
    }
}
