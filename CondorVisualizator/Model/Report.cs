using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CondorVisualizator.Model
{
    public class Report
    {
       public ObservableCollection<CollectionGraphs> CountBand { get; set; }
       public ObservableCollection<CollectionGraphs> GeneralBand { get; set; }
       public ObservableCollection<CollectionGraphs> DensityBand { get; set; }
       public ObservableCollection<CollectionGraphs> Layer1Band { get; set; }
       public ObservableCollection<CollectionGraphs> Layer2Band { get; set; }
       public ObservableCollection<CollectionGraphs> Layer3Band { get; set; }
       public ObservableCollection<CollectionGraphs> AllLayerBand { get; set; }
       public ObservableCollection<CollectionGraphs> RutBand { get; set; }
       public ObservableCollection<CollectionGraphs> IRIBand { get; set; }
    }
}
