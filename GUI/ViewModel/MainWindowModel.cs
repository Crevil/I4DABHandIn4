using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAL;
using DAL.Entities;
using GUI.Annotations;
using GUI.Model;
using GUI.ViewModel.Graph;
using GUI.ViewModel.Graph.Types;
using GUI.ViewModel.MultiSelection;
using OxyPlot.Wpf;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        private GDL _gdl;
        public MainWindowModel([NotNull] GDL gdl)
        {
            if (gdl == null) throw new ArgumentNullException("gdl");

            _gdl = gdl;

            Commands = new Commands();

            Appartments = new ObservableCollection<Appartment>(gdl.GetAppartments());
            Sensors = new ObservableCollection<Sensor>(_gdl.GetSensors()); // List of sensors on GUI

            SensorTypes = new ObservableCollection<string>();

            foreach (var s in Sensors.GroupBy(g => g.Description))// List of sensortypes on GUI
                SensorTypes.Add(s.Key);

            // Setup selection event handler
            _selectedAppartments.CollectionChanged += (sender, e) => SelectionChanged();
        }

        #region Appartment handling
        public ObservableCollection<Appartment> Appartments { get; private set; }
        private readonly ObservableCollection<Appartment> _selectedAppartments = new ObservableCollection<Appartment>();
        public ObservableCollection<Appartment> SelectedAppartments
        {
            get { return _selectedAppartments; }
        }

        public event EventHandler AppartmentSelectionChanged;
        #endregion // Appartment handling

        #region Sensor handling
        public ObservableCollection<string> SensorTypes { get; private set; }
        public ObservableCollection<Sensor> Sensors { get; private set; }

        private string _selectedSensorType;

        public string SelectedSensorType
        {
            get { return _selectedSensorType; }
            set
            {
                if (_selectedSensorType == value) return;
                _selectedSensorType = value;
                SelectionChanged(); 
            }
        }

        #endregion // Sensor lists
        public Commands Commands { get; set; }

        #region Plot handling

        private Graph.Graph _graph;
        public Graph.Graph Graph
        {
            get { return _graph; }
            set
            {
                if (_graph == value) return;
                _graph = value;
                OnPropertyChanged();
            }
        }

        private void SelectionChanged()
        {
            // If nothing is selected, clear plot model
            if ((SelectedAppartments.Count <= 0 || SelectedSensorType == null) && Graph != null )
                return;

            var measurements = _gdl.GetMeasurements(SelectedAppartments, SelectedSensorType);
            // This might be done better! Breaking OCP
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

            Graph = new Graph.Graph(measurements, type);
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
