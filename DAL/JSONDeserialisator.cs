﻿using System;
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
    public static class JSONDeserialisator
    {

        public static Tuple<ICollection<Appartment>, ICollection<Sensor>> DeserialiseOriginalFile(string json)
        {
            int appartmentBegin = json.IndexOf("[", 0);
            int appartmentEnd = json.IndexOf("]", appartmentBegin);
            int sensorBegin = json.IndexOf("[", appartmentEnd);
            int sensorEnd = json.IndexOf("]", sensorBegin);

            string appartments = json.Substring(appartmentBegin, appartmentEnd-appartmentBegin+1);
            ICollection<Appartment> appList = JsonConvert.DeserializeObject<ICollection<Appartment>>(appartments);
            int len = json.Length;
            string sensors = json.Substring(sensorBegin, sensorEnd-sensorBegin+1);
            ICollection<Sensor> senList = JsonConvert.DeserializeObject<ICollection<Sensor>>(sensors);

            return new Tuple<ICollection<Appartment>, ICollection<Sensor>>(appList, senList);

        }

        public static List<Measurement> DeserialiseMeasurement(string json)
        {
            int begin = json.IndexOf("[", 0);
            int end = json.IndexOf("]", begin);
            string o = json.Substring(begin, end-begin+1);
            return JsonConvert.DeserializeObject<List<Measurement>>(o);
        }
    }
}

//old method
//            using (StreamReader r = new StreamReader(filepath))
//            {
//                string json = r.ReadToEnd();
//                int begin = json.IndexOf("[", 0);
//                int end = json.IndexOf("]", begin);
//                string o = json.Substring(begin, end-begin+1);
//                return JsonConvert.DeserializeObject<List<Measurement>>(o);
//            }