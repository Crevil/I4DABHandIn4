using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DAL.Entities;
using GUI.Annotations;
using GUI.ViewModel.Graph.Types;
using OxyPlot;
using OxyPlot.Wpf;

namespace GUI.ViewModel.Graph
{
    public class Graph : INotifyPropertyChanged
    {
        private PlotView _plotView;
        private readonly IGraphType _type;
        private ICollection<Measurement> _measurements;

        public PlotView PlotView
        {
            get { return _plotView; }
            set { _plotView = value; OnPropertyChanged(); }
        }

        public Graph(ICollection<Measurement> measurements)
        {
            _measurements = measurements;
            PlotView = new PlotView {Model = new PlotModel()};

            _type = new TemperatureGraph
            {
                PlotModel = PlotView.Model, 
                Measurements = _measurements
            };
            _type.SetUpModel();
            _type.LoadData();

            PlotView.Model.Title = "Data";
            PlotView.InvalidatePlot();
        }
        public void UpdateModel() { _type.UpdateModel(); }

        #region Plot markup
        public static readonly List<OxyColor> Colors = new List<OxyColor>
                                            {
                                                OxyColors.Green,
                                                OxyColors.IndianRed,
                                                OxyColors.Coral,
                                                OxyColors.Chartreuse,
                                                OxyColors.Azure
                                            };

        public static readonly List<MarkerType> MarkerTypes = new List<MarkerType>
                                                   {
                                                       MarkerType.Plus,
                                                       MarkerType.Star,
                                                       MarkerType.Diamond,
                                                       MarkerType.Triangle,
                                                       MarkerType.Cross
                                                   };
        #endregion // Plot markup

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
