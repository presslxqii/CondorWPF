using System;
using System.Collections.Generic;
using System.Diagnostics;
using signals;

namespace CondorVisualizator.Model
{
    public class ReadStartStop
    {
        public List<TimeStartStop> StlList;
        
        public ReadStartStop(string beginDate, string endDate)
        {
            StlList = new List<TimeStartStop>();
            var hdas = Connect.ServerHdaConnect();
            var tagsList = new Tags();
            Signal opredsignal = null;
            var listTag = new List<string>();
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "Start":
                        listTag.Add(tag.Value);
                        break;
                  
                }
            }
           var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "Start":
                        opredsignal = di.Dictionary[tag.Value];
                        break;
                 
                }
            }
            Debug.Assert(opredsignal != null, "opredsignal != null");
            for (var sec = 0; sec <= (opredsignal.DtEnd - opredsignal.DtBegin).TotalSeconds - 1; sec+=25)
            {
                var dt = opredsignal.DtBegin.AddSeconds(sec);
                var alarm = Seconds.GetValue(opredsignal, dt).OpcRec;

                var alarms = new TimeStartStop {Time = alarm.Timestamp, Value = Convert.ToBoolean(alarm.Value)};

                StlList.Add(alarms);

            }
        }
        public List<TimeStartStop> GetMassurm3()
        {
            return StlList;
        }

    }
}
