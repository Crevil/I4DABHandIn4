using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GUI.Annotations;

namespace GUI.ViewModel
{
    public class ButtonsModel : INotifyPropertyChanged
    {
        private bool _jsonReadButtonsEnabled;

        private const string LiveButtonTextNotRunning = "Read Live";
        private const string LiveButtonTextRunning = "Cancel";
        private bool _staticButtonEnabled;
        private string _liveButtonContent;

        public ButtonsModel()
        {
            LiveButtonContent = "Read Live";
            StaticButtonEnabled = true;
            JsonReadButtonsEnabled = false;

        }

        public string LiveButtonContent
        {
            get { return _liveButtonContent; }
            set
            {
                if (Equals(_liveButtonContent, value)) return;
                _liveButtonContent = value;
                OnPropertyChanged();
            }
        }

        public bool StaticButtonEnabled
        {
            get { return _staticButtonEnabled; }
            set
            {
                if (Equals(_staticButtonEnabled, value)) return;
                _staticButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool JsonReadButtonsEnabled
        {
            get { return _jsonReadButtonsEnabled; }
            set
            {
                if (Equals(_jsonReadButtonsEnabled, value)) return;
                _jsonReadButtonsEnabled = value;
                OnPropertyChanged();
            }
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

        public void SetStaticVisibilty()
        {
            StaticButtonEnabled = false;
        }
    }
}