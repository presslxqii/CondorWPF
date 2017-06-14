using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CondorVisualizator.Model;
using CondorVisualizator.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

using Brushes = System.Windows.Media.Brushes;
using Chart = System.Windows.Controls.DataVisualization.Charting.Chart;
using Color = System.Windows.Media.Color;
using Control = System.Windows.Controls.Control;
using Convert = System.Convert;
using Graphics = CondorVisualizator.Model.Graphics;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using PdfDocument = PdfSharp.Pdf.PdfDocument;
using Style = System.Windows.Style;

namespace CondorVisualizator.View
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private double _tempsize;
        private double _scrollSize;
        private Chart chart;
        private Graff Gridd;
        private Grid Gridnorm;
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
            }
        }

        public DataRoad Info;
        public float Distance;
        public bool collor { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            
            _tempsize = DensityScrollViewer.ContentHorizontalOffset;

            Closing += (s, e) => ViewModelLocator.Cleanup();
           
            Messenger.Default.Register<DataRoad>(this, "ShowInfo", message =>
            {
                 Info = message;
            });
            Messenger.Default.Register<float>(this, "ShowInfoDistance", message =>
            {
                Distance = message;
            });
            Messenger.Default.Register<ObservableCollection<Graphics>>(this, "ShowGeneralGraff", message => 
                GeneralGraff.DrawGraphic(message, collor = true));
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
                LineSeries LS;
                AreaSeries AS;
                ColumnSeries CL;
                List<byte> CountGener;
                List<float> Distance;
                 List<float> Layer123;
                List<double> Plotnost;
                List<Graphics> dataValues;
                LinearAxis linerAxx;
                LinearAxis linerAxy;
                Setter setter;
                Style style;
                Style legendStyle;
                Setter legendSetterw;
                Setter legendSetterl;
                AreaDataPoint areaDataPoint;
                int max;
                Control control;
                LineDataPoint lineDataPoint;
                Setter setterpoint;
                switch (_greatGraphicName)
                {

                    case "CountChart":
                        CountGener = new List<byte>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            CountGener.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        dataValues = new List<Graphics>();

                        for (var i = 0; i < CountGener.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.CountLayer = CountGener[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                        }
                        setter = new Setter();
                
                        setter.Property = BackgroundProperty;
                        setter.Value = Brushes.Black;
                        style = new Style();
                        var columnDataPoint = new ColumnDataPoint();
                        style.TargetType = columnDataPoint.GetType();
                        style.Setters.Add(setter);
                        
                        CL = new ColumnSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "CountLayer",
                            IndependentValuePath = "Distance"
                        };
                        CL.DataPointStyle = style;

                      



                        linerAxx = new LinearAxis();
                        linerAxy = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = 4;
                        linerAxx.Title = "Расстояние, [м]";
                       
                        chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Количество слоёв", 

                        };
                           legendSetterw = new Setter();
                        legendSetterl = new Setter();
                        legendStyle = new Style();
                        control = new Control();
                        legendSetterw.Property = WidthProperty;
                        legendSetterw.Value = (double) 0;
                        legendSetterl.Property = HeightProperty;
                        legendSetterl.Value = (double) 0;
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(legendSetterl);
                        legendStyle.Setters.Add(legendSetterw);
                        chart.LegendStyle = legendStyle;
                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        chart.Series.Add(CL);
                        LayoutRoot.Children.Add(chart);

                        break;

                    case "ChartLayer3":
                        Layer123 = new List<float>();
                        Distance = new List<float>();

                        foreach (var count in message[0])
                        {
                            Layer123.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        dataValues = new List<Graphics>();
                        max = Convert.ToInt16(Layer123[0]);
                        for (var i = 0; i < Layer123.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer3 = Layer123[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            if (Layer123[i] > max)
                            {
                                max = Convert.ToInt16(Layer123[i]);
                            }
                        }
                         linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "Толщина, [см]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;
                     
                        

                        chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Толщина слоя №1",

                        };

                        setter = new Setter();
                        setterpoint = new Setter();
                        setterpoint.Property = OpacityProperty;
                        setterpoint.Value = (double)0;
                        setter.Property = BackgroundProperty;
                        setter.Value = Brushes.DodgerBlue;

                        style = new Style();
                        areaDataPoint = new AreaDataPoint();
                        style.TargetType = areaDataPoint.GetType();
                        style.Setters.Add(setterpoint);
                        style.Setters.Add(setter);

                        AS = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer3",
                            IndependentValuePath = "Distance",
                            
                        };
                        AS.DataPointStyle = style;
                        legendSetterw = new Setter();
                        legendSetterl = new Setter();
                        legendStyle = new Style();
                        control = new Control();
                        legendSetterw.Property = WidthProperty;
                        legendSetterw.Value = (double) 0;
                        legendSetterl.Property = HeightProperty;
                        legendSetterl.Value = (double) 0;
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(legendSetterl);
                        legendStyle.Setters.Add(legendSetterw);
                        chart.LegendStyle = legendStyle;
                    

                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        chart.Series.Add(AS);
                        LayoutRoot.Children.Add(chart);

                        break;


                    case "ChartLayer2":
                        Layer123 = new List<float>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            Layer123.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        dataValues = new List<Graphics>();
                        max = Convert.ToInt16(Layer123[0]);
                        for (var i = 0; i < Layer123.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer2 = Layer123[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            if (Layer123[i] > max)
                            {
                                max = Convert.ToInt16(Layer123[i]);
                            }
                        }
                        linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "Толщина, [см]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;

                    


                        chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Толщина слоя №2",

                        };
                        setter = new Setter();
                        setterpoint = new Setter();
                        setterpoint.Property = OpacityProperty;
                        setterpoint.Value = (double)0;
                        setter.Property = BackgroundProperty;
                        setter.Value = Brushes.DarkRed;
                        style = new Style();
                        areaDataPoint = new AreaDataPoint();
                        style.TargetType = areaDataPoint.GetType();
                        style.Setters.Add(setter);
                        style.Setters.Add(setterpoint);
                        AS = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer2",
                            IndependentValuePath = "Distance"
                        };
                        AS.DataPointStyle = style;
                         legendSetterw = new Setter();
                        legendSetterl = new Setter();
                        legendStyle = new Style();
                        control = new Control();
                        legendSetterw.Property = WidthProperty;
                        legendSetterw.Value = (double) 0;
                        legendSetterl.Property = HeightProperty;
                        legendSetterl.Value = (double) 0;
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(legendSetterl);
                        legendStyle.Setters.Add(legendSetterw);
                        chart.LegendStyle = legendStyle;
                        
                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        
                        chart.Series.Add(AS);
                        LayoutRoot.Children.Add(chart);

                        break;

                    case "ChartLayer1":
                        Layer123 = new List<float>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            Layer123.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        dataValues = new List<Graphics>();
                        max = Convert.ToInt16(Layer123[0]);
                        for (var i = 0; i < Layer123.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer1 = Layer123[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            if (Layer123[i]>max)
                            {
                                max = Convert.ToInt16(Layer123[i]);
                            }
                        }
                       
                      


                        linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "Толщина, [см]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;


                        setter = new Setter();
                        setterpoint = new Setter();
                        setterpoint.Property = OpacityProperty;
                        setterpoint.Value = (double)0;
                        setter.Property = BackgroundProperty;
                        setter.Value = Brushes.OliveDrab;
                        style = new Style();
                        areaDataPoint = new AreaDataPoint();
                        style.TargetType = areaDataPoint.GetType();
                        style.Setters.Add(setter);
                        style.Setters.Add(setterpoint);
                        AS = new AreaSeries
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "Layer1",
                            IndependentValuePath = "Distance"
                        };
                        AS.DataPointStyle = style;


                        chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Толщина слоя №3",

                        };

                         legendSetterw = new Setter();
                        legendSetterl = new Setter();
                        legendStyle = new Style();
                        control = new Control();
                        legendSetterw.Property = WidthProperty;
                        legendSetterw.Value = (double) 0;
                        legendSetterl.Property = HeightProperty;
                        legendSetterl.Value = (double) 0;
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(legendSetterl);
                        legendStyle.Setters.Add(legendSetterw);
                        chart.LegendStyle = legendStyle;

                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                       
                        chart.Series.Add(AS);
                        LayoutRoot.Children.Add(chart);

                        break;

                    case "ChartAll":
                        var Layer1 = new List<float>();
                        var Layer2 = new List<float>();
                        var Layer3 = new List<float>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            Layer1.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }
                        foreach (var count in message[2])
                        {
                            Layer2.Add(Convert.ToByte(count));
                        }
                        foreach (var count in message[3])
                        {
                            Layer3.Add(Convert.ToByte(count));
                        }

                        dataValues = new List<Graphics>();
                        max = Convert.ToInt16(Layer3[0]);
                        
                        for (var i = 0; i < Layer1.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.Layer3 = Layer3[i];
                            graph.Layer2 = Layer2[i];
                            graph.Layer1 = Layer1[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            
                            if (Layer3[i]> max)
                            {
                                max = Convert.ToInt16(Layer3[i]);
                            }
                        }

                        linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        linerAxy = new LinearAxis();
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

                        chart = new Chart()
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
                        legendStyle = new Style();
                        control = new Control();
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(setterlegendcollor);
                        legendStyle.Setters.Add(setterlegendborder);
                        chart.LegendStyle = legendStyle;




                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;

                        chart.Series.Add(ser3);
                        chart.Series.Add(ser2);
                        chart.Series.Add(ser1);
                        
                        
                        LayoutRoot.Children.Add(chart);

                        break;


                    case "Plotnost":
                        Plotnost = new List<double>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            Plotnost.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        dataValues = new List<Graphics>();
                        max = Convert.ToInt16(Plotnost[0]);
                        for (var i = 0; i < Plotnost.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.IntensityN1 = Plotnost[i];
                            graph.Distance = Distance[i];
                            dataValues.Add(graph);
                            if (Plotnost[i] > max)
                            {
                                max = Convert.ToInt16(Plotnost[i]);
                            }
                        }
                         linerAxx = new LinearAxis();
                        linerAxx.Orientation = AxisOrientation.X;
                        linerAxx.ShowGridLines = true;
                        linerAxx.Title = "Расстояние, [м]";
                        linerAxy = new LinearAxis();
                        linerAxy.Orientation = AxisOrientation.Y;
                        linerAxy.ShowGridLines = true;
                        linerAxy.Title = "[кг/м^3]";
                        linerAxy.Minimum = 0;
                        linerAxy.Maximum = max + 2;

                        setter = new Setter();
                        setterpoint = new Setter();
                        setterpoint.Property = OpacityProperty;
                        setterpoint.Value = (double)0;
                        setter.Property = BackgroundProperty;
                        setter.Value = Brushes.OliveDrab;
                        style = new Style();
                        lineDataPoint = new LineDataPoint();
                        style.TargetType = lineDataPoint.GetType();
                        style.Setters.Add(setter);
                        style.Setters.Add(setterpoint);
                        LS = new LineSeries()
                        {
                            ItemsSource = dataValues,
                            DependentValuePath = "IntensityN1",
                            IndependentValuePath = "Distance"

                        };
                        LS.DataPointStyle = style;

                      

                        chart = new Chart()
                        {
                            Background = new SolidColorBrush(Color.FromRgb(3, 94, 129)),
                            Title = "Плотность",

                        };
                        
                           legendSetterw = new Setter();
                        legendSetterl = new Setter();
                        legendStyle = new Style();
                        control = new Control();
                        legendSetterw.Property = WidthProperty;
                        legendSetterw.Value = (double) 0;
                        legendSetterl.Property = HeightProperty;
                        legendSetterl.Value = (double) 0;
                        legendStyle.TargetType = control.GetType();
                        legendStyle.Setters.Add(legendSetterl);
                        legendStyle.Setters.Add(legendSetterw);
                        chart.LegendStyle = legendStyle;
                       

                        chart.Axes.Add(linerAxx);
                        chart.Axes.Add(linerAxy);
                        chart.MouseLeftButtonDown += MouseLeftButtonDownGrid;
                        
                        chart.Series.Add(LS);
                        LayoutRoot.Children.Add(chart);

                        break;
                    case "GeneralGraff":
                        CountGener = new List<byte>();
                        Distance = new List<float>();
                        foreach (var count in message[0])
                        {
                            CountGener.Add(Convert.ToByte(count));
                        }
                        foreach (var distance in message[1])
                        {
                            Distance.Add(Convert.ToSingle(distance));
                        }

                        var dataValues1 = new ObservableCollection<Graphics>();

                        for (var i = 0; i < CountGener.Count - 1; i++)
                        {
                            var graph = new Graphics();
                            graph.GeneralState = CountGener[i];
                            graph.Distance = Distance[i];
                            dataValues1.Add(graph);
                        }

                        Gridnorm = new Grid();
                        Gridd = new Graff();
                        
                        Gridd.DrawGraphic(dataValues1 , collor = false);
                        Gridd.Width = 1111;
                        Gridd.Height = 234;
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
                        collor1.HorizontalAlignment = HorizontalAlignment.Left;
                        collor1.VerticalAlignment = VerticalAlignment.Top;
                        collor2.HorizontalAlignment = HorizontalAlignment.Left;
                        collor2.VerticalAlignment = VerticalAlignment.Top;
                        collor1.Margin = new Thickness(370, 650, 0, 0);
                        collor2.Margin = new Thickness(690, 650, 0, 0);
                        collortext1.Text = "- не соответствует";
                        collortext2.Text = "- соответствует";
                        collortext1.FontSize = 19;
                        collortext2.FontSize = 19;
                        collortext1.HorizontalAlignment = HorizontalAlignment.Left;
                        collortext1.VerticalAlignment = VerticalAlignment.Top;
                        collortext2.HorizontalAlignment = HorizontalAlignment.Left;
                        collortext2.VerticalAlignment = VerticalAlignment.Top;
                        collortext1.Margin = new Thickness(440, 657, 0, 0);
                        collortext2.Margin = new Thickness(760, 657, 0, 0);
                        text1.Text = "Общее состояние дорожного покрытия";
                        text2.Text = "Расстояние, [м]";
                        text2.FontStyle = FontStyles.Italic;
                        text1.FontSize = 22;
                        text2.FontSize = 19;
                        text1.HorizontalAlignment = HorizontalAlignment.Left;
                        text1.VerticalAlignment = VerticalAlignment.Top;
                        text2.HorizontalAlignment = HorizontalAlignment.Left;
                        text2.VerticalAlignment = VerticalAlignment.Top;
                        text1.Margin = new Thickness(440,350,0,0);
                        text2.Margin = new Thickness(570, 530, 0, 0);
                        Gridnorm.Children.Add(collortext1);
                        Gridnorm.Children.Add(collortext2);
                        Gridnorm.Children.Add(collor1);
                        Gridnorm.Children.Add(collor2);
                        Gridnorm.Children.Add(text1);
                        Gridnorm.Children.Add(text2);
                        Gridnorm.Children.Add(Gridd);
                        Gridnorm.Background = new SolidColorBrush(Color.FromRgb(3, 94, 129));
                        LayoutRoot.Children.Add(Gridnorm);
                    
                        Gridnorm.MouseLeftButtonDown += MouseLeftButtonDownGrid1;
                        Gridd.MouseLeftButtonDown += MouseLeftButtonDownGrid1;
                       
                        break;
                }


            });

        }

    

        private void Button(object sender, RoutedEventArgs e)
        {
            ChartLayer1.Width = 350;
            // Create a new PDF document
            var document = new PdfDocument();
            // Create an empty page
            var page = document.AddPage();
            // Get an XGraphics object for drawing
            var gfx = XGraphics.FromPdfPage(page);
            var options = new XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always);
            // Create a font
            var font = new XFont("Verdana", 16, XFontStyle.BoldItalic, options);
            // Draw the text
            gfx.DrawString(@"Состояние дорожного покрытия", font, XBrushes.Black,
                new XRect(0, 20, page.Width, page.Height),
                XStringFormats.TopCenter);
       
            var fonttext = new XFont("Times New Roman", 14, XFontStyle.Regular, options);
            gfx.DrawString("Название дороги:" , fonttext, XBrushes.Black,
                new XRect(30, 60, page.Width, page.Height),
                XStringFormats.TopLeft);
            gfx.DrawString(Info.RoadName, fonttext, XBrushes.Black,
              new XRect(230, 60, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString("Номер измерения:" , fonttext, XBrushes.Black,
               new XRect(30, 75, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(Info.NumMess.ToString(), fonttext, XBrushes.Black,
             new XRect(230, 75, page.Width, page.Height),
             XStringFormats.TopLeft);
            gfx.DrawString("Дата проведения измерения:", fonttext, XBrushes.Black,
               new XRect(30, 90, page.Width, page.Height),
               XStringFormats.TopLeft);
            gfx.DrawString(Info.TimeStart, fonttext, XBrushes.Black,
              new XRect(230, 90, page.Width, page.Height),
              XStringFormats.TopLeft);
            gfx.DrawString("Общая протяженность участка:", fonttext, XBrushes.Black,
               new XRect(30, 105, page.Width, page.Height),
               XStringFormats.TopLeft);
            //gfx.DrawLine(XPens.Black, 0, 120, page.Width, 120);
            gfx.DrawString(Distance + " м", fonttext, XBrushes.Black,
             new XRect(230, 105, page.Width, page.Height),
             XStringFormats.TopLeft);
           



            //var sPdfFileName = Path.GetTempPath() + "PDFFile.pdf";
          
            var imageAll = Path.GetTempPath() + "allgraf.png";
            var imageGen = Path.GetTempPath() + "general.png";
            var imageLay1 = Path.GetTempPath() + "layer1.png";
            var imageLay2 = Path.GetTempPath() + "layer2.png";
            var imageLay3 = Path.GetTempPath() + "layer3.png";
            var imagePlotn = Path.GetTempPath() + "density.png";
            var imageCount = Path.GetTempPath() + "count.png";
            //Вызываем метод, чтобы сохранить график
            SaveAsPng(GetImage(GridAll), imageAll);
            SaveAsPng(GetImage(GridGen), imageGen);
            SaveAsPng(GetImage(GridLay1), imageLay1);
            SaveAsPng(GetImage(GridLay2), imageLay2);
            SaveAsPng(GetImage(GridLay3), imageLay3);
            SaveAsPng(GetImage(GridPlotn), imagePlotn);
            SaveAsPng(GetImage(GridCount), imageCount);

            //CreatePdfFromImage(sImagePath, sPdfFileName);


           
                var form = new XForm(document, XUnit.FromMillimeter(1500), XUnit.FromMillimeter(1600));
                // Create an XGraphics object for drawing the contents of the form.
                var formGfx = XGraphics.FromForm(form);
                // Draw a large transparent rectangle to visualize the area the form occupies
                //var back = XColors.Orange;
                //var brush = new XSolidBrush(back);
                //formGfx.DrawRectangle(brush, -300, -300, 300, 300);
                //var state = formGfx.Save();
                formGfx.DrawImage(XImage.FromFile(imageGen), 180, -4);
                formGfx.DrawImage(XImage.FromFile(imageCount), 35, 170);
                formGfx.DrawImage(XImage.FromFile(imageAll), 350, 170);
                formGfx.DrawImage(XImage.FromFile(imageLay3), 35, 390);
                formGfx.DrawImage(XImage.FromFile(imageLay2), 350, 390);
                formGfx.DrawImage(XImage.FromFile(imageLay1), 35, 600);
                formGfx.DrawImage(XImage.FromFile(imagePlotn), 350, 600);
               
                //formGfx.Restore(state);
                formGfx.Dispose();
                // Draw the form on the page of the document in its original size
                gfx.DrawImage(form, 10, 130, 3600, 3800);



            //DrawImage(gfx, sImagePath, 50, 50, 250, 250);
   
            // Show save file dialog box
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
                // Save document
                var filename1 = dlg.FileName;
                using (var stm = File.Create(filename1))
                {
                    document.Save(stm);
                }
                Process.Start(filename1);
            }
            
          
        }

      
   
  
        //Создаём изображение
        public  RenderTargetBitmap GetImage(UIElement view)
        {
            
            var size = new Size(view.RenderSize.Width, view.RenderSize.Height);
            if (size.IsEmpty)
                return null;

            var result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 100, 100, PixelFormats.Pbgra32);

            var drawingvisual = new DrawingVisual();
            using (var context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(view), null, new Rect(0, 0, (int)size.Width, (int)size.Height));
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

            using (var stm = File.Create(targetFile))
            {
                encoder.Save(stm);
            }
      
        }

       
        private void MouseLeftButtonDownGrid(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(chart);
        }
        private void MouseLeftButtonDownGrid1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            LayoutRoot.Children.Remove(Gridnorm);
            
        }
     
        private void MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
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
  

        private void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _greatGraphicName = (sender as Chart).Name;
            (DataContext as MainViewModel).GraffCountCommand.Execute((sender as Chart).Name);
        }
        private void MouseLeftButtonDown1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _greatGraphicName = (sender as Graff).Name;
            (DataContext as MainViewModel).GraffCountCommand.Execute((sender as Graff).Name);
        }

  

 
        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (DataContext as MainViewModel).OkRelayCommand.Execute(null);
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollSize = (sender as ScrollViewer).ContentHorizontalOffset;
        }

    }
}