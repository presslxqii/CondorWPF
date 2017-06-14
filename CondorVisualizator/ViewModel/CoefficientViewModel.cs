using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using CondorVisualizator.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MessageBox = System.Windows.Forms.MessageBox;

namespace CondorVisualizator.ViewModel
{
    public class CoefficientViewModel : ViewModelBase 
    {
        private double _coefficientK;
        //public double CoefficientK
        //{
        //    get { return _coefficientK; }
        //    set { Set(ref _coefficientK, value); }
        //}

        private List<DataRoad> _listBand;
        public List<DataRoad> ListBand
        {
            get { return _listBand; }
            set { Set(ref _listBand, value); }
        }
        private DataRoad _band;
        public DataRoad Band
        {
            get { return _band; }
            set { Set(ref _band, value); }
        }

        private bool _coefficientAll;
        private double _coefficientB;
        //public double CoefficientB
        //{
        //    get { return _coefficientB; }
        //    set { Set(ref _coefficientB, value); }
        //}
        public RelayCommand OkRelayCommand { get; set; }
        public RelayCommand CloseRelayCommand { get; set; }

        public CoefficientViewModel()
        {

            OkRelayCommand = new RelayCommand(CathExeption);
            CloseRelayCommand = new RelayCommand(Close);

            Messenger.Default.Register<List<DataRoad>>(this, "CollectionBands", Coefficient);
            //Messenger.Default.Register<List<object>>(this, "CoefficientAllBand", message =>
            //{
            //    _coefficientAll = Convert.ToBoolean(message[0]);
            //    _coefficientB = Convert.ToDouble(message[1]);
            //    _coefficientK = Convert.ToDouble(message[2]);

            //});

        }
        public void Coefficient(List<DataRoad> band)
        {
            var argument =new List<DataRoad>();
            //foreach (var road in band)
            //{
            //    road.KoefficB = 0;
            //    road.KoefficK = 0;
            //}
            foreach (var road in band)
            {
                road.KoefficB = 0;
                road.KoefficK = 0;
                if (!road.CoefficientTF)
                {
                    argument.Add(road);
                }
            }
            ListBand = argument;


            



            //var coefficients = new List<double> {CoefficientB, CoefficientK};
            //if (CoefficientK <= 0 || CoefficientB < 0)
            //{
            //    MessageBox.Show(@"Значения не могут быть меньше 0", @"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
         
            //else
            //{
            //    //SendCoefficient(coefficients);
            //    Messenger.Default.Send(coefficients, "Coefficient");
                
            //}

            

        }

        public void Select()
        {
            Messenger.Default.Send(ListBand, "Coefficient");
            //Band.KoefficB = _coefficientB;
            //Band.KoefficK = _coefficientK;
            ShowCoefficientWindow(false); 
        }

        public void CathExeption()
        {
            var flag = true;
            foreach (var road in ListBand)
            {
                var min = ((road.GraphicsesCollection[0].IntensityN1 +
                                    road.GraphicsesCollection[0].IntensityN2) / 2 -
                                   road.KoefficB) /
                                  road.KoefficK;
                for (var i = 0; i < road.GraphicsesCollection.Count - 1; i ++)
                {
                    var per = ((road.GraphicsesCollection[i].IntensityN1 +
                                        road.GraphicsesCollection[i].IntensityN2) / 2 -
                                       road.KoefficB) /
                                      road.KoefficK;
                    if (per < min)
                    {
                        min = per;
                    }
                }
                if (min < 0)
                {
                    flag = false;
                    MessageBox.Show(@"Показание плотности не могут быть отрицательными. Исправьте коэфициенты для полосы №"+road.Band, @"Ошибка ввода коэффициентов", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
              
              
            }

            if (flag)
            {
                Select();
            }


        }


        public void Close()
        {
          
            ShowCoefficientWindow(false);
            Messenger.Default.Send(true, "IsEnableWindow");
        }
        
        public void ShowCoefficientWindow(bool argument)
        {
            Messenger.Default.Send(argument, "CoefficientWindow");
        }
    }
}
