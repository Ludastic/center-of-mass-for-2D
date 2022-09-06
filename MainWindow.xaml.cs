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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Shape> shapes = new List<Shape>();
        List<double> areas = new List<double>();
        double sum_area=0;
        public MainWindow()
        {
            InitializeComponent();
        }
        Point start, end;
        private void Main_canv_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            end = e.GetPosition(Main_canv);
            if (Rectan.IsChecked==true)
            {
                Inform.Content = "";
                Rectangle tempRect = new Rectangle();
                tempRect.Stroke = new SolidColorBrush(Colors.White);
                tempRect.StrokeThickness = 1;
                tempRect.Fill = new SolidColorBrush(Colors.Blue);
                tempRect.Width = Math.Abs(start.X - end.X);
                tempRect.Height = Math.Abs(start.Y - end.Y);
                Canvas.SetLeft(tempRect, Math.Min(start.X, end.X));
                Canvas.SetTop(tempRect, Math.Min(start.Y, end.Y));
                shapes.Add(tempRect);
                Main_canv.Children.Add(tempRect);
                areas.Add(tempRect.Width * tempRect.Height);
                sum_area += tempRect.Width * tempRect.Height;
            }
            else if (Elip.IsChecked==true)
            {
                Inform.Content = "";
                Ellipse tempEllipse = new Ellipse();
                tempEllipse.Stroke = new SolidColorBrush(Colors.White);
                tempEllipse.StrokeThickness = 1;
                tempEllipse.Fill = new SolidColorBrush(Colors.Blue);
                tempEllipse.Width = Math.Abs(start.X - end.X);
                tempEllipse.Height = Math.Abs(start.Y - end.Y);
                Canvas.SetLeft(tempEllipse, Math.Min(start.X, end.X));
                Canvas.SetTop(tempEllipse, Math.Min(start.Y, end.Y));
                shapes.Add(tempEllipse);
                Main_canv.Children.Add(tempEllipse);
                areas.Add(tempEllipse.Width/2 * tempEllipse.Height/2 * Math.PI);
                sum_area += tempEllipse.Width / 2 * tempEllipse.Height / 2 * Math.PI;
            }
            else
            {
                Inform.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Inform.Content = "Select figure type";
            }
            
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Main_canv.Children.Clear();
            shapes.Clear();
            areas.Clear();
            sum_area = 0;
        }
        private void Main_canv_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(Main_canv);
            for (int i=shapes.Count-1; i>-1; i--)
            {
                if (Canvas.GetLeft(shapes[i])<=position.X && Canvas.GetLeft(shapes[i])+shapes[i].Width>=position.X && Canvas.GetTop(shapes[i]) <= position.Y && Canvas.GetTop(shapes[i]) + shapes[i].Height >= position.Y)
                {
                    Main_canv.Children.Remove(shapes[i]);
                    shapes.RemoveAt(i);
                    sum_area -= areas[i];
                    areas.RemoveAt(i);
                    break;
                }
            }
        }
        private void Calculate_Click(object sender, RoutedEventArgs e)
        {
            if (shapes.Count == 0)
            {
                Inform.Foreground = new SolidColorBrush(Colors.Red);
                Inform.Content = "You need to draw\n at least one shape!";
            }
            else
            {
                Point[] centeres = new Point[shapes.Count];
                for (int i = 0; i < shapes.Count; i++)
                {
                    centeres[i] = new Point(shapes[i].Width / 2, shapes[i].Height / 2);
                }
                for (int i = 0; i < centeres.Length; i++)
                {
                    Ellipse tempEllipse = new Ellipse();
                    tempEllipse.Stroke = new SolidColorBrush(Colors.Red);
                    tempEllipse.StrokeThickness = 4;
                    tempEllipse.Fill = new SolidColorBrush(Colors.Red);
                    tempEllipse.Width = 5;
                    tempEllipse.Height = 5;
                    Canvas.SetLeft(tempEllipse, Canvas.GetLeft(shapes[i]) + centeres[i].X - tempEllipse.Width / 2);
                    Canvas.SetTop(tempEllipse, Canvas.GetTop(shapes[i]) + centeres[i].Y - tempEllipse.Height / 2);
                    Main_canv.Children.Add(tempEllipse);
                }
                Point main_center = new Point(0, 0);
                double verh = 0;
                for (int i = 0; i < shapes.Count; i++)
                {
                    verh += areas[i] * (centeres[i].X + Canvas.GetLeft(shapes[i]));
                }
                main_center.X = verh / sum_area;
                verh = 0;
                for (int i = 0; i < shapes.Count; i++)
                {
                    verh += areas[i] * (Canvas.GetTop(shapes[i]) + centeres[i].Y);
                }
                main_center.Y = verh / sum_area;
                Ellipse mainEllipse = new Ellipse();
                mainEllipse.Stroke = new SolidColorBrush(Colors.Yellow);
                mainEllipse.StrokeThickness = 5;
                mainEllipse.Fill = new SolidColorBrush(Colors.Yellow);
                mainEllipse.Width = 6;
                mainEllipse.Height = 6;
                Canvas.SetLeft(mainEllipse, main_center.X - mainEllipse.Width / 2);
                Canvas.SetTop(mainEllipse, main_center.Y - mainEllipse.Height / 2);
                Main_canv.Children.Add(mainEllipse);
                Inform.Foreground = new SolidColorBrush(Colors.Green);
                Inform.Content = "It's all okay!";
            }
        }
        private void Main_canv_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(Main_canv);
            
            Inform.Foreground = new SolidColorBrush(Colors.Yellow);
            Inform.Content = "Current Mouse position:\n X: "+ pos.X + "  Y: "+ Convert.ToInt32(pos.Y);
        }
        private void Main_canv_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Rectan.IsChecked == false && Elip.IsChecked == false)
            {
                Inform.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                Inform.Content = "Select figure type";
            }
            else
            {
                start = e.GetPosition(Main_canv);
            }
        }
    }
}
