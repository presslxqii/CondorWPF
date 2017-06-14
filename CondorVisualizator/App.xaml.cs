using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using CondorVisualizator.View;
using GalaSoft.MvvmLight.Threading;
using System.IO;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace CondorVisualizator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;
        private Mutex _instanceMutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            var path = "C:\\Tag.ini";
            if (!File.Exists(path))
            {
                using (var fs = File.Create(path))
                {
                    var info = new System.Text.UTF8Encoding(true).GetBytes("CountLayers = " + "\r\n" +
                         "Distance = " + "\r\n" + "OverallAssessment = " + "\r\n" + "CoefficientB = "
                         + "\r\n" + "CoefficientK = " + "\r\n" + "Layer1 = " + "\r\n" + "Layer2 = "
                         + "\r\n" + "Layer3 = " + "\r\n" + "Sensor1 = " + "\r\n" + "Sensor2 = "
                         + "\r\n" + "MeasuringNum = " + "\r\n" + "NameRoad = " + "\r\n" + "Start = "
                         + "\r\n" + "MasterIpHda = " + "\r\n" + "SlaveIpHda = " + "\r\n" + "OpcHdaId = "
                         + "\r\n" + "Coating = " + "\r\n" + "DesignCount = " + "\r\n" + "DesignLayer1 = " 
                         + "\r\n" + "DesignLayer2 = "
                         + "\r\n" + "DesignLayer3 = "
                         + "\r\n" + "Rut = " + "\r\n" + "Customer = " + "\r\n" + "Builder = "
                         + "\r\n" + "RoadSegment1 = " + "\r\n" + "RoadSegment2 = " + "\r\n" + "RoadSegment3 = "
                         + "\r\n" + "RoadSegment4 = " + "\r\n" + "Direction = " + "\r\n" + "Band = "+ "\r\n" + "IRI = "
                         + "\r\n" + "StandartsDensity = " + "\r\n" + "StandartsDepth = " + "\r\n" + "StandartsIRI = "
                         + "\r\n" + "StandartsLenght = " + "\r\n" + "StandartsRut = " + "\r\n" + "StandartsWidth = "
                         + "\r\n" + "OverallRating = " + "\r\n" + "Latitude = " + "\r\n" + "Longitude = "
                          + "\r\n" + "CoefficientTF = "
                         );
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);

                }
            }
            


            bool createdNew;
            _instanceMutex = new Mutex(true, @"Global\ControlPanel", out createdNew);
            if (!createdNew)
            {
                MessageBox.Show(@"Программа уже запущена",@"Внимание",MessageBoxButton.OK,MessageBoxImage.Information);
                _instanceMutex = null;
                Application.Current.Shutdown();
                return;
            }
            base.OnStartup(e);
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;
            
            EventManager.RegisterClassHandler(typeof(DatePicker),
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler(DatePicker_Loaded));
            _notifyIcon = new NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = CondorVisualizator.Properties.Resources.Iconic_e027_0_;
            _notifyIcon.Visible = true;

            CreateContextMenu();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            if (_instanceMutex != null)
                _instanceMutex.ReleaseMutex();
            base.OnExit(e);
        }
        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }
       
       
        static App()
        {
            DispatcherHelper.Initialize();
     
        }
        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        static void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            var dp = sender as DatePicker;
            if (dp == null) return;

            var tb = GetChildOfType<DatePickerTextBox>(dp);
            if (tb == null) return;

            var wm = tb.Template.FindName("PART_Watermark", tb) as ContentControl;
            if (wm == null) return;

            wm.Content = "Выберите дату";
        }
        public class ValueToBoolConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value != null)
                {
                    if ((string)value == "0")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
