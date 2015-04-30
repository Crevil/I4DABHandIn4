namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Newdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appartments",
                c => new
                    {
                        AppartmentId = c.Int(nullable: false, identity: true),
                        Floor = c.Int(nullable: false),
                        No = c.Int(nullable: false),
                        Size = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.AppartmentId);
            
            CreateTable(
                "dbo.Measurements",
                c => new
                    {
                        AppartmentId = c.Int(nullable: false),
                        SensorId = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                        Timestamp = c.String(),
                    })
                .PrimaryKey(t => new { t.AppartmentId, t.SensorId })
                .ForeignKey("dbo.Appartments", t => t.AppartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Sensors", t => t.SensorId, cascadeDelete: true)
                .Index(t => t.AppartmentId)
                .Index(t => t.SensorId);
            
            CreateTable(
                "dbo.Sensors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SensorId = c.Int(nullable: false),
                        CalibrationCoeff = c.String(),
                        Description = c.String(),
                        CalibrationDate = c.String(),
                        ExternalRef = c.String(),
                        Unit = c.String(),
                        CalibrationEquation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogId = c.Int(nullable: false, identity: true),
                        Operation = c.String(),
                        LogEntryInserted = c.String(),
                        LogEntryDeleted = c.String(),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateStoredProcedure(
                "dbo.Appartment_Insert",
                p => new
                    {
                        Floor = p.Int(),
                        No = p.Int(),
                        Size = p.Double(),
                    },
                body:
                    @"INSERT [dbo].[Appartments]([Floor], [No], [Size])
                      VALUES (@Floor, @No, @Size)
                      
                      DECLARE @AppartmentId int
                      SELECT @AppartmentId = [AppartmentId]
                      FROM [dbo].[Appartments]
                      WHERE @@ROWCOUNT > 0 AND [AppartmentId] = scope_identity()
                      
                      SELECT t0.[AppartmentId]
                      FROM [dbo].[Appartments] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[AppartmentId] = @AppartmentId"
            );
            
            CreateStoredProcedure(
                "dbo.Appartment_Update",
                p => new
                    {
                        AppartmentId = p.Int(),
                        Floor = p.Int(),
                        No = p.Int(),
                        Size = p.Double(),
                    },
                body:
                    @"UPDATE [dbo].[Appartments]
                      SET [Floor] = @Floor, [No] = @No, [Size] = @Size
                      WHERE ([AppartmentId] = @AppartmentId)"
            );
            
            CreateStoredProcedure(
                "dbo.Appartment_Delete",
                p => new
                    {
                        AppartmentId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Appartments]
                      WHERE ([AppartmentId] = @AppartmentId)"
            );
            
            CreateStoredProcedure(
                "dbo.Measurement_Insert",
                p => new
                    {
                        AppartmentId = p.Int(),
                        SensorId = p.Int(),
                        Value = p.Double(),
                        Timestamp = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Measurements]([AppartmentId], [SensorId], [Value], [Timestamp])
                      VALUES (@AppartmentId, @SensorId, @Value, @Timestamp)"
            );
            
            CreateStoredProcedure(
                "dbo.Measurement_Update",
                p => new
                    {
                        AppartmentId = p.Int(),
                        SensorId = p.Int(),
                        Value = p.Double(),
                        Timestamp = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Measurements]
                      SET [Value] = @Value, [Timestamp] = @Timestamp
                      WHERE (([AppartmentId] = @AppartmentId) AND ([SensorId] = @SensorId))"
            );
            
            CreateStoredProcedure(
                "dbo.Measurement_Delete",
                p => new
                    {
                        AppartmentId = p.Int(),
                        SensorId = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Measurements]
                      WHERE (([AppartmentId] = @AppartmentId) AND ([SensorId] = @SensorId))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Measurement_Delete");
            DropStoredProcedure("dbo.Measurement_Update");
            DropStoredProcedure("dbo.Measurement_Insert");
            DropStoredProcedure("dbo.Appartment_Delete");
            DropStoredProcedure("dbo.Appartment_Update");
            DropStoredProcedure("dbo.Appartment_Insert");
            DropForeignKey("dbo.Measurements", "SensorId", "dbo.Sensors");
            DropForeignKey("dbo.Measurements", "AppartmentId", "dbo.Appartments");
            DropIndex("dbo.Measurements", new[] { "SensorId" });
            DropIndex("dbo.Measurements", new[] { "AppartmentId" });
            DropTable("dbo.Logs");
            DropTable("dbo.Sensors");
            DropTable("dbo.Measurements");
            DropTable("dbo.Appartments");
        }
    }
}
