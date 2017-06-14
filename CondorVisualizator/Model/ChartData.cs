using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CondorVisualizator.Model
{
    public class ChartData
    {

        
        public ObservableCollection<CollectionGraphs> CountBand;
       
        public ObservableCollection<CollectionGraphs> GeneralBand;
        
        public ObservableCollection<CollectionGraphs> DensityBand;
       
        public ObservableCollection<CollectionGraphs> Layer1Band;
        
        public ObservableCollection<CollectionGraphs> Layer2Band;
       
        public ObservableCollection<CollectionGraphs> Layer3Band;
       
        public ObservableCollection<CollectionGraphs> AllLayerBand;

        public ObservableCollection<CollectionGraphs> RutBand;
        public ObservableCollection<CollectionGraphs> IRIBand;
    }
}
