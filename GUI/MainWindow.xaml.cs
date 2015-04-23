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

            _viewModel = new MainWindowModel();
            DataContext = _viewModel;

            CompositionTarget.Rendering += CompositionTargetRendering;
            stopwatch.Start();

            InitializeComponent();

        }

        private long frameCounter;
        private Stopwatch stopwatch = new Stopwatch();
        private long lastUpdateMilliSeconds;

        private void CompositionTargetRendering(object sender, EventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds > lastUpdateMilliSeconds + 5000)
            {
                _viewModel.Graph.UpdateModel();
                GraphPlot.InvalidatePlot(true);
                lastUpdateMilliSeconds = stopwatch.ElapsedMilliseconds;
            }
        }
    }
}
