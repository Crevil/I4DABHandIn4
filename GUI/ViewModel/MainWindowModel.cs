using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using DAL;
using DAL.Entities;
using GUI.Annotations;
using GUI.Model;
using GUI.ViewModel.Converters;
using GUI.ViewModel.Graph;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private readonly GDL _gdl;
        private const int Max = 11803;

        public Commands Commands { get; set; }
        public Progress Progress { get; set; }

        public MainWindowModel([NotNull] GDL gdl, [NotNull] DbRepository repository)
        {
            if (gdl == null) throw new ArgumentNullException("gdl");
            if (repository == null) throw new ArgumentNullException("repository");

            _gdl = gdl;

            Commands = new Commands(_gdl);

            // Initialisere progress class og workeren
            Progress = new Progress(0, Max);
            Commands.Worker = new Worker(Progress, repository);

            Appartments = new ObservableCollection<Appartment>(gdl.GetAppartments());
            Sensors = new ObservableCollection<Sensor>(_gdl.GetSensors());
            UpdateSensorTypesList();

            _measurementsLock = new object();
            Measurements = new ObservableCollection<Measurement>();

            // Event on appartment selection changing
            SelectedAppartments.CollectionChanged += (sender, e) => GetMeasurements();

            // Event on static data loaded
            _gdl.StaticDataLoaded += (sender, e) => StaticDataLoaded();

            // Event on new loaded data
            Commands.Worker.ProgressChanged += (sender, e) => GetMeasurements();
            Commands.Worker.ProcessCompleted += (sender, e) => GetMeasurements();
        }

        private void GetMeasurements()
        {
            if (SelectedAppartments.Count <= 0 || SelectedSensorType == null) return;

            var measurements = Task.Run(() => _gdl.GetMeasurements(SelectedAppartments, SelectedSensorType.Item1));
            Measurements = new ObservableCollection<Measurement>(measurements.Result);
        }

        private void StaticDataLoaded()
        {
            var sensorFuture = Task.Run(() => _gdl.GetSensors());
            sensorFuture.ContinueWith(s =>
            {
                Sensors = new ObservableCollection<Sensor>(sensorFuture.Result);
                UpdateSensorTypesList();
            });

            var appartmentFuture = Task.Run(() => _gdl.GetAppartments());
            Appartments = new ObservableCollection<Appartment>(appartmentFuture.Result);
            Commands.JsonButtonsEnabled = true;
        }

        #region Appartment handling

        private ObservableCollection<Appartment> _appartments;
        public ObservableCollection<Appartment> Appartments
        {
            get { return _appartments; }
            set
            {
                if (Equals(_appartments, value)) return;
                _appartments = value; 
                OnPropertyChanged();
            }
        }

        private readonly ObservableCollection<Appartment> _selectedAppartments = new ObservableCollection<Appartment>();
        public ObservableCollection<Appartment> SelectedAppartments
        {
            get { return _selectedAppartments; }
        }
        #endregion // Appartment handling

        #region Sensor handling
        private ObservableCollection<Tuple<string, string>> _sensorTypes;
        private ObservableCollection<Sensor> _sensors;
        private Tuple<string, string> _selectedSensorType;

        public ObservableCollection<Tuple<string, string>> SensorTypes
        {
            get { return _sensorTypes; }
            private set
            {
                if (Equals(_sensorTypes, value)) return;
                _sensorTypes = value; 
                OnPropertyChanged();}
        }

        public ObservableCollection<Sensor> Sensors
        {
            get { return _sensors; }
            private set
            {
                if(Equals(_sensors, value)) return;
                _sensors = value;
                UpdateSensorTypesList();
            }
        }

        private void UpdateSensorTypesList()
        {
            var types = new ObservableCollection<Tuple<string, string>>();

            foreach (var s in Sensors.GroupBy(g => g.Description, g => new Tuple<string, string>(g.Description, g.Unit)))
                types.Add(s.First());

            SensorTypes = types;
        }

        public Tuple<string, string> SelectedSensorType
        {
            get { return _selectedSensorType; }
            set
            {
                if (Equals(_selectedSensorType, value)) return;
                _selectedSensorType = value;
                GetMeasurements();
            }
        }
        #endregion // Sensor lists
        
        #region Plot handling

        private Graph.Graph _graph;
        private ObservableCollection<Measurement> _measurements;
        private readonly object _measurementsLock;

        public Graph.Graph Graph
        {
            get { return _graph; }
            set
            {
                if (Equals(_graph, value)) return;
                _graph = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Measurement> Measurements
        {
            get { return _measurements; }
            set
            {
                if (Equals(_measurements, value)) return;
                lock (_measurementsLock)
                    _measurements = value;

                if(_measurements.Count > 0)
                    Dispatcher.CurrentDispatcher.BeginInvoke(new Action(RedrawGraph));
            }
        }

        private void RedrawGraph()
        {
            // If nothing is selected, do nothing
            if (SelectedAppartments.Count <= 0 || SelectedSensorType == null)
                return;

            var converter = new SensorTypeToStringConverter();

            var type = new GraphPlot {
                            Title = (string)converter.Convert(SelectedSensorType, null, null, null),
                            Unit = SelectedSensorType.Item2,
                            YMininumValue = (Measurements.Min(m => m.Value) < 0) ? Measurements.Min(m => m.Value) : 0
            };

            Graph = new Graph.Graph(_measurements, type);
        }
        #endregion //Plot handling

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
