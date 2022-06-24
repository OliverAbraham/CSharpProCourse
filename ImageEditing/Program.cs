using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSharpBildbearbeitung
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Image<Rgba32> image = Image.Load("test1.bmp"))
            {
                // image is now in a file format agnositic structure in memory as a series of Rgba32 pixels

                // resize the image in place and return it for chaining
                image.Mutate(ctx => ctx.Resize(image.Width * 2, image.Height * 2));

                // based on the file extension pick an encoder then encode and write the data to disk
                image.Save("test1a.jpg"); 
            }
        }
    }
}
