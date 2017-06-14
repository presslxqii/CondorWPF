using System;
using System.Collections.Generic;
using System.Linq;
using signals;


namespace CondorVisualizator.Model
{
    class ReadAllSelect
    {
        private List<RoadName> Messur;
        //Конструктор
        public ReadAllSelect(string beginDate, string endDate)
        {
            var tagsList = new Tags();
            Messur = new List<RoadName>(); ;
            var hdas = Connect.ServerHdaConnect();
            //var listTag = tagsList.GetMassurm().Select(tags => Convert.ToString(tags.Value)).ToList();
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {

                    
                    case "NameRoad":
                        listTag.Add(tag.Value);
                        break;
                    
                }
            }
           
            Signal name = null;
          
            
            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    
                    case "NameRoad":
                        name = di.Dictionary[tag.Value];
                        break;
                   

                }
            }

            for (var sec = 0; sec <= (name.DtEnd - name.DtBegin).TotalSeconds - 1; sec += 25)
            {

                var nm = name.DtBegin.AddSeconds(sec);
                var nam = Seconds.GetValue(name, nm).OpcRec;

                var alarms = new RoadName { Time = nam.Timestamp, Name = Convert.ToString(nam.Value) };
                if (nam.Value.ToString() == string.Empty) continue;
                var flag = true; 
            
                
                    Messur.Add(alarms);
                
            }
        }

        internal class RoadName
        {
            public string Name { get; set; }
            public string Time { get; set; }
        }

        public
            List<RoadName> GetMassurm()
        {
            return Messur;
        }

    }
}
