using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using Newtonsoft.Json;

namespace TestOfDal
{
    class Program
    {
        private static void Main(string[] args)
        {
            var context = new Context();
            var appRepos = new Repository<Appartment>(context);
            var sensorRepos = new Repository<Sensor>(context);
            var measureRepos = new Repository<Measurement>(context);

            var apptest = new Appartment(1, 1, 1);
            var sensortest = new Sensor { Description = "John"};
            var testMeasurement = new Measurement(1,"timestamp",sensortest,apptest);

            appRepos.Add(apptest).Wait();
            sensorRepos.Add(sensortest).Wait();
            measureRepos.Add(testMeasurement).Wait();

            var read = appRepos.FindWithInclude(a => a.AppartmentId == 13, m => m.Measurements).Result;
            /*JSONDeserialisator ser = new JSONDeserialisator();

            string begin = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/";
            string end = ".json";

            int max = 11803;

            // HENTER ALLE 11803 json filer fra nettet hvis den får lov at løbe :D
            for (int i = 1; i < max-11800; i++)
            {
                List<Measurement> test = ser.DeserialiseMeasurement(StringDownloader.DownloadStringFromURL(begin + i.ToString() + end));
                // DO SOMETHING WITH LIST
                Console.WriteLine(i);
            }

            

            string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            Tuple<List<Appartment>, List<Sensor>> t = ser.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));
            while (true)
            {
                
            }*/
        }
    }
}
