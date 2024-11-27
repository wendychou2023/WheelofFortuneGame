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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1155116677_ProjectI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WheelofFortune currentGame;
        int money = 1000;
        public MainWindow()
        {
            InitializeComponent();
            if (currentGame == null)
            {
                currentGame = new WheelofFortune(canvas, textBlock, money, myGrid);
            }
            textBlockRule.Text = "Green: $0   Blue: $10\n" + "Red: $100   Gray: -$500";
        }

        
        private void startButton_Click(object sender, RoutedEventArgs e)
        {            
            currentGame.StartSpin();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            currentGame.StopSpin();  
        }      
    }


    public class WheelofFortune
    {                
        Color[] colors = new Color[] { Colors.Gray, Colors.Blue, Colors.Red, Colors.Green };
        Dictionary<Color, int> dict = new Dictionary<Color, int>();
        Canvas canvas;
        TextBlock textBlock;        
        int money;        
        Path[] paths;
        Grid myGrid;

        public WheelofFortune(Canvas canvas, TextBlock textBlock, int money, Grid myGrid)
        {
            this.canvas = canvas;
            this.textBlock = textBlock;
            this.money = money;
            UpdateWallet(money);
            this.myGrid = myGrid;

            dict.Add(colors[0], -500);
            dict.Add(colors[1], 10);
            dict.Add(colors[2], 100);
            dict.Add(colors[3], 0);                     
            
            CreateWheel();
        }

        public void StartSpin()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.To = 360;
            da.From = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            RotateTransform rt = new RotateTransform();
            canvas.RenderTransform = rt;
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            da.RepeatBehavior = RepeatBehavior.Forever;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        int spinTo;
        public void StopSpin()
        {            
            DoubleAnimation da2 = new DoubleAnimation();
            Random rdn = new Random();
            spinTo = rdn.Next(720, 1080);
            da2.To = spinTo;
            da2.From = 0;
            da2.Duration = new Duration(TimeSpan.FromSeconds(5));
            RotateTransform rt2 = new RotateTransform();
            canvas.RenderTransform = rt2;
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            da2.SpeedRatio = 4;
            da2.DecelerationRatio = 0.9;
            da2.RepeatBehavior = new RepeatBehavior(1);
            da2.Completed += new EventHandler(animCompleted);
            rt2.BeginAnimation(RotateTransform.AngleProperty, da2);            
        }

        private void animCompleted(object sender, EventArgs e)
        {
            CalculateScore();
        }

        private void CalculateScore()
        {
            //angle in radian
            double angle = Math.PI/((double)180/(double)(-90 - spinTo));
            Point point = new Point(75 + 30 * Math.Cos(angle), 75 + 30 * Math.Sin(angle));
            int index = -1;
            
            for (int i=0; i<4; i++)
            {
                if (paths[i].Data.FillContains(point))
                {
                    index = i;
                    break;
                }       

            }

            Color pointingColor = colors[index];
            money = money + dict[pointingColor];
            UpdateWallet(money);
        }

        private void UpdateWallet(int money) 
        {
            this.money = money;
            textBlock.Text = "You have $" + money;

            if (money < 0 || money==0)
            {
                MessageBox.Show("You don't have money! Game Over!!");
                Reset();
            }
        }

        private void Reset()
        {
            canvas.Children.Clear();
            CreateWheel();
            UpdateWallet(1000);
        }
        
        
        private void CreateWheel()
        {
            // Random number represents the number of triangle that each of the colors has
            // Each color has at least one triangle. There are a total of 100 triangles.
            Random rand = new Random();
            List<int> list = new List<int>();
            int total = 360;
            int sum = 0;
            list.Add(rand.Next(90, 135));
            list.Add(rand.Next(45, 90));
            list.Add(rand.Next(45, 90));
            sum = list[0] + list[1] + list[2];
            list.Add(total - sum);

            list[1] += list[0];
            list[2] += list[1];
            list[3] += list[2];

            List<double> angle = new List<double>();
            angle.Add(Math.PI / ((double)180 / (double)list[0]));
            angle.Add(Math.PI / ((double)180 / (double)list[1]));
            angle.Add(Math.PI / ((double)180 / (double)list[2]));
            angle.Add(Math.PI / ((double)180 / (double)list[3]));

            paths = new Path[4];
            for (int i = 0; i < 4; i++)
            {
                // Create an ArcSegment to define the geometry of the path.
                ArcSegment myArcSegment = new ArcSegment();
                myArcSegment.Size = new Size(75, 75);
                myArcSegment.SweepDirection = SweepDirection.Clockwise;
                myArcSegment.Point = new Point(75 + 75 * Math.Cos(angle[i]), 75 + 75 * Math.Sin(angle[i]));

                PathSegmentCollection myPathSegmentCollection = new PathSegmentCollection();
                myPathSegmentCollection.Add(myArcSegment);

                // Create a PathFigure to be used for the PathGeometry of myPath.
                PathFigure myPathFigure = new PathFigure();

                // Set the starting point for the PathFigure 
                if (i == 0)
                {
                    myPathFigure.StartPoint = new Point(75 + 75 * Math.Cos(0), 75 + 75 * Math.Sin(0));
                }
                else
                {
                    myPathFigure.StartPoint = new Point(75 + 75 * Math.Cos(angle[i - 1]), 75 + 75 * Math.Sin(angle[i - 1]));
                }

                myPathFigure.Segments = myPathSegmentCollection;
                //Create ArcSegment ends here

                //Create 2 LineSegments to form a sector 
                PathFigure myLinePathFigure1 = new PathFigure();
                if (i == 0)
                {
                    myLinePathFigure1.StartPoint = new Point(75 + 75 * Math.Cos(0), 75 + 75 * Math.Sin(0));
                }
                else
                {
                    myLinePathFigure1.StartPoint = new Point(75 + 75 * Math.Cos(angle[i - 1]), 75 + 75 * Math.Sin(angle[i - 1]));
                }

                LineSegment myLineSegment1 = new LineSegment();
                myLineSegment1.Point = new Point(75, 75);

                //PathFigure myLinePathFigure2 = new PathFigure();

                LineSegment myLineSegment2 = new LineSegment();
                myLineSegment2.Point = new Point(75 + 75 * Math.Cos(angle[i]), 75 + 75 * Math.Sin(angle[i]));

                PathSegmentCollection myLinePathSegmentCollection = new PathSegmentCollection();
                myLinePathSegmentCollection.Add(myLineSegment1);
                myLinePathSegmentCollection.Add(myLineSegment2);

                myLinePathFigure1.Segments = myLinePathSegmentCollection;
                //Create 2 LineSegments ends here

                PathFigureCollection myPathFigureCollection = new PathFigureCollection();
                myPathFigureCollection.Add(myPathFigure);
                myPathFigureCollection.Add(myLinePathFigure1);

                PathGeometry myPathGeometry = new PathGeometry();
                myPathGeometry.Figures = myPathFigureCollection;


                // Create a path to draw a geometry with.
                paths[i] = new Path();
                paths[i].Stroke = Brushes.Black;
                paths[i].StrokeThickness = 1;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();
                mySolidColorBrush.Color = colors[i];
                paths[i].Fill = mySolidColorBrush;

                // specify the shape of the path using the path geometry.
                paths[i].Data = myPathGeometry;
                canvas.Children.Add(paths[i]);

            }

            //The purple line that indicates where the wheel stops
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Purple;
            myLine.X1 = 393;
            myLine.X2 = 393;
            myLine.Y1 = 105;
            myLine.Y2 = 65;
            myLine.StrokeThickness = 3;
            myGrid.Children.Add(myLine);

        }


    }
}
