using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AForgeWebcamPlayerWinForms
{
    public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
            OpenLocalWebcamVideo();
		}

        private void OpenLocalWebcamVideo()
        {
            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm( );

            if ( form.ShowDialog( this ) == DialogResult.OK )
            {
                VideoCaptureDevice videoSource = form.VideoDevice;
                OpenVideoSource( videoSource );
            }
        }

        private void OpenVideoSource( IVideoSource source )
        {
            this.Cursor = Cursors.WaitCursor;
            CloseCurrentVideoSource( );

            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start( );

            this.Cursor = Cursors.Default;
        }

        // Close video source if it is running
        private void CloseCurrentVideoSource( )
        {
            if ( videoSourcePlayer.VideoSource != null )
            {
                videoSourcePlayer.SignalToStop( );

                // wait ~ 3 seconds
                for ( int i = 0; i < 30; i++ )
                {
                    if ( !videoSourcePlayer.IsRunning )
                        break;
                    System.Threading.Thread.Sleep( 100 );
                }

                if ( videoSourcePlayer.IsRunning )
                {
                    videoSourcePlayer.Stop( );
                }

                videoSourcePlayer.VideoSource = null;
            }
        }

        // New frame received by the player
        private void videoSourcePlayer_NewFrame( object sender, ref Bitmap image )
        {
            DateTime now = DateTime.Now;
            Graphics g = Graphics.FromImage( image );

            // paint current time
            SolidBrush brush = new SolidBrush( Color.Red );
            g.DrawString( now.ToString( ), this.Font, brush, new PointF( 5, 5 ) );
            brush.Dispose( );

            g.Dispose( );
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseCurrentVideoSource();
        }
    }
}
