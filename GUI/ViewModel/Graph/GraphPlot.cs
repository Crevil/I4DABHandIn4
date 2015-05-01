using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entities;
using GUI.Annotations;
using GUI.ViewModel.Converters;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GUI.ViewModel.Graph
{
    public class GraphPlot
    {
        public ICollection<Measurement> Measurements { get; set; }

        public string Title { get; set; }

        public string Unit { get; set; }
        public double YMininumValue { get; set; }

        public PlotModel PlotModel { get; set; }

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
                IntervalLength = 80, 
                StringFormat = "d-M-yyyy hh:mm:ss", 
            };
            PlotModel.Axes.Add(dateAxis);

            var valueAxisLeft = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Minimum = YMininumValue,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = Title,
                Unit = Unit
            };
            PlotModel.Axes.Add(valueAxisLeft);
        }

        public void LoadData()
        {
            var dataPerDetector = SelectMeasurements(Measurements);
            var appartmentConverter = new AppartmentToStringConverter();

            PlotModel.Series.Clear();
            foreach (var data in dataPerDetector)
            {
                var line = new LineSeries
                {
                    StrokeThickness = 2,
                    MarkerSize = 3,
                    MarkerStroke = Graph.Colors[data.Key % Graph.Colors.Count],
                    MarkerType = Graph.MarkerTypes[data.Key % Graph.MarkerTypes.Count],
                    CanTrackerInterpolatePoints = false,
                    Title = (string)appartmentConverter.Convert(data.First().Appartment, null, null, null),
                    Smooth = false
                };

                AddMeasurementPoint(data, line);

                PlotModel.Series.Add(line);
            }
        }
        private static IEnumerable<IGrouping<int, Measurement>> SelectMeasurements(ICollection<Measurement> measurements)
        {
            return measurements.GroupBy(m => m.Appartment.AppartmentId).OrderBy(m => m.Key).ToList();
        }

        public void AddMeasurementPoint(IEnumerable<Measurement> data, [NotNull] DataPointSeries lineSerie)
        {
            if (lineSerie == null) throw new ArgumentNullException("lineSerie");
            data = data.OrderBy(d => d.Timestamp);
            data.ToList().ForEach(
                d =>
                    lineSerie.Points.Add
                        (
                            new DataPoint(
                                DateTimeAxis.ToDouble(d.Timestamp),
                                d.Value
                                )
                        )
                );
        }
    }
}