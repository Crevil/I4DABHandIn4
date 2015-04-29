CREATE PROCEDURE DataFromAppartment
	@AppartmentId int = NULL
AS
	SELECT Appartments.*, Measurements.*, Sensors.*
	FROM Appartments INNER JOIN 
		Measurements ON Appartments.AppartmentId = Measurements.AppartmentId INNER JOIN 
		Sensors ON Measurements.SensorId = Sensors.SensorId
		WHERE Appartments.AppartmentId = @AppartmentId
ORDER BY Measurements.SensorId
RETURN
GO
