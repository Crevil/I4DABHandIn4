CREATE TRIGGER InsertAppartmentTrigger ON [dbo].[Appartments]
FOR INSERT
AS
	DECLARE @LogString NVARCHAR(MAX)
	SELECT @LogString = (SELECT AppartmentId + ' ' + Floor
	+ ' ' + No + ' ' + Size FROM Inserted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Appartment Insert',@LogString,0)
GO