using OxyPlot;

namespace GUI.ViewModel.Graph
{
    public interface IGraphType
    {
        void SetUpModel();
        PlotModel PlotModel { get; set; }
        void UpdateModel();
        void LoadData();
    }
}