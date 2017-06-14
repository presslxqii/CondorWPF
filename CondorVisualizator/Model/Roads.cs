using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CondorVisualizator.Model
{
    public class Roads
    {

        public string RoadName { get; set; }
        public ObservableCollection<Areas> Areas = new ObservableCollection<Areas>();
        //public string Direction { get; set; }
        //public string TimeStart { get; set; }
        //public string AllRoad { get; set; }

        public ObservableCollection<Areas> Areases
        {
            get { return Areas; }
            set { Areas = value; }
        }
    }
}
