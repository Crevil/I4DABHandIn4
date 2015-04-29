using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DAL;
using DAL.Entities;
using GUI.Model;
using MvvmFoundation.Wpf;

namespace GUI.ViewModel
{
    public class Commands : INotifyPropertyChanged
    {
        public Worker Worker { get; set; }

        private string _liveButtonContent;
        public string LiveButtonContent
        {
            get { return _liveButtonContent;}
            set
            {
                if (_liveButtonContent == value) return;
                _liveButtonContent = value;
                NotifyPropertyChanged();
            }
        }

        private const string liveButtonTextNotRunning = "Read Live";
        private const string liveButtonTextRunning = "Cancel";

        public Commands()
        {
            StaticButtonEnabled = true;
            LiveButtonContent = "Read Live";
        }

        private bool _staticButtonEnabled;
        public bool StaticButtonEnabled
        {
            get { return _staticButtonEnabled;}
            set
            {
                if (_staticButtonEnabled == value) return;
                _staticButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ICommand _staticCommand;
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
            ThreadPool.QueueUserWorkItem(delegate
            {
                GDL.LoadOriginal();
            });

        }

        private ICommand _liveCommand;
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
            if (LiveButtonContent == liveButtonTextRunning)
            {
                Worker.Cancel();
            }
            else
            {
                if (!Worker.DoLive()) return;
            }
            LiveButtonContent = (_liveButtonContent == liveButtonTextNotRunning) ? liveButtonTextRunning : liveButtonTextNotRunning;
            
        }

        private ICommand _singleCommand;
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
    }
}
