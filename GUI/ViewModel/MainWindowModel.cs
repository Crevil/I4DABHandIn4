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
using GUI.ViewModel.MultiSelection;
using OxyPlot.Wpf;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        public MainWindowModel()
        {
            //var db = new DbRepository();
            //Appartments = new ObservableCollection<Appartment>(db.Appartments);

            Appartments = new ObservableCollection<Appartment> // List of appartments on GUI
            {
                new Appartment {AppartmentId = 1},
                new Appartment {AppartmentId = 2},
                new Appartment {AppartmentId = 3}
            };

            Sensors = new ObservableCollection<Sensor> // List of sensors on GUI
            {
                new Sensor {Description = "Sensor Test1"},
                new Sensor {Description = "Sensor Test1"},
                new Sensor {Description = "Sensor Test2"},
                new Sensor {Description = "Sensor Test3"}
            };

            Commands = new Commands();
            Plot = new PlotView();

            SensorTypes = new ObservableCollection<string>();

            foreach (var s in Sensors.GroupBy(g => g.Description))// List of sensortypes on GUI
            {
                SensorTypes.Add(s.Key);
            }

            #region Setup selection event handlers
            _selectedSensors.CollectionChanged += (sender, e) =>
            {
                if (SensorSelectionChanged == null) return;
                SensorSelectionChanged.Invoke(SelectedSensors, new SelectionChangedArgs { Sensors = SelectedSensors, Appartments = SelectedAppartments });
            };

            _selectedAppartments.CollectionChanged += (sender, e) =>
            {
                if (AppartmentSelectionChanged == null) return;
                AppartmentSelectionChanged(SelectedAppartments, new SelectionChangedArgs { Sensors = SelectedSensors, Appartments = SelectedAppartments });
            };


            _selectedAppartments.CollectionChanged += (sender, e) => AppartmentsSelectionChanged();
            #endregion //  Setup selection event handlers
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
        private readonly ObservableCollection<Sensor> _selectedSensors = new ObservableCollection<Sensor>();
        public ObservableCollection<Sensor> SelectedSensors
        {
            get { return _selectedSensors; }
        }

        public event EventHandler SensorSelectionChanged;
        #endregion // Sensor lists
        public Commands Commands { get; set; }

        #region Plot handling
        public Graph.Graph Graph { get; set; }
        private PlotView _plot;
        private IDataProvider _dataProvider;
        public PlotView Plot
        {
            get { return _plot; }
            set
            {
                if (Equals(_plot, value)) return;
                _plot = value;
                OnPropertyChanged();
            }
        }

        private void AppartmentsSelectionChanged()
        {
            // If nothing is selected, clear plot model
            if (_selectedAppartments.Count <= 0)
            {
                Plot.Model = null;
                return;
            }

            _dataProvider = new AppartmentTemperatureDataProvider(_selectedAppartments);
            Graph = new Graph.Graph(_dataProvider);

            Plot.Model = Graph.PlotModel;
            Plot.Model.Title = "Data";
            Plot.InvalidatePlot();
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
