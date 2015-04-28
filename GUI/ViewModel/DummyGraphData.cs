using System;
using System.Collections.Generic;

namespace GUI.ViewModel
{
    class DummyGraphData
    {
        public List<DummyMeasurement> GetData()
        {
            var measurements = new List<DummyMeasurement>();

            var startDate = DateTime.Now.AddSeconds(-10);
            var r = new Random();

            for (var j = 0; j < 11; j++) // Create data
            {
                measurements.Add(new DummyMeasurement() { DetectorId = 1, DateTime = startDate.AddSeconds(j), Value = r.Next(1, 30) });
            }

            measurements.Sort((m1, m2) => m1.DateTime.CompareTo(m2.DateTime));

            return measurements;
        }

        public List<DummyMeasurement> GetUpdateData(DateTime dateTime)
        {
            var measurements = new List<DummyMeasurement>();
            var r = new Random();

            measurements.Add(new DummyMeasurement() { DetectorId = 1, DateTime = dateTime.AddSeconds(1), Value = r.Next(1, 30) });

            return measurements;
        }
    }
}