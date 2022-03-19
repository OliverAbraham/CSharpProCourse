using NumberToLcd;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Digitaluhr
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _Timer;
        private int _Digit;
        private SolidColorBrush _FillColor  = new SolidColorBrush(Color.FromRgb(0xA6, 0x04, 0x21));
        private SolidColorBrush _EmptyColor = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE));

        public MainWindow()
        {
            InitializeComponent();
            _Timer = new DispatcherTimer();
            _Timer.Tick += Timer_Tick;
            _Timer.Interval = new TimeSpan(hours:0, minutes:0, seconds:1);
            _Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            NumberToLcdConverter Converter = new NumberToLcdConverter();
            Converter.Convert(_Digit);
            this.a.Fill = (Converter.OutputLines[0][1] != ' ') ? _FillColor : _EmptyColor ;
            this.f.Fill = (Converter.OutputLines[1][0] != ' ') ? _FillColor : _EmptyColor ;
            this.b.Fill = (Converter.OutputLines[1][2] != ' ') ? _FillColor : _EmptyColor ;
            this.g.Fill = (Converter.OutputLines[2][1] != ' ') ? _FillColor : _EmptyColor ;
            this.e.Fill = (Converter.OutputLines[3][0] != ' ') ? _FillColor : _EmptyColor ;
            this.c.Fill = (Converter.OutputLines[3][2] != ' ') ? _FillColor : _EmptyColor ;
            this.d.Fill = (Converter.OutputLines[4][1] != ' ') ? _FillColor : _EmptyColor ;
            _Digit = (++_Digit) % 10;
        }
    }
}
