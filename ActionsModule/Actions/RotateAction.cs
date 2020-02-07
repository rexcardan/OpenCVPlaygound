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
    public class RotateAction : ImageAction
    {
        public RotateAction()
        {
            this.Name = "Rotate";
            this.Action = (input) =>
            {
                var transformMatrix = Cv2.GetRotationMatrix2D(new Point2f(input.Width / 2, input.Height / 2), 0 - this.Degrees, 1);
                var size = input.Size();
                var result = input.WarpAffine(transformMatrix, size);
                var cx = size.Width * (1 - this.Crop) / 2;
                var cy = size.Height * (1 - this.Crop) / 2;
                var roi = new Rect((int)cx, (int)cy, (int)(size.Width * this.Crop), (int)(size.Height * this.Crop));
                return new Mat(result, roi);
            };
        }

        private double degrees = 0;
        [Slider(-360, 360, 0.01, isIntegerType: false)]
        public double Degrees
        {
            get { return this.degrees; }
            set
            {
                SetProperty(ref this.degrees, value);
            }
        }
        
        private double crop = 1.0;
        [Slider(0.1, 1.0, 0.01, isIntegerType: false)]
        public double Crop
        {
            get { return this.crop; }
            set
            {
                SetProperty(ref this.crop, value);
            }
        }
    }
}
