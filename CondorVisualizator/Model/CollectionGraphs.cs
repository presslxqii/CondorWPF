using System.Collections.ObjectModel;

namespace CondorVisualizator.Model
{
    public class CollectionGraphs
    {
        private ObservableCollection<Graphics> Graphicses = new ObservableCollection<Graphics>();

        public ObservableCollection<Graphics> GraphicsesCollection
        {
            get { return Graphicses; }
            set { Graphicses = value; }
        }

        public double Size { get; set; }
        public int Band { get; set; }
    }
}
