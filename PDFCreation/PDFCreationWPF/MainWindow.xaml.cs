using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace PDFCreationWPF
{
	public partial class MainWindow : Window
	{
        //
        // Doku und Beispiele:
        // http://pdfsharp.net/wiki/PDFsharpSamples.ashx
        //


        //
	    // Hilfe bei dieser Fehlermeldung:
	    // 
	    // Der Typ "FontFamily" ist in einer nicht referenzierten Assembly definiert. 
	    // Fügen Sie einen Verweis auf die Assembly "System.Drawing.Common, 
	    // Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51" hinzu.
	    //
	    // das ist zu tun:
	    //
	    // Auf XFont gehen, Strg + ., dann bietet er an:
	    // "System.Common.Drawing installieren"
        //


        //
        // Hilfe bei dieser Fehlermeldung:
        // System.NotSupportedException: "No data is available for encoding 1252. 
        // For information on defining a custom encoding, see the documentation 
        // for the Encoding.RegisterProvider method."
        // 
	    //
	    // das ist zu tun:
        // 1. Nuget-Paket "System.Text.Encoding.CodePages" installieren
	    // 2. diesen Code hinzufügen:
        //    var codePageProvider = CodePagesEncodingProvider.Instance;
        //    Encoding.RegisterProvider(codePageProvider);
        //

		public MainWindow()
		{
            var codePageProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(codePageProvider);

			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			// Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";
 
            // Create an empty page
            PdfPage page = document.AddPage();
 
            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
 
            // Create a font
            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
 
            // Draw the text
            gfx.DrawString("Hello, World!", font, XBrushes.Black,
              new XRect(0, 0, page.Width, page.Height),
              XStringFormats.Center);
 
            // Save the document...
            const string filename = "HelloWorld.pdf";
            document.Save(filename);
            
            // ...and start a viewer.
            Process.Start("explorer.exe", filename);
		}
	}
}
