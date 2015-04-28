namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTypesAndNames : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measurements", "LocationId", "dbo.Locations");
            DropIndex("dbo.Measurements", new[] { "LocationId" });
            DropPrimaryKey("dbo.Measurements");
            CreateTable(
                "dbo.Appartments",
                c => new
                    {
                        AppartmentId = c.Int(nullable: false, identity: true),
                        Floor = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Size = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AppartmentId);
            
            AddColumn("dbo.Measurements", "AppartmentId", c => c.Int(nullable: false));
            AlterColumn("dbo.Sensors", "CalibrationCoeff", c => c.String());
            AlterColumn("dbo.Sensors", "ExternalRef", c => c.String());
            AddPrimaryKey("dbo.Measurements", new[] { "AppartmentId", "SensorId" });
            CreateIndex("dbo.Measurements", "AppartmentId");
            AddForeignKey("dbo.Measurements", "AppartmentId", "dbo.Appartments", "AppartmentId", cascadeDelete: true);
            DropColumn("dbo.Measurements", "LocationId");
            DropTable("dbo.Locations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        Floor = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Size = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId);
            
            AddColumn("dbo.Measurements", "LocationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Measurements", "AppartmentId", "dbo.Appartments");
            DropIndex("dbo.Measurements", new[] { "AppartmentId" });
            DropPrimaryKey("dbo.Measurements");
            AlterColumn("dbo.Sensors", "ExternalRef", c => c.Int(nullable: false));
            AlterColumn("dbo.Sensors", "CalibrationCoeff", c => c.Int(nullable: false));
            DropColumn("dbo.Measurements", "AppartmentId");
            DropTable("dbo.Appartments");
            AddPrimaryKey("dbo.Measurements", new[] { "LocationId", "SensorId" });
            CreateIndex("dbo.Measurements", "LocationId");
            AddForeignKey("dbo.Measurements", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
