using CondorVisualizator.Model;
using CondorVisualizator.ViewModel;
using GalaSoft.MvvmLight.Messaging;

namespace CondorVisualizator.View
{
    /// <summary>
    /// Interaction logic for Measurment.xaml
    /// </summary>
    public partial class Measurment
    { 
        public Measurment()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Messenger.Default.Send(true, "IsEnableBand");
        }
    }
}
