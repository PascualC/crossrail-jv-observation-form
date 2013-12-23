using System;
using System.Data.Entity.Migrations;

namespace Crossrail.ObservationForm.DataLayer.Migrations
{
    public partial class ObservationCategoryReferenceOrderIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ObservationCategory", "Reference", c => c.String(nullable: false, defaultValue: string.Empty));
            AddColumn("dbo.ObservationCategory", "OrderIndex", c => c.Int(nullable: false, defaultValue: 0));
            AlterColumn("dbo.ObservationCategory", "Name", c => c.String(nullable: false, defaultValue: string.Empty));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ObservationCategory", "Name", c => c.String());
            DropColumn("dbo.ObservationCategory", "OrderIndex");
            DropColumn("dbo.ObservationCategory", "Reference");
        }
    }
}
