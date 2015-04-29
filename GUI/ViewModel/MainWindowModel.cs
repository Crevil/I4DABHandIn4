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

        public Graph.Graph Graph { get; set; }
        public event EventHandler GraphDataChanged;
        public Commands Commands { get; set; }

        private IDataProvider _dataProvider;

        public MainWindowModel()
        {
            Appartments = new Appartments();
            Sensors = new Sensors();
            Commands = new Commands();
            //_dataProvider = new AppartmentTemperatureDataProvider(_selectedAppartments);
            //Graph = new Graph.Graph(_dataProvider);

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

            _selectedAppartments.CollectionChanged += (sender, e) =>
            {
                _dataProvider = new AppartmentTemperatureDataProvider(_selectedAppartments);
                Graph = new Graph.Graph(_dataProvider);
                Graph.PlotModel.InvalidatePlot(true);
                if(PropertyChanged != null) PropertyChanged(sender, new PropertyChangedEventArgs("Graph"));
                if (GraphDataChanged != null) GraphDataChanged(sender, e);
            };
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
