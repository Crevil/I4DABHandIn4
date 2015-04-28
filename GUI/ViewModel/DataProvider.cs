using System;
using System.Collections.Generic;
using System.Globalization;
using DAL.Entities;
namespace GUI.ViewModel
{
    public class DataProvider : IDataProvider
    {
        public List<Measurement> GetData()
        {
            var r = new Random();

            var list = new List<Measurement>();
            for (var i = 0; i < 2; i++)
            {
                for (var j = 4; j >= 0; j--)
                {

                    list.Add(new Measurement()
                    {
                        Timestamp =
                            ConvertToUnixTimestamp(DateTime.Now.AddSeconds(j * -5)).ToString(CultureInfo.CurrentCulture),
                        Value = r.Next(0, 10),
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
            for (var i = 0; i < 2; i++)
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
