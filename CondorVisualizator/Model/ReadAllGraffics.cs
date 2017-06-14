using System;
using System.Collections.Generic;
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
       private List<Graphics> Messur;
       

        public ReadAllGraffics(string beginDate, string endDate)
        {
            var tagsList = new Tags();
            Messur = new List<Graphics>();
            
            var hdas = Connect.ServerHdaConnect();
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "CountLayers":
                       listTag.Add(tag.Value);
                        break;
                    case "Distance":
                        listTag.Add(tag.Value);
                        break;
                    case "OverallAssessment":
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
                    case "Rut":
                        listTag.Add(tag.Value);
                        break;
                    case "Band":
                        listTag.Add(tag.Value);
                        break;
                    case "IRI":
                        listTag.Add(tag.Value);
                        break;
                    case "Latitude":
                        listTag.Add(tag.Value);
                        break;
                    case "Longitude":
                        listTag.Add(tag.Value);
                        break;
                    
                        
                }
            }
            Signal distance = null;
            Signal countLayer = null;
            Signal generalState = null;
        
            Signal layer1 = null;
            Signal layer2 = null;
            Signal layer3 = null;
            Signal n1 = null;
            Signal n2 = null;
            Signal rut = null;
            Signal band = null;
            Signal iri = null;
            Signal latitude = null;
            Signal longitude = null;
           
            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "CountLayers":
                        countLayer = di.Dictionary[tag.Value];
                        break;
                    case "Distance":
                        distance = di.Dictionary[tag.Value];
                        break;
                    case "OverallAssessment":
                        generalState = di.Dictionary[tag.Value];
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
                    case "Rut":
                        rut = di.Dictionary[tag.Value];
                        break;
                    case "Band":
                        band = di.Dictionary[tag.Value];
                        break;
                    case "IRI":
                        iri = di.Dictionary[tag.Value];
                        break;
                    case "Latitude":
                        latitude = di.Dictionary[tag.Value];
                        break;
                    case "Longitude":
                        longitude = di.Dictionary[tag.Value];
                        break; 


                }
            }
         
            for (var sec = 0; sec <= (countLayer.DtEnd - countLayer.DtBegin).TotalSeconds - 1; sec ++)
            {
                var middle = new Graphics();
               
                    var ct = countLayer.DtBegin.AddSeconds(sec);
                    var countl = Seconds.GetValue(countLayer, ct).OpcRec;
                    var general = Seconds.GetValue(generalState, ct).OpcRec;
                   
                    var lr1 = Seconds.GetValue(layer1, ct).OpcRec;
                    var lr2 = Seconds.GetValue(layer2, ct).OpcRec;
                    var lr3 = Seconds.GetValue(layer3, ct).OpcRec;
                    var int1 = Seconds.GetValue(n1, ct).OpcRec;
                    var int2 = Seconds.GetValue(n2, ct).OpcRec;
                    var dse = Seconds.GetValue(distance, ct).OpcRec;
                    var rt = Seconds.GetValue(rut, ct).OpcRec;
                    var bd = Seconds.GetValue(band, ct).OpcRec;
                    var ir = Seconds.GetValue(iri, ct).OpcRec;
                    var lade = Seconds.GetValue(latitude, ct).OpcRec;
                    var lode = Seconds.GetValue(longitude, ct).OpcRec;
                    
                    middle.CountLayer = Convert.ToByte(countl.Value);
                    middle.GeneralState = Convert.ToBoolean(general.Value);
                   
                    middle.Layer1 = Convert.ToSingle(lr1.Value);
                    middle.Layer2 = Convert.ToSingle(lr2.Value);
                    middle.Layer3 = Convert.ToSingle(lr3.Value);
                    middle.IntensityN1 = Convert.ToDouble(int1.Value);
                    middle.IntensityN2 = Convert.ToDouble(int2.Value);
                    middle.Distance = Convert.ToSingle(dse.Value);
                    middle.Rut = Convert.ToByte(rt.Value);
                    middle.Band = Convert.ToUInt16(bd.Value);
                    middle.IRI = Convert.ToDouble(ir.Value);
                    middle.Latitude = Convert.ToSingle(lade.Value);
                    middle.Longitude = Convert.ToSingle(lode.Value);
                   


                    Messur.Add(middle);
            }
            
        }

        public List<Graphics> GetMassurm()
        {
            return Messur;
        }
    }
}
