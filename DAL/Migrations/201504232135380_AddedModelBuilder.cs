namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedModelBuilder : DbMigration
    {
        public override void Up()
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
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        SensorId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        Timestamp = c.String(),
                    })
                .PrimaryKey(t => new { t.LocationId, t.SensorId })
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Sensors", t => t.SensorId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        SensorId = c.Int(nullable: false, identity: true),
                        CalibrationCoeff = c.Int(nullable: false),
                        Description = c.String(),
                        CalibrationDate = c.String(),
                        ExternalRef = c.Int(nullable: false),
                        Unit = c.String(),
                        CalibrationEquation = c.String(),
                    })
                .PrimaryKey(t => t.SensorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.Measurements", "LocationId", "dbo.Locations");
            DropIndex("dbo.Measurements", new[] { "SensorId" });
            DropIndex("dbo.Measurements", new[] { "LocationId" });
            DropTable("dbo.Sensors");
            DropTable("dbo.Measurements");
            DropTable("dbo.Locations");
        }
    }
}
