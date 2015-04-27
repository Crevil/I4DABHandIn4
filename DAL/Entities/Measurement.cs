namespace DAL.Entities
{
    public class Measurement
    {
        public double Value { get; set; }
        public string Timestamp { get; set; }

        public Sensor Sensor { get; set; }
        public int SensorId { get; set; }
        public Appartment Appartment { get; set; }
        public int AppartmentId { get; set; }

        public Measurement()
        {
            
        }
    }
}
