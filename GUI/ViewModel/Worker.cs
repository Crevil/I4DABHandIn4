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
        private Progress _progress;
        private Graph.Graph _graph;
        private int _count = 0;
        private int _max = 11803;

        // 1 = single, 2 = live
        private int state = 0;

        public event SCM.ProgressChangedEventHandler ProgressChanged;

        public event SCM.PropertyChangedEventHandler PropertyChanged;

        public event SCM.AsyncCompletedEventHandler AsyncCompleted;

        public Worker(Progress p, Graph.Graph graph, DbRepository repository)
        {
            Repository = repository;
            _progress = p;

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
            if (state == 0) backgroundWorker.DoWork += BackgroundWorker_DoSingle;
            else if (state == 2)
            {
                backgroundWorker.DoWork -= BackgroundWorker_DoLiveLogging;
                backgroundWorker.DoWork += BackgroundWorker_DoSingle;
            }

            state = 1;

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
            if (state == 0) backgroundWorker.DoWork += BackgroundWorker_DoLiveLogging;
            else if (state == 1)
            {
                backgroundWorker.DoWork -= BackgroundWorker_DoSingle;
                backgroundWorker.DoWork += BackgroundWorker_DoLiveLogging;
            }

            state = 2;

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
            _progress.Max = _max;
            _count = _progress.Current;

            while (_count < _max)
            {
                _count++;

                Repository.AddCollectionOfMeasurements(GDL.LoadJson(_count));

                // UPDATE VIEW MODEL!

                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                backgroundWorker.ReportProgress(_count);
                Thread.Sleep(5000);
            }

        }

        private void BackgroundWorker_DoSingle(object sender, SCM.DoWorkEventArgs e)
        {
            backgroundWorker.CancelAsync();
            _progress.Max = _max;
            _count = _progress.Current;
            _count++;

            Task.Run(() => Repository.AddCollectionOfMeasurements(GDL.LoadJson(_count)));

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
            _progress.Current = _count;
            if(_graph != null) _graph.UpdateModel();
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, SCM.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _progress.Current = _count;
                if (_graph != null) _graph.UpdateModel();
            }
            else if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An Error Occurred in BackgroundWorker");
            }
        }
    }
}
