using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GUI.Annotations;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public Locations Locations { get; set; }
        public Sensors Sensors { get; set; }

        public Graph Graph { get; set; }

        public MainWindowModel()
        {
            Locations = new Locations();
            Sensors = new Sensors();
            Graph = new Graph();
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion // PropertyChanged
    }
}
