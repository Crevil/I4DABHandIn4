using System.Collections;
using System.Collections.Generic;

namespace DAL.Entities
{
    public class Sensor
    {
        public int SensorId { get; set; }
        public string CalibrationCoeff { get; set; }
        public string Description { get; set; }
        public string CalibrationDate { get; set; }
        public string ExternalRef { get; set; }
        public string Unit { get; set; }
        public string CalibrationEquation { get; set; }

        public ICollection<Measurement> Measurements { get; set; }

        public Sensor()
        {
            
        }
    }
}
