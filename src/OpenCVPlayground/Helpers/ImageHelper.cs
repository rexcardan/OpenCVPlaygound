using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace OpenCVPlayground.Helpers
{
    public class ImageHelper
    {
        public static void Write(Mat image, WriteableBitmap bmp)
        {
            if (image.Rows != bmp.PixelHeight || image.Cols != bmp.PixelWidth)
            {
                Cv2.Resize(image, image, new OpenCvSharp.Size(bmp.PixelWidth, bmp.PixelHeight));
            }
            var pixels = GetPixels(image);
            var stride = image.Cols * image.ElemSize();
            bmp.WritePixels(
                    new Int32Rect(0, 0, bmp.PixelWidth, bmp.PixelHeight),
                    pixels,
                    stride,
                    0);
        }

        public static byte[] GetPixels(Mat image)
        {
            byte[] pixels = new byte[image.Width * image.ElemSize() * image.Height];
            Marshal.Copy(image.Data, pixels, 0, pixels.Length);
            return pixels;
        }
    }
}
