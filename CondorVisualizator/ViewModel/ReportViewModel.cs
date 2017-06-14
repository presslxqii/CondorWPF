using System.Collections.Generic;
using System.IO.Packaging;
using CondorVisualizator.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CondorVisualizator.ViewModel
{
    public class ReportViewModel : ViewModelBase 
    {
        public RelayCommand OkRelayCommand { get; set; }
        public RelayCommand CloseRelayCommand { get; set; }
        public string TextNumMess { get; set; }
        public bool Density { get; set; }
        public bool Rutting { get; set; }
        public bool Thicknesses { get; set; }


        public ReportViewModel ()
        {
           
            OkRelayCommand = new RelayCommand(Select);
            CloseRelayCommand = new RelayCommand(Close);

        }
        private void Close()
        {
            SendCloseSettingsReport(true);
        }
        private void Select()
        {

            var trueFalse = new List<bool> {Density, Thicknesses, Rutting};

            Messenger.Default.Send(trueFalse, "TrueFalse");

            SendNumMess(TextNumMess);
            SendSettingsReport(true);
            SendCloseSettingsReport(true);
        }
        public void SendSettingsReport(bool argument)
        {
            Messenger.Default.Send(argument, "CorrectSettingsReport");
        }
        public void SendCloseSettingsReport(bool argument)
        {
            Messenger.Default.Send(argument, "CloseSettingsReport");
        }

        public void SendNumMess(string argument)
        {
            Messenger.Default.Send(argument, "SendNumMess");
        }
     
     
     

    }
}
