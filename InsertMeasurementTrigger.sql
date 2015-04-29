CREATE TRIGGER InsertMeasurementTrigger ON [dbo].[Measurements]
FOR Insert
AS
	DECLARE @InsertString NVARCHAR(MAX)
	SELECT  @InsertString = (SELECT Value + ' ' + Timestamp + ' ' + SensorId + ' ' + AppartmentId FROM Inserted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Measurement Delete',@InsertString,0)
GO