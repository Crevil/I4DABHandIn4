using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using GUI.ViewModel;

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

            AppartmentListBox.SelectedIndex = 0;
            _viewModel = new MainWindowModel();
            DataContext = _viewModel;
            _viewModel.GraphDataChanged += RedrawPlot;

        }

        private void RedrawPlot(object sender, EventArgs eventArgs)
        {
            if (GraphPlot == null) return;
            GraphPlot.InvalidatePlot();
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _lastUpdateMilliSeconds;

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (_stopwatch.ElapsedMilliseconds <= _lastUpdateMilliSeconds + 5000) return;

            _viewModel.Graph.UpdateModel();

            GraphPlot.InvalidatePlot();
            _lastUpdateMilliSeconds = _stopwatch.ElapsedMilliseconds;
        }
    }
}
