namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changingprecisionontimestamp : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Measurements");
            AlterColumn("dbo.Measurements", "Timestamp", c => c.DateTime(nullable: false, precision: 3, storeType: "datetime2"));
            AddPrimaryKey("dbo.Measurements", new[] { "SensorId", "Timestamp" });
            AlterStoredProcedure(
                "dbo.Measurement_Insert",
                p => new
                    {
                        SensorId = p.Int(),
                        Timestamp = p.DateTime(storeType: "datetime2"),
                        Value = p.Double(),
                        AppartmentId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Measurements]([SensorId], [Timestamp], [Value], [AppartmentId])
                      VALUES (@SensorId, @Timestamp, @Value, @AppartmentId)"
            );
            
            AlterStoredProcedure(
                "dbo.Measurement_Update",
                p => new
                    {
                        SensorId = p.Int(),
                        Timestamp = p.DateTime(storeType: "datetime2"),
                        Value = p.Double(),
                        AppartmentId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Measurements]
                      SET [Value] = @Value, [AppartmentId] = @AppartmentId
                      WHERE (([SensorId] = @SensorId) AND ([Timestamp] = @Timestamp))"
            );
            
            AlterStoredProcedure(
                "dbo.Measurement_Delete",
                p => new
                    {
                        SensorId = p.Int(),
                        Timestamp = p.DateTime(storeType: "datetime2"),
                    },
                body:
                    @"DELETE [dbo].[Measurements]
                      WHERE (([SensorId] = @SensorId) AND ([Timestamp] = @Timestamp))"
            );
            
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Measurements");
            AlterColumn("dbo.Measurements", "Timestamp", c => c.DateTime(nullable: false));
            AddPrimaryKey("dbo.Measurements", new[] { "SensorId", "Timestamp" });
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
