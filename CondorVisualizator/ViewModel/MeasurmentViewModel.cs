using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CondorVisualizator.Model;
using CondorVisualizator.View;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace CondorVisualizator.ViewModel
{
    public class MeasurmentViewModel : ViewModelBase
    {
        
        private bool _selectButton;
        public bool IsEnableSelectButton
        {
            get { return _selectButton; }
            set { Set(ref _selectButton, value); }
        }



        private Areas _selecedMeasurment;
        public Areas SelectMeasurment
        {
            get { return _selecedMeasurment; }
            set
            {
                Set(ref _selecedMeasurment, value);
                IsEnable();
            }
        }

        private bool _flag = false;
        private Areas _correctAreas = new Areas();

        public Areas CorrectAreas
        {
            get { return _correctAreas; }
            set { Set(ref _correctAreas, value); }
        }
        public RelayCommand OkRelayCommand { get; set; }
        public RelayCommand CloseRelayCommand { get; set; }
        public Areas Band = new Areas();

        public MeasurmentViewModel()
        {

            
          
            Messenger.Default.Register<Areas>(this, "BandSelected", message =>
            {
                Band = message;
                SelectedMeasurment(Band);
            });
            //Messenger.Default.Register<bool>(this, "IsEnableBand", IsEnable);
            IsEnableSelectButton = false;
            OkRelayCommand = new RelayCommand(Select);
            CloseRelayCommand = new RelayCommand(Close);
            Messenger.Default.Send(false, "IsEnableWindow");
        }
        public void SelectedMeasurment(Areas argument)
        {
            SelectMeasurment = argument;
            Messenger.Default.Send(false, "IsEnableWindow");
        }

        private void IsEnable()
        {
            try
            {
                foreach (var road in SelectMeasurment.BandCollection)
                {
                    if (road.Check)
                    {
                        IsEnableSelectButton = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(@"Выберите участок дороги", @"Участок дороги",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
           
        }


        private void Close()
        {
            SendClose(true);
            Messenger.Default.Send(true, "IsEnableWindow");
        }
        private void Select()
        {
            //var flagCoefficient = false;
            CorrectAreas = new Areas();
            foreach (var road in SelectMeasurment.BandCollection)
            {
                if (!road.Check) continue;
                CorrectAreas.BandCollection.Add(road);
                _flag = true;
                //if (road.CoefficientTF)
                //{
                //    flagCoefficient = true;
                //}
            }
            SendClose(true);
            Messenger.Default.Send(CorrectAreas, "CorrectBandSelected");
            
         
           
            
        }

        //public void SendCorrectBand(Areas areas)
        //{
        //    Messenger.Default.Send(areas, "CorrectBandSelected");
        //}
        public void SendClose(bool argument)
        {
            Messenger.Default.Send(argument, "CloseNewWindow");
        }
        //public void ShowCoefficientWindow(bool argument)
        //{
        //    Messenger.Default.Send(argument, "CoefficientWindow");
        //}
    }
}
