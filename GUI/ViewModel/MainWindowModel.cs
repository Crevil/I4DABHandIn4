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

            SensorTypes = new ObservableCollection<string>();

            foreach (var s in Sensors.GroupBy(g => g.Description))// List of sensortypes on GUI
            {
                SensorTypes.Add(s.Key);
            }

            #region Setup selection event handlers

            _selectedAppartments.CollectionChanged += (sender, e) =>
            {
                if (AppartmentSelectionChanged == null) return;
                AppartmentSelectionChanged(SelectedAppartments, new SelectionChangedArgs { Sensor = SelectedSensor, Appartments = SelectedAppartments });
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
        
        private Sensor _selectedSensor;
        public Sensor SelectedSensor
        {
            get { return _selectedSensor; }
            set { _selectedSensor = value; }
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

        private void AppartmentsSelectionChanged()
        {
            // If nothing is selected, clear plot model
            if (_selectedAppartments.Count <= 0 && Graph != null)
            {
                Graph.PlotView.Model = null;
                return;
            }
            var g = new GDL();
            Graph = new Graph.Graph(g.GetMeasurements(_selectedAppartments, _selectedSensor));

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
