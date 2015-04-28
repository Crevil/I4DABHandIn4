using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GUI.Annotations;
using GUI.ViewModel.Graph.Types;
using OxyPlot;

namespace GUI.ViewModel.Graph
{
    public class Graph : INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        private readonly IGraphType _type;
        private IDataProvider _dataProvider;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { _plotModel = value; OnPropertyChanged(); }
        }

        public IDataProvider DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; OnPropertyChanged(); }
        }

        public Graph(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

            PlotModel = new PlotModel();
            _type = new TemperatureGraph {PlotModel = PlotModel, DataProvider = dataProvider};
            _type.SetUpModel();
            _type.LoadData();
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
