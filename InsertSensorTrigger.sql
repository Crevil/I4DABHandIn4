CREATE TRIGGER InsertSensorTrigger ON [dbo].[Sensors]
FOR INSERT
AS
	DECLARE @InsertString NVARCHAR(MAX)
	SELECT  @InsertString = (SELECT SensorId + ' ' + CalibrationCoeff + ' ' + Description + ' ' + CalibrationDate + ' ' + ExternalRef + ' ' + Unit + ' ' + CalibrationEquation FROM Inserted)
	INSERT INTO dbo.Logs (Operation,LogEntryInserted,LogEntryDeleted) VALUES ('Sensor Insert',@InsertString,0)
GO