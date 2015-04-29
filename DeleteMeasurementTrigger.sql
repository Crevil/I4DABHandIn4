CREATE TRIGGER DeleteMeasurementTrigger ON [dbo].[Measurements]
FOR DELETE
AS
	DECLARE @DeleteString NVARCHAR(MAX)
	SELECT  @DeleteString = (SELECT Value + ' ' + Timestamp + ' ' + SensorId + ' ' + AppartmentId FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Measurement Delete',0,@DeleteString)
GO