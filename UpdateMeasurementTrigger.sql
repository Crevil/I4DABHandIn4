CREATE TRIGGER UpdateMeasurementTrigger ON [dbo].[Measurements]
FOR UPDATE
AS
	DECLARE @InsertString NVARCHAR(MAX)
	DECLARE @DeleteString NVARCHAR(MAX)
	SELECT  @InsertString = (SELECT Value + ' ' + CONVERT(nvarchar(MAX), [Timestamp], 20) + ' ' + SensorId + ' ' + AppartmentId FROM Inserted)
	SELECT  @DeleteString = (SELECT Value + ' ' + CONVERT(nvarchar(MAX), [Timestamp], 20) + ' ' + SensorId + ' ' + AppartmentId FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Measurement Update',@InsertString,@DeleteString)
GO