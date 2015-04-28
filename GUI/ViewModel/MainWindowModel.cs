using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private IDataProvider _dataProvider;

        public MainWindowModel()
        {
            Appartments = new Appartments();
            Sensors = new Sensors();

            _dataProvider = new DataProvider();
            Graph = new Graph.Graph(_dataProvider);

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
