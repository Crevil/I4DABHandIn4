namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SensorIdNotDatabaseGenerated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AlterColumn("dbo.Sensors", "SensorId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Sensors", "SensorId");
            AddForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors", "SensorId", cascadeDelete: true);
            AlterStoredProcedure(
                "dbo.Sensor_Insert",
                p => new
                    {
                        SensorId = p.Int(),
                        CalibrationCoeff = p.String(),
                        Description = p.String(),
                        CalibrationDate = p.String(),
                        ExternalRef = p.String(),
                        Unit = p.String(),
                        CalibrationEquation = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Sensors]([SensorId], [CalibrationCoeff], [Description], [CalibrationDate], [ExternalRef], [Unit], [CalibrationEquation])
                      VALUES (@SensorId, @CalibrationCoeff, @Description, @CalibrationDate, @ExternalRef, @Unit, @CalibrationEquation)"
            );
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors");
            DropPrimaryKey("dbo.Sensors");
            AlterColumn("dbo.Sensors", "SensorId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Sensors", "SensorId");
            AddForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors", "SensorId", cascadeDelete: true);
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
