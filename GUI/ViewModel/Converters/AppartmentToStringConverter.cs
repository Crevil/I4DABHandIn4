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
    public class AppartmentToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var appartment = value as Appartment;
            if (appartment == null) return string.Format("Error converting appartment");

            return string.Format("Floor {0}, No {1}", appartment.Floor, appartment.No);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
