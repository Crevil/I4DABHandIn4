using System.Collections.Generic;
using DAL.Entities;
using OxyPlot;

namespace GUI.ViewModel.Graph
{
    public interface IGraphType
    {
        void SetUpModel();
        PlotModel PlotModel { get; set; }
        ICollection<Measurement> Measurements { get; set; } 
        void UpdateModel();
        void LoadData();
    }
}