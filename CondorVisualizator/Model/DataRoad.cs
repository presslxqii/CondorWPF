using System;
using System.ComponentModel;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.DataGrid;


namespace CondorVisualizator.Model
{

    public class DataRoad
    {
        private List<Graphics> _graphicses = new List<Graphics>();

        public List<Graphics> GraphicsesCollection
        {
            get { return _graphicses; }
            set { _graphicses = value; }
        }
        public string RoadName { get; set; }
        public UInt16 NumMess { get; set; }
       
        public string TimeStop { get; set; }
        public string SelectbeginDate { get; set; }
        public string SelectendDate { get; set; }
        public string Coating { get; set; }
        public int DesignCount { get; set; }
        public double DesignLayer1 { get; set; }
        public double DesignLayer2 { get; set; }
        public double DesignLayer3 { get; set; }
        public string Builder { get; set; }
        public string Customer { get; set; }
        public string RoadSegment1 { get; set; }
        public string RoadSegment2 { get; set; }
        public string RoadSegment3 { get; set; }
        public string RoadSegment4 { get; set; }
        public string Direction { get; set; }
        public string TimeStart { get; set; }
        public string AllRoad { get; set; }
        public int Band { get; set; }
        public bool Check { get; set; }
        public double StandartsDensity { get; set; }
        public double StandartsDepth { get; set; }
        public double StandartsIRI { get; set; }
        public double StandartsLenght { get; set; }
        public double StandartsRut { get; set; }
        public double StandartsWidth { get; set; }
        public bool CoefficientTF { get; set; }
        public double KoefficK { get; set; }
        public double KoefficB { get; set; }

    }
}
