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
            bool createdNew;
            _instanceMutex = new Mutex(true, @"Global\ControlPanel", out createdNew);
            if (!createdNew)
            {
                MessageBox.Show(@"Программа уже запущена");
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
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = CondorVisualizator.Properties.Resources.Icon1;
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
