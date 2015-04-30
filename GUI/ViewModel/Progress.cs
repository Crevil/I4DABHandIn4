using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GUI.ViewModel
{
    public class Progress : INotifyPropertyChanged
    {
        private int _current;
        public int Current
        {
            get { return _current; }
            set
            {
                if (_current == value) return;
                _current = value;
                NotifyPropertyChanged();
            }
        }

        private int _max;
        public int Max
        {
            get { return _max; }
            set
            {
                if (_max == value) return;
                _max = value;
                NotifyPropertyChanged();
            }
        }

        public Progress(int current, int max)
        {
            _current = current;
            _max = max;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
