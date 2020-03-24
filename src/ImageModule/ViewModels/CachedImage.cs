using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace ImageModule.ViewModels
{
    public class CachedImage : IDisposable
    {
        public static double PreviewSize = 80;

        private readonly Mat image;

        private CachedImage(Mat image, BitmapSource preview)
        {
            this.image = image;
            this.Preview = preview;
        }

        public static CachedImage[] FromFiles(string[] files)
        {
            return files
                    .Select(FromFile)
                    .ToArray();
        }

        public static CachedImage FromFile(string file)
        {
            var image = new Mat(file);
            var resizeFactor = image.Width > image.Height
                                ? PreviewSize / image.Width
                                : PreviewSize / image.Height;
            var previewW = image.Width * resizeFactor;
            var previewH = image.Height * resizeFactor;
            using (var resized = image.Resize(new Size(previewW, previewH)))
            {
                var preview = resized.ToBitmapSource();
                var result = new CachedImage(image, preview);
                return result;
            }
        }

        public BitmapSource Preview { get; }

        public void Dispose()
        {
            this.image?.Dispose();
        }

        public Mat GetCopy()
        {
            return this.image.Clone();
        }
    }
}
