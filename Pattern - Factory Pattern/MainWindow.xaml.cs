using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pattern___Factory_Pattern
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // using a factory to "produce" objects
            var _shapes = new List<Shape>();
            _shapes.Add(ShapeFactory.CreateLine());
            _shapes.Add(ShapeFactory.CreateTriangle());
            _shapes.Add(ShapeFactory.CreateRectangle());
            _shapes.Add(ShapeFactory.CreatePolygon());
            _shapes.Add(ShapeFactory.CreateCircle());
        }
    }

    public class ShapeFactory
    {
        public static Shape CreateLine()
        {
            var line = new Line() { X1 = 0, Y1 = 0, X2 = 100, Y2 = 150, StrokeThickness = 5, Stroke = Brushes.Black };
            return line;
        }

        public static Shape CreateTriangle()
        {
            var polygon = new Polygon() { StrokeThickness = 1,  Stroke = Brushes.Black, Fill = Brushes.DarkBlue };
            polygon.Points.Add(new Point(0  ,100));
            polygon.Points.Add(new Point(100,100));
            polygon.Points.Add(new Point(50 ,0  ));
            polygon.Points.Add(new Point(0  ,100));
            return polygon;
        }

        public static Shape CreateRectangle()
        {
            var polygon = new Polygon() { StrokeThickness = 1,  Stroke = Brushes.Black, Fill = Brushes.DarkRed };
            polygon.Points.Add(new Point(0  ,100));
            polygon.Points.Add(new Point(100,100));
            polygon.Points.Add(new Point(100,  0));
            polygon.Points.Add(new Point(0  ,  0));
            return polygon;
        }

        public static Shape CreatePolygon()
        {
            var polygon = new Polygon() { StrokeThickness = 1,  Stroke = Brushes.Black, Fill = Brushes.DarkGreen };
            polygon.Points.Add(new Point(  0,  0));
            polygon.Points.Add(new Point( 20, 50));
            polygon.Points.Add(new Point(  0,100));
            polygon.Points.Add(new Point( 50, 80));
            polygon.Points.Add(new Point(100,100));
            polygon.Points.Add(new Point( 80, 50));
            polygon.Points.Add(new Point(100,  0));
            polygon.Points.Add(new Point( 50, 20));
            polygon.Points.Add(new Point(  0,  0));
            return polygon;
        }

        public static Shape CreateCircle()
        {
            var circle = new Ellipse() { Width = 100, Height = 100, StrokeThickness = 1,  Stroke = Brushes.Black, Fill = Brushes.DarkCyan };
            return circle;
        }
    }
}
