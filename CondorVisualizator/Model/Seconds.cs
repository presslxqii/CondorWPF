using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPCRecord;
using signals;

namespace CondorVisualizator.Model
{
    internal class Seconds
    {
        
      
        public static SOpcRecord GetValue(Signal signal, object obj_dt)
        {
            DateTime dt = DateTime.Now;
            if (obj_dt is string) dt = System.Convert.ToDateTime(obj_dt);
            else if (obj_dt is DateTime) dt = (DateTime) obj_dt;
            else
                dt = signal.DtBegin;


            if (dt < signal.DtBegin || dt > signal.DtEnd) return null;

            var rec = new OpcRecord();
            var sRec = new SOpcRecord();
            rec.Quality = signal.PrevValue.Quality;
            rec.Timestamp = dt.ToString();
            rec.Value = signal.PrevValue.Value;

            if (signal.ValueCollection[0] == null)
            {
                rec.Timestamp = dt.ToString();
                sRec.OpcRec = rec;
                sRec.Sec = (signal.DtEnd - dt).TotalSeconds;
                return sRec;
            }

            if (System.Convert.ToDateTime(signal.ValueCollection[0].Timestamp) > dt)
            {
                rec.Timestamp = dt.ToString();
                sRec.OpcRec = rec;
                sRec.Sec = (System.Convert.ToDateTime(signal.ValueCollection[0].Timestamp) - dt).TotalSeconds;
                return sRec;
            }

            var i1 = 0;
            var i2 = signal.ValueCollection.Count - 1;
            if (System.Convert.ToDateTime(signal.ValueCollection[i2].Timestamp) <= dt)
            {
                rec.Quality = signal.ValueCollection[i2].Quality;
                rec.Value = signal.ValueCollection[i2].Value;
                rec.Timestamp = dt.ToString();
                sRec.OpcRec = rec;
                sRec.Sec = (signal.DtEnd - dt).TotalSeconds;
                return sRec;
            }

            var x = 0;
            while (i2 - i1 >= 2)
            {
                x = i1 + (i2 - i1) / 2;
                if (System.Convert.ToDateTime(signal.ValueCollection[x].Timestamp) <= dt) i1 = x;
                else i2 = x;
            }
            x = i1;
            rec.Quality = signal.ValueCollection[x].Quality;
            rec.Value = signal.ValueCollection[x].Value;
            rec.Timestamp = dt.ToString();
            sRec.OpcRec = rec;
            sRec.Sec = (System.Convert.ToDateTime(signal.ValueCollection[x + 1].Timestamp) - dt).TotalSeconds;
            return sRec;
        }
      

        public static DataTable GetSignalValuesBySeconds(Signal signal, string dtBegin = null, string dtEnd = null)
        {
            var table = new DataTable();

            if (signal.ValueCollection.Count == 0 || signal.ValueCollection[0] == null)
            {
                if (signal.PrevValue.Value == null)
                    return table;
            }
            DateTime dt1;
            DateTime dt2;
            List<OpcRecord> result = new List<OpcRecord>();

            var key = new DataColumn();
            key.DataType = System.Type.GetType("System.Int32");
            key.ColumnName = "key";
            table.Columns.Add(key);


            var value = new DataColumn();
            value.DataType = System.Type.GetType("System.String");
            value.ColumnName = "Value";
            table.Columns.Add(value);

            var quality = new DataColumn();
            value.DataType = System.Type.GetType("System.String");
            quality.ColumnName = "Quality";
            table.Columns.Add(quality);

            var timestamp = new DataColumn();
            timestamp.DataType = System.Type.GetType("System.String");
            timestamp.ColumnName = "Timestamp";
            table.Columns.Add(timestamp);


            DataColumn[] keys = new DataColumn[1];
            keys[0] = key;
            table.PrimaryKey = keys;


            if (dtBegin == null)
            {
                dt1 = System.Convert.ToDateTime(signal.DtBegin);
            }
            else
                dt1 = System.Convert.ToDateTime(dtBegin);

            if (dtEnd == null)
                dt2 = dt1.AddDays(1);
            else
                dt2 = System.Convert.ToDateTime(dtEnd);

            int j = 0;
            for (int i = 0; dt1.AddSeconds(i) < dt2; i++)
            {
                DataRow row = table.NewRow();
                if (j != signal.ValueCollection.Count && signal.ValueCollection[j] != null)
                {
                    if (System.Convert.ToDateTime(signal.ValueCollection[j].Timestamp.ToString()) <= dt1.AddSeconds(i))
                    {

                        var exit = false;
                        while (!exit)
                        {
                            if (j != signal.ValueCollection.Count - 1)
                                if (System.Convert.ToDateTime(signal.ValueCollection[j + 1].Timestamp.ToString()) >
                                    dt1.AddSeconds(i)) exit = true;
                            j++;
                            if (j >= signal.ValueCollection.Count)
                            {
                                j = signal.ValueCollection.Count;
                                exit = true;
                            }

                        }
                    }
                }
                DateTime t;
                if (signal.ValueCollection[0] == null) t = dt1.AddSeconds(i + 1);
                else t = System.Convert.ToDateTime(signal.ValueCollection[0].Timestamp);

                if (System.Convert.ToDateTime(dt1.AddSeconds(i)) < t)
                {
                    if (signal.PrevValue.Value == null)
                        row["Value"] = "";
                    else row["Value"] = signal.PrevValue.Value.ToString();
                    row["Quality"] = signal.PrevValue.Quality.ToString();
                }
                else
                {
                    if (signal.ValueCollection[j - 1].Value == null)
                        row["Value"] = "";
                    else row["Value"] = signal.ValueCollection[j - 1].Value.ToString();
                    row["Quality"] = signal.ValueCollection[j - 1].Quality.ToString();
                }
                row["Timestamp"] = dt1.AddSeconds(i).ToString();
                row["key"] = i;
                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable GetSignalValuesByMinutes(Signal signal, string dtBegin = null, string dtEnd = null)
        {

            var table = new DataTable();

            if (signal.ValueCollection.Count == 0 || signal.ValueCollection[0] == null)
            {
                if (signal.PrevValue.Value == null)
                    return table;
            }
            DateTime dt1;
            DateTime dt2;
            List<OpcRecord> result = new List<OpcRecord>();

            var key = new DataColumn();
            key.DataType = System.Type.GetType("System.Int32");
            key.ColumnName = "key";
            table.Columns.Add(key);


            var value = new DataColumn();
            value.DataType = System.Type.GetType("System.String");
            value.ColumnName = "Value";
            table.Columns.Add(value);

            var quality = new DataColumn();
            value.DataType = System.Type.GetType("System.String");
            quality.ColumnName = "Quality";
            table.Columns.Add(quality);

            var timestamp = new DataColumn();
            timestamp.DataType = System.Type.GetType("System.String");
            timestamp.ColumnName = "Timestamp";
            table.Columns.Add(timestamp);


            DataColumn[] keys = new DataColumn[1];
            keys[0] = key;
            table.PrimaryKey = keys;


            if (dtBegin == null)
            {
                dt1 = System.Convert.ToDateTime(signal.DtBegin);
            }
            else
                dt1 = System.Convert.ToDateTime(dtBegin);

            if (dtEnd == null)
                dt2 = dt1.AddDays(1);
            else
                dt2 = System.Convert.ToDateTime(dtEnd);

            int j = 0;
            for (int i = 0; dt1.AddMinutes(i) < dt2; i++)
            {
                DataRow row = table.NewRow();
                if (j != signal.ValueCollection.Count && signal.ValueCollection[j] != null)
                {
                    if (System.Convert.ToDateTime(signal.ValueCollection[j].Timestamp.ToString()) <= dt1.AddMinutes(i))
                    {

                        var exit = false;
                        while (!exit)
                        {
                            if (j != signal.ValueCollection.Count - 1)
                                if (System.Convert.ToDateTime(signal.ValueCollection[j + 1].Timestamp.ToString()) >
                                    dt1.AddMinutes(i)) exit = true;
                            j++;
                            if (j >= signal.ValueCollection.Count)
                            {
                                j = signal.ValueCollection.Count;
                                exit = true;
                            }

                        }
                    }
                }
                DateTime t;
                if (signal.ValueCollection[0] == null) t = dt1.AddMinutes(i + 1);
                else t = System.Convert.ToDateTime(signal.ValueCollection[0].Timestamp);

                if (System.Convert.ToDateTime(dt1.AddMinutes(i)) < t)
                {
                    if (signal.PrevValue.Value == null)
                        row["Value"] = "";
                    else row["Value"] = signal.PrevValue.Value.ToString();
                    row["Quality"] = signal.PrevValue.Quality.ToString();
                }
                else
                {
                    if (signal.ValueCollection[j - 1].Value == null)
                        row["Value"] = "";
                    else row["Value"] = signal.ValueCollection[j - 1].Value.ToString();
                    row["Quality"] = signal.ValueCollection[j - 1].Quality.ToString();
                }
                row["Timestamp"] = dt1.AddMinutes(i).ToString();
                row["key"] = i;
                table.Rows.Add(row);
            }

            return table;
        }
    }
}

