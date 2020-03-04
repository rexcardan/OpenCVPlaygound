using ActionsModule.Attributes;
using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ActionsModule.Actions
{
    [Category("Transformations")]
    public class CropAction : ImageAction
    {
        public CropAction()
        {
            this.Name = "Crop";
            this.Action = (input) =>
            {
                var roi = new Rect((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
                var result = new Mat(input, roi);
                input.Dispose();
                return result;
            };
        }

        private double x = 0;
        [ImportExport]
        [Slider(0, 3000)]
        public double X
        {
            get { return this.x; }
            set
            {
                SetProperty(ref this.x, value);
            }
        }

        private double y = 0;
        [ImportExport]
        [Slider(0, 3000)]
        public double Y
        {
            get { return this.y; }
            set
            {
                SetProperty(ref this.y, value);
            }
        }

        private double width = 100;
        [ImportExport]
        [Slider(0, 3000)]
        public double Width
        {
            get { return this.width; }
            set
            {
                SetProperty(ref this.width, value);
            }
        }

        private double height = 100;
        [ImportExport]
        [Slider(0, 3000)]
        public double Height
        {
            get { return this.height; }
            set
            {
                SetProperty(ref this.height, value);
            }
        }
    }
}
