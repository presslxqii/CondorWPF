using System.Collections.ObjectModel;

namespace CondorVisualizator.Model
{
    public class Areas
    {
        public ObservableCollection<DataRoad> Band = new ObservableCollection<DataRoad>();

        public ObservableCollection<DataRoad> BandCollection
        {
            get { return Band; }
            set { Band = value; }
        }
        public string RoadName { get; set; }
        public string Direction { get; set; }
        public string AllRoad { get; set; }
        public string TimeStart { get; set; }
        public string TimeStop { get; set; }

    }
}
