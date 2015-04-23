using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GUI.ViewModel
{
    public class Location
    {
        public string Name { get; set; }
    }

    public class Sensor
    {
        public string Name { get; set; }
    }
    public class Locations : ObservableCollection<Location>, INotifyPropertyChanged
    {
        public Locations()
        {
            Add(new Location { Name = "Test1" });
            Add(new Location { Name = "Test2" });
            Add(new Location { Name = "Test3" });

        }
    }

    public class Sensors : ObservableCollection<Sensor>, INotifyPropertyChanged
    {
        public Sensors()
        {
            Add(new Sensor { Name = "Sensor Test1" });
            Add(new Sensor { Name = "Sensor Test2" });
            Add(new Sensor { Name = "Sensor Test3" });

        }
    }
}
