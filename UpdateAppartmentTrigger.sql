CREATE TRIGGER UpdateAppartmentTrigger ON [dbo].[Appartments]
FOR UPDATE
AS
	DECLARE @InsertString NVARCHAR(MAX)
	DECLARE @DeleteString NVARCHAR(MAX)
	SELECT  @InsertString = (SELECT AppartmentId + ' ' + Floor + ' ' + Number + ' ' + Size FROM Inserted)
	SELECT  @DeleteString = (SELECT AppartmentId + ' ' + Floor + ' ' + Number + ' ' + Size FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Appartment Update',@InsertString,@DeleteString)
GO