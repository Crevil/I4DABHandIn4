using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using DAL.Entities;
using GUI.Annotations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GUI.ViewModel
{
    public class Graph : INotifyPropertyChanged
    {
        private PlotModel _plotModel;
        private DateTime _lastUpdate = DateTime.Now;
        private readonly IDataProvider _data;

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set
            { 
                _plotModel = value; 
                OnPropertyChanged();
            }
        }

        public Graph()
        {
            PlotModel = new PlotModel();
            _data = new DataProvider();

            SetUpModel();
            LoadData();
        }


        private readonly List<OxyColor> _colors = new List<OxyColor>
                                            {
                                                OxyColors.Green,
                                                OxyColors.IndianRed,
                                                OxyColors.Coral,
                                                OxyColors.Chartreuse,
                                                OxyColors.Azure
                                            };

        private readonly List<MarkerType> _markerTypes = new List<MarkerType>
                                                   {
                                                       MarkerType.Plus,
                                                       MarkerType.Star,
                                                       MarkerType.Diamond,
                                                       MarkerType.Triangle,
                                                       MarkerType.Cross
                                                   };


        private void SetUpModel()
        {
            PlotModel.LegendOrientation = LegendOrientation.Horizontal;
            PlotModel.LegendPlacement = LegendPlacement.Outside;
            PlotModel.LegendPosition = LegendPosition.TopRight;
            PlotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            PlotModel.LegendBorder = OxyColors.Black;

            var dateAxis = new DateTimeAxis()
            {
                Position = AxisPosition.Bottom,
                Title = "Date",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80
            };
            PlotModel.Axes.Add(dateAxis);

            var valueAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid, 
                MinorGridlineStyle = LineStyle.Dot, 
                Title = "Value"
            };
            PlotModel.Axes.Add(valueAxis);

        }

        private void LoadData()
        {
            var measurements = _data.GetData();

            var dataPerDetector = measurements.GroupBy(m => m.SensorId).OrderBy(m => m.Key).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = _colors[data.Key],
                    MarkerType = _markerTypes[data.Key],
                    CanTrackerInterpolatePoints = false,
                    Title = string.Format("Sensor {0}", data.Key),
                    Smooth = false,
                };

                AddMeasurementPoint(data, lineSerie);

                PlotModel.Series.Add(lineSerie);
            }
            _lastUpdate = DateTime.Now;
        }

        private void AddMeasurementPoint(IEnumerable<Measurement> data, [NotNull] LineSeries lineSerie)
        {
            if (lineSerie == null) throw new ArgumentNullException("lineSerie");

            data.ToList().ForEach(
                    d =>
                        lineSerie.Points.Add
                        (
                            new DataPoint(
                                Axis.ToDouble(
                                    DataProvider.ConvertFromUnixTimestamp(
                                        double.Parse(d.Timestamp, CultureInfo.CurrentCulture)
                                    )
                                ),
                                d.Value
                            )
                        )
                   );
        }

        public void UpdateModel()
        {
            var measurements = _data.GetUpdateData(_lastUpdate);
            var dataPerDetector = measurements.GroupBy(m => m.SensorId).OrderBy(m => m.Key).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = PlotModel.Series[data.Key] as LineSeries;
                if (lineSerie != null)
                {
                    AddMeasurementPoint(data, lineSerie);
                }
            }
            _lastUpdate = DateTime.Now;
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
