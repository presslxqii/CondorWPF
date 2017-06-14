using System;
using System.ComponentModel;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.DataGrid;


namespace CondorVisualizator.Model
{

    public class DataRoad
    {   
        public string RoadName { get; set; }
        public UInt16 NumMess { get; set; }
        public string TimeStart { get; set; }
        public string TimeStop { get; set; }
        public string SelectbeginDate { get; set; }
        public string SelectendDate { get; set; }


    }
}
