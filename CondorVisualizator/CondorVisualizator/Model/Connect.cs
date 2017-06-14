using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CondorVisualizator.Model
{
    class Connect
    {
        public static string MasterIpHda;
        public static string SlaveIpHda;
        public static string OpcHdaId;
        public static Opc.Hda.Server ServerHdaConnect()
        {
            var tagsList = new Tags();
            
            //Задаем расположение серверов
            foreach (var tag in tagsList.GetMassurm())
            {
                switch (tag.Name)
                {
                    case "MasterIpHda":
                        MasterIpHda = tag.Value;
                        break;
                    case "SlaveIpHda":
                        SlaveIpHda = tag.Value;
                        break;
                    case "OpcHdaId":
                        OpcHdaId = tag.Value;
                        break;
                }
            }
            //const string masterIpHda = "localhost";
            //const string slaveIpHda = "localhost";
            //const string opcHdaId = "Infinity.OPCHDAServer";
            //Соединение с сервером
            var url = new Opc.URL("opchda://" + MasterIpHda + "/" + OpcHdaId);
            var serverHda = new Opc.Hda.Server(new OpcCom.Factory(), url);
            try
            {
                serverHda.Connect();
            }
            catch
            {
                url = new Opc.URL("opchda://" + SlaveIpHda + "/" + OpcHdaId);
                serverHda = new Opc.Hda.Server(new OpcCom.Factory(), url);
                try
                {
                    serverHda.Connect();
                }
                catch (Exception )
                {
                    MessageBox.Show("Ошибка подключения");
                }
            }
            // System.Threading.Thread.Sleep(1500);
            return serverHda;
        }
    }
}
