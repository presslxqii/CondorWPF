using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using CondorVisualizator.Model;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;


namespace CondorVisualizator.View
{
    public partial class Graff
    {
        public Button button;
        private int parametr;
        private double _densityStandarts;
        //private ObservableCollection<CollectionGraphs> _density;
        
        //private double _band;
        public Graff()
        {
            InitializeComponent();

            //Messenger.Default.Register<ObservableCollection<CollectionGraphs>>(this, "Density", message =>
            //{
            //    _density = message;
              
            //});

            Messenger.Default.Register<double>(this, "DensityStandearts", message =>
            {
                _densityStandarts = message;
              
            });
            Messenger.Default.Register<int>(this, "SendParametr", message =>
            {
                parametr = message;
              
            });

            Messenger.Default.Register<double>(this, "SendWidth", message =>
            {
                var _band = message;
                BigGridRow.Width = _band - GridForButton.Width;
                UserControl.Width = _band;
                GridAll.Width = _band ;
            });
        }
        public void DrawGraphic(ObservableCollection<CollectionGraphs> graphicsList, bool collor, int band)
        {
            //Grid gridRow = null;
            //gridRow.ColumnDefinitions.Clear();
            //gridRow.Children.Clear();
            //gridRow.RowDefinitions.Clear();
            BigGridRow.RowDefinitions.Clear();
            BigGridRow.Children.Clear();
            BigGridRow.ColumnDefinitions.Clear();

            GridForButton.RowDefinitions.Clear();
            GridForButton.Children.Clear();
            GridForButton.ColumnDefinitions.Clear();
            


            //GridForButton.RowDefinitions.Clear();
            //GridForButton.Children.Clear();
            //GridForButton.ColumnDefinitions.Clear();
            
            var flag = false;
            UserControl.Height = 140;
            BigGridRow.Height = 80;
            GridForButton.Height = 80;
            var k = 0;
            var j = 0;
           
            

            foreach (var graphse in graphicsList)
            {
                 


                for (var i = 0; i < graphse.GraphicsesCollection.Count; i++)
                {
                    
                    for (var l = i+1; l < graphse.GraphicsesCollection.Count; l++)
                    {
                        if (graphse.GraphicsesCollection[i].Distance > graphse.GraphicsesCollection[l].Distance)
                        {
                            var temp1 = graphse.GraphicsesCollection[i].Distance;
                            var temp2 = graphse.GraphicsesCollection[i].GeneralState;
                            var temp3 = graphse.GraphicsesCollection[i].IntensityN1;
                            graphse.GraphicsesCollection[i].Distance = graphse.GraphicsesCollection[l].Distance;
                            graphse.GraphicsesCollection[i].GeneralState = graphse.GraphicsesCollection[l].GeneralState;
                            graphse.GraphicsesCollection[i].IntensityN1 = graphse.GraphicsesCollection[l].IntensityN1;
                            graphse.GraphicsesCollection[l].Distance = temp1;
                            graphse.GraphicsesCollection[l].GeneralState = temp2;
                            graphse.GraphicsesCollection[l].IntensityN1 = temp3;
                        }   
                    }
                }
                var gridRow = new Grid();
                if (flag)
                {
                    UserControl.Height += 75;
                    BigGridRow.Height += 50;
                    GridForButton.Height +=  50;
                }
                gridRow.RowDefinitions.Add(new RowDefinition());
                gridRow.RowDefinitions.Add(new RowDefinition());

                for (var i = 0; i < graphse.GraphicsesCollection.Count; i++)
                {
                    gridRow.ColumnDefinitions.Add(new ColumnDefinition());
                   
                    if (graphse.GraphicsesCollection[i].GeneralState &&
                        graphse.GraphicsesCollection[i].IntensityN1 >=_densityStandarts)
                    {
                        var greenRect1 = new Rectangle {Fill = Brushes.Green};
                        gridRow.Children.Add(greenRect1);
                        Grid.SetRow(greenRect1, 0);
                        Grid.SetColumn(greenRect1, i);
                    }
                    else
                    {
                        var redRect = new Rectangle { Fill = Brushes.Red };
                        gridRow.Children.Add(redRect);
                        Grid.SetRow(redRect, 0);
                        Grid.SetColumn(redRect, i);
                    }
                }
                if (parametr == 0)
                {
                    parametr  = graphse.GraphicsesCollection.Count;
                }
                var st = graphse.GraphicsesCollection.Count/parametr;
                if (st == 0)
                {
                    st = 1;
                }
                for (var i = 0; i < graphse.GraphicsesCollection.Count; i += st)
                {

                    var label = new Label
                    {
                        Content = graphse.GraphicsesCollection[i].Distance,
                        Foreground = collor ? Brushes.White : Brushes.Black
                    };
                    label.Content = graphse.GraphicsesCollection[i].Distance;
                    gridRow.Children.Add(label);
                    Grid.SetRow(label, 1);
                    Grid.SetColumn(label, i);
                    Grid.SetColumnSpan(label, 5);
                }
                flag = true;
                //UserControl.Add(gridRow);
                BigGridRow.RowDefinitions.Add(new RowDefinition());
                GridForButton.RowDefinitions.Add(new RowDefinition());
                
                BigGridRow.Children.Add(gridRow);

                //var Listbutton = new Button[] {};
                //var button = new Button();
                button = new Button();
                Grid.SetRow(gridRow,k);
                button.Width = 30;
                button.Height = 30;
                //button.Command = new RelayCommand(SelectBand);
                button.Click += SelectBand;
                button.Content = graphse.Band;

                GridForButton.Children.Add(button);
                Grid.SetRow(button,j);
                
                //Grid.SetRowSpan(Button, 2);
                k += 1;
                j += 1;

            }
        }

        public void SelectBand(object sender, RoutedEventArgs e)
        {
            var o = e.Source as Button;
            if (o != null) SendBand(o.Content);
        }

        public void SendBand(object selectedband)
        {
            Messenger.Default.Send(selectedband, "SelectedBand");
        }
    }
}