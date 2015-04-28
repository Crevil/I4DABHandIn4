using System.Collections.ObjectModel;
using System.ComponentModel;
using DAL.Entities;

namespace GUI.ViewModel
{
    public class Appartments : ObservableCollection<Appartment>, INotifyPropertyChanged
    {
        public Appartments()
        {
            Add(new Appartment { AppartmentId = 1 });
            Add(new Appartment { AppartmentId = 2 });
            Add(new Appartment { AppartmentId = 3 });

        }
    }
}
