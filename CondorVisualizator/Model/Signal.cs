using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Opc;
using Opc.Da;
using OPCRecord;
using System.Diagnostics;

namespace signals
{

    public class Signal
    {
        public string Tag;
        public string ValueType;
        public object Obj;
        public OpcRecord PrevValue;
        public List<OpcRecord> ValueCollection;
        public OpcRecord NextValue;
        public Dictionary<string, string> PropDict;
        public DateTime DtBegin;
        public DateTime DtEnd;


        /// <summary>
        /// Сигнал со словарем свойств сигнала (propID массив идентификаторов сигнала) 
        /// </summary>
        public static string GetPropertyValue(Opc.Da.Server server_da, string tagName, int PropID)
        {
            string tempString = "";
            try
                //Соединяемся с сервером и считываем свойство сигнала PropID
            {
                EventLog.WriteEntry("test", tagName);
                Opc.Da.Item[] temp = new Opc.Da.Item[1];

                temp[0] = new Opc.Da.Item(new Opc.ItemIdentifier(tagName));

                Opc.Da.ItemValueResult[] values = server_da.Read(temp);

                PropertyID[] propId = new PropertyID[1];

                ItemIdentifier[] itemProp = new ItemIdentifier[] {new ItemIdentifier(tagName)};
                ItemPropertyCollection[] propCollection;
                //EventLog.WriteEntry("test", "GetProperties - " + itemProp[0].ItemName + "; name - " + propId[0].Name);
                propCollection = server_da.GetProperties(itemProp, null, true);

                //EventLog.WriteEntry("test", "GetProperties - ok");
                if (propCollection.Count() > 0)
                    foreach (Opc.Da.ItemProperty prop in propCollection[0])
                    {
                        if (prop.ID.Code == PropID) tempString = System.Convert.ToString(prop.Value);
                    }
            }
            catch
                //В случае неудачного чтения, возвращаем null
            {
                tempString = null;
            }
            return tempString;
        }







        public Signal(Opc.Da.Server serverDa, string tag, string[] propID = null)
        {
            Tag = tag;
            PropDict = new Dictionary<string, string>();

            string alarmText;
            alarmText = GetPropertyValue(serverDa, tag, 1);
            if (alarmText == "System.Byte" ^ alarmText == "System.UInt16" ^ alarmText == "System.UInt32" ^ alarmText == "System.SByte" ^ alarmText == "System.Int16" ^ alarmText == "System.Int32")
                ValueType = "long";
            if (alarmText == "System.Single" ^ alarmText == "System.Double")
                ValueType = "double";
            if (alarmText == "System.String")
                ValueType = "string";
            if (alarmText == "System.Boolean")
                ValueType = "bool";

            if (propID != null)
            {
                for (int i = 0; i < propID.Count(); i++)
                {
                    alarmText = GetPropertyValue(serverDa, tag, System.Convert.ToInt32(propID[i]));
                    if (!PropDict.ContainsKey(propID[i]))
                    {
                        PropDict.Add(propID[i], alarmText);
                    }
                    else PropDict.Add(propID[i], null);
                }
            }
        }
    }

}
