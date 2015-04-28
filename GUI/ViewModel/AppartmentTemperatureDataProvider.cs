using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using DAL.Entities;
namespace GUI.ViewModel
{
    public class AppartmentTemperatureDataProvider : IDataProvider
    {
        private ObservableCollection<Appartment> _appartments;

        public AppartmentTemperatureDataProvider(ObservableCollection<Appartment> appartments)
        {
            _appartments = appartments;
        }

        public List<Measurement> GetData()
        {
            var r = new Random();

            var list = new List<Measurement>();
            for (var i = 0; i < _appartments.Count; i++)
            {
                for (var j = 4; j >= 0; j--)
                {

                    list.Add(new Measurement
                    {
                        Timestamp =
                            ConvertToUnixTimestamp(DateTime.Now.AddSeconds(j * -5)).ToString(CultureInfo.CurrentCulture),
                        Value = 22,
                        SensorId = i
                    });
                }
            }
            return list;

        }

        public List<Measurement> GetUpdateData(DateTime date)
        {
            var r = new Random();

            var list = new List<Measurement>();
            for (var i = 0; i < _appartments.Count; i++)
            {
                    list.Add(new Measurement()
                    {
                        Timestamp =
                            ConvertToUnixTimestamp(DateTime.Now).ToString(CultureInfo.CurrentCulture),
                        Value = r.Next(0, 10),
                        SensorId = i
                    });
            }
            return list;
        }

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
