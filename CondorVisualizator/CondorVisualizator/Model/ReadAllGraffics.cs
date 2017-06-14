using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using signals;


namespace CondorVisualizator.Model
{
    public class ReadAllGraffics
    {
        //  struct str
        //{
        //    public string Name;
        //    public string Value;
        //}
       public List<Graphics> Messur;
       

        public ReadAllGraffics(string beginDate, string endDate)
        {
            var tagsList = new Tags();
            Messur = new List<Graphics>();
            var hdas = Connect.ServerHdaConnect();
            //var listTag = tagsList.GetMassurm().Select(tags => Convert.ToString(tags.Value)).ToList();
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "Layers":
                       listTag.Add(tag.Value);
                        break;
                    case "Distance":
                        listTag.Add(tag.Value);
                        break;
                    case "Overallassessment":
                        listTag.Add(tag.Value);
                        break;
                    case "CoefficientB":
                        listTag.Add(tag.Value);
                        break;
                    case "CoefficientK":
                        listTag.Add(tag.Value);
                        break;
                    case "Layer1":
                        listTag.Add(tag.Value);
                        break;
                    case "Layer2":
                        listTag.Add(tag.Value);
                        break;
                    case "Layer3":
                        listTag.Add(tag.Value);
                        break;
                    case "Sensor1":
                        listTag.Add(tag.Value);
                        break;
                    case "Sensor2":
                        listTag.Add(tag.Value);
                        break; 
                }
            }
            Signal distance = null;
            Signal countLayer = null;
            Signal generalState = null;
            Signal koefficB = null;
            Signal koefficK = null;
            Signal layer1 = null;
            Signal layer2 = null;
            Signal layer3 = null;
            Signal n1 = null;
            Signal n2 = null;
            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "Layers":
                        countLayer = di.Dictionary[tag.Value];
                        break;
                    case "Distance":
                        distance = di.Dictionary[tag.Value];
                        break;
                    case "Overallassessment":
                        generalState = di.Dictionary[tag.Value];
                        break;
                    case "CoefficientB":
                        koefficB = di.Dictionary[tag.Value];
                        break;
                    case "CoefficientK":
                        koefficK = di.Dictionary[tag.Value];
                        break;
                    case "Layer1":
                        layer1 = di.Dictionary[tag.Value];
                        break;
                    case "Layer2":
                        layer2 = di.Dictionary[tag.Value];
                        break;
                    case "Layer3":
                        layer3 = di.Dictionary[tag.Value];
                        break;
                    case "Sensor1":
                        n1 = di.Dictionary[tag.Value];
                        break;
                    case "Sensor2":
                        n2 = di.Dictionary[tag.Value];
                        break;
                     }
            }
         
            //int schet = Convert.ToInt16(((countLayer.DtEnd - countLayer.DtBegin).TotalSeconds)/k);
            //if (schet == 0)
            //{
            //    schet = 1;
            //}
            for (var sec = 0; sec <= (countLayer.DtEnd - countLayer.DtBegin).TotalSeconds - 1; sec ++)
            {
                var middle = new Graphics();
                
               
                    var ct = countLayer.DtBegin.AddSeconds(sec);
                    var countl = Seconds.GetValue(countLayer, ct).OpcRec;
                    var general = Seconds.GetValue(generalState, ct).OpcRec;
                    var kfb = Seconds.GetValue(koefficB, ct).OpcRec;
                    var kfk = Seconds.GetValue(koefficK, ct).OpcRec;
                    var lr1 = Seconds.GetValue(layer1, ct).OpcRec;
                    var lr2 = Seconds.GetValue(layer2, ct).OpcRec;
                    var lr3 = Seconds.GetValue(layer3, ct).OpcRec;
                    var int1 = Seconds.GetValue(n1, ct).OpcRec;
                    var int2 = Seconds.GetValue(n2, ct).OpcRec;
                    var dse = Seconds.GetValue(distance, ct).OpcRec;
                    middle.CountLayer = Convert.ToByte(countl.Value);
                    middle.GeneralState = Convert.ToByte(general.Value);
                    middle.KoefficB = Convert.ToByte(kfb.Value);
                    middle.KoefficK = Convert.ToByte(kfk.Value);
                    middle.Layer1 = Convert.ToSingle(lr1.Value);
                    middle.Layer2 = Convert.ToSingle(lr2.Value);
                    middle.Layer3 = Convert.ToSingle(lr3.Value);
                    middle.IntensityN1 = Convert.ToDouble(int1.Value);
                    middle.IntensityN2 = Convert.ToDouble(int2.Value);
                    middle.Distance = Convert.ToSingle(dse.Value);
                
                //var alarms = new Graphics();
                //var ct1 = countLayer.DtBegin.AddSeconds(sec);
                //var dse = Seconds.GetValue(distance, ct1).OpcRec;
   
                //alarms.CountLayer = Convert.ToByte(middle.CountLayer/schet);
                //alarms.GeneralState = Convert.ToByte(middle.GeneralState);
                //alarms.KoefficB = Convert.ToByte(middle.KoefficB);
                //alarms.KoefficK = Convert.ToByte(middle.KoefficK);
                //alarms.Layer1 = Convert.ToSingle(middle.Layer1);
                //alarms.Layer2 = Convert.ToSingle(middle.Layer2);
                //alarms.Layer3 = Convert.ToSingle(middle.Layer3 );
                //alarms.IntensityN1 = Convert.ToDouble(middle.IntensityN1);
                //alarms.IntensityN2 = Convert.ToDouble(middle.IntensityN2);
                //alarms.Distance = Convert.ToSingle(dse.Value);
                Messur.Add(middle);
                
            }
        }

        public List<Graphics> GetMassurm()
        {
            return Messur;
        }
    }
}
