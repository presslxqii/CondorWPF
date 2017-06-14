using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;

namespace CondorVisualizator.View
{
    /// <summary>
    /// Interaction logic for CoefficientWindow.xaml
    /// </summary>
    public partial class CoefficientWindow : Window
    {
        public CoefficientWindow()
        {
            InitializeComponent();
            B.IsEnabled = false;
            K.IsEnabled = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.Text, 0));
            if (e.Text == ".")
            {
                e.Handled = false;
            }

           
        }


        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            B.IsEnabled = true;
            K.IsEnabled = true;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var argument = new List<object>();
          
                B.IsEnabled = true;
                K.IsEnabled = true;
                Box.IsEnabled = false;
                Box.Text = "";
                argument.Add(true);
                argument.Add(B.Text);
                argument.Add(K.Text);
                Messenger.Default.Send(argument, "CoefficientAllBand");
      
        }
    }
}
