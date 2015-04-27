using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace DAL
{


    public class JSONDeserialisator
    {

        public Tuple<List<Appartment>, List<Sensor>> DeserialiseOriginalFile(string filepath)
        {
            using (StreamReader r = new StreamReader(filepath))
            {
                string json = r.ReadToEnd();
                int appartmentBegin = json.IndexOf("[", 0);
                int appartmentEnd = json.IndexOf("]", appartmentBegin);
                int sensorBegin = json.IndexOf("[", appartmentEnd);
                int sensorEnd = json.IndexOf("]", sensorBegin);

                string appartments = json.Substring(appartmentBegin, appartmentEnd-appartmentBegin+1);
                List<Appartment> appList = JsonConvert.DeserializeObject<List<Appartment>>(appartments);
                int len = json.Length;
                string sensors = json.Substring(sensorBegin, sensorEnd-sensorBegin+1);
                List<Sensor> senList = JsonConvert.DeserializeObject<List<Sensor>>(sensors);

                return new Tuple<List<Appartment>, List<Sensor>>(appList, senList);
            }
        }

        public List<Measurement> DeserialiseMeasurement(string filepath)
        {
            using (StreamReader r = new StreamReader(filepath))
            {
                string json = r.ReadToEnd();
                int begin = json.IndexOf("[", 0);
                int end = json.IndexOf("]", begin);
                string o = json.Substring(begin, end-begin+1);
                return JsonConvert.DeserializeObject<List<Measurement>>(o);
            }
        }


        public class Measurement
        {
            public double Value { get; set; }
            public string Timestamp { get; set; }

            public Sensor Sensor { get; set; }
            public int SensorId { get; set; }
            public Appartment Appartment { get; set; }
            public int AppartmentId { get; set; }
        }

        public class Appartment
        {
            public int AppartmentId { get; set; }
            public int Floor { get; set; }
            public int Number { get; set; }
            public double Size { get; set; }
        }

        public class Sensor
        {
            public int SensorId { get; set; }
            public string CalibrationCoeff { get; set; }
            public string Description { get; set; }
            public string CalibrationDate { get; set; }
            public string ExternalRef { get; set; }
            public string Unit { get; set; }
            public string CalibrationEquation { get; set; }
        }
    }
}
