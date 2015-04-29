using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using DAL.Entities;
using GUI.Annotations;
using GUI.ViewModel.MultiSelection;
using OxyPlot.Wpf;

namespace GUI.ViewModel
{
    public class MainWindowModel : INotifyPropertyChanged
    {
        #region Appartment handling
        public Appartments Appartments { get; private set; }
        private readonly ObservableCollection<Appartment> _selectedAppartments = new ObservableCollection<Appartment>();
        public ObservableCollection<Appartment> SelectedAppartments
        {
            get { return _selectedAppartments; }
        }

        public event EventHandler AppartmentSelectionChanged;
        #endregion // Appartment handling

        #region Sensor handling
        public Sensors Sensors { get; private set; }

        private readonly ObservableCollection<Sensor> _selectedSensors = new ObservableCollection<Sensor>();
        public ObservableCollection<Sensor> SelectedSensors
        {
            get { return _selectedSensors; }
        }

        public event EventHandler SensorSelectionChanged;
        #endregion // Sensor lists

        #region Plot handling
        public Graph.Graph Graph { get; set; }
        public Commands Commands { get; set; }

        private PlotView _plot;
        public PlotView Plot
        {
            get { return _plot; }
            set
            {
                if (_plot == value) return;
                _plot = value;
                OnPropertyChanged();
            }
        }

        private void AppartmentsSelectionChanged()
        {
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
        private IDataProvider _dataProvider;

        #endregion //Plot handling

        public MainWindowModel()
        {
            Appartments = new Appartments();
            Sensors = new Sensors();
            Plot = new PlotView();

            Commands = new Commands();
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
            #endregion //  Setup selection event handlers

            _selectedAppartments.CollectionChanged += (sender, e) => AppartmentsSelectionChanged();
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
    }
}
