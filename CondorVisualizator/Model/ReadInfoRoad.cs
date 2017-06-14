using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using signals;

namespace CondorVisualizator.Model
{
    class ReadInfoRoad
    {
        private DataRoad alarms; 
        public ReadInfoRoad(string beginDate, string endDate)
        {
            var tagsList = new Tags();
            //Messur = new List<DataRoad>();

            var hdas = Connect.ServerHdaConnect();
            //var listTag = tagsList.GetMassurm().Select(tags => Convert.ToString(tags.Value)).ToList();
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {

                    ////case "MeasuringNum":
                    ////    listTag.Add(tag.Value);
                    ////    break;
                    ////case "Direction":
                    ////    listTag.Add(tag.Value);
                    ////    break;
                    case "Band":
                        listTag.Add(tag.Value);
                        break;

                    case "Coating":
                        listTag.Add(tag.Value);
                        break;
                    case "DesignCount":
                        listTag.Add(tag.Value);
                        break;
                    case "DesignLayer1":
                        listTag.Add(tag.Value);
                        break;
                    case "DesignLayer2":
                        listTag.Add(tag.Value);
                        break;
                    case "DesignLayer3":
                        listTag.Add(tag.Value);
                        break;
                    case "Customer":
                        listTag.Add(tag.Value);
                        break;
                    case "Builder":
                        listTag.Add(tag.Value);
                        break;


                    case "StandartsDensity":
                        listTag.Add(tag.Value);
                        break;
                    case "StandartsDepth":
                        listTag.Add(tag.Value);
                        break;
                    case "StandartsIRI":
                        listTag.Add(tag.Value);
                        break;
                    case "StandartsLenght":
                        listTag.Add(tag.Value);
                        break;
                    case "StandartsRut":
                        listTag.Add(tag.Value);
                        break;
                    case "StandartsWidth":
                        listTag.Add(tag.Value);
                        break;
                    case "CoefficientTF":
                        listTag.Add(tag.Value);
                        break;
                    case "CoefficientB":
                        listTag.Add(tag.Value);
                        break;

                    case "CoefficientK":
                        listTag.Add(tag.Value);
                        break;
                }
            }
            //Signal nummess = null;
            //Signal direction = null;
            Signal band = null;
            Signal coating = null;
            Signal designcount = null;
            Signal designlayer1 = null;
            Signal designlayer2 = null;
            Signal designlayer3 = null;
            Signal customer = null;
            Signal builder = null;

            Signal standartsDensity = null;
            Signal standartsDepth = null;
            Signal standartsIRI = null;
            Signal standartsLenght = null;
            Signal standartsRut = null;
            Signal standartsWidth = null;
            Signal coefficientTF = null;
            Signal koefficB = null;
            Signal koefficK = null;


            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    ////case "MeasuringNum":
                    ////    nummess = di.Dictionary[tag.Value];
                    ////    break;
                    ////case "Direction":
                    ////    direction = di.Dictionary[tag.Value];
                    ////    break;
                    case "Band":
                        band = di.Dictionary[tag.Value];
                        break;
                    case "Coating":
                        coating = di.Dictionary[tag.Value];
                        break;
                    case "DesignCount":
                        designcount = di.Dictionary[tag.Value];
                        break;
                    case "DesignLayer1":
                        designlayer1 = di.Dictionary[tag.Value];
                        break;
                    case "DesignLayer2":
                        designlayer2 = di.Dictionary[tag.Value];
                        break;
                    case "DesignLayer3":
                        designlayer3 = di.Dictionary[tag.Value];
                        break;
                    case "Customer":
                        customer = di.Dictionary[tag.Value];
                        break;
                    case "Builder":
                        builder = di.Dictionary[tag.Value];
                        break;


                    case "StandartsDensity":
                        standartsDensity = di.Dictionary[tag.Value];
                        break;
                    case "StandartsDepth":
                        standartsDepth = di.Dictionary[tag.Value];
                        break;
                    case "StandartsIRI":
                        standartsIRI = di.Dictionary[tag.Value];
                        break;
                    case "StandartsLenght":
                        standartsLenght = di.Dictionary[tag.Value];
                        break;
                    case "StandartsRut":
                        standartsRut = di.Dictionary[tag.Value];
                        break;
                    case "StandartsWidth":
                        standartsWidth = di.Dictionary[tag.Value];
                        break;
                    case "CoefficientTF":
                        coefficientTF = di.Dictionary[tag.Value];
                        break;
                    case "CoefficientB":
                        koefficB = di.Dictionary[tag.Value];
                        break;
                    case "CoefficientK":
                        koefficK = di.Dictionary[tag.Value];
                        break;


                }
            }

            //for (var sec = 0; sec <= (nummess.DtEnd - nummess.DtBegin).TotalSeconds - 1; sec += 25)
            //{

            var nm = coating.DtBegin.AddSeconds(1);
            ////var nume = Seconds.GetValue(nummess, nm).OpcRec;

            ////var dct = Seconds.GetValue(direction, nm).OpcRec;
            var bnd = Seconds.GetValue(band, nm).OpcRec;
            var coa = Seconds.GetValue(coating, nm).OpcRec;
            var dc = Seconds.GetValue(designcount, nm).OpcRec;
            var dl1 = Seconds.GetValue(designlayer1, nm).OpcRec;
            var dl2 = Seconds.GetValue(designlayer2, nm).OpcRec;
            var dl3 = Seconds.GetValue(designlayer3, nm).OpcRec;
            var cr = Seconds.GetValue(customer, nm).OpcRec;
            var br = Seconds.GetValue(builder, nm).OpcRec;

            var kfb = Seconds.GetValue(koefficB, nm).OpcRec;
            var kfk = Seconds.GetValue(koefficK, nm).OpcRec;

            var stndrtDensity = Seconds.GetValue(standartsDensity, nm).OpcRec;
            var stndrtDepth = Seconds.GetValue(standartsDepth, nm).OpcRec;
            var stndrtIRI = Seconds.GetValue(standartsIRI, nm).OpcRec;
            var stndrtLenght = Seconds.GetValue(standartsLenght, nm).OpcRec;
            var stndrtRut = Seconds.GetValue(standartsRut, nm).OpcRec;
            var stndrtWidth = Seconds.GetValue(standartsWidth, nm).OpcRec;
            var ctf = Seconds.GetValue(coefficientTF, nm).OpcRec;

            if (coa.Value == null)
            {
                coa.Value = "Не задано";
            }
            if (dc.Value == null)
            {
                dc.Value = "Не задано";
            }
            if (dl1.Value == null)
            {
                dl1.Value = "Не задано";
            }
            if (cr.Value == null)
            {
                cr.Value = "Не задано";
            }
            if (br.Value == null)
            {
                br.Value = "Не задано";
            }
            //if (dct.Value == null)
            //{
            //    dct.Value = "Не задано";
            //}

            alarms = new DataRoad

            {
                ////NumMess = Convert.ToUInt16(nume.Value),

                ////Direction = Convert.ToString(dct.Value),
                Band = Convert.ToInt16(bnd.Value),

                ////TimeStart = Convert.ToString(nume.Timestamp)
                Coating = Convert.ToString(coa.Value),
                DesignCount = Convert.ToInt16(dc.Value),
                DesignLayer1 = Convert.ToDouble(dl1.Value),
                DesignLayer2 = Convert.ToDouble(dl2.Value),
                DesignLayer3 = Convert.ToDouble(dl3.Value),
                Builder = Convert.ToString(br.Value),
                Customer = Convert.ToString(cr.Value),

                StandartsDensity = Convert.ToDouble(stndrtDensity.Value),
                StandartsDepth = Convert.ToDouble(stndrtDepth.Value),
                StandartsIRI = Convert.ToDouble(stndrtIRI.Value),
                StandartsLenght = Convert.ToDouble(stndrtLenght.Value),
                StandartsRut = Convert.ToDouble(stndrtRut.Value),
                StandartsWidth = Convert.ToDouble(stndrtWidth.Value),
                CoefficientTF = Convert.ToBoolean(ctf.Value),
                KoefficB = Convert.ToByte(kfb.Value),
                KoefficK = Convert.ToByte(kfk.Value)
            };


            //Messur.Add(alarms);

            //}
        }

        public
            DataRoad GetMassurm()
        {
            return alarms;
        }

    }
}
