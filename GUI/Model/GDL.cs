using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;

namespace GUI.Model
{
    public class GDL
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


         public ICollection<Measurement> GetMeasurements(ICollection<Appartment> appartments, Sensor sensor )
         {
             var r = new Random();

             var list = new List<Measurement>();

             for (var i = 0; i < appartments.Count; i++)
             {
                 for (var j = 4; j >= 0; j--)
                 {

                     list.Add(
                         new Measurement
                         {
                             Timestamp =
                                 TimeHelpers.ConvertToUnixTimestamp(DateTime.Now.AddSeconds(j * -5)).ToString(CultureInfo.CurrentCulture),
                             Value = r.Next(0, 20),
                             SensorId = i,
                             AppartmentId = appartments.ElementAt(i).AppartmentId
                         });
                 }
             }
             return list;
         }
    }


    /// <summary>
    /// Helper class for time convertion between doubles and DateTime
    /// </summary>
    public static class TimeHelpers
    {
        public static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
