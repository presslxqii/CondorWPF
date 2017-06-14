using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using CondorVisualizator.Model;
using GalaSoft.MvvmLight.Messaging;


namespace CondorVisualizator.View
{
    public partial class Graff
    {

        public Graff()
        {
            InitializeComponent();
        }

        public void DrawGraphic(ObservableCollection<Graphics> graphicsList, bool collor )
        {
            GridRow.ColumnDefinitions.Clear();
            GridRow.Children.Clear();
            
            for (var i = 0; i < graphicsList.Count; i++)
            {
                GridRow.ColumnDefinitions.Add(new ColumnDefinition());
                if (graphicsList[i].GeneralState <= 7)
                {
                    var redRect = new Rectangle {Fill = Brushes.Red};
                    GridRow.Children.Add(redRect);
                    Grid.SetRow(redRect, 0);
                    Grid.SetColumn(redRect, i);
                }

                if (graphicsList[i].GeneralState < 7) continue;
                var greenRect1 = new Rectangle {Fill = Brushes.Green};
                
                GridRow.Children.Add(greenRect1);
                Grid.SetRow(greenRect1, 0);
                Grid.SetColumn(greenRect1, i);
            }

            var st = graphicsList.Count/5;
            if (st == 0)
            {
                st = 1;
            }

            for (var i = 0; i < graphicsList.Count; i += st)
            {
               
                var label = new Label {Content = graphicsList[i].Distance};
                if (collor)
                {
                    label.Foreground = Brushes.Black;
                }
                else
                {
                    label.Foreground = Brushes.White; 
                }
                     
                                 
                label.Content = graphicsList[i].Distance;
                GridRow.Children.Add(label);
                Grid.SetRow(label, 2);
                Grid.SetColumn(label, i);
                Grid.SetColumnSpan(label, 4);
                

            }
        }

       
    }
}