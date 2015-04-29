CREATE TRIGGER UpdateSensorTrigger ON [dbo].[Sensors]
FOR UPDATE
AS
	DECLARE @InsertString NVARCHAR(MAX)
	DECLARE @DeleteString NVARCHAR(MAX)
	SELECT  @InsertString = (SELECT SensorId + ' ' + CalibrationCoeff + ' ' + Description + ' ' + CalibrationDate + ' ' + ExternalRef + ' ' + Unit + ' ' + CalibrationEquation FROM Inserted)
	SELECT  @DeleteString = (SELECT SensorId + ' ' + CalibrationCoeff + ' ' + Description + ' ' + CalibrationDate + ' ' + ExternalRef + ' ' + Unit + ' ' + CalibrationEquation FROM Deleted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Sensor Update',@InsertString,@DeleteString)
GO