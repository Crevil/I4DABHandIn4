using System;
using System.Collections.Generic;

namespace GUI.ViewModel
{
    public interface IDataProvider
    {
        List<DAL.Entities.Measurement> GetData();
        List<DAL.Entities.Measurement> GetUpdateData(DateTime date);
    }
}