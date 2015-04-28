using System.Collections.ObjectModel;
using System.ComponentModel;
using DAL.Entities;

namespace GUI.ViewModel
{
    public class Sensors : ObservableCollection<Sensor>, INotifyPropertyChanged
    {
        public Sensors()
        {
            Add(new Sensor { Description = "Sensor Test1" });
            Add(new Sensor { Description = "Sensor Test2" });
            Add(new Sensor { Description = "Sensor Test3" });

        }
    }
}