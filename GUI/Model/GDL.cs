using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace GUI.Model
{
     static public class GDL
    {
        private static int _jsonCounter = 0;

        static public Task LoadOriginal()
        {
            string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            Tuple<List<Appartment>, List<Sensor>> t = JSONDeserialisator.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));

            // do something to DB
            return null;
        }

        static public Task LoadNextJson()
        {
            string begin = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/";
            string end = ".json";
            List<Measurement> test = JSONDeserialisator.DeserialiseMeasurement(StringDownloader.DownloadStringFromURL(begin + _jsonCounter.ToString() + end));

            
            // do something to the DB
            return null;
        }
    }
}
