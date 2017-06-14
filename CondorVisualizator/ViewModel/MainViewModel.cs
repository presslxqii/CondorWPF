using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using CondorVisualizator.Model;
using CondorVisualizator.View;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;
using Task = System.Threading.Tasks.Task;

namespace CondorVisualizator.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase // INotifyPropertyChanged - почитать
    {

        private bool _coefficientKb;
        private double _coefficientK;
        private double _coefficientB;

        private readonly object _locker = new object();
#region
        private Areas correctBand { get; set; }
        private ObservableCollection<RoadName> _selectedItem;
        public ObservableCollection<RoadName> SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); }
            
        }


        private ObservableCollection<DataRoad> _massRoads;
        public ObservableCollection<DataRoad> MassRoads
        {
            get { return _massRoads; }
            set { Set(ref _massRoads, value); }

        }

        private DateTime? _beginDate;
        public DateTime? BeginDate
        {
            get { return _beginDate; }
            set { Set(ref _beginDate, value); }
        }
        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { Set(ref _endDate, value); }
        }
        private List<RoadName> _dataRoads = new List<RoadName>();
        public List<RoadName> DataRoads
        {
            get { return _dataRoads; }
            set { Set(ref _dataRoads, value); }
        }

        private ObservableCollection<Roads> _roadses = new ObservableCollection<Roads>();

        public ObservableCollection<Roads> Roadses
        {
             
            get { return _roadses; }
            set { Set(ref _roadses, value); }

        }
        private ObservableCollection<Areas> _areases;

        public ObservableCollection<Areas> Areases
        {
            get { return _areases; }
            set { Set(ref _areases, value); }
        }
        private Areas _okData;
        public Areas OkDataRoad
        {
            get { return _okData; }
            set { Set(ref _okData, value); }
        }

        //private int _band;

        private List<List<TimeStartStop>> _listListovnorm = new List<List<TimeStartStop>>();
        public List<List<TimeStartStop>> ListListovnorm
        {
            get { return _listListovnorm; }
            set { Set(ref _listListovnorm, value); }
        }
        private List<string> _startList = new List<string>();
        public List<string> Start
        {

            get { return _startList; }
            set { Set(ref _startList, value); }
        }
        private List<string> _endList = new List<string>();
        public List<string> End
        {
            get { return _endList; }
            set { Set(ref _endList, value); }
        }
        private List<List<TimeStartStop>> _listTime = new List<List<TimeStartStop>>();
        public List<List<TimeStartStop>> ListTime
        {
            get { return _listTime; }
            set { Set(ref _listTime, value); }
        }
        private List<TimeStartStop> _startStops = new List<TimeStartStop>();
        public List<TimeStartStop> StartStops
        {
            get { return _startStops; }
            set { Set(ref _startStops, value); }
        }
#region Band#1
        private CollectionGraphs _count = new CollectionGraphs();
        public CollectionGraphs Count
        {
            get { return _count; }
            set { Set(ref _count, value); }
        }
        private CollectionGraphs _general = new CollectionGraphs();
        public CollectionGraphs General
        {
            get { return _general; }
            set { Set(ref _general, value); }
        }
        private CollectionGraphs _layer1 = new CollectionGraphs();
        public CollectionGraphs Layer1
        {
            get { return _layer1; }
            set { Set(ref _layer1, value); }
        }

        private CollectionGraphs _rut = new CollectionGraphs();
        public CollectionGraphs Rut
        {
            get { return _rut; }
            set { Set(ref _rut, value); }
        }

        private CollectionGraphs _layer2 = new CollectionGraphs();
        public CollectionGraphs Layer2
        {
            get { return _layer2; }
            set { Set(ref _layer2, value); }
        }
        private CollectionGraphs _layer3 = new CollectionGraphs();
        public CollectionGraphs Layer3
        {
            get { return _layer3; }
            set { Set(ref _layer3, value); }
        }
        private CollectionGraphs _plotnost = new CollectionGraphs();
        public CollectionGraphs Plotnost
        {
            get { return _plotnost; }
            set { Set(ref _plotnost, value); }
        }
        private CollectionGraphs _allLayer = new CollectionGraphs();
        public CollectionGraphs AllLayer
        {
            get { return _allLayer; }
            set { Set(ref _allLayer, value); }
        }
#endregion
        private List<TimeStartStop> _newmass = new List<TimeStartStop>();
        public List<TimeStartStop> NewMass
        {
            get { return _newmass; }
            set { Set(ref _newmass, value); }
        }
        private string _infoRoad;
        public string InfoRoad
        {
            get { return _infoRoad; }
            set { Set(ref _infoRoad, value); }
        }
        private bool _buttonVisibility;
        public bool ButtonVisibility
        {
            get { return _buttonVisibility; }
            set { Set(ref _buttonVisibility, value); }
        }
        private bool _regulationProgressRingVisibility;
        public bool RegulationProgressRingVisibility
        {
            get { return _regulationProgressRingVisibility; }
            set { Set(ref _regulationProgressRingVisibility, value); }
        }
        private bool _regulationGen;
        public bool RegulationGen
        {
            get { return _regulationGen; }
            set { Set(ref _regulationGen, value); }
        }
        private bool _infoVision;
        public bool InfoVision
        {
            get { return _infoVision; }
            set { Set(ref _infoVision, value); }
        }
        private bool _parametrs;
        public bool Parametrs
        {
            get { return _parametrs; }
            set { Set(ref _parametrs, value); }
        }
        private bool _regulationPlotn;
        public bool RegulationPlotn
        {
            get { return _regulationPlotn; }
            set { Set(ref _regulationPlotn, value); }
        }
        private bool _regulationCoun;
        public bool RegulationCoun
        {
            get { return _regulationCoun; }
            set { Set(ref _regulationCoun, value); }
        }
        private bool _regulationLayer1;
        public bool RegulationLayer1
        {
            get { return _regulationLayer1; }
            set { Set(ref _regulationLayer1, value); }
        }
        private bool _regulationLayer2;
        public bool RegulationLayer2
        {
            get { return _regulationLayer2; }
            set { Set(ref _regulationLayer2, value); }
        }

        private bool _selectButton;
        public bool IsEnableSelectButton
        {
            get { return _selectButton; }
            set { Set(ref _selectButton, value); }
        }

        private bool _regulationLayer3;
        public bool RegulationLayer3
        {
            get { return _regulationLayer3; }
            set { Set(ref _regulationLayer3, value); }
        }
        private bool _regulationRut;
        public bool RegulationRut
        {
            get { return _regulationRut; }
            set { Set(ref _regulationRut, value); }
        }


        private bool _regulationAll;
        public bool RegulationAll
        {
            get { return _regulationAll; }
            set { Set(ref _regulationAll, value); }
        }
        private bool _regulationDrop;
        public bool RegulationDrop
        {
            get { return _regulationDrop; }
            set { Set(ref _regulationDrop, value); }
        }
        private bool _regulationRegPlot;
        public bool RegulationRegPlot
        {
            get { return _regulationRegPlot; }
            set { Set(ref _regulationRegPlot, value); }
        }
        private bool _regulationRegCount;
        public bool RegulationRegCoun
        {
            get { return _regulationRegCount; }
            set { Set(ref _regulationRegCount, value); }
        }
        private bool _regulationRegLay1;
        public bool RegulationRegLay1
        {
            get { return _regulationRegLay1; }
            set { Set(ref _regulationRegLay1, value); }
        }
        private bool _regulationRegLay2;
        public bool RegulationRegLay2
        {
            get { return _regulationRegLay2; }
            set { Set(ref _regulationRegLay2, value); }
        }
        private bool _regulationRegLay3;
        public bool RegulationRegLay3
        {
            get { return _regulationRegLay3; }
            set { Set(ref _regulationRegLay3, value); }
        }
        private bool _regulationRegRut;
        public bool RegulationRegRut
        {
            get { return _regulationRegRut; }
            set { Set(ref _regulationRegRut, value); }
        }
        private bool _regulationRegAll;
        public bool RegulationRegAll
        {
            get { return _regulationRegAll; }
            set { Set(ref _regulationRegAll, value); }
        }
        private bool _report;
        public bool Report
        {
            get { return _report; }
            set { Set(ref _report, value); }
        }
        private bool _border;
        public bool Border
        {
            get { return _border; }
            set { Set(ref _border, value); }
        }

       
       public int AddClean { get; set; }
       private double _size;
        public double Size 
         {
             get { return _size; }
             set { Set(ref _size, value); }
        }
        private ObservableCollection<ObservableCollection<RoadName>> _normList =
        new ObservableCollection<ObservableCollection<RoadName>>();
        public ObservableCollection<ObservableCollection<RoadName>> NormList
        {
            get { return _normList; }
            set { Set(ref _normList, value); }
        }

        private bool _cleanAdd;
        private string _DateStart;
        private double _standartsDensity;
        private double scrollWidth;
        private Areas BandsAreas { get; set; }
        public ChartData AllChart { get; set; }
        //List<List<Graphics>> CollectionGraphic;
        List<DataRoad> infoCollectionGraphic; 
        public RelayCommand AddRelayCommand { get; set; }
        
        public RelayCommand VisionRelayCommand { get; set; }
        public RelayCommand CleanRelayCommand { get; set; }
        public RelayCommand OkRelayCommand { get; set; }
        public RelayCommand FilterRelayCommand { get; set; }
        public RelayCommand ResetRelayCommand { get; set; }
        public RelayCommand ReportRelayCommand { get; set; }

        
 
        public RelayCommand<string> GraffCountCommand { get; set; }
        //public ScrollViewer ScrollWigth { get; set; }
        private double _koefficB;
        private double _koefficK;
#endregion
        public MainViewModel(IDataService dataService)

        {
            _cleanAdd = true;
            Parametrs = true;
            InfoVision = false;
            ButtonVisibility = false;
            RegulationDrop = false;
            RegulationGen = false;
            RegulationPlotn = false;
            RegulationCoun = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationRut = false;
            RegulationAll = false;
            Report = false;
            Border = false;
            IsEnableSelectButton = true;



            //Parametrs = true;
            //InfoVision = true;
            //ButtonVisibility = true;
            //RegulationDrop = true;
            //RegulationGen = true;
            //RegulationPlotn = true;
            //RegulationCoun = true;
            //RegulationLayer1 = true;
            //RegulationLayer2 = true;
            //RegulationLayer3 = true;
            //RegulationRut = true;
            //RegulationAll = true;
            //Report = true;
            //Border = true;
            //IsEnableSelectButton = true;

            dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                       
                    }

                });


            Messenger.Default.Register<List<DataRoad>>(this, "Coefficient", message =>
            {
                CorrectBand(message,1);
            });

            Messenger.Default.Register<bool>(this, "SelectionChanged", message =>
            {
                
                RoadSection(SelectedItem);
            });

                Messenger.Default.Register<bool>(this, "WidthGeneral", message =>
            {
                SendWidth(Size);
            });
            Messenger.Default.Register<bool>(this, "DensityStandartsNew", message =>
            {
                SendDensityStandart(_standartsDensity);
            });
            Messenger.Default.Register<bool>(this, "IsEnableWindow", message =>
            {
                IsEnableSelectButton = message;
            });
            Messenger.Default.Register<double>(this, "ScrollWidth", message =>
            {
                scrollWidth = message;
            });


            
            Messenger.Default.Register<bool>(this, "IsEnable", IsEnable);
            Messenger.Default.Register<double>(this, "width", message =>
            {
               Size  = message;
            });
            Messenger.Default.Register<bool>(this, "border", message =>
            {
                Border = message;
            });

            Messenger.Default.Register<Areas>(this, "CorrectBandSelected", message =>
            {
                correctBand = new Areas();
                correctBand = message;

                EnterCoefficient(correctBand);
                //CorrectBand(correctBand, 1);
            });

            //Messenger.Default.Register<object>(this, "SelectedBand", message =>
            //{

            //    var selectBand = message;
            //    CorrectBand(correctBand, selectBand);

            //});

            
             _DateStart = string.Format("{0}.{1}.{2} {3}:{4}:{5}", DateTime.Now.Day, DateTime.Now.Month - 1, DateTime.Now.Year,
                 DateTime.Now.Hour,DateTime.Now.Minute, DateTime.Now.Second);
             BeginDate = DateTime.Parse(_DateStart);
            var tempTime = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToShortDateString(), DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
            //var startstop = new ReadStartStop("29.05.2017 11:37:00", "29.05.2017 11:47:00");
            var startstop = new ReadStartStop(_DateStart, tempTime);
            _startStops = startstop.GetMassurm3();
            Get();
            CleanRelayCommand =new RelayCommand(Clean);
            AddRelayCommand = new RelayCommand(Add);
            OkRelayCommand = new RelayCommand(Select);
            FilterRelayCommand = new RelayCommand(Filter);
            ResetRelayCommand = new RelayCommand(Reset);
            VisionRelayCommand = new RelayCommand(Vision);
            GraffCountCommand = new RelayCommand<string>(Graff);
            ReportRelayCommand = new RelayCommand(ReportGen);
           
        }
        /// <summary>
        /// Чтение из сервера все измерения
        /// </summary>
        /// <param name="Roads"></param>
        ///  public async Task<ObservableCollection<RoadName> CicleRoadSection(int k = 40)
        
        private void RoadSection(ObservableCollection<RoadName> Roads)
        {
            MassRoads = new ObservableCollection<DataRoad>();
            var dataRoad = new DataRoad();
            foreach (var roadName in Roads)
            {
                var date = new DataRoadSection(roadName.TimeStart, roadName.TimeStop);
                dataRoad = new DataRoad
                {
                   
                    AllRoad = date.GetMassurm(),
                   
                    TimeStart = roadName.TimeStart,
                    TimeStop = roadName.TimeStop
                    
                   
                };
                //_coefficientKb = date.GetMassurm()[0].CoefficientTF;
                MassRoads.Add(dataRoad);
            }

            for (var r = 0; r < MassRoads.Count; r++)
            {
                for (var k = r + 1; k < MassRoads.Count; k++)
                {
                    if (MassRoads[r].Band > MassRoads[k].Band)
                    {
                        var temp = MassRoads[r];
                        MassRoads[r] = MassRoads[k];
                        MassRoads[k] = temp;
                    }
                }
            }
            //Roadses = new ObservableCollection<Roads>();
            Areases = new ObservableCollection<Areas>();
            var massBand = new Areas();
            for (var j = 0; j < MassRoads.Count; j++)
            {
                var flag = true;
                if (Areases.Count != 0)
                {
                    for (var l = 0; l < Areases.Count; l++)
                    {
                        if (Areases[l].AllRoad == MassRoads[j].AllRoad)
                        {

                            Areases[l].BandCollection.Add(MassRoads[j]);
                            Areases[l].AllRoad = MassRoads[j].AllRoad;
                            Areases[l].TimeStart = MassRoads[j].TimeStart;
                            Areases[l].TimeStop = MassRoads[j].TimeStop;
                            flag = false;
                        }
                    }
                    if (!flag) continue;
                   
                    massBand = new Areas();
                    massBand.BandCollection.Add(MassRoads[j]);
                    massBand.AllRoad = MassRoads[j].AllRoad;
                    massBand.TimeStart = MassRoads[j].TimeStart;
                    massBand.TimeStop = MassRoads[j].TimeStop;
                    Areases.Add(massBand);
                }
                else
                {
                   
                    massBand.AllRoad = MassRoads[j].AllRoad;
                    massBand.BandCollection.Add(MassRoads[j]);
                    massBand.TimeStart = MassRoads[j].TimeStart;
                    massBand.TimeStop = MassRoads[j].TimeStop;

                    Areases.Add(massBand);
                }
            }
           
        }


        private void IsEnable(bool ar)
        {
            if (OkDataRoad == null) return;
            IsEnableSelectButton = true;
        }

        private void ReportGen()
        {

            Messenger.Default.Send(AllChart, "Test");
        }



        private int _parametr;
        public ReadAllGraffics ReadAllGraffics;
        private ReadInfoRoad readInfoRoad;
       
        private DataRoad _selectRoad;

        public void Graff(string parametr)
        {
            string chart;
            switch (parametr)
            {
                case  "CountChart":
                    chart = "CountChart";
                    Parametr(Count, chart);
                    break;
                case "ChartLayer3":
                    chart = "ChartLayer3";
                    Parametr(Layer3, chart);
                    break;
                case "ChartLayer2":
                        chart = "ChartLayer2";
                    Parametr(Layer2, chart);
                    break;
                case "ChartLayer1":
                     chart = "ChartLayer1";
                    Parametr(Layer1, chart);
                    break;
                case "ChartAll":
                    chart = "ChartAll";
                    Parametr(AllLayer, chart);
                    break;
                case "Plotnost":
                    chart = "Plotnost";
                    Parametr(Plotnost, chart);
                    break;
                case "Rut":
                    chart = "Rut";
                    Parametr(Rut, chart);
                    break;
                case "GeneralGraff":
                 chart = "GeneralGraff";
                 Parametr(General, chart);
                    break;
            }
            
        }

        public void Parametr(CollectionGraphs argument1, string argument2)
        {
            var parametrs = new List<List<object>>();
            var parametrs2 = new List<object>();
            var parametrs3 = new List<object>();
            var parametrs4 = new List<object>();
            var parametrs5 = new List<object>();
            foreach (var graphicse in argument1.GraphicsesCollection)
            {
                if (argument2 == "CountChart")
                {
                    parametrs2.Add(graphicse.CountLayer);
                }
                if (argument2 == "ChartLayer3")
                {
                    parametrs2.Add(graphicse.Layer3);
                }
                if (argument2 == "ChartLayer2")
                {
                    parametrs2.Add(graphicse.Layer2);
                }
                if (argument2 == "ChartLayer1")
                {
                    parametrs2.Add(graphicse.Layer1);
                }
                if (argument2 == "ChartAll")
                {

                    parametrs2.Add(graphicse.Layer1);
                    parametrs4.Add(graphicse.Layer2);
                    parametrs5.Add(graphicse.Layer3);
                }
                if (argument2 == "Plotnost")
                {
                    parametrs2.Add(graphicse.IntensityN1);
                }
                if (argument2 == "Rut")
                {
                    parametrs2.Add(graphicse.Rut);
                }
                if (argument2 == "GeneralGraff")
                {
                    parametrs2.Add(graphicse.GeneralState);
                }
                parametrs3.Add(graphicse.Distance);
            }
            parametrs.Add(parametrs2);
            parametrs.Add(parametrs3);
            if (argument2 == "ChartAll")
            {
                 parametrs.Add(parametrs4);
                 parametrs.Add(parametrs5);
            }
            SendMessage(parametrs);
        }
        public void SendMessage(List<List<object>> listGraff)
        {
            Messenger.Default.Send(listGraff, "GreatGraphic");
        }
        /// <summary>
        /// Кнопка сброса графиков
        /// </summary>
        #region Button
        public void Vision()
        {
            Plotnost.GraphicsesCollection.Clear();
            General.GraphicsesCollection.Clear();
            AllLayer.GraphicsesCollection.Clear();
            General.GraphicsesCollection.Clear();
            Layer1.GraphicsesCollection.Clear();
            Layer2.GraphicsesCollection.Clear();
            Layer3.GraphicsesCollection.Clear();
            Rut.GraphicsesCollection.Clear();
            Parametrs = true;
            InfoVision = false;
            ButtonVisibility = false;
            RegulationAll = false;
            RegulationCoun = false;
            RegulationGen = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationRut = false;
            RegulationPlotn = false;
            Report = false;
            Border = false;
        }

       
        /// <summary>
        /// Кнопка увеличения масштаба
        /// </summary>

        public async void Add()
        {
            if (!_cleanAdd) return;
            

            try
            {
                _parametr += 1;
               

                    SendParametr(_parametr);
                
                //if (_okData[0].TimeStart == null || _okData[0].TimeStop == null) return;
                AddClean = AddClean + 5;
                Size = Size + 100;
                Parametrs = false;
                RegulationProgressRingVisibility = true;
                RegulationRegLay1 = true;
                RegulationRegLay2 = true;
                RegulationRegLay3 = true;
                RegulationRegRut = true;
                RegulationRegCoun = true;
                RegulationRegAll = true;
                RegulationRegPlot = true;
                //ReadAllGraffics = new ReadAllGraffics(, _okData.TimeStop);
                lock (_locker)
                {
                    infoCollectionGraphic = new List<DataRoad>();
                }
                for (var i = 0; i < BandsAreas.Band.Count; i++)
                {
                    ReadAllGraffics = new ReadAllGraffics(BandsAreas.Band[i].TimeStart, BandsAreas.Band[i].TimeStop);
                    lock (_locker)
                    {
                        infoCollectionGraphic.Add(readInfoRoad.GetMassurm());

                        infoCollectionGraphic[i].GraphicsesCollection = ReadAllGraffics.GetMassurm();
                    }
                }
               
                
                //Коллекция всех полос
                var countBand = await CicleCoun(AddClean);
                var generalBand = await CicleGen(AddClean);
                var densityBand = await CicleDensity(AddClean);
                var layer1Band = await CicleLay1(AddClean);
                var layer2Band = await CicleLay2(AddClean);
                var layer3Band = await CicleLay3(AddClean);
                var allLayerBand = await CicleAll(AddClean);
                var rutBand = await CicleRut(AddClean);
                //Биндинг на график с полосой №1
                //General = generalBand[0];
                AllChart = new ChartData
                {
                    AllLayerBand = allLayerBand,
                    CountBand = countBand,
                    DensityBand = densityBand,
                    Layer1Band = layer1Band,
                    Layer2Band = layer2Band,
                    Layer3Band = layer3Band,
                    GeneralBand = generalBand,
                    RutBand = rutBand
                };


                SendMessageGeneralPoint(new ObservableCollection<CollectionGraphs>(generalBand));
                SendMessageGeneralPoint(generalBand);
                SendWidth(Size);
               

                //SendMessageGeneralPoint(new ObservableCollection<Graphics>(General));
                //SendMessageGeneralPoint(General);
            }
            catch (Exception)
            {

                MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
         
        }
        /// <summary>
        /// Кнопка уменьшения масштаба
        /// </summary>
        public async void Clean()
        {
           try
            {
                
                if (_parametr > 5)
                {
                    _parametr -= 1;
                    SendParametr(_parametr);
                }
                //if (_okData[0].TimeStart == null || _okData[0].TimeStop == null) return;
                if (AddClean <= 40) return;
                AddClean = AddClean - 5;
                Size = Size - 100;
                Parametrs = false;
                RegulationProgressRingVisibility = true;
                RegulationRegLay1 = true;
                RegulationRegLay2 = true;
                RegulationRegLay3 = true;
                RegulationRegRut = true;
                RegulationRegCoun = true;
                RegulationRegAll = true;
                RegulationRegPlot = true;
                //ReadAllGraffics = new ReadAllGraffics(_okData.TimeStart, _okData.TimeStop);
              
                
                //Коллекция всех полос
                var countBand = await CicleCoun(AddClean);
                var generalBand = await CicleGen(AddClean);
                var densityBand = await CicleDensity(AddClean);
                var layer1Band = await CicleLay1(AddClean);
                var layer2Band = await CicleLay2(AddClean);
                var layer3Band = await CicleLay3(AddClean);
                var allLayerBand = await CicleAll(AddClean);
                var rutBand = await CicleRut(AddClean);
                //Биндинг на график с полосой №1
                //General = generalBand[0];
                AllChart = new ChartData
                {
                    AllLayerBand = allLayerBand,
                    CountBand = countBand,
                    DensityBand = densityBand,
                    Layer1Band = layer1Band,
                    Layer2Band = layer2Band,
                    Layer3Band = layer3Band,
                    GeneralBand = generalBand,
                    RutBand = rutBand
                };


                SendMessageGeneralPoint(new ObservableCollection<CollectionGraphs>(generalBand));
                SendMessageGeneralPoint(generalBand);
                SendWidth(Size);
                
                //SendMessageGeneralPoint(new ObservableCollection<Graphics>(General));
                //SendMessageGeneralPoint(General);
            }
           catch (Exception)
           {

               MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
           }
        
        }
        /// <summary>
        /// Кнопка выбора данных по диапозону выбранной даты
        /// </summary>
        public void Filter()
        {
            
            //try
            //{
                _selectRoad = new DataRoad { SelectbeginDate = BeginDate.ToString(), SelectendDate = EndDate.ToString() };
                var startstop = new ReadStartStop(_selectRoad.SelectbeginDate, _selectRoad.SelectendDate);
                _startStops = startstop.GetMassurm3();
                Get();
            //}
            //catch (Exception)
            //{

            //    MessageBox.Show(@"Выберите дату", @"Ошибка выбора даты", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
          

        }

        public void EnterCoefficient(Areas bands)
        {
            _parametr = 5;
            SendParametr(_parametr);
            BandsAreas = bands;
            NewWindow(false);
            //try
            //{

            Parametrs = false;
            InfoVision = true;
            ButtonVisibility = true;
            RegulationProgressRingVisibility = true;
            RegulationRegLay1 = true;
            RegulationRegLay2 = true;
            RegulationRegLay3 = true;
            RegulationRegRut = true;
            RegulationRegCoun = true;
            RegulationRegAll = true;
            RegulationRegPlot = true;
            Report = true;
            Border = true;
            //Чтение значений с сервера всех выбранных полос участка



            infoCollectionGraphic = new List<DataRoad>();
            //CollectionGraphic = new List<List<Graphics>>();
            //for (var i = 0; i < BandsAreas.Band.Count; i++)
            //{
            //    readInfoRoad = new ReadInfoRoad(BandsAreas.Band[i].TimeStart, BandsAreas.Band[i].TimeStop);
            //    infoCollectionGraphic.Add(readInfoRoad.GetMassurm());
            //}


            var flag = false;
            for (var i = 0; i < BandsAreas.Band.Count; i++)
            {
                readInfoRoad = new ReadInfoRoad(BandsAreas.Band[i].TimeStart, BandsAreas.Band[i].TimeStop);
                ReadAllGraffics = new ReadAllGraffics(BandsAreas.Band[i].TimeStart, BandsAreas.Band[i].TimeStop);
                infoCollectionGraphic.Add(readInfoRoad.GetMassurm());

                infoCollectionGraphic[i].GraphicsesCollection = ReadAllGraffics.GetMassurm();
                if (!infoCollectionGraphic[i].CoefficientTF)
                {
                    flag = true;
                }



            }
           
            if (flag)
            {
                Messenger.Default.Send(true, "CoefficientWindow");
                Messenger.Default.Send(infoCollectionGraphic, "CollectionBands");
                System.Windows.MessageBox.Show(@"Задайте коэффициенты для расчёта плотности", @"Расчёт плотности", MessageBoxButton.OK, MessageBoxImage.Asterisk);
               
            }
            else
            {
                CorrectBand(infoCollectionGraphic,1);
            }
           
        }
        /// <summary>
        /// Рисование графиков по выбранным полосам
        /// </summary>
        public async void CorrectBand(List<DataRoad> infoCollectionGraphic, object numBand)
        {
            
          
            //Коллекция всех полос
            var densityBand = await CicleDensity();
            var countBand = await CicleCoun();
            var generalBand = await CicleGen();
            var layer1Band = await CicleLay1();
            var layer2Band = await CicleLay2();
            var layer3Band = await CicleLay3();
            var allLayerBand = await CicleAll();
            var rutBand = await CicleRut();
            var iriBand = await CicleIRI();
            if (infoCollectionGraphic != null)
            {
                var dist = infoCollectionGraphic[0].GraphicsesCollection[0].Distance;
                for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i++)
                {
                    var per = infoCollectionGraphic[0].GraphicsesCollection[i].Distance;
                    if (per > dist)
                    {
                        dist = per;
                    }
                }
                //SendMessageInfoDistance(dist);
                Messenger.Default.Send(dist, "ShowInfoDistance");
                var ListData = new List<DataRoad>();
                foreach (var collect in infoCollectionGraphic)
                {

                    foreach (var t in OkDataRoad.BandCollection)
                    {
                        if (collect.Band == t.Band)
                        {

                            ListData.Add(t);
                        }
                    }
                }

                var cBand = ListData[0].Band;

                for (var i = 0; i < ListData.Count; i++)
                {
                    if (ListData[i].Band > cBand)
                    {
                        cBand = ListData[i].Band;
                    }

                }
                for (var i = 0; i < ListData.Count; i++)
                {
                    if (ListData[i].Band.ToString() == numBand.ToString())
                    {
                        InfoRoad = Convert.ToString(ListData[i].RoadName + "\r" + ListData[i].AllRoad + "\r"
                          +  @"№" +ListData[i].Band + "\r" + ListData[i].TimeStart + "\r" + dist + " м" + "\r"
                                                    + ListData[i].DesignCount + "\r" + ListData[i].DesignCount+ " см"
                                                    + "\r" +
                                                    ListData[i].Coating + "\r" + ListData[i].Customer + "\r" +
                                                    ListData[i].Builder
                                                    + "\r" + ListData[i].Direction + "\r" + cBand);
                    }
                }
                //SendDateEnd(ListData[ListData.Count - 1].TimeStop);
                Messenger.Default.Send(ListData[ListData.Count - 1].TimeStop, "ShowDateEnd");
                _standartsDensity = ListData[0].StandartsDensity;
                SendDensityStandart(_standartsDensity);
                //SendMessageInfo(ListData[0]);
                Messenger.Default.Send(ListData[0], "ShowInfo");
            }

            var report = new Report
            {
                CountBand = countBand,
                GeneralBand = generalBand,
                DensityBand = densityBand,
                Layer1Band = layer1Band,
                Layer2Band = layer2Band,
                Layer3Band = layer3Band,
                AllLayerBand = allLayerBand,
                RutBand = rutBand,
                IRIBand = iriBand
            };

            AllChart = new ChartData
            {
                AllLayerBand = allLayerBand,
                CountBand = countBand,
                DensityBand = densityBand,
                Layer1Band = layer1Band,
                Layer2Band = layer2Band,
                Layer3Band = layer3Band,
                GeneralBand = generalBand,
                RutBand = rutBand,
                IRIBand = iriBand
            };


            for (var i = 0; i < generalBand.Count; i++)
            {
                var band = generalBand[i];
                if (band.Band.ToString() == numBand.ToString())
                {
                    
                    Plotnost = densityBand[i];
                    Count = countBand[i];
                    Layer1 = layer1Band[i];
                    Layer2 = layer2Band[i];
                    Layer3 = layer3Band[i];
                    AllLayer = allLayerBand[i];
                    Rut = rutBand[i];
                    
                }
            }




            Size = scrollWidth - 20;
            SendWidth(Size);

                SendMessageGeneralPoint(new ObservableCollection<CollectionGraphs>(generalBand));
                SendMessageGeneralPoint(generalBand);

                Messenger.Default.Send(Size, "startWidth");

                
                //SendCollection(report);
                Messenger.Default.Send(report, "ShowCollection");
           
           
            //SendReport(ReadAllGraffics);
            Messenger.Default.Send(ReadAllGraffics, "ShowRep");
            //}
            //catch (Exception)
            //{
            //    InfoVision = false;
            //    ButtonVisibility = false;
            //    Border = false;
            //    RegulationProgressRingVisibility = false;
            //    RegulationRegLay1 = false;
            //    RegulationRegLay2 = false;
            //    RegulationRegLay3 = false;
            //    RegulationRegRut = false;
            //    RegulationRegCoun = false;
            //    RegulationRegAll = false;
            //    RegulationRegPlot = false;
            //    MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //}
            AddClean = 40;


            
            //SendBand(OkDataRoad.Band.Count);
            Messenger.Default.Send(OkDataRoad.Band.Count, "Band");

        }

        private ReadAllBand readAllBand;
        /// <summary>
        /// Кнопка выбора участка
        /// </summary>
        public void Select()
        {
            var listRoad = new Areas();
            foreach (var road in OkDataRoad.BandCollection)
            {
                readAllBand = new ReadAllBand(road.TimeStart, road.TimeStop);
                readAllBand.GetMassurm().TimeStart = road.TimeStart;
                readAllBand.GetMassurm().TimeStop = road.TimeStop;
                listRoad.BandCollection.Add(readAllBand.GetMassurm());
              
            }
            


            
            //Открыть новое окно
            IsEnableSelectButton = false;
            NewWindow(true);
            //Отправить коллекцию в окно
            //SendBand(OkDataRoad);
            Messenger.Default.Send(listRoad, "BandSelected");
        }
        /// <summary>
        /// Кнопка сброса на весь диапозон
        /// </summary>
        public void Reset()
        {
            BeginDate = DateTime.Parse(_DateStart);
            EndDate = DateTime.Now;
            Plotnost.GraphicsesCollection.Clear();
            General.GraphicsesCollection.Clear();
            AllLayer.GraphicsesCollection.Clear();
            General.GraphicsesCollection.Clear();
            Layer1.GraphicsesCollection.Clear();
            Layer2.GraphicsesCollection.Clear();
            Layer3.GraphicsesCollection.Clear();
            Rut.GraphicsesCollection.Clear();
            Parametrs = true;
            InfoVision = false;
            ButtonVisibility = false;
            RegulationAll = false;
            RegulationCoun = false;
            RegulationGen = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationRut = false;
            RegulationPlotn = false;
            Border = false;
            Report = false;
            var tempTime = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToShortDateString(), DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
            var startstop = new ReadStartStop(_DateStart, tempTime);
            _startStops = startstop.GetMassurm3();
            Get();
        }
        #endregion
        #region Cicle
        public async Task<ObservableCollection<CollectionGraphs>> CicleGen(int k = 40)
        {
            var generalTask = await Task.Run(() =>
            {
                lock (_locker)
                {
                    //ObservableCollection<Graphics> general = null;
                    var listGeneral = new ObservableCollection<CollectionGraphs>();
                    foreach (var gen in infoCollectionGraphic)
                    {
                        //general = new ObservableCollection<Graphics>();
                        var general = new CollectionGraphs();
                       
                        var schet = gen.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                            _cleanAdd = false;

                        }
                        else
                        {
                            _cleanAdd = true;
                        }

                        for (var i = 0; i <= gen.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(gen.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.IntensityN1 += ((gen.GraphicsesCollection[j].IntensityN1 +
                                                        gen.GraphicsesCollection[j].IntensityN2) / 2 -
                                                       _coefficientB) /
                                                      _coefficientK;
                            }
                            
                            var graphics = new Graphics
                            {
                                IntensityN1 = middle.IntensityN1 / schet,
                                Distance = gen.GraphicsesCollection[i].Distance,
                                GeneralState = Convert.ToBoolean(gen.GraphicsesCollection[i].GeneralState),
                                
                            };

                            general.Band = gen.GraphicsesCollection[i].Band;
                            general.GraphicsesCollection.Add(graphics);
                        }
                        listGeneral.Add(general);
                       
                    }
                    RegulationGen = true;
                    RegulationProgressRingVisibility = false;
                    return listGeneral; 
                }
               
            });
            return generalTask;
        }

        public async Task<ObservableCollection<CollectionGraphs>> CicleDensity(int k = 40)
        {
            
            var plotTask = await Task.Run(() =>
            {
                lock (_locker)

                {
                    var listDensity = new ObservableCollection<CollectionGraphs>();
                    //var plot = new ObservableCollection<Graphics>();
                  
                    foreach (var plot in infoCollectionGraphic)
                    {
                        var density = new CollectionGraphs();
                        var max = ((plot.GraphicsesCollection[0].IntensityN1 +
                                    plot.GraphicsesCollection[0].IntensityN2)/2 -
                                   plot.KoefficB) /
                                  plot.KoefficK;
                        var min = ((plot.GraphicsesCollection[0].IntensityN1 +
                                    plot.GraphicsesCollection[0].IntensityN2) / 2 -
                                   plot.KoefficB) /
                                  plot.KoefficK;
                        var schet = plot.GraphicsesCollection.Count/k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < plot.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(plot.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.IntensityN1 += (((plot.GraphicsesCollection[j].IntensityN1 +
                                                        plot.GraphicsesCollection[j].IntensityN2) / 2) -
                                                       plot.KoefficB) /
                                                      plot.KoefficK;
                            }
                            var graphics = new Graphics
                            {
                                IntensityN1 = middle.IntensityN1/schet,
                                Distance = plot.GraphicsesCollection[i].Distance,
                                Latitude = plot.GraphicsesCollection[i].Latitude,
                                Longitude = plot.GraphicsesCollection[i].Longitude

                            };
                            var per = ((plot.GraphicsesCollection[i].IntensityN1 +
                                        plot.GraphicsesCollection[i].IntensityN2) / 2 -
                                       plot.KoefficB) /
                                      plot.KoefficK;

                            if (per > max)
                            {
                                max = per;
                            }
                            if (per < min)
                            {
                                min = per;
                            }
                            //plot.Add(graphics);
                            density.Size = max + 2;
                           
                            density.GraphicsesCollection.Add(graphics);
                            if (min<0)
                            {
                                
                                //var startstop = new ReadStartStop("29.05.2017 11:37:00", "29.05.2017 11:47:00");
                                //_startStops = startstop.GetMassurm3();
                                //Get();
                                Environment.Exit(0);
                                //Application.Restart();
                            }
                            
                        }
                        //SizePlot = max + 2;
                        listDensity.Add(density);
                    }
                    
                    RegulationPlotn = true;
                    RegulationRegPlot = false;
                    return listDensity;
                }
               
            });
            return plotTask;
            
        }
       
        public async Task<ObservableCollection<CollectionGraphs>> CicleCoun(int k = 40)
        {
            
            var countTask = await Task.Run(() => 
            {

                lock (_locker)
                {
                    var listCount = new ObservableCollection<CollectionGraphs>();
                    listCount.Clear();
                    foreach (var count in infoCollectionGraphic)
                    {
                        var coun = new CollectionGraphs();
                        var schet = count.GraphicsesCollection.Count/k;
                        if (schet == 0)
                        {
                            schet = 1;

                        }

                        for (var i = 0; i < count.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(count.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.CountLayer += count.GraphicsesCollection[j].CountLayer;
                            }
                            var graphics = new Graphics
                            {
                                CountLayer = Convert.ToByte(middle.CountLayer/schet),
                                Distance = count.GraphicsesCollection[i].Distance
                            };
                            //coun.Add(graphics);
                            coun.GraphicsesCollection.Add(graphics);
                        }
                        coun.Band = count.GraphicsesCollection[0].Band;
                        listCount.Add(coun);
                    }
                    RegulationCoun = true;
                    RegulationRegCoun = false;
                    return listCount;
                }
                
            });
            return countTask;
            
        }


        public async Task<ObservableCollection<CollectionGraphs>> CicleLay1(int k = 40)
        {
            var lay1Task = await Task.Run(() =>
            {

                lock (_locker)
                {

                    var listLay1 = new ObservableCollection<CollectionGraphs>();
                    foreach (var _lay1 in infoCollectionGraphic)
                    {
                        var lay1 = new CollectionGraphs();
                        int max = Convert.ToInt16(_lay1.GraphicsesCollection[0].Layer1);
                        var schet = _lay1.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _lay1.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_lay1.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.Layer1 += _lay1.GraphicsesCollection[j].Layer1;
                            }
                            var graphics = new Graphics
                            {
                                Layer1 = middle.Layer1 / schet,
                                Distance = _lay1.GraphicsesCollection[i].Distance

                            };


                            if (_lay1.GraphicsesCollection[i].Layer1 > max)
                            {
                                max = Convert.ToInt16(_lay1.GraphicsesCollection[i].Layer1);
                            }
                            lay1.GraphicsesCollection.Add(graphics);  
                        }
                        lay1.Size = max + 2;
                        listLay1.Add(lay1);
                    }
                    
                    RegulationLayer1 = true;
                    RegulationRegLay1 = false;
                    return listLay1;
                }
            });
            return lay1Task;
        }

        public async Task<ObservableCollection<CollectionGraphs>> CicleIRI(int k = 40)
        {
            var iriTask = await Task.Run(() =>
            {

                lock (_locker)
                {

                    var listIRI = new ObservableCollection<CollectionGraphs>();
                    foreach (var _iri in infoCollectionGraphic)
                    {
                        var iri = new CollectionGraphs();

                        var schet = _iri.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _iri.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_iri.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.IRI += Convert.ToSingle(_iri.GraphicsesCollection[j].IRI);
                            }
                            var graphics = new Graphics
                            {
                                IRI = middle.IRI / schet,
                                //Distance = _iri[i].Distance

                            };


                            //if (_lay1[i].Layer1 > max)
                            //{
                            //    max = Convert.ToInt16(_lay1[i].Layer1);
                            //}
                            iri.GraphicsesCollection.Add(graphics);
                        }
                        //lay1.Size = max + 2;
                        listIRI.Add(iri);
                    }
                    return listIRI;
                }
            });
            return iriTask;
        }


        public async Task<ObservableCollection<CollectionGraphs>> CicleLay2(int k = 40)
        {
            var lay2Task =  await Task.Run(() =>
            {

                var listLay2 = new ObservableCollection<CollectionGraphs>();
                lock (_locker)
                {
                    foreach (var _lay2 in infoCollectionGraphic)
                    {
                        var lay2 = new CollectionGraphs();
                        int max = Convert.ToInt16(_lay2.GraphicsesCollection[0].Layer2);
                        var schet = _lay2.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _lay2.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_lay2.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.Layer2 += _lay2.GraphicsesCollection[j].Layer2;
                            }
                            var graphics = new Graphics
                            {
                                Layer2 = middle.Layer2/schet,
                                Distance = _lay2.GraphicsesCollection[i].Distance
                            };
                            if (_lay2.GraphicsesCollection[i].Layer2 > max)
                            {
                                max = Convert.ToInt16(_lay2.GraphicsesCollection[i].Layer2);
                            }
                            lay2.GraphicsesCollection.Add(graphics);
                        }
                        lay2.Size = max + 2;
                        listLay2.Add(lay2);
                    }
                    RegulationLayer2 = true;
                    RegulationRegLay2 = false;
                    return listLay2;
                }
            });
            return lay2Task;
        }

        public async Task<ObservableCollection<CollectionGraphs>> CicleLay3(int k = 40)
        {
            var lay3Task = await Task.Run(() =>
            {

                var listLay3 = new ObservableCollection<CollectionGraphs>();
                //var lay3 = new ObservableCollection<Graphics>();
                lock (_locker)

                {
                    foreach (var _lay3 in infoCollectionGraphic)
                    {
                        var lay3 = new CollectionGraphs();
                        int max = Convert.ToInt16(_lay3.GraphicsesCollection[0].Layer3);
                        var schet = ReadAllGraffics.GetMassurm().Count/k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _lay3.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_lay3.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.Layer3 += _lay3.GraphicsesCollection[j].Layer3;
                            }
                            var graphics = new Graphics
                            {
                                Layer3 = middle.Layer3/schet,
                                Distance = _lay3.GraphicsesCollection[i].Distance
                            };


                            if (_lay3.GraphicsesCollection[i].Layer3 > max)
                            {
                                max = Convert.ToInt16(_lay3.GraphicsesCollection[i].Layer3);
                            }

                            lay3.GraphicsesCollection.Add(graphics);
                        }
                        lay3.Size = max + 2;
                        listLay3.Add(lay3);
                    }
                    
                    RegulationLayer3 = true;
                    RegulationRegLay3 = false;
                    return listLay3;
                }
            }); 
            return lay3Task;
        }

        public async Task<ObservableCollection<CollectionGraphs>> CicleRut(int k = 40)
        {
            var rutTask = await Task.Run(() =>
            {
                var listRut = new ObservableCollection<CollectionGraphs>();
                
                lock (_locker)
                {
                    foreach (var _rut in infoCollectionGraphic)
                    {
                        var rut = new CollectionGraphs();
                        int max = Convert.ToInt16(_rut.GraphicsesCollection[0].Rut);
                        var schet = _rut.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _rut.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_rut.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.Rut += _rut.GraphicsesCollection[j].Rut;
                            }
                            var graphics = new Graphics
                            {
                                Rut = middle.Rut / schet,
                                Distance = _rut.GraphicsesCollection[i].Distance
                            };
                            if (_rut.GraphicsesCollection[i].Rut > max)
                            {
                                max = Convert.ToInt16(_rut.GraphicsesCollection[i].Rut);
                            }

                            rut.GraphicsesCollection.Add(graphics);
                        }
                        rut.Size = max + 2;
                        listRut.Add(rut);
                    }
                   
                   
                    RegulationRut = true;
                    RegulationRegRut = false;
                    return listRut;
                }
            });
            return rutTask;
        }

        public async Task<ObservableCollection<CollectionGraphs>> CicleAll(int k = 40)
        {
            var allTask = await Task.Run(() =>
            {


                var listAll = new ObservableCollection<CollectionGraphs>();
                lock (_locker)
                {
                    foreach (var _all in infoCollectionGraphic)
                    {
                        var all = new CollectionGraphs();
                        int max = Convert.ToInt16(_all.GraphicsesCollection[0].Layer3
                            + _all.GraphicsesCollection[0].Layer2 +
                                 _all.GraphicsesCollection[0].Layer1);
                        var schet = _all.GraphicsesCollection.Count / k;
                        if (schet == 0)
                        {
                            schet = 1;
                        }
                        for (var i = 0; i < _all.GraphicsesCollection.Count - 1; i += schet)
                        {
                            var middle = new Graphics();
                            for (var j = i; j <= i + schet - 1; j++)
                            {
                                if (!(_all.GraphicsesCollection.Count - 1 > j)) continue;

                                middle.Layer3 += _all.GraphicsesCollection[j].Layer3 +
                                    _all.GraphicsesCollection[j].Layer2 +
                                    _all.GraphicsesCollection[j].Layer1;
                                middle.Layer2 += _all.GraphicsesCollection[j].Layer2 +
                                                _all.GraphicsesCollection[j].Layer1;
                                middle.Layer1 += _all.GraphicsesCollection[j].Layer1;
                            }
                            var graphics = new Graphics
                            {
                                Layer3 = middle.Layer3 / schet,
                                Layer2 = middle.Layer2 / schet,
                                Layer1 = middle.Layer1 / schet,
                                Distance = _all.GraphicsesCollection[i].Distance
                            };

                            var per = _all.GraphicsesCollection[i].Layer3
                                + _all.GraphicsesCollection[i].Layer2 +
                                    _all.GraphicsesCollection[i].Layer1;

                            if (per > max)
                            {
                                max = Convert.ToInt16(per);
                            }
                            all.GraphicsesCollection.Add(graphics);
                        }
                        all.Size = max + 2;
                        listAll.Add(all);
                    }
                    RegulationAll = true;
                    RegulationRegAll = false;
                    
                    return listAll;
                }
            });
            return allTask;
        }
        #endregion
        /// <summary>
        /// Метод получения всех данных
        /// </summary>
        public void Get()
        {
            ListTime = new List<List<TimeStartStop>>();
            var i = 1;
            while (_startStops.Count != i)
            {

                if (_startStops[i - 1].Value == _startStops[i].Value && _startStops.Count != (i + 1))
                {
                    _newmass.Add(_startStops[i - 1]);
                }
                else
                {
                    _newmass.Add(_startStops[i - 1]);
                    ListTime.Add(_newmass);
                    _newmass = new List<TimeStartStop>();
                }
                i++;
            }
            if (_startStops.Count == i)
            {
                if (_startStops[i - 2].Value == _startStops[i - 1].Value)
                {
                    ListTime[ListTime.Count - 1].Add(_startStops[i - 1]);
                }
                else
                {
                    _newmass.Add(_startStops[i - 1]);
                    _listTime.Add(_newmass);
                }
            }
            ListListovnorm = new List<List<TimeStartStop>>();
            foreach (var da in _listTime)
            {
                if (da[0].Value)
                {
                    _listListovnorm.Add(da);
                }
            }
            DataRoads = new List<RoadName>();
            
            var dataRoad = new RoadName();
            foreach (var datenorm in _listListovnorm)
            {
                if (datenorm.Count ==1)
                {
                    datenorm.Add(datenorm[0]);
                }
            }
            foreach (var datenorm in _listListovnorm)
            {
                
               
                  var  date = new ReadAllSelect(datenorm[0].Time, datenorm[1].Time);
             
                
                _startList.Add(datenorm[0].Time);
                _endList.Add(datenorm[datenorm.Count - 1].Time);
               
                    dataRoad = new RoadName()
                    {

                        Name = date.GetMassurm()[0].Name,
                        TimeStart = date.GetMassurm()[0].Time,
                        TimeStop = datenorm[datenorm.Count - 1].Time


                    };
              
             
               

                DataRoads.Add(dataRoad);

            }

            NormList = new ObservableCollection<ObservableCollection<RoadName>>();
            var massname = new ObservableCollection<RoadName>();
            for (var j = 0; j < DataRoads.Count; j++)
            {
                var flag = true;
                if (NormList.Count != 0)
                {
                    for (var l = 0; l < NormList.Count; l++)
                    {
                        if (_normList[l][0].Name != DataRoads[j].Name) continue;
                        NormList[l].Add(DataRoads[j]);
                        flag = false;
                    }
                    if (!flag) continue;
                    massname = new ObservableCollection<RoadName> { DataRoads[j] };
                    NormList.Add(massname);
                }
                else
                {
                    massname.Add(DataRoads[j]);
                    NormList.Add(massname);
                }
            }



            RegulationDrop = true;
        }
        public class RoadName
        {
            public string Name { get; set; }
            public string TimeStart { get; set; }
            public string TimeStop { get; set; }
        }

    
        #region Message



        //public void SendBand(Areas areas)
        //{
        //    Messenger.Default.Send(areas, "BandSelected");
        //}
        public void SendWidth(double width)
        {
            Messenger.Default.Send(width, "SendWidth");
        }

        public void SendParametr(int parametr)
        {
            Messenger.Default.Send(parametr, "SendParametr");
        }

        //public void SendMessageMeasurment(Areas infoRoad)
        //{
        //    Messenger.Default.Send(infoRoad, "ShowInfo");
        //}

        //public void SendAllPlot(ChartData argumentAreas)
        //{
        //    Messenger.Default.Send(argumentAreas, "ShowAllPlot");
        //}
        //public void SendImage(ChartData argument)
        //{
        //    Messenger.Default.Send(argument, "Test");
        //}

        ///<summary>
        ///Сообщение об открытии нового окна с измерениями 
        /// </summary>
        /// <param name="newWindow"></param>
        public void NewWindow(bool newWindow)
        {
            Messenger.Default.Send(newWindow, "ShowNewWindow");
        }


        /// <summary>
        /// Передача коллекции в график общего качества 
        /// </summary>
        /// <param name="listGraff"></param>
        public void SendMessageGeneralPoint(ObservableCollection<CollectionGraphs> listGraff)
        {
            Messenger.Default.Send(listGraff, "ShowGeneralGraff");
        }
        //public void SendMessageInfo(DataRoad infoRoad)
        //{
        //    Messenger.Default.Send(infoRoad, "ShowInfo");
        //}
        //public void SendDateEnd(string infoRoad)
        //{
        //    Messenger.Default.Send(infoRoad, "ShowDateEnd");
        //}
        //public void SendMessageInfoDistance(float infoDistance)
        //{
        //    Messenger.Default.Send(infoDistance, "ShowInfoDistance");
        //}

        //public void SendReport(ReadAllGraffics listRep)
        //{
        //    Messenger.Default.Send(listRep, "ShowRep");
        //}

        //public void SendCollection(Report listRep)
        //{
        //    Messenger.Default.Send(listRep, "ShowCollection");
        //}
        //public void SendBand(int band)
        //{
        //    Messenger.Default.Send(band, "Band");
        //}

        public void SendDensityStandart(double argument)
        {
            Messenger.Default.Send(argument, "DensityStandearts");
        }
        //public void SendDensity(ObservableCollection<CollectionGraphs> argument)
        //{
        //    Messenger.Default.Send(argument, "Density");
        //}
    }
#endregion
}