using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Opc;
using OPCRecord;
using signals;

namespace CondorVisualizator.Model
{

    class SignalDict
    {
         public Dictionary<string, Signal> Dictionary;
        private Opc.Da.Server serverDa = null;
        private Opc.Hda.Server serverHda = null;
        public SignalDict(Opc.Hda.Server Hda, List<string> tagsLst, string[] propID = null, string dtBegin = null, string dtEnd = null, bool restrict = false)
        {

            CreateSignalDict(Hda, tagsLst, propID, dtBegin, dtEnd, restrict);
        }

        private void CreateSignalDict(Opc.Hda.Server Hda, List<string> tagsLst, string[] propID = null, string dtBegin = null, string dtEnd = null, bool restrict = false)
        {

            Dictionary = new Dictionary<string, Signal>();
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            var memRec = new OpcRecord();
            if (dtBegin != null)
                if (serverHda == null)
                    serverHda = Hda;
           

       
            for (int i = 0; i < tagsLst.Count; i++)
            {
                
                var signal = new Signal(serverDa, tagsLst[i], propID);

                if (dtBegin != null)
                {

                    try
                    {

                  
                        signal.ValueCollection = new List<OpcRecord>();
                        dt1 = System.Convert.ToDateTime(dtBegin);
                        dt2 = DateTime.Today;
                        DateTime lastdt = dt1;

                        if (dtEnd == null) dtEnd = dt2.AddDays(1).ToString();
                        dt2 = System.Convert.ToDateTime(dtEnd);

                        signal.DtBegin = dt1;
                        signal.DtEnd = dt2;

                        var identifiers = new ItemIdentifier[1];
                        identifiers[0] = new Opc.ItemIdentifier(tagsLst[i]);

                        Opc.IdentifiedResult[] items = serverHda.CreateItems(identifiers);
                        OpcRecord rec = null;
                        bool exit = false;
                         Opc.Hda.ItemValueCollection[] vForPrev = serverHda.ReadRaw(new Opc.Hda.Time(dt1),
                            new Opc.Hda.Time(dt1.AddSeconds(1)), 0, true, items);
                        signal.PrevValue = new OpcRecord();
                        if (vForPrev.Length > 0)
                        {
                            if (vForPrev[0].Count > 0)
                            {
                                signal.PrevValue.Value = vForPrev[0][0].Value;
                                signal.PrevValue.Quality = vForPrev[0][0].Quality.GetCode();
                                signal.PrevValue.Timestamp = vForPrev[0][0].Timestamp.ToString();
                            }
                        }
                        Opc.Hda.ItemValueCollection[] vForNext =
                            serverHda.ReadRaw(new Opc.Hda.Time(dt2.AddMilliseconds(-1)), new Opc.Hda.Time(dt2), 0, true,
                                items);
                        signal.NextValue = new OpcRecord();
                        if (vForNext.Length > 0)
                        {
                            if (vForNext[0].Count > 0)
                            {

                                signal.NextValue.Value = vForNext[0][vForNext[0].Count - 1].Value;
                                signal.NextValue.Quality = vForNext[0][vForNext[0].Count - 1].Quality.GetCode();
                                signal.NextValue.Timestamp = vForNext[0][vForNext[0].Count - 1].Timestamp.ToString();

                            }
                        }

                        while (!exit)
                        {
                            Opc.Hda.ItemValueCollection[] values = serverHda.ReadRaw(new Opc.Hda.Time(lastdt),
                                new Opc.Hda.Time(dt2), 0, false, items);

                            for (int j = 0; j < values.Length; j++)
                            {
                                if (values[j].Count <= 1) exit = true;
                                for (int k = 0; k < values[j].Count; k++)
                                {
                                    rec = new OpcRecord();
                                    if (signal.ValueType == "long")
                                        if (values[j][k].Value != null)
                                            rec.Value = System.Convert.ToInt64(values[j][k].Value.ToString());
                                        else rec.Value = 0;
                                    if (signal.ValueType == "double")
                                        if (values[j][k].Value != null)
                                            rec.Value = System.Convert.ToDouble(values[j][k].Value.ToString());
                                        else rec.Value = 0.0;
                                    if (signal.ValueType == "string")
                                        if (values[j][k].Value != null)
                                            rec.Value = values[j][k].Value.ToString();
                                        else rec.Value = "";
                                    if (signal.ValueType == "bool")
                                    {
                                        if (values[j][k].Value != null)
                                            rec.Value = values[j][k].Value.ToString().ToLower().Trim() == "true";
                                        else rec.Value = false;
                                    }

                                    rec.Quality = values[j][k].Quality.GetCode();
                                    rec.Timestamp = values[j][k].Timestamp.ToString();
                                    rec.Value = values[j][k].Value;
                                    lastdt = values[j][k].Timestamp;
                                    if (values[j].Count > 1)
                                    {
                                        if (k == values[j].Count - 1 &&
                                            values[j][k].Timestamp == values[j][k - 1].Timestamp)
                                        {
                                            lastdt = lastdt.AddMilliseconds(1);
                                        }
                                    }
                                    if (k != values[j].Count - 1)
                                    {
                                        if (restrict & signal.ValueCollection.Count > 0)
                                        {
                                            if (
                                                System.Convert.ToDateTime(
                                                    signal.ValueCollection[signal.ValueCollection.Count - 1].Timestamp)
                                                    .ToString() ==
                                                System.Convert.ToDateTime(rec.Timestamp).ToString() &&
                                                rec.Quality >= 192)
                                            {
                                                signal.ValueCollection[signal.ValueCollection.Count - 1] = rec;
                                            }
                                            else
                                                signal.ValueCollection.Add(rec);
                                        }
                                        else
                                            signal.ValueCollection.Add(rec);
                                    }
                                }
                            }
                        }

                        if (restrict & signal.ValueCollection.Count > 0)
                        {
                            if (
                                System.Convert.ToDateTime(
                                    signal.ValueCollection[signal.ValueCollection.Count - 1].Timestamp).ToString() ==
                                System.Convert.ToDateTime(rec.Timestamp).ToString() && rec.Quality >= 192)
                            {
                                signal.ValueCollection[signal.ValueCollection.Count - 1] = rec;
                            }
                            else
                                signal.ValueCollection.Add(rec);
                        }
                        else
                            signal.ValueCollection.Add(rec);
                    }

                    catch (Exception)
                    {

                        MessageBox.Show(@"Отсутствует лицензия или проверьте подключения к серверу C:\Tag.ini", @"Ошибка подключения сервера", MessageBoxButton.OK, MessageBoxImage.Error);

                        Environment.Exit(0);
                    }
                    }
                
                if (restrict)
                    {
                        if (signal.ValueCollection.Count > 0)
                            if (signal.ValueCollection[0] == null)
                                signal.ValueCollection.Remove(signal.ValueCollection[0]);

                        var restrictLeft = new OpcRecord();
                        restrictLeft.Value = signal.PrevValue.Value;
                        restrictLeft.Quality = signal.PrevValue.Quality;
                        restrictLeft.Timestamp = dt1.ToString();
                        signal.ValueCollection.Insert(0, restrictLeft);

                        var restrictRight = new OpcRecord();
                        restrictRight.Value = signal.ValueCollection[signal.ValueCollection.Count - 1].Value;
                        restrictRight.Quality = signal.ValueCollection[signal.ValueCollection.Count - 1].Quality;
                        restrictRight.Timestamp = dt2.ToString();
                        signal.ValueCollection.Add(restrictRight);
                    }
                    Dictionary.Add(tagsLst[i], signal);
                }
            
           
            
        }

    }
    }

