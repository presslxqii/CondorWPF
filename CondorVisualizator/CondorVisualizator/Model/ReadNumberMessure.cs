using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Helpers;
using Microsoft.SqlServer.Server;
using Microsoft.Windows.Controls;

namespace CondorVisualizator.Model
{   //Создали новый класс
    class ReadNumberMessure
    {   //Объявили листа классов Messurement
        public List<NumMess> messur3;
       //Конструктор
        public ReadNumberMessure()
        {

            //Инцелизировали messure
            messur3 = new List<NumMess>();
            //Подключились к серверу истории
            var HDAS = Connect.ServerHdaConnect();
            //Создали лист сигналов
            List<string> listag2 = new List<string>();
            //Добавили в лист определённый сигнал
            listag2.Add("Kondor.Number_measure");
            //Объявили новую переменную и проинцелезировали уже созданным типом
            var Di = new SignalDict(HDAS, listag2, null, "27.10.2016 00:00:00", DateTime.Now.ToLongDateString());
            //Достали тот сигнал который нас интересует
            var opredsignal = Di.Dictionary["Kondor.Number_measure"];
            //var test = Di.Dictionary["Kondor.Plotnomer.kachestvo"].ValueCollection[0].Value;
            //for (var min = 0; min <= (opredsignal.DtEnd - opredsignal.DtBegin).TotalMinutes - 1; min++)
            //Создали цикл для вытаскивание всех значений сигнала
            for (var sec = 0; sec <= (opredsignal.DtEnd - opredsignal.DtBegin).TotalSeconds - 1; sec++)
            {
                //Создали новую переменную  и отдали значение
                var dt = opredsignal.DtBegin.AddSeconds(sec);
                //Создали новую переменную  и отдали значение из метода GetValue
                var alarm1 = Seconds.GetValue(opredsignal, dt).OpcRec;
                //Создали новую переменную и проинцелизировали её как тип данных
                var alarms1 = new NumMess();
                alarms1.quality3 = alarm1.Quality;
                alarms1.time3 = alarm1.Timestamp;
                alarms1.value3 = (short) alarm1.Value;
                messur3.Add(alarms1);
             
            }

        }


        //Создали метод, чтобы передать лист со значениями
        public List<NumMess> GetMassurm2()
        {
            return messur3;
        }
       
    }
}
