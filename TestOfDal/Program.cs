using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using Newtonsoft.Json;

namespace TestOfDal
{
    class Program
    {
        static void Main(string[] args)
        {
            //using (var db = new Context())
            //{
            //    var test = new Location(1,1,1);
            //    db.Locations.Add(test);
            //    db.SaveChanges();
            //}

            JSONDeserialisator ser = new JSONDeserialisator();

            List<JSONDeserialisator.Measurement> test = ser.DeserialiseMeasurement("../2.json");

            Tuple<List<JSONDeserialisator.Appartment>, List<JSONDeserialisator.Sensor>> t = ser.DeserialiseOriginalFile("../GFKSC002_original.txt");
            while (true)
            {
                
            }
        }
    }
}
