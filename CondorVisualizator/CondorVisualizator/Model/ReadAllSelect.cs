using System;
using System.Collections.Generic;
using System.Linq;
using signals;


namespace CondorVisualizator.Model
{
    class ReadAllSelect
    {
     public List<DataRoad> Messur;
        //Конструктор
        public ReadAllSelect(string beginDate, string endDate)
        {
            var tagsList = new Tags();
            Messur = new List<DataRoad>(); ;
            var hdas = Connect.ServerHdaConnect();
            //var listTag = tagsList.GetMassurm().Select(tags => Convert.ToString(tags.Value)).ToList();
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {

                    case "MeasuringNum":
                        listTag.Add(tag.Value);
                        break;
                    case "NameRoad":
                        listTag.Add(tag.Value);
                        break;
                }
            }
            Signal nummess = null;
            Signal name = null;
            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "MeasuringNum":
                        nummess = di.Dictionary[tag.Value];
                        break;
                    case "NameRoad":
                        name = di.Dictionary[tag.Value];
                        break;
                }
            }
        
            for (var sec = 0; sec <= (nummess.DtEnd - nummess.DtBegin).TotalSeconds - 1; sec+=25)
            {
                
                var nm = nummess.DtBegin.AddSeconds(sec);
                var nume = Seconds.GetValue(nummess, nm).OpcRec;
                var nam = Seconds.GetValue(name, nm).OpcRec;
                var alarms = new DataRoad
                {
                    NumMess = Convert.ToUInt16(nume.Value),
                    RoadName = Convert.ToString(nam.Value)
                };
                if (nume.Value != null && nume.Value.ToString() != string.Empty && nam.Value != null && nam.Value.ToString() != string.Empty)
                { 
                    var flag = true; 
                    foreach (var way in Messur) 
                    {
                        if (way.NumMess == alarms.NumMess && way.RoadName == alarms.RoadName) 
                        {
                            flag = false;

                        }
                    }
                    if (flag)
                    {
                        Messur.Add(alarms);
                    }

                }
            }
        }
    public
            List<DataRoad> GetMassurm()
        {
            return Messur;
        }

    }
}
