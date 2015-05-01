using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using DAL.Entities;

namespace GUI.ViewModel.Converters
{
    public class SensorTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value as Tuple<string, string>;
            if (type == null) return string.Format("Error converting sensor");
            var r = string.Format("{0}", type.Item1.Replace("(*", "").Replace(type.Item2 + "*)", "").Replace("*)", ""));
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
