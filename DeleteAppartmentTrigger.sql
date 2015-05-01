CREATE TRIGGER DeleteAppartmentTrigger ON [dbo].[Appartments]
FOR DELETE
AS
	DECLARE @LogString NVARCHAR(MAX)
	SELECT @LogString = (SELECT AppartmentId + ' ' + Floor + ' ' + No + ' ' + Size FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Appartment Delete',0,@LogString)
GO