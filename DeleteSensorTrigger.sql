CREATE TRIGGER DeleteSensorTrigger ON [dbo].[Sensors]
FOR DELETE
AS
	DECLARE @DeleteString NVARCHAR(MAX)
	SELECT  @DeleteString = (SELECT SensorId + ' ' + CalibrationCoeff + ' ' + Description + ' ' + CalibrationDate + ' ' + ExternalRef + ' ' + Unit + ' ' + CalibrationEquation FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Sensor Delete',0,@DeleteString)
GO