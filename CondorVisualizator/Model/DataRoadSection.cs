using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using signals;

namespace CondorVisualizator.Model
{
    public class DataRoadSection
    {
        //public List<DataRoad> Messur;
        private string alarms;
        public DataRoadSection(string beginDate, string endDate)
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

 
                    case "RoadSegment1":
                        listTag.Add(tag.Value);
                        break;
                    case "RoadSegment2":
                        listTag.Add(tag.Value);
                        break;
                    case "RoadSegment3":
                        listTag.Add(tag.Value);
                        break;
                    case "RoadSegment4":
                        listTag.Add(tag.Value);
                        break;
           
                }
            }
            Signal road1 = null;
            Signal road2 = null;
            Signal road3 = null;
            Signal road4 = null;
         


            var di = new SignalDict(hdas, listTag, null, beginDate, endDate);
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "RoadSegment1":
                        road1 = di.Dictionary[tag.Value];
                        break;
                    case "RoadSegment2":
                        road2 = di.Dictionary[tag.Value];
                        break;
                    case "RoadSegment3":
                        road3 = di.Dictionary[tag.Value];
                        break;
                    case "RoadSegment4":
                        road4 = di.Dictionary[tag.Value];
                        break;
             


                }
            }

            //for (var sec = 0; sec <= (road1.DtEnd - road1.DtBegin).TotalSeconds - 1; sec += 25)
            //{

                var nm = road1.DtBegin.AddSeconds(1);
                var rs1 = Seconds.GetValue(road1, nm).OpcRec;
                var rs2 = Seconds.GetValue(road2, nm).OpcRec;
                var rs3 = Seconds.GetValue(road3, nm).OpcRec;
                var rs4 = Seconds.GetValue(road4, nm).OpcRec;



            alarms = "км" + Convert.ToString(rs1.Value) + "+" + Convert.ToString(rs2.Value) + " - " + "км" +
                     Convert.ToString(rs3.Value) + "+" + Convert.ToString(rs4.Value);
            //TimeStart = Convert.ToString(rs1.Timestamp)





            //Messur.Add(alarms);

            //}
        }

        public
            string GetMassurm()
        {
            return alarms;
        }

    }
}
