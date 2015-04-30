using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAL;
using DAL.Entities;

namespace GUI.Model
{
    public class GDL
    {
        private DbRepository _repository;

        public event EventHandler OriginalLoaded;

        public GDL(DbRepository repository)
        {
            _repository = repository;
        }

        public void LoadOriginal()
        {
            string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            Tuple<ICollection<Appartment>, ICollection<Sensor>> t = JSONDeserialisator.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));

            _repository.AddCollectionOfAppartments(t.Item1).Wait();
            _repository.AddCollectionOfSensors(t.Item2).Wait();

            if (OriginalLoaded != null)
                OriginalLoaded(this, new EventArgs());
        }

        static public ICollection<Measurement> LoadJson(int nr)
        {
            string begin = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/dataGDL/data/";
            string end = ".json";
            return JSONDeserialisator.DeserialiseMeasurement(StringDownloader.DownloadStringFromURL(begin + nr.ToString() + end));
        }

        /// <summary>
        /// Returns a list of measurements associated with appartments and sensor type
        /// </summary>
        /// <param name="appartments">Collection of appartments to search from</param>
        /// <param name="sensorType">Sensor type to search from</param>
        /// <returns></returns>
        public ICollection<Measurement> GetMeasurements(ICollection<Appartment> appartments, string sensorType )
        {
            var r = _repository.GetMeasurements(appartments, sensorType);
            return r;
        }

        /// <summary>
        /// Gets appartments from database
        /// </summary>
        /// <returns>Collection of appartments</returns>
        public ICollection<Appartment> GetAppartments()
        {
            return _repository.Appartments;
        }

        /// <summary>
        /// Gets sensors from database
        /// </summary>
        /// <returns>Collection of sensors</returns>
        public ICollection<Sensor> GetSensors()
        {
            return _repository.Sensors;
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
