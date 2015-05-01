using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GUI.Model;
using MvvmFoundation.Wpf;

namespace GUI.ViewModel
{
    public class Commands : INotifyPropertyChanged
    {
        private readonly GDL _gdl;
        private const string LiveButtonTextNotRunning = "Read Live";
        private const string LiveButtonTextRunning = "Cancel";

        public Worker Worker { get; set; }

        #region Private ICommands

        private ICommand _staticCommand;
        private ICommand _liveCommand;
        private ICommand _singleCommand;

        #endregion // Private ICommands

        public Commands(GDL gdl)
        {
            StaticButtonEnabled = true;
            JsonButtonsEnabled = false;
            LiveButtonContent = "Read Live";
            _gdl = gdl;
        }

        private string _liveButtonContent;
        public string LiveButtonContent
        {
            get { return _liveButtonContent;}
            set
            {
                if (Equals(_liveButtonContent, value)) return;
                _liveButtonContent = value;
                NotifyPropertyChanged();
            }
        }

        private bool _staticButtonEnabled;
        public bool StaticButtonEnabled
        {
            get { return _staticButtonEnabled;}
            set
            {
                if (Equals(_staticButtonEnabled, value)) return;
                _staticButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _jsonButtonsEnabled;
        public bool JsonButtonsEnabled
        {
            get { return _jsonButtonsEnabled; }
            set
            {
                if (Equals(_jsonButtonsEnabled, value)) return;
                _jsonButtonsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Load static data
        /// </summary>
        public ICommand ClickStaticCommand
        {
            get
            {
                return _staticCommand ??
                   (_staticCommand = new RelayCommand(StaticCommand));
            }
        }
        private void StaticCommand()
        {
            StaticButtonEnabled = false;
            Task.Run(() =>_gdl.LoadStaticData());
        }

        /// <summary>
        ///  Load measurements live
        /// </summary>
        public ICommand ClickLiveCommand
        {
            get
            {
                return _liveCommand ??
                   (_liveCommand = new RelayCommand(LiveCommand));
            }
        }
        private void LiveCommand()
        {
            if (LiveButtonContent == LiveButtonTextRunning)
            {
                Worker.Cancel();
            }
            else
            {
                if (!Worker.DoLive()) return;
            }
            LiveButtonContent = (_liveButtonContent == LiveButtonTextNotRunning) ? LiveButtonTextRunning : LiveButtonTextNotRunning;
            
        }

        /// <summary>
        ///  Load a single measurement set
        /// </summary>
        public ICommand ClickSingleCommand
        {
            get
            {
                return _singleCommand ??
                   (_singleCommand = new RelayCommand(SingleCommand));
            }
        }
        private void SingleCommand()
        {
            Worker.DoSingle();
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion // PropertyChanged

    }
}
