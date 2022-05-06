﻿using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Threading.Tasks;

#if WINDOWS
using System.Windows.Forms;
#endif

namespace libfintx
{
    /// <summary>
    /// Helper object needed for entering a TAN.
    /// </summary>
    public class TANDialog
    {
#if WINDOWS
        public PictureBox PictureBox { get; }
#else
        public object PictureBox { get; }
#endif

        public bool RenderFlickerCodeAsGif { get; }

        public int FlickerWidth { get; }

        public int FlickerHeight { get; }

        public HBCIDialogResult DialogResult { get; internal set; }

        public Image<Rgba32> FlickerImage { get; internal set; }

        public Image<Rgba32> MatrixImage { get; internal set; }

        private readonly Func<TANDialog, Task<string>> _waitForTanAsync;

        /// <summary>
        /// Render Flickercode as GIF.
        /// </summary>
        /// <param name="waitForTanAsync"></param>
        /// <param name="dialogResult"></param>
        /// <param name="flickerImage"></param>
        /// <param name="flickerWidth"></param>
        /// <param name="flickerHeight"></param>
        public TANDialog(Func<TANDialog, Task<string>> waitForTanAsync, int flickerWidth = 320, int flickerHeight = 120)
            : this(waitForTanAsync)
        {
            RenderFlickerCodeAsGif = true;
            FlickerWidth = flickerWidth;
            FlickerHeight = flickerHeight;
        }

        /// <summary>
        /// Render TANCode (Flicker/Matrix) in WinForms.
        /// </summary>
        /// <param name="waitForTanAsync"></param>
        /// <param name="dialogResult"></param>
        /// <param name="pictureBox"></param>
        public TANDialog(Func<TANDialog, Task<string>> waitForTanAsync, object pictureBox)
            : this(waitForTanAsync)
        {
            PictureBox = pictureBox;
        }


        /// <summary>
        /// Enter a TAN without any visual components, e.g. pushTAN or mobileTAN.
        /// </summary>
        /// <param name="waitForTanAsync">Function which takes a </param>
        /// <param name="dialogResult"></param>
        /// <param name="matrixImage"></param>
        public TANDialog(Func<TANDialog, Task<string>> waitForTanAsync)
        {
            _waitForTanAsync = waitForTanAsync;
        }

        /// <summary>
        /// Wait for the user to enter a TAN.
        /// </summary>
        /// <param name="dialogResult">The <code>HBCIDialogResult</code> from the bank which requests the TAN. Can be used to display bank messages in the dialog.</param>
        /// <returns></returns>
        internal async Task<string> WaitForTanAsync()
        {
            return await _waitForTanAsync?.Invoke(this);
        }
    }
}
