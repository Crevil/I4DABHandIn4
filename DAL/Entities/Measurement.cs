using System;

namespace DAL.Entities
{
    public class Measurement
    {
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }

        public Sensor Sensor { get; set; }
        public int SensorId { get; set; }
        public Appartment Appartment { get; set; }
        public int AppartmentId { get; set; }

        public Measurement()
        {
            
        }

        public Measurement(double value, DateTime timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }

        public Measurement(double value, DateTime timestamp, Sensor sensor, Appartment appartment)
        {
            Value = value;
            Timestamp = timestamp;
            Sensor = sensor;
            Appartment = appartment;
        }
    }
}
