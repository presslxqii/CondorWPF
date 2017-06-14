using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPCRecord;
using signals;

namespace CondorVisualizator.Model
{
    class SignalMath
    {
        public static OpcRecord GetLast(Signal signal, object obj_dt = null)
        {
            var dt = DateTime.Now;
            if (obj_dt is string) dt = System.Convert.ToDateTime(obj_dt);
            else
                if (obj_dt is DateTime) dt = (DateTime)obj_dt;
                else
                    dt = signal.DtEnd;

            var rec = new OpcRecord();
            if (obj_dt == null)
            {
                rec.Timestamp = signal.DtEnd.ToString();
                dt = signal.DtEnd;
            }
            else rec.Timestamp = dt.ToString();
            var exit = false;
            if (signal.ValueCollection == null) return rec;
            var i = signal.ValueCollection.Count - 1;
            if (i == 0) exit = true;

            while (!exit)
            {
                var val = signal.ValueCollection[i];
                if (val.Quality >= 192 && dt <= System.Convert.ToDateTime(val.Timestamp))
                {
                    exit = true;
                    rec.Quality = val.Quality;
                    rec.Timestamp = val.Timestamp;
                    rec.Value = val.Value;
                }
                i--;
                if (i < 0) exit = true;
            }
            if (rec.Value == null || rec.Quality < 192)
            {
                if (signal.PropDict.ContainsKey("2"))
                {
                    rec.Value = signal.PropDict["2"];
                    rec.Quality = 192;
                    rec.Timestamp = dt.ToString();
                }
            }

            return rec;
        }
        public static int CountSwitches(Signal signal, string s)
        {

            int res = 0;
            var prev = signal.PrevValue;
            var current = signal.PrevValue;
            foreach (var v in signal.ValueCollection)
            {
                if (v != null && v.Value != null)
                {
                    current = v;
                    if (prev.Value == null)
                    {
                        if (signal.ValueType == "bool") prev.Value = false;
                        else
                            if (signal.ValueType == "string") prev.Value = "";
                            else prev.Value = 0;
                    }
                    if (prev.Value.ToString().ToLower() == s.ToLower() && current.Value.ToString().ToLower() != s.ToLower() && prev.Quality >= 192 && current.Quality >= 192)
                        res++;
                }
                prev = v;
            }
            var next = signal.NextValue;
            if (current.Value == null)
            {
                if (signal.ValueType == "bool") current.Value = false;
                else
                    if (signal.ValueType == "string") current.Value = "";
                    else current.Value = 0;
            }


            if (current.Value.ToString() == s.ToLower() && current.Quality >= 192)
                res++;
            return res;
        }

        public static int Count(Signal signal, string s)
        {

            int res = 0;
            foreach (var v in signal.ValueCollection)
            {
                if (v != null && v.Value != null)
                {
                    if (v.Value.ToString().ToLower() == s.ToLower() && v.Quality >= 192)
                        res++;
                }
            }
            return res;
        }


        public static double SumTime(Signal signal, string s)
        {
            var prev = signal.PrevValue;
            if (System.Convert.ToDateTime(prev.Timestamp) < signal.DtBegin) prev.Timestamp = signal.DtBegin.ToString();
            var current = prev;
            double time = 0;
            foreach (var v in signal.ValueCollection)
            {
                if (v != null && v.Value != null)
                {
                    current = v;
                    if (prev.Value == null)
                    {
                        if (signal.ValueType == "bool") prev.Value = false;
                        else
                            if (signal.ValueType == "string") prev.Value = "";
                            else prev.Value = 0;
                    }
                    if (prev.Value.ToString().ToLower() == s.ToLower() && prev.Quality >= 192)
                    {
                        time = time + (System.Convert.ToDateTime(current.Timestamp) - System.Convert.ToDateTime(prev.Timestamp)).TotalHours;
                    }
                }
                prev = v;

            }
            var next = signal.NextValue;
            if (System.Convert.ToDateTime(next.Timestamp) > signal.DtEnd) next.Timestamp = signal.DtEnd.ToString();
            if (current.Value == null)
            {
                if (signal.ValueType == "bool") current.Value = false;
                else
                    if (signal.ValueType == "string") current.Value = "";
                    else current.Value = 0;
            }

            if (current.Value.ToString().ToLower() == s.ToLower() && current.Quality >= 192)
            {
                time = time + (System.Convert.ToDateTime(signal.DtEnd) - System.Convert.ToDateTime(current.Timestamp)).TotalHours;
            }
            return time;
        }

        public static Signal UnionSignals(List<Signal> LstSignal, string TagName)
        {
            if (LstSignal.Count < 1) return null;
            var newSignal = LstSignal[0];
            if (LstSignal.Count == 1)
            {
                newSignal = Union2Signals(newSignal, newSignal);
                return newSignal;
            }

            for (var i = 1; i < LstSignal.Count; i++)
            {
                newSignal = Union2Signals(newSignal, LstSignal[i]);
            }
            newSignal.Tag = TagName;
            newSignal.ValueType = "long";
            return newSignal;
        }
        private static Signal Union2Signals(Signal s1, Signal s2)
        {
            var exit = false;
            var s = new Signal(null, "");
            var newLst = new List<OpcRecord>();
            s.ValueCollection = newLst;

            if (s1.DtBegin < s2.DtBegin) s.DtBegin = s1.DtBegin; else s.DtBegin = s2.DtBegin;
            if (s1.DtEnd > s2.DtEnd) s.DtEnd = s1.DtEnd; else s.DtEnd = s2.DtEnd;

            var prevS1 = new OpcRecord();
            prevS1.Value = s1.PrevValue.Value;
            prevS1.Timestamp = s1.PrevValue.Timestamp;
            prevS1.Quality = s1.PrevValue.Quality;

            var prevS2 = new OpcRecord();
            prevS2.Value = s2.PrevValue.Value;
            prevS2.Timestamp = s2.PrevValue.Timestamp;
            prevS2.Quality = s2.PrevValue.Quality;

            var nextS1 = new OpcRecord();
            nextS1.Value = s1.NextValue.Value;
            nextS1.Timestamp = s1.NextValue.Timestamp;
            nextS1.Quality = s1.NextValue.Quality;

            var nextS2 = new OpcRecord();
            nextS2.Value = s2.NextValue.Value;
            nextS2.Timestamp = s2.NextValue.Timestamp;
            nextS2.Quality = s2.NextValue.Quality;

            OpcRecord val_s1_prev = prevS1;
            OpcRecord val_s2_prev = prevS2;
            OpcRecord val_s2 = prevS2;
            OpcRecord val_s1 = prevS1;
            bool endOne = false;

            if (prevS1.Quality < 192) prevS1.Value = 0;
            if (nextS1.Quality < 192) nextS1.Value = 0;
            if (prevS2.Quality < 192) prevS2.Value = 0;
            if (nextS2.Quality < 192) nextS2.Value = 0;

            if (prevS1.Timestamp == null) prevS1.Timestamp = s.DtBegin.ToString();
            if (prevS2.Timestamp == null) prevS2.Timestamp = s.DtBegin.ToString();
            if (nextS1.Timestamp == null) nextS1.Timestamp = s.DtEnd.ToString();
            if (nextS2.Timestamp == null) nextS2.Timestamp = s.DtEnd.ToString();


            if (System.Convert.ToDateTime(prevS1.Timestamp) > System.Convert.ToDateTime(prevS2.Timestamp))
                s.PrevValue = prevS1;
            else
                s.PrevValue = prevS2;

            if (prevS1.Quality < 192 && prevS2.Quality >= 192) s.PrevValue = prevS2;
            if (prevS1.Quality >= 192 && prevS2.Quality > 192) s.PrevValue = prevS1;

            if (System.Convert.ToDateTime(nextS1.Timestamp) <= System.Convert.ToDateTime(nextS2.Timestamp))
                s.NextValue = nextS1;
            else
                s.NextValue = nextS2;

            if (nextS1.Quality < 192 && nextS2.Quality >= 192) s.NextValue = nextS2;
            if (nextS1.Quality >= 192 && nextS2.Quality > 192) s.NextValue = nextS1;

            s.PrevValue.Value = System.Convert.ToInt32(prevS1.Value) + System.Convert.ToInt32(prevS2.Value);
            s.PrevValue.Timestamp = s.DtBegin.ToString();

            try
            {
                if (s1.ValueCollection[s1.ValueCollection.Count - 1] != null && s2.ValueCollection[s2.ValueCollection.Count - 1] != null)
                    s.NextValue.Value = System.Convert.ToInt32(s1.ValueCollection[s1.ValueCollection.Count - 1].Value) + System.Convert.ToInt32(s2.ValueCollection[s2.ValueCollection.Count - 1].Value);

                if (s1.ValueCollection[s1.ValueCollection.Count - 1] == null && s2.ValueCollection[s2.ValueCollection.Count - 1] != null) s.NextValue.Value = s2.ValueCollection[s2.ValueCollection.Count - 1].Value;
                if (s1.ValueCollection[s1.ValueCollection.Count - 1] != null && s2.ValueCollection[s2.ValueCollection.Count - 1] == null) s.NextValue.Value = s1.ValueCollection[s1.ValueCollection.Count - 1].Value;

                s.NextValue.Timestamp = s.DtEnd.ToString();
            }
            catch { }

            if (s1.ValueCollection[0] == null && s2.ValueCollection[0] == null)
            {
                s.ValueCollection.Add(null);
                return s;
            }

            if (s1.ValueCollection[0] == null)
            {

                var prev = 0;
                var lastDt = s2.DtEnd;
                lastDt = System.Convert.ToDateTime(nextS1.Timestamp);
                if (prevS1.Quality > 192) prev = System.Convert.ToInt32(prevS1.Value);

                foreach (var r in s2.ValueCollection)
                {
                    var rec = new OpcRecord();
                    rec.Timestamp = r.Timestamp;
                    var s1val = 0;
                    if (r.Quality >= 192) s1val = System.Convert.ToInt32(r.Value);

                    if (System.Convert.ToDateTime(r.Timestamp) < lastDt)
                        rec.Value = s1val + prev;

                    rec.Quality = r.Quality;
                    if (r.Quality < 192 && System.Convert.ToInt32(r.Value) > 0) rec.Quality = 192;

                    s.ValueCollection.Add(rec);
                }
                return s;
            }

            if (s2.ValueCollection[0] == null)
            {
                var prev = 0;
                var lastDt = s1.DtEnd;
                lastDt = System.Convert.ToDateTime(nextS2.Timestamp);
                if (prevS2.Quality > 192) prev = System.Convert.ToInt32(prevS2.Value);

                foreach (var r in s1.ValueCollection)
                {
                    var rec = new OpcRecord();
                    rec.Timestamp = r.Timestamp;
                    var s2val = 0;
                    if (r.Quality >= 192) s2val = System.Convert.ToInt32(r.Value);

                    if (System.Convert.ToDateTime(r.Timestamp) < lastDt)
                        rec.Value = s2val + prev;

                    rec.Quality = r.Quality;
                    if (r.Quality < 192 && System.Convert.ToInt32(r.Value) > 0) rec.Quality = 192;

                    s.ValueCollection.Add(rec);
                }
                return s;
            }

            var i = 0;
            var j = 0;
            while (!exit)
            {

                if (i < s1.ValueCollection.Count) val_s1 = s1.ValueCollection[i];
                else
                {
                    val_s1 = val_s2;
                    endOne = true;

                }
                if (j < s2.ValueCollection.Count) val_s2 = s2.ValueCollection[j];
                else
                {
                    val_s2 = val_s1;
                    endOne = true;
                }

                if (i != 0 && i - 1 < s1.ValueCollection.Count) val_s1_prev = s1.ValueCollection[i - 1];
                if (j != 0 && j - 1 < s2.ValueCollection.Count) val_s2_prev = s2.ValueCollection[j - 1];

                var newRec = new OpcRecord();
                if (System.Convert.ToDateTime(val_s1.Timestamp) < System.Convert.ToDateTime(val_s2.Timestamp))
                {
                    newRec.Value = val_s1.Value;
                    newRec.Quality = val_s1.Quality;
                    newRec.Timestamp = val_s1.Timestamp;

                    if (val_s2_prev != null)
                        if (System.Convert.ToDateTime(val_s2_prev.Timestamp) <= System.Convert.ToDateTime(val_s1.Timestamp))
                        {
                            var v1 = 0;
                            if (val_s2_prev.Quality >= 192) v1 = System.Convert.ToInt32(val_s2_prev.Value);
                            var v2 = 0;
                            if (val_s1.Quality >= 192) v2 = System.Convert.ToInt32(val_s1.Value);
                            newRec.Value = v1 + v2;
                        }

                    newLst.Add(newRec);
                    i++;
                }
                if (System.Convert.ToDateTime(val_s1.Timestamp) > System.Convert.ToDateTime(val_s2.Timestamp))
                {
                    newRec.Value = val_s2.Value;
                    newRec.Quality = val_s2.Quality;
                    newRec.Timestamp = val_s2.Timestamp;

                    if (val_s1_prev != null)
                        if (System.Convert.ToDateTime(val_s1_prev.Timestamp) <= System.Convert.ToDateTime(val_s2.Timestamp))
                        {
                            var v1 = 0;
                            if (val_s1_prev.Quality >= 192) v1 = System.Convert.ToInt32(val_s1_prev.Value);
                            var v2 = 0;
                            if (val_s2.Quality >= 192) v2 = System.Convert.ToInt32(val_s2.Value);
                            newRec.Value = v1 + v2;
                        }

                    newLst.Add(newRec);
                    j++;
                }
                if (System.Convert.ToDateTime(val_s1.Timestamp) == System.Convert.ToDateTime(val_s2.Timestamp))
                {
                    newRec.Value = val_s1.Value;
                    newRec.Quality = val_s1.Quality;
                    newRec.Timestamp = val_s1.Timestamp;

                    if (newRec.Quality < 192)
                    {
                        newRec.Value = val_s2.Value;
                        newRec.Quality = val_s2.Quality;
                        newRec.Timestamp = val_s2.Timestamp;
                    }

                    if (!endOne)
                    {
                        var v1 = 0;
                        if (val_s1.Quality >= 192) v1 = Convert.ToInt32(val_s1.Value);
                        var v2 = 0;
                        if (val_s2.Quality >= 192) v2 = Convert.ToInt32(val_s2.Value);
                        newRec.Value = v1 + v2;
                    }
                    else
                    {
                        if (i >= s1.ValueCollection.Count)
                        {
                            var v1 = 0;
                            if (s1.ValueCollection[s1.ValueCollection.Count - 1].Quality >= 192) v1 = System.Convert.ToInt32(s1.ValueCollection[s1.ValueCollection.Count - 1].Value);
                            var v2 = 0;
                            if (val_s2.Quality >= 192) v2 = System.Convert.ToInt32(val_s2.Value);
                            newRec.Value = v1 + v2;
                        }
                        if (j >= s2.ValueCollection.Count)
                        {
                            var v1 = 0;
                            if (s2.ValueCollection[s2.ValueCollection.Count - 1].Quality >= 192) v1 = System.Convert.ToInt32(s2.ValueCollection[s2.ValueCollection.Count - 1].Value);
                            var v2 = 0;
                            if (val_s1.Quality >= 192) v2 = System.Convert.ToInt32(val_s1.Value);
                            newRec.Value = v1 + v2;
                        }
                    }

                    newLst.Add(newRec);
                    i++;
                    j++;
                }

                if (i >= s1.ValueCollection.Count && j >= s2.ValueCollection.Count)
                {
                    exit = true;
                }

            }

            s.ValueCollection = newLst;
            foreach (var r in s.ValueCollection)
            {
                if (r.Quality < 192 && System.Convert.ToInt32(r.Value) > 0)
                    r.Quality = 192;
            }


            return s;
        }
    }
}
