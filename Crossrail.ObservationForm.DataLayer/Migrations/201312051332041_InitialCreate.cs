using System;
using System.Data.Entity.Migrations;

namespace Crossrail.ObservationForm.DataLayer.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contract",
                c => new
                    {
                        ContractId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ContractId);
            
            CreateTable(
                "dbo.Observation",
                c => new
                    {
                        ObservationId = c.Int(nullable: false, identity: true),
                        ObservationTypeId = c.Int(nullable: false),
                        ObservationCategoryId = c.Int(),
                        ContractId = c.Int(nullable: false),
                        Location = c.String(nullable: false),
                        ObservationDate = c.DateTime(nullable: false),
                        BriefDescription = c.String(nullable: false),
                        OtherCategory = c.String(),
                        ReportedBy = c.String(),
                        PhoneNumber = c.String(),
                        ActionsTaken = c.String(nullable: false),
                        ContractorInvolved = c.String(),
                        FilePath = c.String(),
                        Email = c.String(),
                        CompanyObservationReference = c.String(),
                    })
                .PrimaryKey(t => t.ObservationId)
                .ForeignKey("dbo.Contract", t => t.ContractId, cascadeDelete: true)
                .ForeignKey("dbo.ObservationCategory", t => t.ObservationCategoryId)
                .ForeignKey("dbo.ObservationType", t => t.ObservationTypeId, cascadeDelete: true)
                .Index(t => t.ContractId)
                .Index(t => t.ObservationCategoryId)
                .Index(t => t.ObservationTypeId);
            
            CreateTable(
                "dbo.ObservationCategory",
                c => new
                    {
                        ObservationCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ObservationCategoryId);
            
            CreateTable(
                "dbo.ObservationType",
                c => new
                    {
                        ObservationTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ObservationTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observation", "ObservationTypeId", "dbo.ObservationType");
            DropForeignKey("dbo.Observation", "ObservationCategoryId", "dbo.ObservationCategory");
            DropForeignKey("dbo.Observation", "ContractId", "dbo.Contract");
            DropIndex("dbo.Observation", new[] { "ObservationTypeId" });
            DropIndex("dbo.Observation", new[] { "ObservationCategoryId" });
            DropIndex("dbo.Observation", new[] { "ContractId" });
            DropTable("dbo.ObservationType");
            DropTable("dbo.ObservationCategory");
            DropTable("dbo.Observation");
            DropTable("dbo.Contract");
        }
    }
}
