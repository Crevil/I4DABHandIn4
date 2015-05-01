using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAL.Entities;
using GUI.Annotations;
using GUI.Model;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace GUI.ViewModel.Graph.Types
{
    public class TemperatureGraph : IGraphType
    {
        public ICollection<Measurement> Measurements { get; set; }

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
                Minimum = DateTimeAxis.ToDouble(new DateTime(2014, 1, 1)),
                Maximum = DateTimeAxis.ToDouble(new DateTime(2014, 12, 30)),
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80, 
                StringFormat = "d-M-yyyy hh:mm:ss", 
            };
            PlotModel.Axes.Add(dateAxis);


            var valueAxisLeft = new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Title = "Temperature",
                Unit = "°"
            };
            PlotModel.Axes.Add(valueAxisLeft);
        }

        public void LoadData()
        {
            var dataPerDetector = SelectMeasurements(Measurements);
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
                    Title = string.Format("Appartment {0}", data.First().AppartmentId),
                    Smooth = false
                };

                AddMeasurementPoint(data, line);

                PlotModel.Series.Add(line);
            }
        }

        public void UpdateModel()
        {
            LoadData(); // Dummy
            // Needs work!  Indexing on Series collection goes out of bounds!
            //if (PlotModel.Series.Count == 0) return;

            //var dataPerDetector = SelectMeasurements(Measurements);

            //foreach (var data in dataPerDetector)
            //{
            //    var lineSerie = PlotModel.Series[data.Key] as LineSeries;
            //    if (lineSerie != null)
            //    {
            //        AddMeasurementPoint(data, lineSerie);
            //    }
            //}
        }

        private static IEnumerable<IGrouping<int, Measurement>> SelectMeasurements(ICollection<Measurement> measurements)
        {
            return measurements.GroupBy(m => m.AppartmentId).OrderBy(m => m.Key).ToList();
        }

        public void AddMeasurementPoint(IEnumerable<Measurement> data, [NotNull] DataPointSeries lineSerie)
        {
            if (lineSerie == null) throw new ArgumentNullException("lineSerie");

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