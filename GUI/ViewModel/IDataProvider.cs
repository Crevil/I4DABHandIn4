using System;
using System.Collections.Generic;

namespace GUI.ViewModel
{
    public interface IDataProvider
    {
        List<Measurement> GetData();
        List<Measurement> GetUpdateData(DateTime date);
    }
}