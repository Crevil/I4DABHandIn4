using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    class DummyGraphData : IDataProvider
    {
        public List<Measurement> GetData()
        {
            var measurements = new List<Measurement>();

            var startDate = DateTime.Now.AddSeconds(-10);
            var r = new Random();

            for (var j = 0; j < 11; j++) // Create data
            {
                measurements.Add(new Measurement() { DetectorId = 1, DateTime = startDate.AddSeconds(j), Value = r.Next(1, 30) });
            }

            measurements.Sort((m1, m2) => m1.DateTime.CompareTo(m2.DateTime));

            return measurements;
        }

        public List<Measurement> GetUpdateData(DateTime dateTime)
        {
            var measurements = new List<Measurement>();
            var r = new Random();

            measurements.Add(new Measurement() { DetectorId = 1, DateTime = dateTime.AddSeconds(1), Value = r.Next(1, 30) });

            return measurements;
        }
    }

    public class Measurement
    {
        public int DetectorId { get; set; }
        public int Value { get; set; }
        public DateTime DateTime { get; set; }
    }
}
