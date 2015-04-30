namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumberToNo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appartments", "No", c => c.Int(nullable: false));
            DropColumn("dbo.Appartments", "Number");
            AlterStoredProcedure(
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
            
            AlterStoredProcedure(
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
            
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appartments", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.Appartments", "No");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
