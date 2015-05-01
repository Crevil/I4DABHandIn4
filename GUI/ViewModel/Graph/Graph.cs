using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DAL.Entities;
using GUI.Annotations;
using OxyPlot;
using OxyPlot.Wpf;

namespace GUI.ViewModel.Graph
{
    public class Graph : INotifyPropertyChanged
    {
        private PlotView _plotView;
        private readonly GraphPlot _type;

        public PlotView PlotView
        {
            get { return _plotView; }
            set
            {
                if (Equals(_plotView, value)) return;
                _plotView = value;
                OnPropertyChanged();
            }
        }

        public Graph([NotNull] ICollection<Measurement> measurements, [NotNull] GraphPlot type)
        {
            if (measurements == null) throw new ArgumentNullException("measurements");
            if (type == null) throw new ArgumentNullException("type");

            _type = type;

            PlotView = new PlotView {Model = new PlotModel()};

            _type.PlotModel = PlotView.Model;
            _type.Measurements = measurements;

            _type.SetUpModel();
            _type.LoadData();

            PlotView.Model.Title = type.Title ?? "Data plot";
            PlotView.InvalidatePlot();
        }

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
                                                       MarkerType.Star,
                                                       MarkerType.Plus,
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
