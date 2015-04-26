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
            public double value { get; set; }
            public string timestamp { get; set; }

            public int sensorId { get; set; }

            public int appartmentId { get; set; }
        }

        public class Appartment
        {
            public int appartmentId { get; set; }
            public int Floor { get; set; }
            public int No { get; set; }
            public double Size { get; set; }
        }

        public class Sensor
        {
            public int sensorId { get; set; }
            public string calibrationCoeff { get; set; }
            public string description { get; set; }
            public string calibrationDate { get; set; }
            public string externalRef { get; set; }
            public string unit { get; set; }
            public string calibrationEquation { get; set; }
        }
    }
}
