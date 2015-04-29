using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GUI.Model;
using GUI.ViewModel;
using OxyPlot.Wpf;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowModel _viewModel;

        public MainWindow()
        {
            CompositionTarget.Rendering += CompositionTargetRendering;
            _stopwatch.Start();

            InitializeComponent();

            var gdl = new GDL();
            _viewModel = new MainWindowModel(gdl);
            DataContext = _viewModel;

        }

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _lastUpdateMilliSeconds;

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (_stopwatch.ElapsedMilliseconds <= _lastUpdateMilliSeconds + 5000) return;

            if (_viewModel == null) return;
            if (_viewModel.Graph == null) return;

            _viewModel.Graph.UpdateModel();
            _viewModel.Graph.PlotView.InvalidatePlot();

            _lastUpdateMilliSeconds = _stopwatch.ElapsedMilliseconds;
        }
    }
}
