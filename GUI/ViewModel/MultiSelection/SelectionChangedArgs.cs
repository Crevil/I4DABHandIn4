using System;
using System.Collections.ObjectModel;
using DAL.Entities;

namespace GUI.ViewModel.MultiSelection
{
    public class SelectionChangedArgs : EventArgs
    {
        public Sensor Sensor { get; set; }
        public ObservableCollection<Appartment> Appartments { get; set; }

    }
}