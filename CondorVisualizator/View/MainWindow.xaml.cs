using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CondorVisualizator.Model;
using CondorVisualizator.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Border = System.Windows.Controls.Border;
using Brushes = System.Windows.Media.Brushes;
using Chart = System.Windows.Controls.DataVisualization.Charting.Chart;
using Color = System.Windows.Media.Color;
using Control = System.Windows.Controls.Control;
using Convert = System.Convert;
using Graphics = CondorVisualizator.Model.Graphics;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MessageBox = System.Windows.MessageBox;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Style = System.Windows.Style;
using VerticalAlignment = System.Windows.VerticalAlignment;

namespace CondorVisualizator.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private double _tempsize;
        private double _scrollSize;
        private Chart _chart;
        private Graff _gridd;
        private UserControl _userControl;
        private Grid _gridnorm;
        private List<float> Plt; 
        private bool _argument = true;

        /// <summary>
        /// Имя большого графика
        /// </summary>
        private string _greatGraphicName;
        public double ScrollSize
        {
            get { return _scrollSize; }
            set
            {
                _scrollSize = value;
                if (_scrollSize == _tempsize) return;
                _tempsize = _scrollSize;
                HeightScrollViewerAll.ScrollToHorizontalOffset(_scrollSize);
                ScrollViewerCount.ScrollToHorizontalOffset(_scrollSize);
                ScrollViewerGeneral.ScrollToHorizontalOffset(_scrollSize);
                ScrollViewerLayer1.ScrollToHorizontalOffset(_scrollSize);
                ScrollViewerLayer2.ScrollToHorizontalOffset(_scrollSize);
                ScrollViewerLayer3.ScrollToHorizontalOffset(_scrollSize);
                DensityScrollViewer.ScrollToHorizontalOffset(_scrollSize);
                RutScrollViewer.ScrollToHorizontalOffset(_scrollSize);
            }
        }
     
        private int _band;
        private bool _newWindow;
        private Measurment _measurment;
        private ReportWindow _reportWindow;
        private bool _report;
        private string _numberMess;
        private DataRoad _showInfo;
        private string _dateEnd;
        private Report _collectionList;
        private bool _densityFlag;
        private bool _thicknessesFlag;
        private bool _ruttingFlag;
        private ObservableCollection<CollectionGraphs> _collorCollection;
        private int n = 1;
        private PdfPage page10;
        private XGraphics gfx10;
        private bool flagSave = true;
        private CoefficientWindow _coefficientWindow;
        private double _widthScrollGeneral;
        private double EndWidth;
        private int x = 80;
        private int _recX = 80;
        private int rec = 86;
        private double recA = 80.5;
        private int z = 20;
        private double q = 19.5;
        private bool argument = true;
        
        
        private ChartData Report { get; set; }
        
        public bool Collor { get; set; }

        public Grid Grid;
        public bool collor { get; set; }
        

       public MainWindow()
        {
            InitializeComponent();
            Band.IsEnabled = false;
           Grid = GridAll;

           //ChartLayer1.Height = GridLay3.ActualHeight - 10;


           ScrollViewerLayer1.ViewportWidth.ToString();
           //(DataContext as MainViewModel).ScrollWigth = ScrollViewerLayer1;
           #region Messengers
           Messenger.Default.Register<bool>(this, "CoefficientWindow", message =>
           {

               if (message)
               {
                   _coefficientWindow = new CoefficientWindow();
                   _coefficientWindow.Show();
                   
               }
               else
               {
                   _coefficientWindow.Close();  
               }
               
           });

       

           Messenger.Default.Register<ChartData>(this, "Test", message =>
           {
               ReportButton.IsEnabled = false;
               Report = message;
               ButtonReport(true);

           });
           
           Messenger.Default.Register<string>(this, "SendNumMess", message =>
           {
               _numberMess = message;
           });

           Messenger.Default.Register<string>(this, "ShowDateEnd", message =>
           {
               _dateEnd = message;
               ChartLayer3.Height = GridLay3.ActualHeight - 5;
               ChartL3.Height = ChartLayer3.Height;
               ChartLayer2.Height = GridLay2.ActualHeight - 5;
               ChartL2.Height = ChartLayer2.Height;
               ChartLayer1.Height = GridLay2.ActualHeight - 5;
               ChartL1.Height = ChartLayer2.Height;
               ChartAll.Height = GridAll.ActualHeight - 25;
               ChartAl.Height = ChartAll.Height;
               CountChart.Height = GridCount.ActualHeight - 5;
               ChartC.Height = CountChart.Height;
               Rut.Height = GridRut.ActualHeight - 5;
               ChartRt.Height = Rut.Height;
               Plotnost.Height = GridPlotn.ActualHeight - 5;
               ChartPl.Height = Plotnost.Height;
               
           });
            Messenger.Default.Register<bool>(this, "ShowNewWindow", ShowWindow);
            Messenger.Default.Register<bool>(this, "CloseSettingsReport", CloseSettingsReport);

            Messenger.Default.Register<Report>(this, "ShowCollection",message =>
            {
                _collectionList = message;
            });

           //Checkbox
        
            Messenger.Default.Register<List<bool>>(this, "TrueFalse", message =>
            {
                _densityFlag = message[0];
                _thicknessesFlag = message[1];
                _ruttingFlag = message[2];
            });
         

            Messenger.Default.Register<double>(this, "startWidth", message =>
            {
                EndWidth = message;
            });

            Messenger.Default.Register<bool>(this, "CorrectSettingsReport", message =>
            {
                Button(Report);
            });
           
            Messenger.Default.Register<bool>(this, "CloseNewWindow", message =>
            {
               var flag = message;
               CloseWindow(flag);
            });
            Messenger.Default.Register<DataRoad>(this, "ShowInfo", message =>
            {
                _showInfo = message;
            });
            _tempsize = DensityScrollViewer.ContentHorizontalOffset;

            Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<ObservableCollection<CollectionGraphs>>(this, "ShowGeneralGraff", message => 
                GeneralGraff.DrawGraphic(message, Collor = true, _band));
            Messenger.Default.Register<ObservableCollection<CollectionGraphs>>(this, "ShowGeneralGraff", message =>
                _collorCollection = message
               );

           
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1),
                IsEnabled = true
            };
            timer.Tick +=
                (o, t) =>
                {
                    TimeBlock.Text = "        " + DateTime.Now.ToLongTimeString() + "\n" +
                                     DateTime.Now.ToLongDateString();
                };
            timer.Start();

            Messenger.Default.Register<List<List<object>>>(this, "GreatGraphic", message =>
            {
                const string titlex = "Расстояние, [м]";
                string charttitle;
                string titley;
                Brush brush;
                int  serias;


                switch (_greatGraphicName)
                {

                    case "CountChart":
                        charttitle = "Количество слоёв";
                        brush = Brushes.Black;
                        serias = 2;
                        Liner(message[0], message[1], titlex, charttitle, null, brush, serias);

                        break;

                    case "ChartLayer3":
                        charttitle = "Толщина слоя №3";
                        titley = "Толщина, [см]";
                        brush = Brushes.DodgerBlue;
                        serias = 1;
                        Liner(message[0], message[1], titlex, charttitle, titley, brush, serias);
                        
                        break;


                    case "ChartLayer2":
                       charttitle = "Толщина слоя №2";
                       titley = "Толщина, [см]";
                       brush = Brushes.DarkRed;
                       serias = 1;
                       Liner(message[0], message[1], titlex, charttitle, titley, brush, serias);

                        break;

                    case "ChartLayer1":
                      charttitle = "Толщина слоя №1";
                       titley = "Толщина, [см]";
                       serias = 1;
                       brush = Brushes.OliveDrab;
                       
                       Liner(message[0], message[1], titlex, charttitle, titley, brush, serias);

                        break;
                    case "Rut":
                        charttitle = "Колейность";
                        brush = Brushes.BlueViolet;
                        serias = 1;
                        Liner(message[0], message[1], titlex, charttitle, null, brush, serias);

                        break;

                    case "ChartAll":
                        var layer2 = new List<float>();
                        var layer3 = new List<float>();
                        var Distance = new List<float>();
                        var layer1 = message[0].Select(count => Convert.ToByte(count)).Select(dummy => (float) dummy).ToList();
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }
                        foreach (var count in message[2])
                        {
                            layer2.Add(Convert.ToByte(count));
                        }
                        foreach (var count in message[3])
                        {
                            layer3.Add(Convert.ToByte(count));
                        }
                        var dataValues = new List<Graphics>();
                        int max = Convert.ToInt16(layer3[0]);
                        
                        for (var i = 0; i < layer1.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer3 = layer3[i];
                            graph.Layer2 = layer2[i];
                            graph.Layer1 = layer1[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            
                            if (layer3[i]> max)
                            {
                                max = Convert.ToInt16(layer3[i]);
                            }
                        }

                        var linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        var linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "Толщина, [см]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;

                        ////
                        var  setter1 = new Setter();
                        var setterpoint1 = new Setter();
                        setterpoint1.Property = OpacityProperty;
                        setterpoint1.Value = (double)0;
                        setter1.Property = BackgroundProperty;
                        setter1.Value = Brushes.OliveDrab;
                        var style1 = new Style();
                        var areaDataPoint1 = new AreaDataPoint();
                        style1.TargetType = areaDataPoint1.GetType();
                        style1.Setters.Add(setter1);
                        style1.Setters.Add(setterpoint1);
                        var ser1 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer1",
                            IndependentValuePath = "Distance",
                            Title = "Слой №3"
                        };
                        ser1.DataPointStyle = style1;
                        ///
                         var  setter2 = new Setter();
                         var setterpoint2 = new Setter();
                        setterpoint2.Property = OpacityProperty;
                        setterpoint2.Value = (double)0;
                        setter2.Property = BackgroundProperty;
                        setter2.Value = Brushes.DarkRed;
                        var style2 = new Style();
                        var areaDataPoint2 = new AreaDataPoint();
                        style2.TargetType = areaDataPoint2.GetType();
                        style2.Setters.Add(setter2);
                        style2.Setters.Add(setterpoint2);
                        var ser2 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer2",
                            IndependentValuePath = "Distance",
                            Title = "Слой №2"
                        };
                        ser2.DataPointStyle = style2;
                        ///
                        var  setter3 = new Setter();
                         var setterpoint3 = new Setter();
                        setterpoint3.Property = OpacityProperty;
                        setterpoint3.Value = (double)0;
                        setter3.Property = BackgroundProperty;
                        setter3.Value = Brushes.DodgerBlue;
                        var style3 = new Style();
                        var areaDataPoint3 = new AreaDataPoint();
                        style3.TargetType = areaDataPoint3.GetType();
                        style3.Setters.Add(setter3);
                        style3.Setters.Add(setterpoint3);
                        var ser3 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer3",
                            IndependentValuePath = "Distance",
                            Title = "Слой №1"
                        };
                        ser3.DataPointStyle = style3;

                        _chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Толщина слоёв",

                        };

                        var setterlegendcollor = new Setter();
                        var setterlegendborder = new Setter();
                        setterlegendcollor.Property = BackgroundProperty;
                        setterlegendcollor.Value = new SolidColorBrush(Color.FromRgb(3, 94, 129));
                        setterlegendborder.Property = BorderBrushProperty;
                        setterlegendborder.Value = new SolidColorBrush(Color.FromRgb(3, 94, 129));
                        var legendStyle = new Style();
                        var control = new Control();
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(setterlegendcollor);
                        legendStyle.Setters.Add(setterlegendborder);
                        _chart.LegendStyle = legendStyle;
                        _chart.Axes.Add(linerAxx);
                        _chart.Axes.Add(linerAxy);
                        _chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        _chart.Series.Add(ser3);
                        _chart.Series.Add(ser2);
                        _chart.Series.Add(ser1);
                        
                        
                        LayoutRoot.Children.Add(_chart);
                        Grid.SetRow(_chart, 0);
                        Grid.SetRowSpan(_chart, 6);
                        Grid.SetColumnSpan(_chart, 5);


                        break;

                    case "Plotnost":
                        //блоч комп
                        charttitle = "Плотность";
                        brush = Brushes.OliveDrab;
                        titley = "[кг/м^3]";
                        serias = 3;
                        Liner(message[0], message[1], titlex, charttitle, titley, brush, serias);
             
                        break;
                    case "GeneralGraff":
                        //блоч комп
                        var countGener = new List<byte>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            countGener.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }
                        var dataValues1 = new ObservableCollection<CollectionGraphs>();
                        for (var i = 0; i < countGener.Count - 1; i++)
                        {
                            var graph = new CollectionGraphs();
                            //graph.GeneralState = CountGener[i];
                            //graph.Distance = Distance[i];
                            dataValues1.Add(graph);
                        }
                       
                        _gridnorm = new Grid();
                        _gridd = new Graff();
                        SendDensityStandarts(true);
                        _gridd.DrawGraphic(_collorCollection, Collor = true, _band);
                        //_gridd.Width = 1111;
                        //_gridd.Height = 230;
                        
                       
                        //SendWidth();
                        var text1 = new TextBlock();
                        var text2 = new TextBlock();
                        var collor1 = new TextBlock();
                        var collor2 = new TextBlock();
                        var collortext1 = new TextBlock();
                        var collortext2 = new TextBlock();
                        collor1.Background = Brushes.Red;
                        collor2.Background = Brushes.Green;
                        collor1.Width = 50;
                        collor1.Height = 50;
                        collor2.Width = 50;
                        collor2.Height = 50;
                        collor1.HorizontalAlignment = HorizontalAlignment.Center;
                        collor1.VerticalAlignment = VerticalAlignment.Center;
                        collor2.HorizontalAlignment = HorizontalAlignment.Center;
                        collor2.VerticalAlignment = VerticalAlignment.Center;
                        collor1.Margin = new Thickness(-450, 750, 0, 0);
                        collor2.Margin = new Thickness(230, 750, 0, 0);
                        collortext1.Text = "- не соответствует";
                        collortext2.Text = "- соответствует";
                        collortext1.FontSize = 19;
                        collortext2.FontSize = 19;
                        collortext1.HorizontalAlignment = HorizontalAlignment.Center;
                        collortext1.VerticalAlignment = VerticalAlignment.Center;
                        collortext2.HorizontalAlignment = HorizontalAlignment.Center;
                        collortext2.VerticalAlignment = VerticalAlignment.Center;
                        collortext1.Margin = new Thickness(-120, 757, 0, 0);
                        collortext2.Margin = new Thickness(460, 757, 0, 0);
                        text1.Text = "Общее состояние дорожного покрытия";
                        text2.Text = "Расстояние, [м]";
                        text2.FontStyle = FontStyles.Italic;
                        text1.FontSize = 22;
                        text2.FontSize = 19;
                        text1.HorizontalAlignment = HorizontalAlignment.Center;
                        text1.VerticalAlignment = VerticalAlignment.Top;
                        text2.HorizontalAlignment = HorizontalAlignment.Center;
                        text2.VerticalAlignment = VerticalAlignment.Top;
                        text1.Margin = new Thickness(0,200,0,0);
                        text2.Margin = new Thickness(0, 700, 0, 0);
                        _gridnorm.Children.Add(collortext1);
                        _gridnorm.Children.Add(collortext2);
                        _gridnorm.Children.Add(collor1);
                        _gridnorm.Children.Add(collor2);
                        _gridnorm.Children.Add(text1);
                        _gridnorm.Children.Add(text2);
                        _gridnorm.Children.Add(_gridd);
                        _gridnorm.Background = new SolidColorBrush(Color.FromRgb(3, 94, 129));
                        
                        LayoutRoot.Children.Add(_gridnorm);
                        Grid.SetRow(_gridnorm, 0);
                        Grid.SetRowSpan(_gridnorm, 6);
                        Grid.SetColumnSpan(_gridnorm, 5);
                        _gridnorm.MouseLeftButtonDown += MouseLeftButtonDownGrid1;
                        _gridd.MouseLeftButtonDown += MouseLeftButtonDownGrid1;
                        SendWidth(LayoutRoot.ActualWidth-400);
                        break;
                }

            });
           
#endregion 
        }

        private void CloseSettingsReport(bool argument)
        {

            if (_reportWindow == null) return;
            if (argument)
            {
                 _reportWindow.Close();
                ReportButton.IsEnabled = true;
            }
          
        }
        private void ButtonReport(bool argument)
        {
            var count = Application.Current.Windows.Count;
            //Проверка на количество открытых окон
            if (count > 2) return;
            if (argument)
            {
                _reportWindow = new ReportWindow();
                _reportWindow.Show();
            }
        }
         
        private void ShowWindow(bool flag)
        {
            var count = Application.Current.Windows.Count;
            //Проверка на количество открытых окон
            if (count > 2) return;
            if (flag)
            {
                _measurment = new Measurment();
                _measurment.Show();
            }

            
            SendScrollWidth(ScrollViewerLayer1.ViewportWidth);
        }
        private void CloseWindow(bool flag)
        {
            
            if (_measurment == null) return;
            if (flag)
            {
                _measurment.Close();
            }
        }

      
       
        /// <summary>
        /// Метод создания граффиков
        /// </summary>
        /// <param name="mess1"></param>
        /// <param name="mess2"></param>
        /// <param name="tittlex"></param>
        /// <param name="charttitle"></param>
        /// <param name="tittley"></param>
        /// <param name="collor"></param>
        /// <param name="serias"></param>
        /// <param name="name"></param>
        public void Chart(List<object> mess1, List<object> mess2, string tittlex, string charttitle,
         string tittley, Brush collor, int serias, string name)
        {
            var ocY = new List<float>();
            var Distance = new List<float>();
            foreach (var count in mess1)
            {
                ocY.Add(Convert.ToByte(count));
            }
            foreach (var distance in mess2)
            {
                Distance.Add(Convert.ToSingle(distance));
            }
            var dataValues = new List<Graphics>();
            var max = Convert.ToInt16(ocY[0]);
            for (var i = 0; i < ocY.Count - 1; i++)
            {
                var graph = new Graphics { Distance = Distance[i] };
                if (serias == 1)
                {
                    graph.Layer3 = ocY[i];
                }
                if (serias == 2)
                {
                    graph.CountLayer = Convert.ToByte(ocY[i]);
                }
                dataValues.Add(graph);
                if (ocY[i] > max)
                {
                    max = Convert.ToInt16(ocY[i]);
                }
                if (serias == 3)
                {
                    graph.IntensityN1 = Convert.ToByte(ocY[i]);
                }
            }
            var linerAxx = new LinearAxis
            {
                Orientation = AxisOrientation.X,

                Title = tittlex,
                Foreground = Brushes.Black
            };
            var linerAxy = new LinearAxis
            {
                Orientation = AxisOrientation.Y,
                ShowGridLines = true,
                Minimum = 0,
                Maximum = max + 2,
                Foreground = Brushes.Black
            };
            if (tittley != null)
            {
                linerAxy.Title = tittley;
            }
            var element = new Chart()
            {
                Background = Brushes.White,
                //Title = charttitle,
                Name = name,
                Width = 650,
                Height = 300,
                
                Foreground = Brushes.Black
            };
            var setter = new Setter();
            var setterpoint = new Setter { Property = OpacityProperty, Value = (double)0 };

            setter.Property = BackgroundProperty;
            setter.Value = collor;
            var style = new Style();
            style.Setters.Add(setter);
            var legendSetterw = new Setter();
            var legendSetterl = new Setter();
            var legendStyle = new Style();
            var control = new Control();
         
            if (serias == 3)
            {
                var LS = new LineSeries()
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "IntensityN1",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style
                };
                linerAxx.ShowGridLines = true;
                style.Setters.Add(setterpoint);
                var lineDataPoint = new LineDataPoint();
                style.TargetType = lineDataPoint.GetType();
                element.Series.Add(LS);
            }
            if (serias == 2)
            {
                var cl = new ColumnSeries
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "CountLayer",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style
                };

                var columnDataPoint = new ColumnDataPoint();
                style.TargetType = columnDataPoint.GetType();

                //style.Setters.Add(setter);
                element.Series.Add(cl);
            }
            if (serias == 1)
            {
                var AS = new AreaSeries
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "Layer3",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style
                };
                linerAxx.ShowGridLines = true;
                style.Setters.Add(setterpoint);
                var areaDataPoint = new AreaDataPoint();
                style.TargetType = areaDataPoint.GetType();
                element.Series.Add(AS);
            }
            legendSetterw.Property = WidthProperty;
            legendSetterw.Value = (double)20;
            legendSetterl.Property = HeightProperty;
            legendSetterl.Value = (double)0;
            legendStyle.TargetType = control.GetType();
            legendStyle.Setters.Add(legendSetterl);
            legendStyle.Setters.Add(legendSetterw);
            element.LegendStyle = legendStyle;
            element.Axes.Add(linerAxx);
            element.Axes.Add(linerAxy);
            Grid.Children.Add(element);
            //LayoutRoot.Children.Add(element);
            //Grid.SetRow(element, 1);
            //Grid.SetColumn(element, 1);
        }



        /// <summary>
        /// Создание всех графиков качеств полос
        /// </summary>
        /// <param name="parametrCollection"></param>
        /// <param name="argument"></param>
        /// <param name="k"></param>
        private void CreatGrid(ObservableCollection<Graphics> parametrCollection, string argument, int k)
        {
            List<object> ocY;
            List<object> Distance;
            var titlex = "Расстояние, [м]";
            switch (argument)
            {
                case "CountBand":
                    
                    ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.CountLayer));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Количество слоёв", null, Brushes.Black, 2, "CountBand"+k);
                    break;
                case "AllLayerBand":
                        var Layer1 = new List<float>();
                        var Layer2 = new List<float>();
                        var Layer3 = new List<float>();
                        Distance = new List<object>();
                        foreach (var count in parametrCollection)
                        {
                            Layer1.Add(Convert.ToByte(count.Layer1));
                        }
                        foreach (var distance in parametrCollection)
                        {
                            Distance.Add(Convert.ToSingle(distance.Distance));
                        }
                        foreach (var count in parametrCollection)
                        {
                            Layer2.Add(Convert.ToByte(count.Layer2));
                        }
                        foreach (var count in parametrCollection)
                        {
                            Layer3.Add(Convert.ToByte(count.Layer3));
                        }
                        var dataValues = new List<Graphics>();
                        var max = Convert.ToInt16(Layer3[0]);
                        
                        for (var i = 0; i < Layer1.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer3 = Layer3[i];
                            graph.Layer2 = Layer2[i];
                            graph.Layer1 = Layer1[i];
                            graph.Distance = Convert.ToSingle(Distance[i]);
                            dataValues.Add(graph);
                            
                            if (Layer3[i]> max)
                            {
                                max = Convert.ToInt16(Layer3[i]);
                            }
                        }

                        var linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                    linerAxx.Foreground = Brushes.Black;
                        var linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "Толщина, [см]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;
                    linerAxy.Foreground = Brushes.Black;


                        var setter1 = new Setter();
                        var setterpoint1 = new Setter();
                        setterpoint1.Property = OpacityProperty;
                        setterpoint1.Value = (double)0;
                        setter1.Property = BackgroundProperty;
                        setter1.Value = Brushes.OliveDrab;
                        var style1 = new Style();
                        var areaDataPoint1 = new AreaDataPoint();
                        style1.TargetType = areaDataPoint1.GetType();
                        style1.Setters.Add(setter1);
                        style1.Setters.Add(setterpoint1);
                        var ser1 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer1",
                            IndependentValuePath = "Distance",
                            Title = "Слой №3"
                        };
                        ser1.DataPointStyle = style1;
                        ///
                         var  setter2 = new Setter();
                         var setterpoint2 = new Setter();
                        setterpoint2.Property = OpacityProperty;
                        setterpoint2.Value = (double)0;
                        setter2.Property = BackgroundProperty;
                        setter2.Value = Brushes.DarkRed;
                        var style2 = new Style();
                        var areaDataPoint2 = new AreaDataPoint();
                        style2.TargetType = areaDataPoint2.GetType();
                        style2.Setters.Add(setter2);
                        style2.Setters.Add(setterpoint2);
                        var ser2 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer2",
                            IndependentValuePath = "Distance",
                            Title = "Слой №2"
                        };
                        ser2.DataPointStyle = style2;
                        ///
                        var  setter3 = new Setter();
                         var setterpoint3 = new Setter();
                        setterpoint3.Property = OpacityProperty;
                        setterpoint3.Value = (double)0;
                        setter3.Property = BackgroundProperty;
                        setter3.Value = Brushes.DodgerBlue;
                        var style3 = new Style();
                        var areaDataPoint3 = new AreaDataPoint();
                        style3.TargetType = areaDataPoint3.GetType();
                        style3.Setters.Add(setter3);
                        style3.Setters.Add(setterpoint3);
                        var ser3 = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer3",
                            IndependentValuePath = "Distance",
                            Title = "Слой №1"
                        };
                        ser3.DataPointStyle = style3;

                        _chart = new Chart()
                        {
                            Foreground= Brushes.Black,
                            //Title = "Толщина слоёв",
                            Name = "AllLayerBand" + k,
                            Width = 650,
                            Height = 300
                        };

                        var setterlegendcollor = new Setter();
                        var setterlegendborder = new Setter();
                        setterlegendcollor.Property = BackgroundProperty;
                        setterlegendcollor.Value = Brushes.Black;
                    setterlegendcollor.Property = HeightProperty;
                    setterlegendcollor.Value = 50;
                    setterlegendcollor.Property = ForegroundProperty;
                    setterlegendcollor.Value = Brushes.Black;
                        setterlegendborder.Property = BorderBrushProperty;
                        setterlegendborder.Value = Brushes.White;
                        var legendStyle = new Style();
                        var control = new Control();
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(setterlegendcollor);
                        legendStyle.Setters.Add(setterlegendborder);
                        _chart.LegendStyle = legendStyle;
                        _chart.Axes.Add(linerAxx);
                        _chart.Axes.Add(linerAxy);
                        _chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        _chart.Series.Add(ser3);
                        _chart.Series.Add(ser2);
                        _chart.Series.Add(ser1);
                        Grid.Children.Add(_chart);
                     //   LayoutRoot.Children.Add(_chart);
                     //Grid.SetRow(_chart, 1);
                     //Grid.SetColumn(_chart, 1);
                    break;
                case "DensityBand":
                    ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.IntensityN1));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Плотность", "[кг/м^3]", Brushes.OliveDrab, 3, "DensityBand" + k);
                    break;
                case "Layer1Band":
                    ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.Layer1));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Толщина слоя №1", "Толщина, [см]", Brushes.OliveDrab, 1, "Layer1Band" + k);
                    break;
                case "Layer2Band":
                    ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.Layer2));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Толщина слоя №2", "Толщина, [см]", Brushes.DarkRed, 1, "Layer2Band" + k);
                    break;
                case "Layer3Band":
                    ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.Layer3));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Толщина слоя №3", "Толщина, [см]", Brushes.DodgerBlue, 1, "Layer3Band" + k);
                    break;
                case "RutBand":
                     ocY = new List<object>();
                    Distance = new List<object>();
                    foreach (var count in parametrCollection)
                    {
                        ocY.Add(Convert.ToByte(count.Rut));
                    }
                    foreach (var distance in parametrCollection)
                    {
                        Distance.Add(Convert.ToSingle(distance.Distance));
                    }
                    Chart(ocY, Distance, titlex, "Колейность", "Колейность, [мм]", Brushes.BlueViolet, 1, "RutBand" + k);
                    break;
            }
        }






        /// <summary>
        /// Кнопка создание отчётов
        /// </summary>

        private void CreateImage(string namePlot, UIElement chart)
        {
            var image = Path.GetTempPath() + namePlot + ".png";
            flagSave = true;
            //var dlg = new SaveFileDialog
            SaveAsPng(GetImage(chart), image);
            //SaveAsPng(GetImage(GridAll), image);
   
        }


        private class DensityBand
        {
            public float Distance { get; set; }
            public double Parametr { get; set; }
        }

      /// <summary>
      /// Таблица несоответствия
      /// </summary>
      /// <param name="document"></param>
      /// <param name="font3"></param>
      /// <param name="font2"></param>
      /// <param name="font9"></param>
      /// <param name="page"></param>
      /// <param name="i"></param>
        private void CreatTable(PdfDocument document,XFont font3, XFont font2, XFont font9, PdfPage page, int i)
        {

            var n = 1;

            //var x = 80;
            //var _recX = 80;
            //var rec = 86;
            //var recA = 80.5;
            //var z = 20;
            //var q = 19.5;
          
            if (argument)
            {
                //Таблица несоответствия
                page10 = document.AddPage();
                gfx10 = XGraphics.FromPdfPage(page10);


                gfx10.DrawString(@"Несоответствие требованиям", font3, XBrushes.Black,
                                new XRect(200, 15, page.Width, page.Height),
                                XStringFormats.TopLeft);
                //горизонтальная
                gfx10.DrawLine(XPens.Black, 50, 35, 550, 35);
                gfx10.DrawLine(XPens.Black, 50, 80, 550, 80);
                gfx10.DrawLine(XPens.Black, 200, 50, 550, 50);
                //Вертикальные
                gfx10.DrawLine(XPens.Black, 50, 35, 50, 80);
                gfx10.DrawLine(XPens.Black, 70, 35, 70, 80);
                gfx10.DrawLine(XPens.Black, 100, 35, 100, 80);
                gfx10.DrawLine(XPens.Black, 135, 35, 135, 80);
                gfx10.DrawLine(XPens.Black, 200, 35, 200, 80);
                gfx10.DrawLine(XPens.Black, 250, 50, 250, 80);
                gfx10.DrawLine(XPens.Black, 300, 50, 300, 80);
                gfx10.DrawLine(XPens.Black, 350, 50, 350, 80);
                gfx10.DrawLine(XPens.Black, 400, 50, 400, 80);
                gfx10.DrawLine(XPens.Black, 450, 50, 450, 80);
                gfx10.DrawLine(XPens.Black, 500, 50, 500, 80);
                gfx10.DrawLine(XPens.Black, 550, 35, 550, 80);

                gfx10.DrawString(@"№", font2, XBrushes.Black,
                                new XRect(55, 55, page.Width, page.Height),
                                XStringFormats.TopLeft);
                gfx10.DrawString(@"[м]", font2, XBrushes.Black,
                                new XRect(79, 55, page.Width, page.Height),
                                XStringFormats.TopLeft);
                gfx10.DrawString(@"Полоса", font2, XBrushes.Black,
                                new XRect(102, 55, page.Width, page.Height),
                                XStringFormats.TopLeft);
                gfx10.DrawString(@"Координаты", font2, XBrushes.Black,
                                new XRect(137, 55, page.Width, page.Height),
                                XStringFormats.TopLeft);

                gfx10.DrawString(@"Параметр", font2, XBrushes.Black,
                                new XRect(330, 38, page.Width, page.Height),
                                XStringFormats.TopLeft);

                gfx10.DrawString(@"Плотность", font9, XBrushes.Black,
                               new XRect(203, 60, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"Количество", font9, XBrushes.Black,
                               new XRect(255, 56, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"слоёв", font9, XBrushes.Black,
                               new XRect(262, 64, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"Толщина", font9, XBrushes.Black,
                               new XRect(307, 56, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"слоя №1", font9, XBrushes.Black,
                               new XRect(311, 64, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"Толщина", font9, XBrushes.Black,
                               new XRect(359, 56, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"слоя №2", font9, XBrushes.Black,
                               new XRect(361, 64, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"Толщина", font9, XBrushes.Black,
                               new XRect(410, 56, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"слоя №3", font9, XBrushes.Black,
                               new XRect(413, 64, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"Колейность", font9, XBrushes.Black,
                               new XRect(454, 60, page.Width, page.Height),
                               XStringFormats.TopLeft);
                gfx10.DrawString(@"IRI", font9, XBrushes.Black,
                               new XRect(520, 60, page.Width, page.Height),
                               XStringFormats.TopLeft);
                argument = false;
            }
          
           
           
            for (var j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
            {
                //Если страница закончилась тосоздаём новую
                if (_recX>=820)
                {
                    page10 = document.AddPage();
                    gfx10 = XGraphics.FromPdfPage(page10);
                    x = 15;
                    //Горизонтальные
                    gfx10.DrawLine(XPens.Black, 50, 15, 550, 15);

                    _recX = 15;
                    rec = 21;
                    recA = 15.5;
                    z = 20;
                    q = 19.5;


                }
               
                    
                if (_collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1 <= _showInfo.StandartsDensity ||
                  _collectionList.CountBand[i].GraphicsesCollection[j].CountLayer != _showInfo.DesignCount ||
                  _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1 <= _showInfo.DesignLayer1 ||
                  _collectionList.Layer2Band[i].GraphicsesCollection[j].Layer2 <= _showInfo.DesignLayer2 ||
                  _collectionList.Layer3Band[i].GraphicsesCollection[j].Layer3 <= _showInfo.DesignLayer3 ||
                  _collectionList.RutBand[i].GraphicsesCollection[j].Rut >= _showInfo.StandartsRut ||
                  _collectionList.IRIBand[i].GraphicsesCollection[j].IRI <= _showInfo.StandartsIRI)
                {
                    if (_collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1 <= _showInfo.StandartsDensity)
                    {

                        gfx10.DrawRectangle(XBrushes.Red, 200, recA, 50, q);

                    }
                    if (_collectionList.CountBand[i].GraphicsesCollection[j].CountLayer != _showInfo.DesignCount)
                    {

                        gfx10.DrawRectangle(XBrushes.Red, 250, recA, 50.5, q);
                    }
                    if (_collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1 <=
                        _showInfo.DesignLayer1)
                    {

                        gfx10.DrawRectangle(XBrushes.Red, 300, recA, 50.5, q);
                    }
                    if (_collectionList.Layer2Band[i].GraphicsesCollection[j].Layer2 <=
                    _showInfo.DesignLayer2)
                    {
                        gfx10.DrawRectangle(XBrushes.Red, 350, recA, 50.5, q);
                    }
                    if (_collectionList.Layer3Band[i].GraphicsesCollection[j].Layer3 <=
                  _showInfo.DesignLayer3)
                    {
                        gfx10.DrawRectangle(XBrushes.Red, 400, recA, 50.5,q);
                    }
                    if (_collectionList.RutBand[i].GraphicsesCollection[j].Rut >=
                       _showInfo.StandartsRut)
                    {
                        gfx10.DrawRectangle(XBrushes.Red, 450, recA, 50.5, q);
                    }

                    if (_collectionList.IRIBand[i].GraphicsesCollection[j].IRI <=
                     _showInfo.StandartsIRI)
                    {
                        gfx10.DrawRectangle(XBrushes.Red, 500, recA, 50.5, q);
                    }

                    gfx10.DrawString(n.ToString(), font9, XBrushes.Black,
                    new XRect(57, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);

                    gfx10.DrawString(_collectionList.DensityBand[i].GraphicsesCollection[j].Distance.ToString(), font9, XBrushes.Black,
                     new XRect(79, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.DensityBand[i].GraphicsesCollection[j].Latitude.ToString(), font9, XBrushes.Black,
                     new XRect(145, rec - 5, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.DensityBand[i].GraphicsesCollection[j].Longitude.ToString(), font9, XBrushes.Black,
                     new XRect(145, rec +4, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.GeneralBand[i].Band.ToString(), font9, XBrushes.Black,
                     new XRect(118, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1.ToString(), font9, XBrushes.Black,
                     new XRect(222, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.CountBand[i].GraphicsesCollection[j].CountLayer.ToString(), font9, XBrushes.Black,
                     new XRect(272, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1.ToString(), font9, XBrushes.Black,
                     new XRect(322, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.Layer2Band[i].GraphicsesCollection[j].Layer2.ToString(), font9, XBrushes.Black,
                     new XRect(372, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.Layer3Band[i].GraphicsesCollection[j].Layer3.ToString(), font9, XBrushes.Black,
                     new XRect(422, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.RutBand[i].GraphicsesCollection[j].Rut.ToString(), font9, XBrushes.Black,
                     new XRect(472, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    gfx10.DrawString(_collectionList.IRIBand[i].GraphicsesCollection[j].IRI.ToString(), font9, XBrushes.Black,
                     new XRect(522, rec, page.Width, page.Height),
                    XStringFormats.TopLeft);
                    //горизонтальная
                    gfx10.DrawLine(XPens.Black, 50, x += z, 550, x);
                    //вертикальные
                    gfx10.DrawLine(XPens.Black, 50, _recX, 50, _recX + z);
                    gfx10.DrawLine(XPens.Black, 70, _recX, 70, _recX + z);
                    gfx10.DrawLine(XPens.Black, 100, _recX, 100, _recX + z);
                    gfx10.DrawLine(XPens.Black, 135, _recX, 135, _recX + z);
                    gfx10.DrawLine(XPens.Black, 200, _recX, 200, _recX + z);
                    gfx10.DrawLine(XPens.Black, 250, _recX, 250, _recX + z);
                    gfx10.DrawLine(XPens.Black, 300, _recX, 300, _recX + z);
                    gfx10.DrawLine(XPens.Black, 350, _recX, 350, _recX + z);
                    gfx10.DrawLine(XPens.Black, 400, _recX, 400, _recX + z);
                    gfx10.DrawLine(XPens.Black, 450, _recX, 450, _recX + z);
                    gfx10.DrawLine(XPens.Black, 500, _recX, 500, _recX + z);
                    gfx10.DrawLine(XPens.Black, 550, _recX, 550, _recX + z);
                    rec += z;
                    recA += z;
                    _recX += z;
                }
                n++;
            }

            
            
        }

        private void DensityTable(PdfDocument document, PdfPage page, XFont font2, XFont font7, XFont font5, string dens1, XGraphics gfx2)
        {
            page = document.AddPage();
            gfx2 = XGraphics.FromPdfPage(page);
            //горизонтальная
            gfx2.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx2.DrawLine(XPens.Black, 50, 45, 550, 45);
            gfx2.DrawLine(XPens.Black, 130, 30, 550, 30);
            //вертикальная 
            gfx2.DrawLine(XPens.Black, 50, 15, 50, 45);
            gfx2.DrawLine(XPens.Black, 550, 15, 550, 45);
            gfx2.DrawLine(XPens.Black, 130, 15, 130, 45);
            gfx2.DrawLine(XPens.Black, 172, 30, 172, 45);
            gfx2.DrawLine(XPens.Black, 214, 30, 214, 45);
            gfx2.DrawLine(XPens.Black, 256, 30, 256, 45);
            gfx2.DrawLine(XPens.Black, 298, 30, 298, 45);
            gfx2.DrawLine(XPens.Black, 340, 30, 340, 45);
            gfx2.DrawLine(XPens.Black, 382, 30, 382, 45);
            gfx2.DrawLine(XPens.Black, 424, 30, 424, 45);
            gfx2.DrawLine(XPens.Black, 466, 30, 466, 45);
            gfx2.DrawLine(XPens.Black, 508, 30, 508, 45);
            gfx2.DrawLine(XPens.Black, 550, 30, 550, 45);

            gfx2.DrawString(@"Дистанция, [м]", font2, XBrushes.Black,
                        new XRect(62, 25, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"Плотность по поласам, [кг/м ]", font2, XBrushes.Black,
                        new XRect(260, 18, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"3", font7, XBrushes.Black,
                        new XRect(377, 15, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"1", font2, XBrushes.Black,
                        new XRect(151, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"2", font2, XBrushes.Black,
                        new XRect(193, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"3", font2, XBrushes.Black,
                        new XRect(235, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"4", font2, XBrushes.Black,
                        new XRect(277, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"5", font2, XBrushes.Black,
                        new XRect(319, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"6", font2, XBrushes.Black,
                        new XRect(361, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"7", font2, XBrushes.Black,
                        new XRect(403, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"8", font2, XBrushes.Black,
                        new XRect(445, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"9", font2, XBrushes.Black,
                        new XRect(487, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx2.DrawString(@"10", font2, XBrushes.Black,
                        new XRect(521, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);


            var g = 45;

            var perX = 45;

            


            //var flag = true;
            //var distanceAll = new List<float>();
            var densityBand1 = new List<DensityBand>();
            var densityBand2 = new List<DensityBand>();
            var densityBand3 = new List<DensityBand>();
            var densityBand4 = new List<DensityBand>();
            var densityBand5 = new List<DensityBand>();
            var densityBand6 = new List<DensityBand>();
            var densityBand7 = new List<DensityBand>();
            var densityBand8 = new List<DensityBand>();
            var densityBand9 = new List<DensityBand>();
            var densityBand10 = new List<DensityBand>();
            var listDensityBand = new List<List<DensityBand>>();




            //var count = new List<int>();
            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
               
                switch (_collectionList.GeneralBand[i].Band)
                {

                    case 1:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB1 = new DensityBand();
                            densB1.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB1.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand1.Add(densB1);
                            

                        }
                        listDensityBand.Add(densityBand1);
                        break;
                    case 2:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB2 = new DensityBand();
                            densB2.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB2.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand2.Add(densB2);

                        }
                        listDensityBand.Add(densityBand2);
                        break;
                    case 3:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB3 = new DensityBand();
                            densB3.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB3.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand3.Add(densB3);

                        }
                        listDensityBand.Add(densityBand3);
                        break;
                    case 4:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB4 = new DensityBand();
                            densB4.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB4.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand4.Add(densB4);

                        }
                        listDensityBand.Add(densityBand4);
                        break;
                    case 5:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB5 = new DensityBand();
                            densB5.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB5.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand5.Add(densB5);

                        }
                        listDensityBand.Add(densityBand5);
                        break;
                    case 6:
                        for (var j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB6 = new DensityBand();
                            densB6.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB6.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand6.Add(densB6);

                        }
                        listDensityBand.Add(densityBand6);
                        break;
                    case 7:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB7 = new DensityBand();
                            densB7.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB7.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand7.Add(densB7);

                        }
                        listDensityBand.Add(densityBand7);
                        break;
                    case 8:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB8 = new DensityBand();
                            densB8.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB8.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand8.Add(densB8);

                        }
                        listDensityBand.Add(densityBand8);
                        break;
                    case 9:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB9 = new DensityBand();
                            densB9.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB9.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand9.Add(densB9);

                        }
                        listDensityBand.Add(densityBand9);
                        break;
                    case 10:
                        for (int j = 0; j < _collectionList.DensityBand[i].GraphicsesCollection.Count; j++)
                        {
                            var densB10 = new DensityBand();
                            densB10.Distance = _collectionList.DensityBand[i].GraphicsesCollection[j].Distance;
                            densB10.Parametr = _collectionList.DensityBand[i].GraphicsesCollection[j].IntensityN1;
                            densityBand10.Add(densB10);

                        }
                        listDensityBand.Add(densityBand10);
                        break;
                }
            }

            var distanceBand = densityBand1;

            for (var i = 0; i < listDensityBand.Count; i++)
            {
                if (distanceBand.Count < listDensityBand[i].Count)
                {
                    distanceBand = listDensityBand[i];
                }
            }

            
           
            var h = 48;
            for (var i = 0; i < distanceBand.Count; i++)
            {
                if (h >= 820)
                {
                    page = document.AddPage();
                    gfx2 = XGraphics.FromPdfPage(page);
                    gfx2.DrawLine(XPens.Black, 50, 15, 550, 15);
                    perX = 15;
                    g = 15;
                    h = 18;
                }
                //дистанция
                gfx2.DrawString(distanceBand[i].Distance.ToString(), font5, XBrushes.Black,
                                    new XRect(62, h, page.Width, page.Height),
                                    XStringFormats.TopLeft);
                try
                {
                    //плотность 1
                    if (densityBand1.Count  != 0)
                    {

                        gfx2.DrawString(densityBand1[i].Parametr.ToString(), font5, XBrushes.Black,
                     new XRect(147, h, page.Width, page.Height),
                     XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft); 
                    }
                 
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft); 
                }
                //плотность 2
                try
                {

                    if (densityBand2.Count != 0)
                    {
                        gfx2.DrawString(densityBand2[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(189, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {

                    gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //плотность 3
                try
                {

                    if (densityBand3.Count != 0)
                    {
                        gfx2.DrawString(densityBand3[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(231, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else{
                        gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //плотность 4
                try
                {
                    if (densityBand4.Count != 0)
                    {
                        gfx2.DrawString(densityBand4[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(273, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //плотность 5
                try
                {
                    if (densityBand5.Count != 0)
                    {
                        gfx2.DrawString(densityBand5[i].Parametr.ToString(), font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft); 
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                   
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 6
                try
                {
                    if (densityBand6.Count != 0)
                    {

                        gfx2.DrawString(densityBand6[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(357, h, page.Width, page.Height),
                            XStringFormats.TopLeft);  
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                       new XRect(357, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 7
                try
                {
                    if (densityBand7.Count != 0)
                    {
                        gfx2.DrawString(densityBand7[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(399, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                         new XRect(399, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 8
                try
                {
                    if (densityBand8.Count != 0)
                    {
                        gfx2.DrawString(densityBand8[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(441, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                       gfx2.DrawString("—", font5, XBrushes.Black,
                       new XRect(441, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 9
                try
                {
                    if (densityBand9.Count != 0)
                    {
                        gfx2.DrawString(densityBand9[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(483, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                          new XRect(483, h, page.Width, page.Height),
                          XStringFormats.TopLeft);   
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(483, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 10
                try
                {
                    if (densityBand10.Count != 0)
                    {
                        gfx2.DrawString(densityBand10[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(525, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx2.DrawString("—", font5, XBrushes.Black,
                         new XRect(525, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx2.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //горизонтальная
                gfx2.DrawLine(XPens.Black, 50, g += 15, 550, g);
                //вертикальная
                gfx2.DrawLine(XPens.Black, 50, perX, 50, perX + 15);
                gfx2.DrawLine(XPens.Black, 130, perX, 130, perX + 15);
                gfx2.DrawLine(XPens.Black, 172, perX, 172, perX + 15);
                gfx2.DrawLine(XPens.Black, 214, perX, 214, perX + 15);
                gfx2.DrawLine(XPens.Black, 256, perX, 256, perX + 15);
                gfx2.DrawLine(XPens.Black, 298, perX, 298, perX + 15);
                gfx2.DrawLine(XPens.Black, 340, perX, 340, perX + 15);
                gfx2.DrawLine(XPens.Black, 382, perX, 382, perX + 15);
                gfx2.DrawLine(XPens.Black, 424, perX, 424, perX + 15);
                gfx2.DrawLine(XPens.Black, 466, perX, 466, perX + 15);
                gfx2.DrawLine(XPens.Black, 508, perX, 508, perX + 15);
                gfx2.DrawLine(XPens.Black, 550, perX, 550, perX + 15);

                h += 15;
                perX += 15;
            }
            
  
        }

        private void RutTable(PdfDocument document, PdfPage page, XFont font2, XGraphics gfx, XFont font5, string dens1)
        {
            //таблица толщины слоёв №2
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            //горизонтальная
            gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx.DrawLine(XPens.Black, 50, 45, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 30, 550, 30);
            //вертикальная 
            gfx.DrawLine(XPens.Black, 50, 15, 50, 45);
            gfx.DrawLine(XPens.Black, 550, 15, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 15, 130, 45);
            gfx.DrawLine(XPens.Black, 172, 30, 172, 45);
            gfx.DrawLine(XPens.Black, 214, 30, 214, 45);
            gfx.DrawLine(XPens.Black, 256, 30, 256, 45);
            gfx.DrawLine(XPens.Black, 298, 30, 298, 45);
            gfx.DrawLine(XPens.Black, 340, 30, 340, 45);
            gfx.DrawLine(XPens.Black, 382, 30, 382, 45);
            gfx.DrawLine(XPens.Black, 424, 30, 424, 45);
            gfx.DrawLine(XPens.Black, 466, 30, 466, 45);
            gfx.DrawLine(XPens.Black, 508, 30, 508, 45);
            gfx.DrawLine(XPens.Black, 550, 30, 550, 45);

            gfx.DrawString(@"Дистанция, [м]", font2, XBrushes.Black,
                        new XRect(62, 25, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"Колейность по поласам", font2, XBrushes.Black,
                        new XRect(260, 18, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"1", font2, XBrushes.Black,
                        new XRect(151, 33, page.Width, page.Height),
                       XStringFormats.TopLeft);
            gfx.DrawString(@"2", font2, XBrushes.Black,
                        new XRect(193, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"3", font2, XBrushes.Black,
                        new XRect(235, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"4", font2, XBrushes.Black,
                        new XRect(277, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"5", font2, XBrushes.Black,
                        new XRect(319, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"6", font2, XBrushes.Black,
                        new XRect(361, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"7", font2, XBrushes.Black,
                        new XRect(403, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"8", font2, XBrushes.Black,
                        new XRect(445, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"9", font2, XBrushes.Black,
                        new XRect(487, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"10", font2, XBrushes.Black,
                        new XRect(521, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);




           


            var rutBand1 = new List<DensityBand>();
            var rutBand2 = new List<DensityBand>();
            var rutBand3 = new List<DensityBand>();
            var rutBand4 = new List<DensityBand>();
            var rutBand5 = new List<DensityBand>();
            var rutBand6 = new List<DensityBand>();
            var rutBand7 = new List<DensityBand>();
            var rutBand8 = new List<DensityBand>();
            var rutBand9 = new List<DensityBand>();
            var rutBand10 = new List<DensityBand>();

            var listRutBand = new List<List<DensityBand>>();


            var g = 45;

            var perX = 45;

            

            //var count = new List<int>();
            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
                DensityBand rutB;
                switch (_collectionList.GeneralBand[i].Band)
                {
                    case 1:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand1.Add(rutB);
                            
                        }
                        listRutBand.Add(rutBand1);
                        break;
                    case 2:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand2.Add(rutB);
                            
                        }
                        listRutBand.Add(rutBand2);
                        break;
                    case 3:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand3.Add(rutB);
                            
                        }
                        listRutBand.Add(rutBand3);
                        break;
                    case 4:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand4.Add(rutB);
                           
                        }
                        listRutBand.Add(rutBand4);
                        break;
                    case 5:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand5.Add(rutB);
                            
                        }
                        listRutBand.Add(rutBand5);
                        break;
                    case 6:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand6.Add(rutB);
                            
                        }
                        listRutBand.Add(rutBand6);
                        break;
                    case 7:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand7.Add(rutB);
                        }
                        listRutBand.Add(rutBand7);
                        break;
                    case 8:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand8.Add(rutB);
                        }
                        listRutBand.Add(rutBand8);
                        break;
                    case 9:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand9.Add(rutB);
                        }
                        listRutBand.Add(rutBand9);
                        break;
                    case 10:
                        for (int j = 0; j < _collectionList.RutBand[i].GraphicsesCollection.Count; j++)
                        {
                            rutB = new DensityBand();
                            rutB.Distance = _collectionList.RutBand[i].GraphicsesCollection[j].Distance;
                            rutB.Parametr = _collectionList.RutBand[i].GraphicsesCollection[j].Rut;
                            rutBand10.Add(rutB);
                        }
                        listRutBand.Add(rutBand10);
                        break;
                }
            }

            var distanceBand = rutBand1;

            for (var i = 0; i < listRutBand.Count; i++)
            {
                if (distanceBand.Count < listRutBand[i].Count)
                {
                    distanceBand = listRutBand[i];
                } 
            }


            
           
            var h = 48;
            for (var i = 0; i < distanceBand.Count; i++)
            {
                if (h >= 820)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
                    perX = 15;
                    g = 15;
                    h = 18;
                }
                gfx.DrawString(distanceBand[i].Distance.ToString(), font5, XBrushes.Black,
                                   new XRect(62, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                try
                {
                    //колейность 1
                    if (rutBand1.Count != 0)
                    {
                        gfx.DrawString(rutBand1[i].Parametr.ToString(), font5, XBrushes.Black,
                         new XRect(147, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }

                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft);
                    }
                    
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft);
                }
                //колейность 2
                try
                {
                    if (rutBand2.Count != 0)
                    {

                        gfx.DrawString(rutBand2[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(189, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(189, h, page.Width, page.Height),
                                XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {

                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 3
                try
                {


                    if (rutBand3.Count != 0)
                    {
                        gfx.DrawString(rutBand3[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(231, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {

                        gfx.DrawString("—", font5, XBrushes.Black,
                                   new XRect(231, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 4
                try
                {
                    if (rutBand4.Count != 0)
                    {
                        gfx.DrawString(rutBand4[i].Parametr.ToString(), font5, XBrushes.Black,
                                   new XRect(273, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(273, h, page.Width, page.Height),
                                XStringFormats.TopLeft); 
                    }
                   
                }
                catch (Exception)
                {
                   
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 5
                try
                {
                    if (rutBand5.Count != 0)
                    {
                        gfx.DrawString(rutBand5[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(315, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                       new XRect(315, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 6
                try
                {
                    if (rutBand6.Count != 0)
                    {
                        gfx.DrawString(rutBand6[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(357, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(357, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 7
                try
                {
                    if (rutBand7.Count != 0)
                    {
                        gfx.DrawString(rutBand7[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(399, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(399, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 8
                try
                {
                    if (rutBand8.Count != 0)
                    {
                        gfx.DrawString(rutBand8[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(441, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 9
                try
                {
                    if (rutBand9.Count != 0)
                    {

                        gfx.DrawString(rutBand9[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(483, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(483, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(483, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 10
                try
                {
                    if (rutBand10.Count != 0)
                    {
                        gfx.DrawString(rutBand10[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(525, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                       new XRect(525, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //горизонтальная
                gfx.DrawLine(XPens.Black, 50, g += 15, 550, g);
                //вертикальная
                gfx.DrawLine(XPens.Black, 50, perX, 50, perX + 15);
                gfx.DrawLine(XPens.Black, 130, perX, 130, perX + 15);
                gfx.DrawLine(XPens.Black, 172, perX, 172, perX + 15);
                gfx.DrawLine(XPens.Black, 214, perX, 214, perX + 15);
                gfx.DrawLine(XPens.Black, 256, perX, 256, perX + 15);
                gfx.DrawLine(XPens.Black, 298, perX, 298, perX + 15);
                gfx.DrawLine(XPens.Black, 340, perX, 340, perX + 15);
                gfx.DrawLine(XPens.Black, 382, perX, 382, perX + 15);
                gfx.DrawLine(XPens.Black, 424, perX, 424, perX + 15);
                gfx.DrawLine(XPens.Black, 466, perX, 466, perX + 15);
                gfx.DrawLine(XPens.Black, 508, perX, 508, perX + 15);
                gfx.DrawLine(XPens.Black, 550, perX, 550, perX + 15);
                h += 15;
                perX += 15;

            }

        }

        private void LayerTable(PdfDocument document, PdfPage page, XFont font2, XFont font5, string dens1, XGraphics gfx)
        {
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            //горизонтальная
            gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx.DrawLine(XPens.Black, 50, 45, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 30, 550, 30);
            //вертикальная 
            gfx.DrawLine(XPens.Black, 50, 15, 50, 45);
            gfx.DrawLine(XPens.Black, 550, 15, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 15, 130, 45);
            gfx.DrawLine(XPens.Black, 172, 30, 172, 45);
            gfx.DrawLine(XPens.Black, 214, 30, 214, 45);
            gfx.DrawLine(XPens.Black, 256, 30, 256, 45);
            gfx.DrawLine(XPens.Black, 298, 30, 298, 45);
            gfx.DrawLine(XPens.Black, 340, 30, 340, 45);
            gfx.DrawLine(XPens.Black, 382, 30, 382, 45);
            gfx.DrawLine(XPens.Black, 424, 30, 424, 45);
            gfx.DrawLine(XPens.Black, 466, 30, 466, 45);
            gfx.DrawLine(XPens.Black, 508, 30, 508, 45);
            gfx.DrawLine(XPens.Black, 550, 30, 550, 45);

            gfx.DrawString(@"Дистанция, [м]", font2, XBrushes.Black,
                        new XRect(62, 25, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №1 по поласам, [см]", font2, XBrushes.Black,
                        new XRect(260, 18, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"1", font2, XBrushes.Black,
                        new XRect(151, 33, page.Width, page.Height),
                       XStringFormats.TopLeft);
            gfx.DrawString(@"2", font2, XBrushes.Black,
                        new XRect(193, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"3", font2, XBrushes.Black,
                        new XRect(235, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"4", font2, XBrushes.Black,
                        new XRect(277, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"5", font2, XBrushes.Black,
                        new XRect(319, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"6", font2, XBrushes.Black,
                        new XRect(361, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"7", font2, XBrushes.Black,
                        new XRect(403, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"8", font2, XBrushes.Black,
                        new XRect(445, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"9", font2, XBrushes.Black,
                        new XRect(487, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"10", font2, XBrushes.Black,
                        new XRect(521, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);

            


            var layBand1 = new List<DensityBand>();
            var layBand2 = new List<DensityBand>();
            var layBand3 = new List<DensityBand>();
            var layBand4 = new List<DensityBand>();
            var layBand5 = new List<DensityBand>();
            var layBand6 = new List<DensityBand>();
            var layBand7 = new List<DensityBand>();
            var layBand8 = new List<DensityBand>();
            var layBand9 = new List<DensityBand>();
            var layBand10 = new List<DensityBand>();

            var layBand = new List<List<DensityBand>>();


            var g = 45;

            var perX = 45;

            

            //var count = new List<int>();
            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
                DensityBand layB;
                switch (_collectionList.GeneralBand[i].Band)
                {
                    case 1:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand1.Add(layB);

                        }
                        layBand.Add(layBand1);
                        break;
                    case 2:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand2.Add(layB);

                        }
                        layBand.Add(layBand2);
                        break;
                    case 3:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand3.Add(layB);

                        }
                        layBand.Add(layBand3);
                        break;
                    case 4:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand4.Add(layB);

                        }
                        layBand.Add(layBand4);
                        break;
                    case 5:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand5.Add(layB);

                        }
                        layBand.Add(layBand5);
                        break;
                    case 6:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand6.Add(layB);

                        }
                        layBand.Add(layBand6);
                        break;
                    case 7:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand7.Add(layB);

                        }
                        layBand.Add(layBand7);
                        break;
                    case 8:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand8.Add(layB);

                        }
                        layBand.Add(layBand8);
                        break;
                    case 9:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand9.Add(layB);

                        }
                        layBand.Add(layBand9);
                        break;
                    case 10:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand10.Add(layB);

                        }
                        layBand.Add(layBand10);
                        break;
                }
            }

            var distanceBand = layBand1;

            for (var i = 0; i < layBand.Count; i++)
            {
                if (distanceBand.Count < layBand[i].Count)
                {
                    distanceBand = layBand[i];
                }
            }




            var h = 48;
            for (var i = 0; i < distanceBand.Count; i++)
            {
                if (h >= 820)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
                    perX = 15;
                    g = 15;
                    h = 18;
                }
                gfx.DrawString(distanceBand[i].Distance.ToString(), font5, XBrushes.Black,
                                   new XRect(62, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                try
                {
                    //колейность 1
                    if (layBand1.Count != 0)
                    {
                        gfx.DrawString(layBand1[i].Parametr.ToString(), font5, XBrushes.Black,
                       new XRect(147, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(147, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }
                   
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft);
                }
                //колейность 2
                try
                {
                    if (layBand2.Count != 0)
                    {

                        gfx.DrawString(layBand2[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(189, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(189, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {

                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 3
                try
                {
                    if (layBand3.Count != 0)
                    {

                        gfx.DrawString(layBand3[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(231, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(231, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 4
                try
                {
                    if (layBand4.Count != 0)
                    {
                        gfx.DrawString(layBand4[i].Parametr.ToString(), font5, XBrushes.Black,
                                     new XRect(273, h, page.Width, page.Height),
                                     XStringFormats.TopLeft); 
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }
                    
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 5
                try
                {
                    if (layBand5.Count != 0)
                    {
                        gfx.DrawString(layBand5[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(315, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(315, h, page.Width, page.Height),
                         XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 6
                try
                {
                    if (layBand6.Count != 0)
                    {
                        gfx.DrawString(layBand6[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(357, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 7
                try
                {
                    if (layBand7.Count != 0)
                    {
                        gfx.DrawString(layBand7[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(399, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 8
                try
                {
                    if (layBand8.Count != 0)
                    {
                        gfx.DrawString(layBand8[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(441, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 9
                try
                {
                    if (layBand9.Count != 0)
                    {
                        gfx.DrawString(layBand9[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(483, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                       new XRect(483, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(483, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 10
                try
                {
                    if (layBand10.Count != 0)
                    {
                        gfx.DrawString(layBand10[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(525, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft); 
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //горизонтальная
                gfx.DrawLine(XPens.Black, 50, g += 15, 550, g);
                //вертикальная
                gfx.DrawLine(XPens.Black, 50, perX, 50, perX + 15);
                gfx.DrawLine(XPens.Black, 130, perX, 130, perX + 15);
                gfx.DrawLine(XPens.Black, 172, perX, 172, perX + 15);
                gfx.DrawLine(XPens.Black, 214, perX, 214, perX + 15);
                gfx.DrawLine(XPens.Black, 256, perX, 256, perX + 15);
                gfx.DrawLine(XPens.Black, 298, perX, 298, perX + 15);
                gfx.DrawLine(XPens.Black, 340, perX, 340, perX + 15);
                gfx.DrawLine(XPens.Black, 382, perX, 382, perX + 15);
                gfx.DrawLine(XPens.Black, 424, perX, 424, perX + 15);
                gfx.DrawLine(XPens.Black, 466, perX, 466, perX + 15);
                gfx.DrawLine(XPens.Black, 508, perX, 508, perX + 15);
                gfx.DrawLine(XPens.Black, 550, perX, 550, perX + 15);
                h += 15;
                perX += 15;

            }


            //таблица толщины слоёв №2
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            //горизонтальная
            gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx.DrawLine(XPens.Black, 50, 45, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 30, 550, 30);
            //вертикальная 
            gfx.DrawLine(XPens.Black, 50, 15, 50, 45);
            gfx.DrawLine(XPens.Black, 550, 15, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 15, 130, 45);
            gfx.DrawLine(XPens.Black, 172, 30, 172, 45);
            gfx.DrawLine(XPens.Black, 214, 30, 214, 45);
            gfx.DrawLine(XPens.Black, 256, 30, 256, 45);
            gfx.DrawLine(XPens.Black, 298, 30, 298, 45);
            gfx.DrawLine(XPens.Black, 340, 30, 340, 45);
            gfx.DrawLine(XPens.Black, 382, 30, 382, 45);
            gfx.DrawLine(XPens.Black, 424, 30, 424, 45);
            gfx.DrawLine(XPens.Black, 466, 30, 466, 45);
            gfx.DrawLine(XPens.Black, 508, 30, 508, 45);
            gfx.DrawLine(XPens.Black, 550, 30, 550, 45);

            gfx.DrawString(@"Дистанция, [м]", font2, XBrushes.Black,
                        new XRect(62, 25, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №2 по поласам, [см]", font2, XBrushes.Black,
                        new XRect(260, 18, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"1", font2, XBrushes.Black,
                        new XRect(151, 33, page.Width, page.Height),
                       XStringFormats.TopLeft);
            gfx.DrawString(@"2", font2, XBrushes.Black,
                        new XRect(193, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"3", font2, XBrushes.Black,
                        new XRect(235, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"4", font2, XBrushes.Black,
                        new XRect(277, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"5", font2, XBrushes.Black,
                        new XRect(319, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"6", font2, XBrushes.Black,
                        new XRect(361, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"7", font2, XBrushes.Black,
                        new XRect(403, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"8", font2, XBrushes.Black,
                        new XRect(445, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"9", font2, XBrushes.Black,
                        new XRect(487, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"10", font2, XBrushes.Black,
                        new XRect(521, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);




            layBand1 = new List<DensityBand>();
            layBand2 = new List<DensityBand>();
            layBand3 = new List<DensityBand>();
            layBand4 = new List<DensityBand>();
            layBand5 = new List<DensityBand>();
            layBand6 = new List<DensityBand>();
            layBand7 = new List<DensityBand>();
            layBand8 = new List<DensityBand>();
            layBand9 = new List<DensityBand>();
            layBand10 = new List<DensityBand>();

            layBand = new List<List<DensityBand>>();


            g = 45;

            perX = 45;



            //var count = new List<int>();
            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
                DensityBand layB;
                switch (_collectionList.GeneralBand[i].Band)
                {
                    case 1:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand1.Add(layB);

                        }
                        layBand.Add(layBand1);
                        break;
                    case 2:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand2.Add(layB);

                        }
                        layBand.Add(layBand2);
                        break;
                    case 3:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand3.Add(layB);

                        }
                        layBand.Add(layBand3);
                        break;
                    case 4:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand4.Add(layB);

                        }
                        layBand.Add(layBand4);
                        break;
                    case 5:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand5.Add(layB);

                        }
                        layBand.Add(layBand5);
                        break;
                    case 6:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand6.Add(layB);

                        }
                        layBand.Add(layBand6);
                        break;
                    case 7:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand7.Add(layB);

                        }
                        layBand.Add(layBand7);
                        break;
                    case 8:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand8.Add(layB);

                        }
                        layBand.Add(layBand8);
                        break;
                    case 9:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand9.Add(layB);

                        }
                        layBand.Add(layBand9);
                        break;
                    case 10:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand10.Add(layB);

                        }
                        layBand.Add(layBand10);
                        break;
                }
            }

            distanceBand = layBand1;

            for (var i = 0; i < layBand.Count; i++)
            {
                if (distanceBand.Count < layBand[i].Count)
                {
                    distanceBand = layBand[i];
                }
            }




            h = 48;
            for (var i = 0; i < distanceBand.Count; i++)
            {
                if (h >= 820)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
                    perX = 15;
                    g = 15;
                    h = 18;
                }
                gfx.DrawString(distanceBand[i].Distance.ToString(), font5, XBrushes.Black,
                                   new XRect(62, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                try
                {
                    //колейность 1
                    if (layBand1.Count != 0)
                    {
                        gfx.DrawString(layBand1[i].Parametr.ToString(), font5, XBrushes.Black,
                       new XRect(147, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(147, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }

                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft);
                }
                //колейность 2
                try
                {
                    if (layBand2.Count != 0)
                    {

                        gfx.DrawString(layBand2[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(189, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(189, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {

                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 3
                try
                {
                    if (layBand3.Count != 0)
                    {

                        gfx.DrawString(layBand3[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(231, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(231, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 4
                try
                {
                    if (layBand4.Count != 0)
                    {
                        gfx.DrawString(layBand4[i].Parametr.ToString(), font5, XBrushes.Black,
                                     new XRect(273, h, page.Width, page.Height),
                                     XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }

                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 5
                try
                {
                    if (layBand5.Count != 0)
                    {
                        gfx.DrawString(layBand5[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(315, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(315, h, page.Width, page.Height),
                         XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 6
                try
                {
                    if (layBand6.Count != 0)
                    {
                        gfx.DrawString(layBand6[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(357, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 7
                try
                {
                    if (layBand7.Count != 0)
                    {
                        gfx.DrawString(layBand7[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(399, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 8
                try
                {
                    if (layBand8.Count != 0)
                    {
                        gfx.DrawString(layBand8[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(441, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 9
                try
                {
                    if (layBand9.Count != 0)
                    {
                        gfx.DrawString(layBand9[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(483, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                       new XRect(483, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(483, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 10
                try
                {
                    if (layBand10.Count != 0)
                    {
                        gfx.DrawString(layBand10[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(525, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //горизонтальная
                gfx.DrawLine(XPens.Black, 50, g += 15, 550, g);
                //вертикальная
                gfx.DrawLine(XPens.Black, 50, perX, 50, perX + 15);
                gfx.DrawLine(XPens.Black, 130, perX, 130, perX + 15);
                gfx.DrawLine(XPens.Black, 172, perX, 172, perX + 15);
                gfx.DrawLine(XPens.Black, 214, perX, 214, perX + 15);
                gfx.DrawLine(XPens.Black, 256, perX, 256, perX + 15);
                gfx.DrawLine(XPens.Black, 298, perX, 298, perX + 15);
                gfx.DrawLine(XPens.Black, 340, perX, 340, perX + 15);
                gfx.DrawLine(XPens.Black, 382, perX, 382, perX + 15);
                gfx.DrawLine(XPens.Black, 424, perX, 424, perX + 15);
                gfx.DrawLine(XPens.Black, 466, perX, 466, perX + 15);
                gfx.DrawLine(XPens.Black, 508, perX, 508, perX + 15);
                gfx.DrawLine(XPens.Black, 550, perX, 550, perX + 15);
                h += 15;
                perX += 15;

            }



            //таблица толщины слоёв №3
            page = document.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            //горизонтальная
            gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx.DrawLine(XPens.Black, 50, 45, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 30, 550, 30);
            //вертикальная 
            gfx.DrawLine(XPens.Black, 50, 15, 50, 45);
            gfx.DrawLine(XPens.Black, 550, 15, 550, 45);
            gfx.DrawLine(XPens.Black, 130, 15, 130, 45);
            gfx.DrawLine(XPens.Black, 172, 30, 172, 45);
            gfx.DrawLine(XPens.Black, 214, 30, 214, 45);
            gfx.DrawLine(XPens.Black, 256, 30, 256, 45);
            gfx.DrawLine(XPens.Black, 298, 30, 298, 45);
            gfx.DrawLine(XPens.Black, 340, 30, 340, 45);
            gfx.DrawLine(XPens.Black, 382, 30, 382, 45);
            gfx.DrawLine(XPens.Black, 424, 30, 424, 45);
            gfx.DrawLine(XPens.Black, 466, 30, 466, 45);
            gfx.DrawLine(XPens.Black, 508, 30, 508, 45);
            gfx.DrawLine(XPens.Black, 550, 30, 550, 45);

            gfx.DrawString(@"Дистанция, [м]", font2, XBrushes.Black,
                        new XRect(62, 25, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №3 по поласам, [см]", font2, XBrushes.Black,
                        new XRect(260, 18, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"1", font2, XBrushes.Black,
                        new XRect(151, 33, page.Width, page.Height),
                       XStringFormats.TopLeft);
            gfx.DrawString(@"2", font2, XBrushes.Black,
                        new XRect(193, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"3", font2, XBrushes.Black,
                        new XRect(235, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"4", font2, XBrushes.Black,
                        new XRect(277, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"5", font2, XBrushes.Black,
                        new XRect(319, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"6", font2, XBrushes.Black,
                        new XRect(361, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"7", font2, XBrushes.Black,
                        new XRect(403, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"8", font2, XBrushes.Black,
                        new XRect(445, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"9", font2, XBrushes.Black,
                        new XRect(487, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);
            gfx.DrawString(@"10", font2, XBrushes.Black,
                        new XRect(521, 33, page.Width, page.Height),
                        XStringFormats.TopLeft);


            layBand1 = new List<DensityBand>();
            layBand2 = new List<DensityBand>();
            layBand3 = new List<DensityBand>();
            layBand4 = new List<DensityBand>();
            layBand5 = new List<DensityBand>();
            layBand6 = new List<DensityBand>();
            layBand7 = new List<DensityBand>();
            layBand8 = new List<DensityBand>();
            layBand9 = new List<DensityBand>();
            layBand10 = new List<DensityBand>();

            layBand = new List<List<DensityBand>>();


            g = 45;

            perX = 45;



            //var count = new List<int>();
            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
                DensityBand layB;
                switch (_collectionList.GeneralBand[i].Band)
                {
                    case 1:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand1.Add(layB);

                        }
                        layBand.Add(layBand1);
                        break;
                    case 2:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand2.Add(layB);

                        }
                        layBand.Add(layBand2);
                        break;
                    case 3:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand3.Add(layB);

                        }
                        layBand.Add(layBand3);
                        break;
                    case 4:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand4.Add(layB);

                        }
                        layBand.Add(layBand4);
                        break;
                    case 5:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand5.Add(layB);

                        }
                        layBand.Add(layBand5);
                        break;
                    case 6:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand6.Add(layB);

                        }
                        layBand.Add(layBand6);
                        break;
                    case 7:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand7.Add(layB);

                        }
                        layBand.Add(layBand7);
                        break;
                    case 8:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand8.Add(layB);

                        }
                        layBand.Add(layBand8);
                        break;
                    case 9:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand9.Add(layB);

                        }
                        layBand.Add(layBand9);
                        break;
                    case 10:
                        for (int j = 0; j < _collectionList.Layer1Band[i].GraphicsesCollection.Count; j++)
                        {
                            layB = new DensityBand();
                            layB.Distance = _collectionList.Layer1Band[i].GraphicsesCollection[j].Distance;
                            layB.Parametr = _collectionList.Layer1Band[i].GraphicsesCollection[j].Layer1;
                            layBand10.Add(layB);

                        }
                        layBand.Add(layBand10);
                        break;
                }
            }

            distanceBand = layBand1;

            for (var i = 0; i < layBand.Count; i++)
            {
                if (distanceBand.Count < layBand[i].Count)
                {
                    distanceBand = layBand[i];
                }
            }




            h = 48;
            for (var i = 0; i < distanceBand.Count; i++)
            {
                if (h >= 820)
                {
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
                    perX = 15;
                    g = 15;
                    h = 18;
                }
                gfx.DrawString(distanceBand[i].Distance.ToString(), font5, XBrushes.Black,
                                   new XRect(62, h, page.Width, page.Height),
                                   XStringFormats.TopLeft);
                try
                {
                    //колейность 1
                    if (layBand1.Count != 0)
                    {
                        gfx.DrawString(layBand1[i].Parametr.ToString(), font5, XBrushes.Black,
                       new XRect(147, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(147, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }

                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                                new XRect(147, h, page.Width, page.Height),
                                XStringFormats.TopLeft);
                }
                //колейность 2
                try
                {
                    if (layBand2.Count != 0)
                    {

                        gfx.DrawString(layBand2[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(189, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(189, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {

                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(189, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 3
                try
                {
                    if (layBand3.Count != 0)
                    {

                        gfx.DrawString(layBand3[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(231, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                              new XRect(231, h, page.Width, page.Height),
                              XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(231, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 4
                try
                {
                    if (layBand4.Count != 0)
                    {
                        gfx.DrawString(layBand4[i].Parametr.ToString(), font5, XBrushes.Black,
                                     new XRect(273, h, page.Width, page.Height),
                                     XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                    }

                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                               new XRect(273, h, page.Width, page.Height),
                               XStringFormats.TopLeft);
                }
                //колейность 5
                try
                {
                    if (layBand5.Count != 0)
                    {
                        gfx.DrawString(layBand5[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(315, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                         new XRect(315, h, page.Width, page.Height),
                         XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(315, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 6
                try
                {
                    if (layBand6.Count != 0)
                    {
                        gfx.DrawString(layBand6[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(357, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(357, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 7
                try
                {
                    if (layBand7.Count != 0)
                    {
                        gfx.DrawString(layBand7[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(399, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(399, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 8
                try
                {
                    if (layBand8.Count != 0)
                    {
                        gfx.DrawString(layBand8[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(441, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(441, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 9
                try
                {
                    if (layBand9.Count != 0)
                    {
                        gfx.DrawString(layBand9[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(483, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                       new XRect(483, h, page.Width, page.Height),
                       XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(483, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //плотность 10
                try
                {
                    if (layBand10.Count != 0)
                    {
                        gfx.DrawString(layBand10[i].Parametr.ToString(), font5, XBrushes.Black,
                            new XRect(525, h, page.Width, page.Height),
                            XStringFormats.TopLeft);
                    }
                    else
                    {
                        gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                    }
                }
                catch (Exception)
                {
                    gfx.DrawString("—", font5, XBrushes.Black,
                        new XRect(525, h, page.Width, page.Height),
                        XStringFormats.TopLeft);
                }
                //горизонтальная
                gfx.DrawLine(XPens.Black, 50, g += 15, 550, g);
                //вертикальная
                gfx.DrawLine(XPens.Black, 50, perX, 50, perX + 15);
                gfx.DrawLine(XPens.Black, 130, perX, 130, perX + 15);
                gfx.DrawLine(XPens.Black, 172, perX, 172, perX + 15);
                gfx.DrawLine(XPens.Black, 214, perX, 214, perX + 15);
                gfx.DrawLine(XPens.Black, 256, perX, 256, perX + 15);
                gfx.DrawLine(XPens.Black, 298, perX, 298, perX + 15);
                gfx.DrawLine(XPens.Black, 340, perX, 340, perX + 15);
                gfx.DrawLine(XPens.Black, 382, perX, 382, perX + 15);
                gfx.DrawLine(XPens.Black, 424, perX, 424, perX + 15);
                gfx.DrawLine(XPens.Black, 466, perX, 466, perX + 15);
                gfx.DrawLine(XPens.Black, 508, perX, 508, perX + 15);
                gfx.DrawLine(XPens.Black, 550, perX, 550, perX + 15);
                h += 15;
                perX += 15;

            }


 
        }
        /// <summary>
        /// Кнопка отчёта
        /// </summary>
        /// <param name="bandsAreas"></param>
        private void Button(ChartData bandsAreas)
        {
            _widthScrollGeneral = GeneralGraff.Width;

            GeneralGraff.Width = EndWidth;
            Messenger.Default.Send(true, "startWidthGeneral");
            argument = true;
            x = 80;
            _recX = 80;
            rec = 86;
            recA = 80.5;
           z = 20;
            q = 19.5;
            Grid = new Grid();
            var k = 1;
            foreach (var area in bandsAreas.CountBand)
            {
                //блоч комп
                CreatGrid(area.GraphicsesCollection, "CountBand", k);
                k++;
            }
          
            k = 1;
            foreach (var area in bandsAreas.DensityBand)
            {
                CreatGrid(area.GraphicsesCollection, "DensityBand", k);
                k++;
            }
            k = 1;
            foreach (var area in bandsAreas.Layer1Band)
            {
                CreatGrid(area.GraphicsesCollection, "Layer1Band", k);
                k++;
            }
            k = 1;
            foreach (var area in bandsAreas.Layer2Band)
            {
                CreatGrid(area.GraphicsesCollection, "Layer2Band", k);
                k++;
            }
            k = 1;
            foreach (var area in bandsAreas.Layer3Band)
            {
                CreatGrid(area.GraphicsesCollection, "Layer3Band", k);
                k++;
            }

            foreach (var area in bandsAreas.GeneralBand)
            {
                CreatGrid(area.GraphicsesCollection, "GeneralBand", k);
            }
            k = 1;
            foreach (var area in bandsAreas.AllLayerBand)
            {
                CreatGrid(area.GraphicsesCollection, "AllLayerBand", k);
                k++;
            }
            k = 1;
            foreach (var area in bandsAreas.RutBand)
            {
                CreatGrid(area.GraphicsesCollection, "RutBand", k);
                k++;
            }
            LayoutRoot.Children.Add(Grid);
            Grid.SetRow(Grid, 2);
            Grid.SetColumn(Grid, 1);
            Grid.SetColumnSpan(Grid,2);
          
            
            var grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromRgb(3, 94, 129));
            var text = new TextBlock();
            text.Text = "Формирование отчёта";
            text.FontSize = 19;
            text.Foreground = Brushes.White;
            text.HorizontalAlignment = HorizontalAlignment.Center;
            text.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(text);
            LayoutRoot.Children.Add(grid);
            Grid.SetRow(grid, 2);
            Grid.SetRowSpan(grid, 5);
            Grid.SetColumnSpan(grid, 5);
            //Gen
            GeneralGraff.DrawGraphic(new ObservableCollection<CollectionGraphs>(_collorCollection), collor = false, _band);
            TextBlockvG.Foreground = Brushes.Black;
            TextBlock1G.Foreground = Brushes.Black;
            TextBlock2G.Foreground = Brushes.Black;
            TextBlockdG.Foreground = Brushes.Black;
            Grid.SetRowSpan(GridGen, 3);
            var imageGen = Path.GetTempPath() + "general.png";
            
            
            //ReportBegin(null, null, TextBlockvG, TextBlockdG);
           


            #region MainList
            // Create a new PDF document
            var document = new PdfDocument();
            // Create an empty page
            var page = document.AddPage();
            

            // Get an XGraphics object for drawing
            var gfx = XGraphics.FromPdfPage(page);
            //var gfx2 = XGraphics.FromPdfPage(page2);
            var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            // Create a font
            var font = new XFont("Times New Roman", 12, XFontStyle.BoldItalic, options);
            var font1 = new XFont("Times New Roman", 12, XFontStyle.Bold, options);
            var font2 = new XFont("Times New Roman", 9, XFontStyle.Bold, options);
            var font3 = new XFont("Times New Roman", 14, XFontStyle.Bold, options);
            var font4 = new XFont("Times New Roman", 8, XFontStyle.Bold, options);
            var font5 = new XFont("Times New Roman", 9, XFontStyle.Regular, options);
            var font6 = new XFont("Times New Roman", 9, XFontStyle.Italic, options);
            var font7 = new XFont("Times New Roman", 5, XFontStyle.Regular, options);
            var font8 = new XFont("Times New Roman", 12, XFontStyle.Regular, options);
            var font9 = new XFont("Times New Roman", 8, XFontStyle.Bold, options);


            //Шапка страницы
            //Горизонтальная
            gfx.DrawLine(XPens.Black, 50, 15, 550, 15);
            gfx.DrawLine(XPens.Black, 150, 60, 450, 60);
            gfx.DrawLine(XPens.Black, 50, 105, 550, 105);
            gfx.DrawLine(XPens.Black, 450, 80, 550, 80);
            //Вертикальная
            gfx.DrawLine(XPens.Black, 50, 15, 50, 105);
            gfx.DrawLine(XPens.Black, 550, 15, 550, 105);
            gfx.DrawLine(XPens.Black, 150, 15, 150, 105);
            gfx.DrawLine(XPens.Black, 450, 15, 450, 105);
            gfx.DrawLine(XPens.Black, 500, 80, 500, 105);
            //Информация об измерениях
            //Таблица
            //Горизонтальная
            gfx.DrawLine(XPens.Black, 50, 120, 550, 120);
            gfx.DrawLine(XPens.Black, 50, 135, 550, 135);
            gfx.DrawLine(XPens.Black, 50, 150, 550, 150);
            gfx.DrawLine(XPens.Black, 50, 165, 550, 165);
            
            //Вертикальная
            gfx.DrawLine(XPens.Black, 50, 120, 50, 165);
            gfx.DrawLine(XPens.Black, 550, 120, 550, 165);
            gfx.DrawLine(XPens.Black, 230, 120, 230, 165);
            
            //Текст в шапке
            gfx.DrawString(@"Измерительный комплекс", font, XBrushes.Black,
                new XRect(5, 20, page.Width, page.Height),
                XStringFormats.TopCenter);
            gfx.DrawString("\"Кондор\"", font, XBrushes.Black,
                new XRect(5, 40, page.Width, page.Height),
                XStringFormats.TopCenter);
            gfx.DrawString(@"ПРОТОКОЛ", font3, XBrushes.Black,
               new XRect(5, 65, page.Width, page.Height),
               XStringFormats.TopCenter);
            gfx.DrawString(@"измерения качественных характеристик", font1, XBrushes.Black,
               new XRect(5, 78, page.Width, page.Height),
               XStringFormats.TopCenter);
            gfx.DrawString(@"дорожного покрытия", font1, XBrushes.Black,
               new XRect(5, 90, page.Width, page.Height),
               XStringFormats.TopCenter);
            //Номер отчёта
            gfx.DrawString(@"ПИ - " + _numberMess, font1, XBrushes.Black,
               new XRect(473, 40, page.Width, page.Height),
               XStringFormats.TopLeft);
           

            //Текст информации о дороге
            gfx.DrawString(@"Заказчик испытания", font2, XBrushes.Black,
                new XRect(55, 122, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.Customer, font5, XBrushes.Black,
                new XRect(235, 122, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Место измерения", font2, XBrushes.Black,
                new XRect(55, 138, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.RoadName +" " + _showInfo.AllRoad, font5, XBrushes.Black,
                new XRect(235, 138, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Дата измерения", font2, XBrushes.Black,
                new XRect(55, 153, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString("c " + _showInfo.TimeStart + " по " + _dateEnd, font5, XBrushes.Black,
                new XRect(235, 153, page.Width, page.Height),
                XStringFormats.TopLeft);
            //Имя таблицы №3
            gfx.DrawString(@"Среднее значение качественных характеристик дорожного покрытия", font1, XBrushes.Black,
               new XRect(0, 170, page.Width, page.Height),
               XStringFormats.TopCenter);
            //Горизонтальная таблица №3
            gfx.DrawLine(XPens.Black, 50, 185, 550, 185);
            gfx.DrawLine(XPens.Black, 50, 215, 550, 215);
            gfx.DrawLine(XPens.Black, 170, 199, 450, 199);
            gfx.DrawLine(XPens.Black, 50, 229, 550, 229);
            gfx.DrawLine(XPens.Black, 50, 243, 550, 243);
            gfx.DrawLine(XPens.Black, 50, 257, 550, 257);
            gfx.DrawLine(XPens.Black, 50, 271, 550, 271);
            gfx.DrawLine(XPens.Black, 50, 285, 550, 285);
            gfx.DrawLine(XPens.Black, 50, 299, 550, 299);
            gfx.DrawLine(XPens.Black, 50, 313, 550, 313);
            

            //Вертикальные
            gfx.DrawLine(XPens.Black, 50, 185, 50, 313);
            gfx.DrawLine(XPens.Black, 142, 185, 142, 313);
            gfx.DrawLine(XPens.Black, 170, 185, 170, 313);
            gfx.DrawLine(XPens.Black, 198, 199, 198,313);
            gfx.DrawLine(XPens.Black, 226, 199, 226, 313);
            gfx.DrawLine(XPens.Black, 254, 199, 254, 313);
            gfx.DrawLine(XPens.Black, 282, 199, 282, 313);
            gfx.DrawLine(XPens.Black, 310, 199, 310, 313);
            gfx.DrawLine(XPens.Black, 338, 199, 338, 313);
            gfx.DrawLine(XPens.Black, 366, 199, 366, 313);
            gfx.DrawLine(XPens.Black, 394, 199, 394, 313);
            gfx.DrawLine(XPens.Black, 422, 199, 422, 313);
            gfx.DrawLine(XPens.Black, 450, 185, 450, 313);
            gfx.DrawLine(XPens.Black, 478, 185, 478, 313);
            gfx.DrawLine(XPens.Black, 514, 185, 514, 313);
            gfx.DrawLine(XPens.Black, 550, 185, 550, 313);
            //Плотность
            double density1 = 0;
            double density2 = 0;
            double density3 = 0;
            double density4 = 0;
            double density5 = 0;
            double density6 = 0;
            double density7 = 0;
            double density8 = 0;
            double density9 = 0;
            double density10 = 0;
            double middleD = 0;
            //Количество слоёв
            var count1 = 0;
            var count2 = 0;
            var count3 = 0;
            var count4 = 0;
            var count5 = 0;
            var count6 = 0;
            var count7 = 0;
            var count8 = 0;
            var count9 = 0;
            var count10 = 0;
            var middleC = 0;
            //Толщина слоя 1
            double band1L1= 0;
            double band2L1 = 0;
            double band3L1 = 0;
            double band4L1 = 0;
            double band5L1 = 0;
            double band6L1 = 0;
            double band7L1 = 0;
            double band8L1 = 0;
            double band9L1 = 0;
            double band10L1 = 0;
            double middleL1 = 0;
            //Толщина слоя 2
            double band1L2 = 0;
            double band2L2 = 0;
            double band3L2 = 0;
            double band4L2 = 0;
            double band5L2 = 0;
            double band6L2 = 0;
            double band7L2 = 0;
            double band8L2 = 0;
            double band9L2 = 0;
            double band10L2 = 0;
            double middleL2 = 0;
            //Толщина слоя 3
            double band1L3 = 0;
            double band2L3 = 0;
            double band3L3 = 0;
            double band4L3 = 0;
            double band5L3 = 0;
            double band6L3 = 0;
            double band7L3 = 0;
            double band8L3 = 0;
            double band9L3 = 0;
            double band10L3 = 0;
            double middleL3 = 0;
            //Колейность
            double rut1 = 0;
            double rut2 = 0;
            double rut3 = 0;
            double rut4 = 0;
            double rut5 = 0;
            double rut6 = 0;
            double rut7 = 0;
            double rut8 = 0;
            double rut9 = 0;
            double rut10 = 0;
            double middleR = 0;
            //IRI
            double iri1 = 0;
            double iri2 = 0;
            double iri3 = 0;
            double iri4 = 0;
            double iri5 = 0;
            double iri6 = 0;
            double iri7 = 0;
            double iri8 = 0;
            double iri9 = 0;
            double iri10 = 0;
            double middleI = 0;
            
           

            for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
            {
                switch (_collectionList.GeneralBand[i].Band)
                {
                    case 1:
                        density1 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count1 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band1L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band1L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band1L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        rut1 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        iri1 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri1 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        rut1 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        density1 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count1 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band1L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band1L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band1L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        
                        break;
                    case 2:
                        density2 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count2 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band2L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band2L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band2L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density2 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count2 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band2L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band2L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band2L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        rut2 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut2 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri2 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri2 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 3:
                        density3 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count3 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band3L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band3L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band3L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density3 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count3 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band3L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band3L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band3L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        rut3 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut3 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri3 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri3 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 4:
                        density4 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count4 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band4L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band4L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band4L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density4 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count4 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band4L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band4L2 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band4L3 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        rut4 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut4 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri4 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri4 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 5:
                        density5 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count5 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band5L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band5L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band5L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density5 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count5 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band5L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band5L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band5L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                         rut5 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut5 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri5 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri5 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 6:
                        density6 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count6 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band6L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band6L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band6L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density6 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count6 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band6L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band6L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band6L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        rut6 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut6 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri6 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri6 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 7:
                        density7 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count7 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band7L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band7L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band7L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density7 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count7 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band7L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band7L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band7L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        rut7 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut7 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri7 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri7 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 8:
                        density8 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count8 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band8L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band8L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band8L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density8 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count8 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band8L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band8L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band8L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                         rut8 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut8 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri8 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri8 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 9:
                        density9 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count9 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band9L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band9L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band9L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density9 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count9 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band9L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band9L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band9L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                        rut9 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut9 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri9 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri9 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                    case 10:
                        density10 += _collectionList.DensityBand[i].GraphicsesCollection.Sum(graghse => graghse.IntensityN1);
                        count10 += _collectionList.CountBand[i].GraphicsesCollection.Sum(graghse => graghse.CountLayer);
                        band10L1 += _collectionList.Layer1Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer1);
                        band10L2 += _collectionList.Layer2Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer2);
                        band10L3 += _collectionList.Layer3Band[i].GraphicsesCollection.Sum(graghse => graghse.Layer3);
                        density10 /= _collectionList.DensityBand[i].GraphicsesCollection.Count;
                        count10 /= _collectionList.CountBand[i].GraphicsesCollection.Count;
                        band10L1 /= _collectionList.Layer1Band[i].GraphicsesCollection.Count;
                        band10L2 /= _collectionList.Layer2Band[i].GraphicsesCollection.Count;
                        band10L3 /= _collectionList.Layer3Band[i].GraphicsesCollection.Count;
                         rut10 += _collectionList.RutBand[i].GraphicsesCollection.Sum(graghse => graghse.Rut);
                        rut10 /= _collectionList.RutBand[i].GraphicsesCollection.Count;
                        iri10 += _collectionList.IRIBand[i].GraphicsesCollection.Sum(graghse => graghse.IRI);
                        iri10 /= _collectionList.IRIBand[i].GraphicsesCollection.Count;
                        break;
                }

                
            }

            
           
            var dens1 = "—";
            var dens2 = "—";
            var dens3 = "—";
            var dens4 = "—";
            var dens5 = "—";
            var dens6 = "—";
            var dens7 = "—";
            var dens8 = "—";
            var dens9 = "—";
            var dens10 = "—";
            
            //Слои
            var ct1 = "—";
            var ct2 = "—";
            var ct3 = "—";
            var ct4 = "—";
            var ct5 = "—";
            var ct6 = "—";
            var ct7 = "—";
            var ct8 = "—";
            var ct9 = "—";
            var ct10 = "—";
            
            //Слои1
            var b1l1 = "—";
            var b2l1 = "—";
            var b3l1 = "—";
            var b4l1 = "—";
            var b5l1 = "—";
            var b6l1 = "—";
            var b7l1 = "—";
            var b8l1 = "—";
            var b9l1 = "—";
            var b10l1 = "—";
            
            //Слой2
            var b1l2 = "—";
            var b2l2 = "—";
            var b3l2 = "—";
            var b4l2 = "—";
            var b5l2 = "—";
            var b6l2 = "—";
            var b7l2 = "—";
            var b8l2 = "—";
            var b9l2 = "—";
            var b10l2 = "—";
            
            //Слой3
            var b1L3 = "—";
            var b2L3 = "—";
            var b3L3 = "—";
            var b4L3 = "—";
            var b5L3 = "—";
            var b6L3 = "—";
            var b7L3 = "—";
            var b8L3 = "—";
            var b9L3 = "—";
            var b10L3 = "—";
            //Колейность
            var r1 = "—";
            var r2 = "—";
            var r3 = "—";
            var r4 = "—";
            var r5 = "—";
            var r6 = "—";
            var r7 = "—";
            var r8 = "—";
            var r9 = "—";
            var r10 = "—";
            //Колейность
            var i1 = "—";
            var i2 = "—";
            var i3 = "—";
            var i4 = "—";
            var i5 = "—";
            var i6 = "—";
            var i7 = "—";
            var i8 = "—";
            var i9 = "—";
            var i10 = "—";
            
            if (density1 != 0.0)
            {
                //density1 /= middle;
                //count1 /= middle;
                dens1 = density1.ToString("#.##");
                ct1 = count1.ToString();
                b1l1 = band1L1.ToString("#.##");
                b1l2 = band1L2.ToString("#.##");
                b1L3 = band1L3.ToString("#.##");
                r1 = rut1.ToString("#.##");
                i1 = iri1.ToString("#.##");

            }
            if (density2 != 0.0)
            {
                dens2 = density2.ToString("#.##");
                ct2 = count2.ToString();
                b2l1 = band2L1.ToString("#.##");
                b2l2 = band2L2.ToString("#.##");
                b2L3 = band2L3.ToString("#.##");
                r2 = rut2.ToString("#.##");
                i2 = iri2.ToString("#.##");
            }
            if (density3 != 0.0)
            {
                dens3 = density3.ToString("#.##");
                ct3 = count3.ToString();
                b3l1 = band3L1.ToString("#.##");
                b3l2 = band3L2.ToString("#.##");
                b3L3 = band3L3.ToString("#.##");
                r3 = rut3.ToString("#.##");
                i3 = iri3.ToString("#.##");
            }
            if (density4 != 0.0)
            {
                dens4 = density4.ToString("#.##");
                ct4 = count4.ToString();
                b4l1 = band4L1.ToString("#.##");
                b4l2 = band4L2.ToString("#.##");
                b4L3 = band4L3.ToString("#.##");
                r4 = rut4.ToString("#.##");
                i4 = iri4.ToString("#.##");
            }

            if (density5 != 0.0)
            {
                dens5 = density5.ToString("#.##");
                ct5 = count5.ToString();
                b5l1 = band5L1.ToString("#.##");
                b5l2 = band5L2.ToString("#.##");
                b5L3 = band5L3.ToString("#.##");
                r5 = rut5.ToString("#.##");
                i5 = iri5.ToString("#.##");
            }
            if (density6 != 0.0)
            {
                dens6 = density6.ToString("#.##");
                ct6 = count6.ToString();
                b6l1 = band6L1.ToString("#.##");
                b6l2 = band6L2.ToString("#.##");
                b6L3 = band6L3.ToString("#.##");
                r6 = rut6.ToString("#.##");
                i6 = iri6.ToString("#.##");
            }
            if (density7 != 0.0)
            {
                dens7 = density7.ToString("#.##");
                ct7 = count7.ToString();
                b7l1 = band7L1.ToString("#.##");
                b7l2 = band7L2.ToString("#.##");
                b7L3 = band7L3.ToString("#.##");
                r7 = rut7.ToString("#.##");
                i7 = iri7.ToString("#.##");
            }
            if (density8 != 0.0)
            {
                dens8 = density8.ToString("#.##");
                ct8 = count8.ToString();
                b8l1 = band8L1.ToString("#.##");
                b8l2 = band8L2.ToString("#.##");
                b8L3 = band8L3.ToString("#.##");
                r8 = rut8.ToString("#.##"); 
                i8 = iri8.ToString("#.##");
            }
            if (density9 != 0.0)
            {
                dens9 = density9.ToString("#.##");
                ct9 = count9.ToString();
                b9l1 = band9L1.ToString("#.##");
                b9l2 = band9L2.ToString("#.##");
                b9L3 = band9L3.ToString("#.##");
                r9 = rut9.ToString("#.##");
                i9 = iri9.ToString("#.##");
            }
            if (density10 != 0.0)
            {
                dens10 = density10.ToString("#.##");
                ct10 = count10.ToString();
                b10l1 = band10L1.ToString("#.##");
                b10l2 = band10L2.ToString("#.##");
                b10L3 = band10L3.ToString("#.##");
                r10 = rut10.ToString("#.##");
                i10 = iri10.ToString("#.##");
            }
            //Среднее значение
            middleD = density1 + density2 + density3 + density4 + density5 + density6 + density7 + density8 + density9 +
                 density10;
            middleD /= _collectionList.GeneralBand.Count;
            middleC = count1 + count2 + count3 + count4 + count5 + count6 + count7 + count8 + count9 + count10;
            middleC /= _collectionList.GeneralBand.Count;
            middleL1 = band1L1 + band2L1 + band3L1 + band4L1 + band5L1 + band6L1 + band7L1 + band8L1 + band9L1 + band10L1;
            middleL1 /= _collectionList.GeneralBand.Count;
            middleL2 = band1L2 + band2L2 + band3L2 + band4L2 + band5L2 + band6L2 + band7L2 + band8L2 + band9L2 + band10L2;
            middleL2 /= _collectionList.GeneralBand.Count;
            middleL3 = band1L3 + band2L3 + band3L3 + band4L3 + band5L3 + band6L3 + band7L3 + band8L3 + band9L3 + band10L3;
            middleL3 /= _collectionList.GeneralBand.Count;
            middleR = rut1 + rut2 + rut3 + rut4 + rut5 + rut6 + rut7 + rut8 + rut9 + rut10;
            middleR /= _collectionList.GeneralBand.Count;
            middleI = iri1 + iri2 + iri3 + iri4 + iri5 + iri6 + iri7 + iri8 + iri9 + iri10;
            middleI /= _collectionList.GeneralBand.Count;


            //Поля таблицы№3
            gfx.DrawString(@"Наимнование", font2, XBrushes.Black,
                new XRect(55, 188, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"показателя", font2, XBrushes.Black,
                new XRect(55, 198, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Ед.", font2, XBrushes.Black,
                new XRect(150, 188, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"изм.", font2, XBrushes.Black,
                new XRect(148, 198, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"кг/м", font5, XBrushes.Black,
                new XRect(148, 218, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"шт", font5, XBrushes.Black,
               new XRect(148, 232, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(@"см", font5, XBrushes.Black,
                new XRect(148, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"см", font5, XBrushes.Black,
                new XRect(148, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"см", font5, XBrushes.Black,
                new XRect(148, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"—", font5, XBrushes.Black,
                new XRect(148, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"мм", font5, XBrushes.Black,
                new XRect(148, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"3", font7, XBrushes.Black,
                new XRect(165, 217, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Среднее значение по полосе", font2, XBrushes.Black,
                new XRect(220, 188, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"1", font2, XBrushes.Black,
                new XRect(182, 203, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(dens1, font5, XBrushes.Black,
                new XRect(174, 218, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(ct1, font5, XBrushes.Black,
                new XRect(174, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b1l1, font5, XBrushes.Black,
                new XRect(174, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b1l2, font5, XBrushes.Black,
                new XRect(174, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b1L3, font5, XBrushes.Black,
                new XRect(174, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(r1, font5, XBrushes.Black,
                new XRect(174, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(i1, font5, XBrushes.Black,
                new XRect(174, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"2", font2, XBrushes.Black,
                 new XRect(210, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens2, font5, XBrushes.Black,
                new XRect(206, 218, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(ct2, font5, XBrushes.Black,
                new XRect(206, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b2l1, font5, XBrushes.Black,
                new XRect(203, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b2l2, font5, XBrushes.Black,
                new XRect(203, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b2L3, font5, XBrushes.Black,
                new XRect(203, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(r2, font5, XBrushes.Black,
                new XRect(203, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(i2, font5, XBrushes.Black,
                new XRect(203, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"3", font2, XBrushes.Black,
                 new XRect(238, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens3, font5, XBrushes.Black,
                 new XRect(232, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct3, font5, XBrushes.Black,
                 new XRect(232, 232, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b3l1, font5, XBrushes.Black,
                 new XRect(232, 246, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b3l2, font5, XBrushes.Black,
                 new XRect(232, 260, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b3L3, font5, XBrushes.Black,
                 new XRect(232, 274, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(r3, font5, XBrushes.Black,
                 new XRect(232, 288, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(i3, font5, XBrushes.Black,
                 new XRect(232, 302, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(@"4", font2, XBrushes.Black,
                 new XRect(266, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens4, font5, XBrushes.Black,
                 new XRect(260, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct4, font5, XBrushes.Black,
                 new XRect(260, 232, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b4l1, font5, XBrushes.Black,
                 new XRect(260, 246, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b4l2, font5, XBrushes.Black,
                 new XRect(260, 260, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b4L3, font5, XBrushes.Black,
                 new XRect(260, 274, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(r4, font5, XBrushes.Black,
                 new XRect(260, 288, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(i4, font5, XBrushes.Black,
                 new XRect(260, 302, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(@"5", font2, XBrushes.Black,
                 new XRect(294, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens5, font5, XBrushes.Black,
                 new XRect(290, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct5, font5, XBrushes.Black,
                 new XRect(290, 232, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b5l1, font5, XBrushes.Black,
                 new XRect(290, 246, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b5l2, font5, XBrushes.Black,
                 new XRect(290, 260, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b5L3, font5, XBrushes.Black,
                 new XRect(290, 274, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(r5, font5, XBrushes.Black,
                 new XRect(290, 288, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(i5, font5, XBrushes.Black,
                 new XRect(290, 302, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(@"6", font2, XBrushes.Black,
                 new XRect(322, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens6, font5, XBrushes.Black,
                 new XRect(318, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct6, font5, XBrushes.Black,
                 new XRect(318, 232, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b6l1, font5, XBrushes.Black,
                 new XRect(318, 246, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b6l2, font5, XBrushes.Black,
                 new XRect(318, 260, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b6L3, font5, XBrushes.Black,
                 new XRect(318, 274, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(r6, font5, XBrushes.Black,
                 new XRect(318, 288, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(i6, font5, XBrushes.Black,
                 new XRect(318, 302, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(@"7", font2, XBrushes.Black,
                 new XRect(350, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens7, font5, XBrushes.Black,
                 new XRect(346, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct7, font5, XBrushes.Black,
                new XRect(346, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b7l1, font5, XBrushes.Black,
               new XRect(346, 246, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(b7l2, font5, XBrushes.Black,
               new XRect(346, 260, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(b7L3, font5, XBrushes.Black,
               new XRect(346, 274, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(r7, font5, XBrushes.Black,
               new XRect(346, 288, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(i7, font5, XBrushes.Black,
               new XRect(346, 302, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(@"8", font2, XBrushes.Black,
                 new XRect(378, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens8, font5, XBrushes.Black,
                 new XRect(374, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct8, font5, XBrushes.Black,
                new XRect(374, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b8l1, font5, XBrushes.Black,
                new XRect(374, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b8l2, font5, XBrushes.Black,
                new XRect(374, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b8L3, font5, XBrushes.Black,
                new XRect(374, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(r8, font5, XBrushes.Black,
                new XRect(374, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(i8, font5, XBrushes.Black,
                new XRect(374, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"9", font2, XBrushes.Black,
                 new XRect(406, 204, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens9, font5, XBrushes.Black,
                 new XRect(400, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct9, font5, XBrushes.Black,
                 new XRect(400, 232, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b9l1, font5, XBrushes.Black,
                 new XRect(400, 246, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b9l2, font5, XBrushes.Black,
                 new XRect(400, 260, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(b9L3, font5, XBrushes.Black,
                 new XRect(400, 274, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(r9, font5, XBrushes.Black,
                 new XRect(400, 288, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(i9, font5, XBrushes.Black,
                 new XRect(400, 302, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(@"10", font2, XBrushes.Black,
                 new XRect(432, 203, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(dens10, font5, XBrushes.Black,
                 new XRect(428, 218, page.Width, page.Height),
                 XStringFormats.TopLeft);
            gfx.DrawString(ct10, font5, XBrushes.Black,
                new XRect(428, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b10l1, font5, XBrushes.Black,
                new XRect(428, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b10l2, font5, XBrushes.Black,
                new XRect(428, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(b10L3, font5, XBrushes.Black,
                new XRect(428, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(r10, font5, XBrushes.Black,
                new XRect(428, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(i10, font5, XBrushes.Black,
                new XRect(428, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Ср.", font2, XBrushes.Black,
                new XRect(458, 188, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"знач.", font2, XBrushes.Black,
                new XRect(455, 198, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleD.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 218, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleC.ToString(), font5, XBrushes.Black,
                new XRect(455, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleL1.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleL2.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleL3.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleR.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(middleI.ToString("#.##"), font5, XBrushes.Black,
                new XRect(455, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Треб-е", font4, XBrushes.Black,
                new XRect(483, 186, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"по", font4, XBrushes.Black,
                new XRect(486, 195, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"проекту", font4, XBrushes.Black,
               new XRect(482, 205, page.Width, page.Height),
               XStringFormats.TopLeft);
            //Standarts
            gfx.DrawString(_showInfo.StandartsDensity.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 218, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.DesignCount.ToString(), font5, XBrushes.Black,
              new XRect(482, 232, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.DesignLayer1.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 246, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.DesignLayer2.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 260, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.DesignLayer3.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 274, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.StandartsRut.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 288, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(_showInfo.StandartsIRI.ToString("#.##"), font5, XBrushes.Black,
              new XRect(482, 302, page.Width, page.Height),
              XStringFormats.TopLeft);
            var gostDensity = "нет";
            var gostCount = "нет";
            var gostLayer1 = "нет";
            var gostLayer2 = "нет";
            var gostLayer3 = "нет";
            var gostRut = "нет";
            var gostIRI = "нет";
            if (middleD >=_showInfo.StandartsDensity)
            {
                gostDensity = "да";
            }
            
            if (middleC == _showInfo.DesignCount)
            {
                gostCount = "да";

            }
            if (middleL1 >= _showInfo.DesignLayer1)
            {
                gostLayer1 = "да";
            }
            if (middleL2 >= _showInfo.DesignLayer2)
            {
                gostLayer2 = "да";
            }
            if (middleL3 >= _showInfo.DesignLayer3)
            {
                gostLayer3 = "да";
            }
            if (middleR <= _showInfo.StandartsRut)
            {
                gostRut = "да";
            }
            if (middleR <= _showInfo.StandartsIRI)
            {
                gostIRI = "да";
            }



            gfx.DrawString(@"Соотв.", font4, XBrushes.Black,
                new XRect(520, 186, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"проект.", font4, XBrushes.Black,
                new XRect(520, 195, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"да/нет", font4, XBrushes.Black,
               new XRect(520, 205, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(gostDensity, font5, XBrushes.Black,
               new XRect(520, 218, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(gostCount, font5, XBrushes.Black,
               new XRect(520, 232, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(gostLayer1, font5, XBrushes.Black,
              new XRect(520, 246, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(gostLayer2, font5, XBrushes.Black,
              new XRect(520, 260, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(gostLayer3, font5, XBrushes.Black,
              new XRect(520, 274, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(gostRut, font5, XBrushes.Black,
              new XRect(520, 288, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString(gostIRI, font5, XBrushes.Black,
             new XRect(520, 302, page.Width, page.Height),
             XStringFormats.TopLeft);


            gfx.DrawString(@"Средняя плотность", font5, XBrushes.Black,
                new XRect(55, 218, page.Width, page.Height),
                XStringFormats.TopLeft);


            
            gfx.DrawString(@"Количество слоёв", font5, XBrushes.Black,
                new XRect(55, 232, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №1", font5, XBrushes.Black,
                new XRect(55, 246, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №2", font5, XBrushes.Black,
                new XRect(55, 260, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Толщина слоя №3", font5, XBrushes.Black,
                new XRect(55, 274, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"Колейность", font5, XBrushes.Black,
                new XRect(55, 288, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"IRI", font5, XBrushes.Black,
                new XRect(55, 302, page.Width, page.Height),
                XStringFormats.TopLeft);
            //Логотипы в шапку
            var form = new XForm(document, XUnit.FromMillimeter(2000), XUnit.FromMillimeter(2000));
            var formGfx = XGraphics.FromForm(form);
            formGfx.DrawImage(XImage.FromFile(@"РосАтомTEST.jpg"), 10, 0);
            formGfx.DrawImage(XImage.FromFile(@"ЭлеСи.jpg"), 105, 10);
            formGfx.Dispose();
            gfx.DrawImage(form, 55, 20, 2500, 2500);
            //Адрес
            gfx.DrawString(@"634021 г. Томск,", font6, XBrushes.Black,
                new XRect(60, 75, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(@"ул. Алтайская 161а", font6, XBrushes.Black,
               new XRect(60, 85, page.Width, page.Height),
               XStringFormats.TopLeft);
            




            #endregion
            //Отрисовка таблиц
            //Таблица плотности
      

            var dlg = new SaveFileDialog
            {
                FileName = "Report",
                DefaultExt = ".text",
                Filter = "Text documents (.pdf)|*.pdf"
            };
            var result = dlg.ShowDialog();
           
            // Process save file dialog box results
            if (result == true)
            {

                //Вызываем метод, чтобы сохранить график
                SaveAsPng(GetImage(GridGen), imageGen);

                var xImage = XImage.FromFile(imageGen);

                //Отрисовка графиков
                var form1 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx1 = XGraphics.FromForm(form1);
                formGfx1.DrawImage(xImage, 185, 290);
                //formGfx.Restore(state);
                
                formGfx1.Dispose();
                // Draw the form on the page of the document in its original size
                gfx.DrawImage(form1, 10, 150, 3600, 3800);
                gfx.Dispose();
                form1.Dispose();

                xImage.Dispose();

                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (int j = 0; j < k; j++)
                    {
                        if ((string)name == "CountBand" + j || (string)name == "AllLayerBand" + j
                            || (string)name == "DensityBand" + j || (string)name == "Layer1Band" + j
                            || (string)name == "Layer2Band" + j || (string)name == "Layer3Band" + j ||
                            (string)name == "RutBand" + j)
                        {
                            CreateImage(name.ToString(), Grid.Children[i]);
                           
                        }
                    }
                }
      //Перебор коллекции от большего к меньшему          
#region test

                foreach (var t in _collectionList.DensityBand)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].IntensityN1;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].IntensityN1 = t.GraphicsesCollection[l].IntensityN1;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].IntensityN1 = temp1;
                        }
                    }
                }
                foreach (var t in _collectionList.RutBand)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].Rut;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].Rut = t.GraphicsesCollection[l].Rut;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].Rut = temp1;
                        }
                    }
                }

                foreach (var t in _collectionList.Layer1Band)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].Layer1;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].Layer1 = t.GraphicsesCollection[l].Layer1;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].Layer1 = temp1;
                        }
                    }
                }
                foreach (var t in _collectionList.Layer2Band)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].Layer2;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].Layer2 = t.GraphicsesCollection[l].Layer2;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].Layer2 = temp1;
                        }
                    }
                }
                foreach (var t in _collectionList.Layer3Band)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].Layer3;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].Layer3 = t.GraphicsesCollection[l].Layer3;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].Layer3 = temp1;
                        }
                    }
                }
                foreach (var t in _collectionList.IRIBand)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].IRI;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].IRI = t.GraphicsesCollection[l].IRI;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].IRI = temp1;
                        }
                    }
                }
                foreach (var t in _collectionList.CountBand)
                {
                    for (var j = 0; j < t.GraphicsesCollection.Count; j++)
                    {
                        for (var l = j + 1; l < t.GraphicsesCollection.Count; l++)
                        {
                            if (!(t.GraphicsesCollection[j].Distance > t.GraphicsesCollection[l].Distance)) continue;
                            var temp = t.GraphicsesCollection[j].Distance;
                            var temp1 = t.GraphicsesCollection[j].CountLayer;
                            t.GraphicsesCollection[j].Distance = t.GraphicsesCollection[l].Distance;
                            t.GraphicsesCollection[j].CountLayer = t.GraphicsesCollection[l].CountLayer;
                            t.GraphicsesCollection[l].Distance = temp;
                            t.GraphicsesCollection[l].CountLayer = temp1;
                        }
                    }
                }
#endregion

                
                //Проверка несоответствия по полосам
                for (var i = 0; i < _collectionList.GeneralBand.Count; i++)
                {
                   

                        switch (_collectionList.GeneralBand[i].Band)
                        {

                            case 1:
                              CreatTable(document, font3, font2 ,font9, page, i);
                                break;
                            case 2:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 3:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 4:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 5:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 6:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 7:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 8:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 9:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                            case 10:
                                CreatTable(document, font3, font2, font9, page, i);
                                break;
                                

                        }
                }


                //Графики в документ
                #region PlotinPage
                var page1 = document.AddPage();
                var form2 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx2 = XGraphics.FromForm(form2);
                //formGfx1.Dispose();
                var gfx1 = XGraphics.FromPdfPage(page1);
                var z1 = 70;
                
                gfx1.DrawString(@"Плотность", font3, XBrushes.Black,
                               new XRect(250, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    
                    for (var j = 0; j < k; j++)
                    {
                        if (z1>820)
                        {
                            z1 = 70;
                        }
                        if ((string) name == "DensityBand" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            formGfx2.DrawImage(xDensity, 175, z1);
                           
                            z1 += 230;
                            xDensity.Dispose();
                        }
                       
                    }
                    
                }

           
                formGfx2.Dispose();
                form2.Dispose();
                gfx1.DrawImage(form2, 10, 0, 3600, 3800);
                var u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u > 820)
                        {
                            u = 50;
                        }
                        if ((string)name == "CountBand" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx1.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);
                            
                            u += 160;
                        }
                    }
                }
                page1 = document.AddPage();
                var form3 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx3 = XGraphics.FromForm(form3);
                //formGfx1.Dispose();
                var gfx5 = XGraphics.FromPdfPage(page1);
                z1 = 70;

                gfx5.DrawString(@"Количество слоёв", font3, XBrushes.Black,
                               new XRect(235, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();

                    for (var j = 0; j < k; j++)
                    {
                        if (z1 > 820)
                        {
                            z1 = 70;
                        }
                        if ((string)name == "CountBand" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            


                            formGfx3.DrawImage(xDensity, 175, z1);
                            
                            z1 += 230;
                            xDensity.Dispose();
                        }
                    }

                }

                formGfx3.Dispose();
                gfx5.DrawImage(form3, 10, 0, 3600, 3800);
                u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u > 820)
                        {
                            u = 50;
                        }
                        if ((string)name == "CountBand" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx5.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);

                            u += 160;
                        }
                    }
                }

                ///////////////////////////
                page1 = document.AddPage();
                var form6 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx6 = XGraphics.FromForm(form6);
                //formGfx1.Dispose();
                var gfx6 = XGraphics.FromPdfPage(page1);
                z1 = 70;

                gfx6.DrawString(@"Толщина слоя №1", font3, XBrushes.Black,
                               new XRect(235, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();

                    for (var j = 0; j < k; j++)
                    {
                        if (z1 > 820)
                        {
                            z1 = 70;
                        }
                        if ((string)name == "Layer1Band" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            formGfx6.DrawImage(xDensity, 175, z1);

                            z1 += 230;
                            xDensity.Dispose();
                            
                        }
                    }

                }

                formGfx6.Dispose();
                gfx6.DrawImage(form6, 10, 0, 3600, 3800);
                gfx6.Dispose();
                u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u> 820)
                        {
                            u= 50;
                        }
                        if ((string)name == "Layer1Band" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx6.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);

                            u += 160;
                        }
                    }
                }
                ////////////////////////////
                page1 = document.AddPage();
                var form7 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx7 = XGraphics.FromForm(form7);
                //formGfx1.Dispose();
                var gfx7 = XGraphics.FromPdfPage(page1);
                z1 = 70;

                gfx7.DrawString(@"Толщина слоя №2", font3, XBrushes.Black,
                               new XRect(235, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();

                    for (var j = 0; j < k; j++)
                    {
                        if (z1 > 820)
                        {
                            z1 = 70;
                        }
                        if ((string)name == "Layer2Band" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            formGfx7.DrawImage(xDensity, 175, z1);
                            z1 += 230;
                            xDensity.Dispose();
                        }
                    }

                }

                formGfx7.Dispose();
                gfx7.DrawImage(form7, 10, 0, 3600, 3800);
                u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u> 820)
                        {
                            u= 50;
                        }
                        if ((string)name == "Layer2Band" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx7.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);

                            u += 160;
                        }
                    }
                }
                //////////////////////
                page1 = document.AddPage();
                var form8 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx8 = XGraphics.FromForm(form8);
                //formGfx1.Dispose();
                var gfx8 = XGraphics.FromPdfPage(page1);
                z1 = 70;

                gfx8.DrawString(@"Толщина слоя №3", font3, XBrushes.Black,
                               new XRect(235, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();

                    for (var j = 0; j < k; j++)
                    {
                        if (z1 > 820)
                        {
                            z1 = 70;
                        }
                        if ((string)name == "Layer3Band" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            formGfx8.DrawImage(xDensity, 175, z1);
                            z1 += 230;
                            xDensity.Dispose();
                        }
                    }

                }

                formGfx8.Dispose();
                gfx8.DrawImage(form8, 10, 0, 3600, 3800);
                u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u > 820)
                        {
                            u = 50;
                        }
                        if ((string)name == "Layer3Band" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx8.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);

                            u += 160;
                        }
                    }
                }
                ////////
                
                page1 = document.AddPage();
                var form9 = new XForm(document, XUnit.FromMillimeter(1800), XUnit.FromMillimeter(1900));
                var formGfx9 = XGraphics.FromForm(form9);
                //formGfx1.Dispose();
                var gfx9= XGraphics.FromPdfPage(page1);
                z1 = 70;

                gfx9.DrawString(@"Колейность", font3, XBrushes.Black,
                               new XRect(245, 25, page.Width, page.Height),
                               XStringFormats.TopLeft);
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();

                    for (var j = 0; j < k; j++)
                    {
                        if (z1 > 820)
                        {
                            z1 = 70;
                        }
                        if ((string)name == "RutBand" + j)
                        {
                            var process = Path.GetTempPath() + name + ".png";
                            var xDensity = XImage.FromFile(process);
                            formGfx9.DrawImage(xDensity, 175, z1);
                            z1 += 230;
                            xDensity.Dispose();
                        }
                    }

                }

                formGfx9.Dispose();
                gfx9.DrawImage(form9, 10, 0, 3600, 3800);
                u = 50;
                for (var i = 0; i < Grid.Children.Count; i++)
                {
                    var name = Grid.Children[i].GetValue(NameProperty);
                    Grid.UpdateLayout();
                    for (var j = 0; j < k; j++)
                    {
                        if (u> 820)
                        {
                            u= 50;
                        }
                        if ((string)name == "RutBand" + j)
                        {
                            // Draw the form on the page of the document in its original size
                            gfx9.DrawString(@"Полоса №" + j, font8, XBrushes.Black,
                               new XRect(258, u, page.Width, page.Height),
                               XStringFormats.TopLeft);

                            u += 160;
                        }
                    }
                }
#endregion
                //Таблица плотности
                ///////////////////////////////////////////////////////////
                #region Parametr
                if (_densityFlag)
                {
                    DensityTable(document,page,font2,font7,font5,dens1, gfx);

                }
                #endregion
                //Таблица колейности
                #region RUT
                //Таблица колейности
                if (_ruttingFlag)
                {
                    RutTable(document,page,font2,gfx,font5,dens1);

                }

                #endregion
                //Таблицы толщин
                #region Layer
                //Таблица толщин слоёв
                if (_thicknessesFlag)
                {

                    LayerTable(document,page,font2,font5,dens1,gfx);

                }
                #endregion


                // Save document
                try
                {
                    var filename1 = dlg.FileName;
                    using (var stm = File.Create(filename1))
                    {
                        document.Save(stm);
                    }
                    Process.Start(filename1);
                }
                catch (Exception)
                {
                    MessageBox.Show(@"Файл с таким именем уже открыт");
                }
            }
            GeneralGraff.DrawGraphic(new ObservableCollection<CollectionGraphs>(_collorCollection), collor = true, _band);
            TextBlockvG.Foreground = Brushes.White;
            TextBlock1G.Foreground = Brushes.White;
            TextBlock2G.Foreground = Brushes.White;
            TextBlockdG.Foreground = Brushes.White;
            Grid.SetRowSpan(GridGen, 1);
            LayoutRoot.Children.Remove(grid);
            LayoutRoot.Children.Remove(Grid);
            _reportWindow.Close();
            flagSave = false;
            document.Dispose();
            GeneralGraff.Width = _widthScrollGeneral;

        }
        #region CreateImage
        //Создаём изображение
        public  RenderTargetBitmap GetImage(UIElement view)
        {

            var size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            if (size.IsEmpty)
                return null;
            var result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 100, 101, PixelFormats.Pbgra32);
            //var result = new RenderTargetBitmap(500, 550, 100, 100, PixelFormats.Pbgra32);
            var drawingvisual = new DrawingVisual();
            using (var context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(-8, -5, (int)size.Width, (int)size.Height));
                //context.DrawRectangle(new VisualBrush(view), null, new Rect(500, 500, 200, 200));
                context.Close();
            }
            result.Render(drawingvisual);
            return result;
        }
        //Сохраняем в .png формате
        public  void SaveAsPng(RenderTargetBitmap src, string targetFile)
        {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(src));

                using (var fs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                {
                    encoder.Save(fs);
                    fs.Close();
                }
        }
#endregion
        public void Liner(List<object> mess1, List<object> mess2, string tittlex, string charttitle , 
            string tittley, Brush collor, int serias)
        {
            var ocY = new List<float>();
            var Distance = new List<float>();
            foreach (var count in mess1)
            {
                ocY.Add(Convert.ToByte(count));
            }
            foreach (var distance in mess2)
            {
                Distance.Add(Convert.ToSingle(distance));
            }
            var dataValues = new List<Graphics>();
            var max = Convert.ToInt16(ocY[0]);
            for (var i = 0; i < ocY.Count - 1; i++)
            {
                var graph = new Graphics {  Distance = Distance[i] };
                if (serias == 1)
                {
                    graph.Layer3 = ocY[i];
                }
                if (serias == 2)
                {
                    graph.CountLayer  = Convert.ToByte(ocY[i]);
                }
                dataValues.Add(graph);
                if (ocY[i] > max)
                {
                    max = Convert.ToInt16(ocY[i]);
                }
                if (serias == 3)
                {
                    graph.IntensityN1 = Convert.ToByte(ocY[i]);
                }
            }
            var linerAxx = new LinearAxis
            {
                Orientation = AxisOrientation.X,
               
                Title = tittlex
            };
            var linerAxy = new LinearAxis
            {
                Orientation = AxisOrientation.Y,
                ShowGridLines = true,
                Minimum = 0,
                Maximum = max + 2
            };
            if (tittley != null)
            {
                linerAxy.Title = tittley;
            }
            _chart = new Chart()
            {
                Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                Title = charttitle,
                

            };
            
            var setter = new Setter();
            var setterpoint = new Setter {Property = OpacityProperty, Value = (double) 0};
            setter.Property = BackgroundProperty;
            setter.Value = collor;

            var style = new Style();

            style.Setters.Add(setter);
            var legendSetterw = new Setter();
            var legendSetterl = new Setter();
            var legendStyle = new Style();
            var control = new Control();
            if (serias == 3)
            {
                var LS = new LineSeries()
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "IntensityN1",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style
                };
                linerAxx.ShowGridLines = true;
                style.Setters.Add(setterpoint);
                var lineDataPoint = new LineDataPoint();
                style.TargetType = lineDataPoint.GetType();
                _chart.Series.Add(LS);
            }
            if (serias == 2)
            {
                var CL = new ColumnSeries
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "CountLayer",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style
                };
              
                var columnDataPoint = new ColumnDataPoint();
                style.TargetType = columnDataPoint.GetType();

                //style.Setters.Add(setter);
                _chart.Series.Add(CL);
            }
            if (serias == 1)
            {
                var AS = new AreaSeries
                {
                    ItemsSource = dataValues,
                    DependentValuePath = "Layer3",
                    IndependentValuePath = "Distance",
                    DataPointStyle = style,

                };
                linerAxx.ShowGridLines = true;
                style.Setters.Add(setterpoint);
                var areaDataPoint = new AreaDataPoint();
                style.TargetType = areaDataPoint.GetType();


                _chart.Series.Add(AS); 
            }
            legendSetterw.Property = WidthProperty;
            legendSetterw.Value = (double)0;
            legendSetterl.Property = HeightProperty;
            legendSetterl.Value = (double)0;
            legendStyle.TargetType = control.GetType();
            legendStyle.Setters.Add(legendSetterl);
            legendStyle.Setters.Add(legendSetterw);
            _chart.LegendStyle = legendStyle;
            _chart.Axes.Add(linerAxx);
            _chart.Axes.Add(linerAxy);
            _chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;

            LayoutRoot.Children.Add(_chart);
            Grid.SetRow(_chart, 0);
            Grid.SetRowSpan(_chart, 6);
            Grid.SetColumnSpan(_chart, 5);


        }


        #region События
        private void MouseLeftButtonDownGrid(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(_chart);
        }
        private void MouseLeftButtonDownGrid1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(_gridnorm);
            SendBorder(true);


        }
     
        private new void MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                (DataContext as MainViewModel).AddRelayCommand.Execute(null);
            }
            else
            {
                (DataContext as MainViewModel).CleanRelayCommand.Execute(null);
            }
        }
  

        private new void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _greatGraphicName = (sender as Chart).Name;
            (DataContext as MainViewModel).GraffCountCommand.Execute((sender as Chart).Name);
        }
        private void MouseLeftButtonDown1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var graff = sender as Graff;
            if (graff != null) _greatGraphicName = graff.Name;
            var mainViewModel = DataContext as MainViewModel;
            if (mainViewModel != null)
                mainViewModel.GraffCountCommand.Execute((sender as Graff).Name);
        }

        

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollSize = (sender as ScrollViewer).ContentHorizontalOffset;
        }
       
        public void SendBorder(bool border)
        {
            Messenger.Default.Send(border, "WidthGeneral");
        }

        public void SendScrollWidth(double border)
        {
            Messenger.Default.Send(border, "ScrollWidth");
        }
        public void SendWidth(double width)
        {
            Messenger.Default.Send(width, "SendWidth");
        }
        public void SendDensityStandarts(bool border)
        {
            Messenger.Default.Send(border, "DensityStandartsNew");
        }
      
        #endregion

        private void Combobox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Band.IsEnabled = true;
            Messenger.Default.Send(true, "SelectionChanged");
            
        }

     
    }
    
}