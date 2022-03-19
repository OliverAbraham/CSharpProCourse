using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Webcam
{
	public partial class MainWindow : Window
	{
		private VideoCaptureDevice _videoCaptureDevice;
		private FilterInfoCollection _filterInfoCollection;

        public MainWindow()
		{
			InitializeComponent();
            FillComboboxAvailableWebcams();
		}

        private void FillComboboxAvailableWebcams()
        {
            _filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterinfo in _filterInfoCollection)
                comboboxAvailableWebcams.Items.Add(filterinfo.Name);

            if (!comboboxAvailableWebcams.Items.IsEmpty)
                comboboxAvailableWebcams.SelectedIndex = 0;

            _videoCaptureDevice = new VideoCaptureDevice();
        }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
            var cameraMoniker = _filterInfoCollection[comboboxAvailableWebcams.SelectedIndex].MonikerString;
            _videoCaptureDevice = new VideoCaptureDevice(cameraMoniker);
            _videoCaptureDevice.NewFrame += new NewFrameEventHandler(NewFrame);
            _videoCaptureDevice.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_videoCaptureDevice.IsRunning)
                _videoCaptureDevice.Stop();
        }

        private void NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone(); 

            Dispatcher.BeginInvoke( new Action(() => 
            {
                SetImage(bitmap);
            }));
		}

        public void SetImage(Bitmap bitmap)
        {
            var handle = bitmap.GetHbitmap();
            try
            {
                var image = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                currentImage.Source = image;
            }
            finally 
            { 
                DeleteObject(handle); 
            }
        }

        //If you get 'dllimport unknown'-, then add 'using System.Runtime.InteropServices;'
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);
    }
}
