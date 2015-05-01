using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DAL;
using DAL.Entities;
using GUI.Annotations;
using GUI.Model;
using GUI.ViewModel.Graph;
using GUI.ViewModel.Graph.Types;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private readonly GDL _gdl;
        private const int Max = 11803;
        private readonly DbRepository _repository;

        public Commands Commands { get; set; }
        public Progress Progress { get; set; }

        public MainWindowModel([NotNull] GDL gdl, [NotNull] DbRepository repository)
        {
            if (gdl == null) throw new ArgumentNullException("gdl");
            if (repository == null) throw new ArgumentNullException("repository");

            _gdl = gdl;
            _repository = repository;

            Commands = new Commands(_gdl);

            // Initialisere progress class og workeren
            Progress = new Progress(0, Max);
            Commands.Worker = new Worker(Progress, _repository);

            Appartments = new ObservableCollection<Appartment>(gdl.GetAppartments());
            Sensors = new ObservableCollection<Sensor>(_gdl.GetSensors());
            UpdateSensorTypesList();

            _measurements = new ObservableCollection<Measurement>();

            // Event on appartment selection changing
            _selectedAppartments.CollectionChanged += (sender, e) => RedrawGraph();
            _measurements.CollectionChanged += (sender, e) => RedrawGraph();

            // Event on static data loaded
            _gdl.StaticDataLoaded += (sender, e) => StaticDataLoaded();

            // Event on new loaded data
            Commands.Worker.ProgressChanged += (sender, e) => HandleNewData();
            Commands.Worker.ProcessCompleted += (sender, e) => HandleNewData();
        }

        private void HandleNewData()
        {
            var measurements = Task.Run(() => _gdl.GetMeasurements(SelectedAppartments, SelectedSensorType));
            _measurements = new ObservableCollection<Measurement>(measurements.Result);
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

            Task.Run(() => RedrawGraph()); // Update graph
        }

        private void UpdateSensorTypesList()
        {
            var types = new ObservableCollection<string>();

            foreach (var s in Sensors.GroupBy(g => g.Description))
                types.Add(s.Key);

            SensorTypes = types;
        }

        #region Appartment handling

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

        public event EventHandler AppartmentSelectionChanged;
        #endregion // Appartment handling

        #region Sensor handling

        public ObservableCollection<string> SensorTypes
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

        private string _selectedSensorType;

        public string SelectedSensorType
        {
            get { return _selectedSensorType; }
            set
            {
                if (_selectedSensorType == value) return;
                _selectedSensorType = value;
                RedrawGraph(); 
            }
        }

        #endregion // Sensor lists
        
        #region Plot handling

        private Graph.Graph _graph;
        private ObservableCollection<Appartment> _appartments;
        private ObservableCollection<string> _sensorTypes;
        private ObservableCollection<Sensor> _sensors;
        private ObservableCollection<Measurement> _measurements; 

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

        private void RedrawGraph()
        {
            // If nothing is selected, do nothing
            if ((SelectedAppartments.Count <= 0 || SelectedSensorType == null) && Graph != null )
                return;

            IGraphType type;
            switch (SelectedSensorType)
            {
                case "Temperatue":
                type = new TemperatureGraph();
                break;
                case "Humidity":
                type = new HumidityGraph();
                break;
                default:
                type = new TemperatureGraph();
                break;
            }

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
