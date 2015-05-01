using System;
using System.Threading;
using System.Windows;
using DAL;
using GUI.Annotations;
using GUI.Model;
using SCM = System.ComponentModel;

namespace GUI.ViewModel
{
    public class Worker
    {
        private readonly SCM.BackgroundWorker _backgroundWorker;
        private readonly DbRepository _repository;
        private readonly Progress _progress;

        private int _count = 0;
        private const int Max = 11803;

        // 1 = single, 2 = live
        private int _state = 0;

        // Events
        public event SCM.ProgressChangedEventHandler ProgressChanged;
        public event SCM.AsyncCompletedEventHandler ProcessCompleted;

        public Worker([NotNull] Progress p, [NotNull] DbRepository repository)
        {
            if (p == null) throw new ArgumentNullException("p");
            if (repository == null) throw new ArgumentNullException("repository");

            _repository = repository;
            _progress = p;

            if (_backgroundWorker != null)
            {
                _backgroundWorker.Dispose();
            }

            _backgroundWorker = new SCM.BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_WorkerCompleted;        
        }

        #region Worker setups
        /// <summary>
        /// Setup worker to do a single data loading
        /// </summary>
        /// <returns></returns>
        public bool DoSingle()
        {
            switch (_state)
            {
                case 0:
                    _backgroundWorker.DoWork += BackgroundWorker_DoSingle;
                    break;
                case 2:
                    _backgroundWorker.DoWork -= BackgroundWorker_DoLiveLogging;
                    _backgroundWorker.DoWork += BackgroundWorker_DoSingle;
                    break;
            }

            _state = 1;

            try
            {
                _backgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set worker up to live data loading
        /// </summary>
        /// <returns></returns>
        public bool DoLive()
        {
            switch (_state)
            {
                case 0:
                    _backgroundWorker.DoWork += BackgroundWorker_DoLiveLogging;
                    break;
                case 1:
                    _backgroundWorker.DoWork -= BackgroundWorker_DoSingle;
                    _backgroundWorker.DoWork += BackgroundWorker_DoLiveLogging;
                    break;
            }

            _state = 2;

            try
            {
                _backgroundWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Cancel()
        {
            _backgroundWorker.CancelAsync();
        }
        #endregion // Worker setups

        #region Work methods
        private void BackgroundWorker_DoLiveLogging(object sender, SCM.DoWorkEventArgs e)
        {
            while (_count < Max && !e.Cancel)
            {
                Backgroundworker_DoAddMeasurements(sender, e);
                Thread.Sleep(1000);
            }

        }

        private void Backgroundworker_DoAddMeasurements(object sender, SCM.DoWorkEventArgs e)
        {
            _progress.Max = Max;
            _count = _progress.Current;
            _count++;

            if (_count == 8) _count++;

            _repository.AddCollectionOfMeasurements(GDL.LoadJson(_count)).Wait();

            _backgroundWorker.ReportProgress(_count);

            if (_backgroundWorker.CancellationPending)
            
            e.Cancel = true;
        }

        private void BackgroundWorker_DoSingle(object sender, SCM.DoWorkEventArgs e)
        {
            _backgroundWorker.CancelAsync();

            Backgroundworker_DoAddMeasurements(sender, e);
        }
        #endregion // Work methods

        #region Events
        private void BackgroundWorker_ProgressChanged(object sender, SCM.ProgressChangedEventArgs e)
        {
            _progress.Current = _count;
            if(ProgressChanged != null) 
                ProgressChanged(sender, e);
        }

        private void BackgroundWorker_WorkerCompleted(object sender, SCM.RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _progress.Current = _count;
                if (ProcessCompleted != null) ProcessCompleted(sender, e);
            }
            else if (e.Error != null)
            {
                // An error was thrown by the DoWork event handler.
                MessageBox.Show(e.Error.Message, "An Error Occurred in BackgroundWorker");
            }
        }
        #endregion // Events
    }
}
