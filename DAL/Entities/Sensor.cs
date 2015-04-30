using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Sensor
    {
        [Key]
        public int Id { get; set; }
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
