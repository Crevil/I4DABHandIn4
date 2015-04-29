using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DAL;
using GUI.Model;
using SCM = System.ComponentModel;

namespace GUI.ViewModel
{
    public class Worker
    {
        private SCM.BackgroundWorker backgroundWorker;
        private DbRepository Repository;
        private Progress progress;
        private int _count = 0;
        private int _max = 11803;
        
        public event SCM.ProgressChangedEventHandler ProgressChanged;

        public event SCM.PropertyChangedEventHandler PropertyChanged;

        public event SCM.AsyncCompletedEventHandler AsyncCompleted;

        public Worker(Progress p)
        {
            Repository = new DbRepository();
            progress = p;

            if (backgroundWorker != null)
            {
                backgroundWorker.Dispose();
            }

            backgroundWorker = new SCM.BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;        
        }

        public bool DoSingle()
        {
            backgroundWorker.DoWork -= BackgroundWorker_DoLiveLogging;
            backgroundWorker.DoWork += BackgroundWorker_DoSingle;

            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DoLive()
        {
            backgroundWorker.DoWork -= BackgroundWorker_DoSingle;
            backgroundWorker.DoWork += BackgroundWorker_DoLiveLogging;

            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Cancel()
        {
            backgroundWorker.CancelAsync();
        }

        private void BackgroundWorker_DoLiveLogging(object sender, SCM.DoWorkEventArgs e)
        {
            progress.Max = _max;
            _count = progress.Current;

            while (_count < _max)
            {
                _count++;

                Task t = Repository.AddCollectionOfMeasurements(GDL.LoadJson(_count));

                // UPDATE VIEW MODEL!

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                backgroundWorker.ReportProgress(_count);
                Thread.Sleep(1000);
            }

        }

        private void BackgroundWorker_DoSingle(object sender, SCM.DoWorkEventArgs e)
        {
            backgroundWorker.CancelAsync();
            progress.Max = _max;
            _count = progress.Current;
            _count++;

            Task t = Repository.AddCollectionOfMeasurements(GDL.LoadJson(_count));
            // UPDATE VIEW MODEL!
            
            backgroundWorker.ReportProgress(_count);

            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }          
        }

        protected virtual void OnProgressChanged(SCM.ProgressChangedEventArgs e)
        {
            var handler = ProgressChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPropertyChanged(SCM.PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, SCM.ProgressChangedEventArgs e)
        {
            progress.Current = _count;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, SCM.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                progress.Current = _count;
            }
            else if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An Error Occurred in BackgroundWorker");
            }
        }
    }
}
