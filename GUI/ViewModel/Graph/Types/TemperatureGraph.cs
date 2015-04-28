using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAL.Entities;
using GUI.Annotations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GUI.ViewModel.Graph.Types
{
    public class TemperatureGraph : IGraphType
    {
        private PlotModel _plotModel; 
        private DateTime _lastUpdate = DateTime.Now;
        public IDataProvider DataProvider { get; set; }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { _plotModel = value; }
        }

        public TemperatureGraph()
        {
        }

        public void SetUpModel()
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

            var valueAxisLeft = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Temperature"
            };
            PlotModel.Axes.Add(valueAxisLeft);
        }

        public void LoadData()
        {
            var measurements = DataProvider.GetData();

            var dataPerDetector = measurements.GroupBy(m => m.SensorId).OrderBy(m => m.Key).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = Graph.Colors[data.Key],
                    MarkerType = Graph.MarkerTypes[data.Key],
                    CanTrackerInterpolatePoints = false,
                    Title = string.Format("Sensor {0}", data.Key),
                    Smooth = false
                };

                AddMeasurementPoint(data, lineSerie);

                _plotModel.Series.Add(lineSerie);
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
                                    ViewModel.DataProvider.ConvertFromUnixTimestamp(
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
            var measurements = DataProvider.GetUpdateData(_lastUpdate);
            var dataPerDetector = measurements.GroupBy(m => m.SensorId).OrderBy(m => m.Key).ToList();

            foreach (var data in dataPerDetector)
            {
                var lineSerie = _plotModel.Series[data.Key] as LineSeries;
                if (lineSerie != null)
                {
                    AddMeasurementPoint(data, lineSerie);
                }
            }
            _lastUpdate = DateTime.Now;
        }
    }
}