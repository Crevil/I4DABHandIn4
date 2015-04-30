﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;


namespace GUI.Model
{
    public class GDL
    {
        static private DbRepository repository = new DbRepository();

        static public void LoadOriginal()
        {
            string originalUrl = "http://userportal.iha.dk/~jrt/i4dab/E14/HandIn4/GFKSC002_original.txt";

            Tuple<ICollection<Appartment>, ICollection<Sensor>> t = JSONDeserialisator.DeserialiseOriginalFile(StringDownloader.DownloadStringFromURL(originalUrl));

            repository.AddCollectionOfAppartments(t.Item1);
            repository.AddCollectionOfSensors(t.Item2);

            // CALL VIEW UPDATE 
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
            // Get sensors of this type in the appartments
            
            // Get measurements from the appartments and sensors


            // Dummy
            // Create random measurements for appartments
            var r = new Random();

            var list = new List<Measurement>();

            for (var i = 0; i < appartments.Count; i++) // For each appartment
            {
                for (var j = 0; j < 5; j++) // Create 5 measurements
                {
                    list.Add(
                        new Measurement
                        {
                            Timestamp =
                                TimeHelpers.ConvertToUnixTimestamp(DateTime.Now.AddSeconds(j * -5)).ToString(CultureInfo.CurrentCulture),
                            Value = r.Next(0, 20),
                            AppartmentId = appartments.ElementAt(i).AppartmentId
                        });
                }
            }

            return list;
        }

        /// <summary>
        /// Gets appartments from database
        /// </summary>
        /// <returns>Collection of appartments</returns>
        public ICollection<Appartment> GetAppartments()
        {
            return new List<Appartment>
            {
                new Appartment
                {
                    AppartmentId = 1,
                    Floor = 0,
                    Number = 1
                },
                new Appartment {
                    AppartmentId = 2,
                    Floor = 2,
                    Number = 1,
                },
                new Appartment
                {
                    AppartmentId = 3,
                    Floor = 2,
                    Number = 4
                }
            };
        }

        /// <summary>
        /// Gets sensors from database
        /// </summary>
        /// <returns>Collection of sensors</returns>
        public ICollection<Sensor> GetSensors()
        {
            return new List<Sensor>
            {
                new Sensor {SensorId = 1, Description = "Temperature"},
                new Sensor {SensorId = 2, Description = "Humidity"},
                new Sensor {SensorId = 3, Description = "Power"},
                new Sensor {SensorId = 4, Description = "Power"}
            };
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
