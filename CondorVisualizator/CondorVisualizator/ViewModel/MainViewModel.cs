using System;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using CondorVisualizator.Model;
using GalaSoft.MvvmLight.Command;

using GalaSoft.MvvmLight.Messaging;
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
        private readonly object _locker = new object();

        private ObservableCollection<DataRoad> _selectedItem;
        public ObservableCollection<DataRoad> SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); }
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
        private List<DataRoad> _dataRoads = new List<DataRoad>();
        public List<DataRoad> DataRoads
        {
            get { return _dataRoads; }
            set { Set(ref _dataRoads, value); }
        }
        private DataRoad _okData;
        public DataRoad OkDataRoad
        {
            get { return _okData; }
            set { Set(ref _okData, value); }
        }
        private ObservableCollection<ObservableCollection<DataRoad>> _normList =
            new ObservableCollection<ObservableCollection<DataRoad>>();
        public ObservableCollection<ObservableCollection<DataRoad>> NormList
        {
            get { return _normList; }
            set { Set(ref _normList, value); }
        }
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
        private ObservableCollection<Graphics> _count = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> Count
        {
            get { return _count; }
            set { Set(ref _count, value); }
        }
        private ObservableCollection<Graphics> _general = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> General
        {
            get { return _general; }
            set { Set(ref _general, value); }
        }
        private ObservableCollection<Graphics> _layer1 = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> Layer1
        {
            get { return _layer1; }
            set { Set(ref _layer1, value); }
        }
        private ObservableCollection<Graphics> _layer2 = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> Layer2
        {
            get { return _layer2; }
            set { Set(ref _layer2, value); }
        }
        private ObservableCollection<Graphics> _layer3 = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> Layer3
        {
            get { return _layer3; }
            set { Set(ref _layer3, value); }
        }
        private ObservableCollection<Graphics> _plotnost = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> Plotnost
        {
            get { return _plotnost; }
            set { Set(ref _plotnost, value); }
        }
        private ObservableCollection<Graphics> _allLayer = new ObservableCollection<Graphics>();
        public ObservableCollection<Graphics> AllLayer
        {
            get { return _allLayer; }
            set { Set(ref _allLayer, value); }
        }
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
        private bool _regulationLayer3;
        public bool RegulationLayer3
        {
            get { return _regulationLayer3; }
            set { Set(ref _regulationLayer3, value); }
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

        private int _size;
       public int AddClean { get; set; }
        public int Size 
         {
             get { return _size; }
             set { Set(ref _size, value); }
        }
        private int _sizeL1;
        public int SizeL1
        {
            get { return _sizeL1; }
            set { Set(ref _sizeL1, value); }
        }
        private int _sizeL2;
        public int SizeL2
        {
            get { return _sizeL2; }
            set { Set(ref _sizeL2, value); }
        }
        private int _sizeL3;
        public int SizeL3
        {
            get { return _sizeL3; }
            set { Set(ref _sizeL3, value); }
        }
        private int _sizeL123;
        public int SizeL123
        {
            get { return _sizeL123; }
            set { Set(ref _sizeL123, value); }
        }
        private double _sizePlot;
        public double SizePlot
        {
            get { return _sizePlot; }
            set { Set(ref _sizePlot, value); }
        }
           public RelayCommand AddRelayCommand { get; set; }
        public RelayCommand VisionRelayCommand { get; set; }
        public RelayCommand CleanRelayCommand { get; set; }
        public RelayCommand OkRelayCommand { get; set; }
        public RelayCommand FilterRelayCommand { get; set; }
        public RelayCommand ResetRelayCommand { get; set; }
 
        public RelayCommand<string> GraffCountCommand { get; set; }
    
        public RelayCommand PlusButtonCommand { get; set; }

        public MainViewModel(IDataService dataService)
        {
           
            InfoVision = false;
            ButtonVisibility = false;
            RegulationDrop = false;
            RegulationGen = false;
            RegulationPlotn = false;
            RegulationCoun = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationAll = false;
            Report = false;
            dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                       
                    }

                });

           BeginDate = DateTime.Parse("20.12.2016");
            var tempTime = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToShortDateString(), DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
            var startstop = new ReadStartStop("21.12.2016 17:00:00", "21.12.2016 18:00:00");
            _startStops = startstop.GetMassurm3();
            Get();
            CleanRelayCommand =new RelayCommand(Clean);
            AddRelayCommand = new RelayCommand(Add);
            OkRelayCommand = new RelayCommand(Select);
            FilterRelayCommand = new RelayCommand(Filter);
            ResetRelayCommand = new RelayCommand(Reset);
            VisionRelayCommand = new RelayCommand(Vision);
            GraffCountCommand = new RelayCommand<string>(Graff);
            
           
        }




        public Graphics Graphics;
        public ReadAllGraffics ReadAllGraffics;
       
        private DataRoad _selectRoad;


        public void Graff(string parametr)
        {

            var parametrs = new List<List<object>>();

            switch (parametr)
            {
                case  "CountChart":
            parametrs = new List<List<object>>();
            var parametrs2 = new List<object>();
            var parametrs3 = new List<object>();
                    foreach (var graphicse in Count)
                    {
                        parametrs2.Add(graphicse.CountLayer);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
                case "ChartLayer3":
                          parametrs = new List<List<object>>();
            parametrs2 = new List<object>();
            parametrs3 = new List<object>();
                    foreach (var graphicse in Layer3)
                    {
                        parametrs2.Add(graphicse.Layer3);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
                case "ChartLayer2":
                          parametrs = new List<List<object>>();
            parametrs2 = new List<object>();
            parametrs3 = new List<object>();
                    foreach (var graphicse in Layer2)
                    {
                        parametrs2.Add(graphicse.Layer2);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
                case "ChartLayer1":
                          parametrs = new List<List<object>>();
            parametrs2 = new List<object>();
            parametrs3 = new List<object>();
                    foreach (var graphicse in Layer1)
                    {
                        parametrs2.Add(graphicse.Layer1);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
                case "ChartAll":
                          parametrs = new List<List<object>>();
                          parametrs2 = new List<object>();
                         parametrs3 = new List<object>();
                     var parametrs4 = new List<object>();
             var parametrs5 = new List<object>();
                    foreach (var graphicse in AllLayer)
                    {
                        parametrs2.Add(graphicse.Layer1);
                        parametrs4.Add(graphicse.Layer2);
                        parametrs5.Add(graphicse.Layer3);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    parametrs.Add(parametrs4);
                    parametrs.Add(parametrs5);
                    break;
                case "Plotnost":
                          parametrs = new List<List<object>>();
            parametrs2 = new List<object>();
            parametrs3 = new List<object>();
                    foreach (var graphicse in Plotnost)
                    {
                        parametrs2.Add(graphicse.IntensityN1);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
                case "GeneralGraff":
                    parametrs = new List<List<object>>();
                    parametrs2 = new List<object>();
                    parametrs3 = new List<object>();
                    foreach (var graphicse in Plotnost)
                    {
                        parametrs2.Add(graphicse.GeneralState);
                        parametrs3.Add(graphicse.Distance);
                    }
                    parametrs.Add(parametrs2);
                    parametrs.Add(parametrs3);
                    break;
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
        public void Vision()
        {
            Plotnost.Clear();
            General.Clear();
            AllLayer.Clear();
            General.Clear();
            Layer1.Clear();
            Layer2.Clear();
            Layer3.Clear();

            InfoVision = false;
            ButtonVisibility = false;
            RegulationAll = false;
            RegulationCoun = false;
            RegulationGen = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationPlotn = false;  
        }

        
        public async void Add()
        {
            try
            {
                if (_okData.TimeStart == null || _okData.TimeStop == null) return;
                AddClean = AddClean + 5;
                Size = Size + 100;
                if (AddClean == 40)
                {
                    Report = true;
                }
                else
                {
                    Report = false;
                }
                RegulationProgressRingVisibility = true;
                RegulationRegLay1 = true;
                RegulationRegLay2 = true;
                RegulationRegLay3 = true;
                RegulationRegCoun = true;
                RegulationRegAll = true;
                RegulationRegPlot = true;
                //ReadAllGraffics = new ReadAllGraffics(_okData.TimeStart, _okData.TimeStop);
                General = await CicleGen(AddClean);
                Plotnost = await CiclePlot(AddClean);
                Count = await CicleCoun(AddClean);
                Layer1 = await CicleLay1(AddClean);
                Layer2 = await CicleLay2(AddClean);
                Layer3 = await CicleLay3(AddClean);
                AllLayer = await CicleAll(AddClean);
                SendMessageGeneralPoint(new ObservableCollection<Graphics>(General));
            }
            catch (Exception)
            {

                MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
         
        }

        public async void Clean()
        {
           try
            {
                if (_okData.TimeStart == null || _okData.TimeStop == null) return;
                if (AddClean <= 40) return;
                AddClean = AddClean - 5;
                Size = Size - 100;
                if (AddClean == 40)
                {
                    Report = true;
                }
                else
                {
                    Report = false;
                }
                RegulationProgressRingVisibility = true;
                RegulationRegLay1 = true;
                RegulationRegLay2 = true;
                RegulationRegLay3 = true;
                RegulationRegCoun = true;
                RegulationRegAll = true;
                RegulationRegPlot = true;
                //ReadAllGraffics = new ReadAllGraffics(_okData.TimeStart, _okData.TimeStop);

                General = await CicleGen(AddClean);
                Plotnost = await CiclePlot(AddClean);
                Count = await CicleCoun(AddClean);
                Layer1 = await CicleLay1(AddClean);
                Layer2 = await CicleLay2(AddClean);
                Layer3 = await CicleLay3(AddClean);
                AllLayer = await CicleAll(AddClean);

                SendMessageGeneralPoint(new ObservableCollection<Graphics>(General));
            }
           catch (Exception)
           {

               MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
           }
        
        }

        public void Filter()
        {
            
            try
            {


                _selectRoad = new DataRoad { SelectbeginDate = BeginDate.ToString(), SelectendDate = EndDate.ToString() };
                var startstop = new ReadStartStop(_selectRoad.SelectbeginDate, _selectRoad.SelectendDate);
                _startStops = startstop.GetMassurm3();
                Get();
            }
            catch (Exception)
            {

                MessageBox.Show(@"Выберите дату", @"Ошибка выбора даты", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
          

        }

        public async void Select()
        {
            try
            {
                InfoVision = true;
                ButtonVisibility = true;
                RegulationProgressRingVisibility = true;
                RegulationRegLay1 = true;
                RegulationRegLay2 = true;
                RegulationRegLay3 = true;
                RegulationRegCoun = true;
                RegulationRegAll = true;
                RegulationRegPlot = true;
                Report = true;
                ReadAllGraffics = new ReadAllGraffics(_okData.TimeStart, _okData.TimeStop);
                General = await CicleGen();
                Plotnost = await CiclePlot();
                Count = await CicleCoun();
                Layer1 = await CicleLay1();
                Layer2 = await CicleLay2();
                Layer3 = await CicleLay3();
                AllLayer = await CicleAll();
                SendMessageGeneralPoint(new ObservableCollection<Graphics>(General));
                InfoRoad = Convert.ToString(_okData.RoadName + "\r" + _okData.NumMess  + "\r"
                    + _okData.TimeStart  + "\r" + _okData.TimeStop);
             

            }
            catch (Exception)
            {
                InfoVision = false;
                ButtonVisibility = false;
                RegulationProgressRingVisibility = false;
                RegulationRegLay1 = false;
                RegulationRegLay2 = false;
                RegulationRegLay3 = false;
                RegulationRegCoun = false;
                RegulationRegAll = false;
                RegulationRegPlot = false;
                MessageBox.Show(@"Выберите участок дороги", @"Ошибка выбора участка дороги", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            AddClean = 40;
            Size = 350;

            //var dist = new Graphics { Distance = ReadAllGraffics.GetMassurm()[0].Distance };
            var dist = ReadAllGraffics.GetMassurm()[0].Distance;
            for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i++)
            {
                var per = ReadAllGraffics.GetMassurm()[i].Distance;
                if (per > dist)
                {
                    dist = per;
                }
            }

            SendMessageInfoDistance(dist);
            SendMessageInfo(OkDataRoad);
        }

        public void Reset()
        {
            BeginDate = DateTime.Parse("20.12.2016");
            EndDate = DateTime.Now;
            Plotnost.Clear();
            General.Clear();
            AllLayer.Clear();
            General.Clear();
            Layer1.Clear();
            Layer2.Clear();
            Layer3.Clear();

            InfoVision = false;
            ButtonVisibility = false;
            RegulationAll = false;
            RegulationCoun = false;
            RegulationGen = false;
            RegulationLayer1 = false;
            RegulationLayer2 = false;
            RegulationLayer3 = false;
            RegulationPlotn = false;
            var tempTime = string.Format("{0} {1}:{2}:{3}", DateTime.Now.ToShortDateString(), DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
            var startstop = new ReadStartStop("20.12.2016 18:00:00", tempTime);
            _startStops = startstop.GetMassurm3();
            Get();
        }
        
        public async Task<ObservableCollection<Graphics>> CicleGen(int k = 40)
        {
            var generalTask = await Task.Run(() =>
            {
                lock (_locker)
                {
                    var general = new ObservableCollection<Graphics>();
                    
                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i <= ReadAllGraffics.GetMassurm().Count - 1; i += schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i+schet -1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;
                            middle.GeneralState += ReadAllGraffics.GetMassurm()[j].GeneralState;
                        }
                       
                        
                        var graphics = new Graphics
                        {
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance,
                            GeneralState = Convert.ToByte(middle.GeneralState / schet)
                        };
                        general.Add(graphics);
                    }
                    RegulationGen = true;
                    RegulationProgressRingVisibility = false;
                    return general;
                }
            });
            return generalTask;
        }

        public async Task<ObservableCollection<Graphics>> CiclePlot(int k = 40)
        {
            var plotTask = await Task.Run(() =>
            {
                lock (_locker)

                {
                    var plot = new ObservableCollection<Graphics>();
                    var max = ((ReadAllGraffics.GetMassurm()[0].IntensityN1 +
                                          ReadAllGraffics.GetMassurm()[0].IntensityN2) / 2 -
                                         Convert.ToDouble(ReadAllGraffics.GetMassurm()[0].KoefficB)) /
                                        Convert.ToDouble(ReadAllGraffics.GetMassurm()[0].KoefficK);
                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i += schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.IntensityN1 += ((ReadAllGraffics.GetMassurm()[j].IntensityN1 +
                                            ReadAllGraffics.GetMassurm()[j].IntensityN2) / 2 -
                                           Convert.ToDouble(ReadAllGraffics.GetMassurm()[j].KoefficB)) /
                                          Convert.ToDouble(ReadAllGraffics.GetMassurm()[j].KoefficK);
                        }
                       
                       
                        var graphics = new Graphics
                        {
                            IntensityN1 =middle.IntensityN1/schet,
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance,
                           
                        };
                       
                        var per = ((ReadAllGraffics.GetMassurm()[i].IntensityN1 +
                                    ReadAllGraffics.GetMassurm()[i].IntensityN2)/2 -
                                   Convert.ToDouble(ReadAllGraffics.GetMassurm()[i].KoefficB))/
                                  Convert.ToDouble(ReadAllGraffics.GetMassurm()[i].KoefficK);

                        if (per > max)
                        {
                            max = per;
                        }
                        
                        plot.Add(graphics);
                    }
                    SizePlot = max + 2;
                    RegulationPlotn = true;
                    RegulationRegPlot = false;
                    return plot;
                }
            });
            return plotTask;
        }

        public async Task<ObservableCollection<Graphics>> CicleCoun(int k = 40)
        {
            var countTask = await Task.Run(() => 
            {
                lock (_locker)
                {
                    var schet = ReadAllGraffics.GetMassurm().Count/k;
                    var coun = new ObservableCollection<Graphics>();
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i +=schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.CountLayer += ReadAllGraffics.GetMassurm()[j].CountLayer;
                        }
                        var graphics = new Graphics
                        {
                            CountLayer = Convert.ToByte(middle.CountLayer/schet),
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance
                        };
                        coun.Add(graphics);

                    }
                
                    RegulationCoun = true;
                    RegulationRegCoun = false;
                    return coun;
                }
            });
            return countTask;
        }


        public async Task<ObservableCollection<Graphics>> CicleLay1(int k = 40)
        {
            var lay1Task = await Task.Run(() =>
            {

                lock (_locker)
                {
                    
                    var lay1 = new ObservableCollection<Graphics>();
                    int max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[0].Layer1);
                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i+=schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.Layer1 += ReadAllGraffics.GetMassurm()[j].Layer1;
                        }
                        var graphics = new Graphics
                        {
                            Layer1 = middle.Layer1/schet,
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance
    
                        };
                        
                       
                        if (ReadAllGraffics.GetMassurm()[i].Layer1 > max)
                        {
                            max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[i].Layer1);
                        }
                       
                        


                        lay1.Add(graphics);
                        

                    }
                    SizeL1 = max + 2;
                    RegulationLayer1 = true;
                    RegulationRegLay1 = false;
                    return lay1;
                }
            });
            return lay1Task;
        }

        public async Task<ObservableCollection<Graphics>> CicleLay2(int k = 40)
        {
            var lay2Task =  await Task.Run(() =>
            {
                int max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[0].Layer2);
                var lay2 = new ObservableCollection<Graphics>();
                lock (_locker)
                {

                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i += schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.Layer2 += ReadAllGraffics.GetMassurm()[j].Layer2;
                        }
                        var graphics = new Graphics
                        {
                            Layer2 = middle.Layer2 / schet,
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance
                        };
                       

                        if (ReadAllGraffics.GetMassurm()[i].Layer2 > max)
                        {
                            max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[i].Layer2);
                        }
                        
                      
                        lay2.Add(graphics);
                     
                    }
                    SizeL2 = max + 2;
                    RegulationLayer2 = true;
                    RegulationRegLay2 = false;
                    return lay2;
                }
            });
            return lay2Task;
        }

        public async Task<ObservableCollection<Graphics>> CicleLay3(int k = 40)
        {
            var lay3Task = await Task.Run(() =>
            {
                int max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[0].Layer3);
                var lay3 = new ObservableCollection<Graphics>();
                lock (_locker)
                {
                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i += schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.Layer3 += ReadAllGraffics.GetMassurm()[j].Layer3;
                        }
                        var graphics = new Graphics
                        {
                            Layer3 = middle.Layer3/schet,
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance
                        };
                       

                        if (ReadAllGraffics.GetMassurm()[i].Layer3 > max)
                        {
                            max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[i].Layer3);
                        }
                        
                        lay3.Add(graphics);
                    }
                    SizeL3 = max + 2;
                    RegulationLayer3 = true;
                    RegulationRegLay3 = false;
                    return lay3;
                }
            }); 
            return lay3Task;
        }

        public async Task<ObservableCollection<Graphics>> CicleAll(int k = 40)
        {
            var allTask = await Task.Run(() =>
            {
                int max = Convert.ToInt16(ReadAllGraffics.GetMassurm()[0].Layer3 + ReadAllGraffics.GetMassurm()[0].Layer2 +
                                  ReadAllGraffics.GetMassurm()[0].Layer1);
                var all = new ObservableCollection<Graphics>();
                lock (_locker)
                {
                    var schet = ReadAllGraffics.GetMassurm().Count / k;
                    for (var i = 0; i < ReadAllGraffics.GetMassurm().Count - 1; i += schet)
                    {
                        var middle = new Graphics();
                        for (var j = i; j <= i + schet - 1; j++)
                        {
                            if (!(ReadAllGraffics.GetMassurm().Count - 1 > j)) continue;

                            middle.Layer3 += ReadAllGraffics.GetMassurm()[j].Layer3 + 
                                ReadAllGraffics.GetMassurm()[j].Layer2 +
                                ReadAllGraffics.GetMassurm()[j].Layer1;
                            middle.Layer2 += ReadAllGraffics.GetMassurm()[j].Layer2 +
                                            ReadAllGraffics.GetMassurm()[j].Layer1;
                            middle.Layer1 += ReadAllGraffics.GetMassurm()[j].Layer1;
                        }
                        var graphics = new Graphics
                        {
                            Layer3 = middle.Layer3 / schet,
                            Layer2 = middle.Layer2 / schet,
                            Layer1 = middle.Layer1 / schet,
                            Distance = ReadAllGraffics.GetMassurm()[i].Distance
                        };
                       
                        var per =  ReadAllGraffics.GetMassurm()[i].Layer3 + ReadAllGraffics.GetMassurm()[i].Layer2 +
                                ReadAllGraffics.GetMassurm()[i].Layer1;

                        if (per > max)
                        {
                            max = Convert.ToInt16(per);
                        }
                        
                        
                        all.Add(graphics);
                    }
                    SizeL123 = max + 2;
                    RegulationAll = true;
                    RegulationRegAll = false;
                    
                    return all;
                }
            });
            return allTask;
        }

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
            _dataRoads = new List<DataRoad>();
            var dataRoad = new DataRoad();
            foreach (var datenorm in _listListovnorm)
            {
                var date = new ReadAllSelect(datenorm[0].Time, datenorm[1].Time);
                _startList.Add(datenorm[0].Time);
                _endList.Add(datenorm[datenorm.Count - 1].Time);
                dataRoad = new DataRoad
                {
                    NumMess = date.GetMassurm()[0].NumMess,
                    RoadName = date.GetMassurm()[0].RoadName,
                    TimeStart = datenorm[0].Time,
                    TimeStop = datenorm[datenorm.Count - 1].Time
                };
                _dataRoads.Add(dataRoad);
            }
            NormList = new ObservableCollection<ObservableCollection<DataRoad>>();
            var massname = new ObservableCollection<DataRoad>();
            for (var j = 0; j < _dataRoads.Count; j++)
            {
                var flag = true;
                if (_normList.Count != 0)
                {
                    for (var l = 0; l < _normList.Count; l++)
                    {
                        if (_normList[l][0].RoadName != _dataRoads[j].RoadName) continue;
                        _normList[l].Add(_dataRoads[j]);
                        flag = false;
                    }
                    if (!flag) continue;
                    massname = new ObservableCollection<DataRoad> {_dataRoads[j]};
                    _normList.Add(massname);
                }
                else
                {
                    massname.Add(_dataRoads[j]);
                    _normList.Add(massname);
                }
            }
            RegulationDrop = true;

        }

        public void SendMessageGeneralPoint(ObservableCollection<Graphics> listGraff)
        {
            Messenger.Default.Send(listGraff, "ShowGeneralGraff");
        }
        public void SendMessageInfo(DataRoad infoRoad)
        {
            Messenger.Default.Send(infoRoad, "ShowInfo");
        }
        public void SendMessageInfoDistance(float infoDistance)
        {
            Messenger.Default.Send(infoDistance, "ShowInfoDistance");
        }
    }
}