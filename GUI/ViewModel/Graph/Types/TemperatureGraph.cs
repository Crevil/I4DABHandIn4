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

            var dataPerDetector = SelectMeasurements(measurements);

            foreach (var data in dataPerDetector)
            {
                var lineSerie = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = Graph.Colors[data.Key],
                    MarkerType = Graph.MarkerTypes[data.Key],
                    CanTrackerInterpolatePoints = false,
                    Title = string.Format("Appartment {0}", data.First().AppartmentId),
                    Smooth = false
                };

                AddMeasurementPoint(data, lineSerie);

                _plotModel.Series.Add(lineSerie);
            }
        }

        public void UpdateModel()
        {
            if (_plotModel.Series.Count == 0) return;

            var measurements = DataProvider.GetUpdateData(DateTime.Now);
            var dataPerDetector = SelectMeasurements(measurements);

            foreach (var data in dataPerDetector)
            {
                var lineSerie = _plotModel.Series[data.Key] as LineSeries;
                if (lineSerie != null)
                {
                    AddMeasurementPoint(data, lineSerie);
                }
            }
        }

        private IEnumerable<IGrouping<int, Measurement>> SelectMeasurements(List<Measurement> measurements)
        {
            return measurements.GroupBy(m => m.SensorId).OrderBy(m => m.Key).ToList();
        }

        private void AddMeasurementPoint(IEnumerable<Measurement> data, [NotNull] DataPointSeries lineSerie)
        {
            if (lineSerie == null) throw new ArgumentNullException("lineSerie");

            data.ToList().ForEach(
                d =>
                    lineSerie.Points.Add
                        (
                            new DataPoint(
                                Axis.ToDouble(
                                    AppartmentTemperatureDataProvider.ConvertFromUnixTimestamp(
                                        double.Parse(d.Timestamp, CultureInfo.CurrentCulture)
                                        )
                                    ),
                                d.Value
                                )
                        )
                );
        }
    }
}