using NumberToLcd;
using System.Windows.Controls;
using System.Windows.Media;

namespace MyControls
{
	public partial class SegmentDisplay : UserControl
    {
        private SolidColorBrush _FillColor  = new SolidColorBrush(Color.FromRgb(0xA6, 0x04, 0x21));
        private SolidColorBrush _EmptyColor = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE));

        public SegmentDisplay()
        {
            InitializeComponent();
        }

        public int Value
        {
            set
            {
                NumberToLcdConverter Converter = new NumberToLcdConverter();
                Converter.Convert(value);
                this.a.Fill = (Converter.OutputLines[0][1] != ' ') ? _FillColor : _EmptyColor ;
                this.f.Fill = (Converter.OutputLines[1][0] != ' ') ? _FillColor : _EmptyColor ;
                this.b.Fill = (Converter.OutputLines[1][2] != ' ') ? _FillColor : _EmptyColor ;
                this.g.Fill = (Converter.OutputLines[2][1] != ' ') ? _FillColor : _EmptyColor ;
                this.e.Fill = (Converter.OutputLines[3][0] != ' ') ? _FillColor : _EmptyColor ;
                this.c.Fill = (Converter.OutputLines[3][2] != ' ') ? _FillColor : _EmptyColor ;
                this.d.Fill = (Converter.OutputLines[4][1] != ' ') ? _FillColor : _EmptyColor ;
            }
        }
    }
}
