namespace DAL.Entities
{
    public class Measurement
    {
        public double Value { get; set; }
        public string Timestamp { get; set; }

        public Sensor Sensor { get; set; }
        public int sensorId { get; set; }
        public Location Location { get; set; }
        public int appartmentId { get; set; }

        public Measurement()
        {
            
        }
    }
}
