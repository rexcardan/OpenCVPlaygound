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
    public class HoughCircles : ImageAction
    {
        private double minDist = 5.0;
        private double dp = 255;
        private double param1 = 100;
        private double param2 = 100;
        private int minRadius = 0;
        private int maxRadius = 0;
        private Color clr = Colors.Red;

        private HoughMethods method = HoughMethods.Gradient;

        public HoughCircles()
        {
            this.Name = "Hough Circles";
            this.Action = (m) =>
            {
                if (m.Channels() != 1)
                {
                    m = m.CvtColor(ColorConversionCodes.BGR2GRAY);
                }
                var circles = m.HoughCircles(method, dp, minDist, param1, param2, minRadius, maxRadius);
                var newImage = m.CvtColor(ColorConversionCodes.GRAY2BGR);
                m.Dispose();

                foreach (var circ in circles)
                {
                    newImage.Circle(new Point(circ.Center.X, circ.Center.Y), (int)circ.Radius, new Scalar(clr.B, clr.G, clr.R));
                }
                HasError = false;
                return newImage;
            };
        }

        [Slider(0, 255)]
        public double DP
        {
            get { return dp; }
            set
            {
                SetProperty(ref dp, value);
            }
        }

        [Slider(0, 500)]
        public double MinDist
        {
            get { return minDist; }
            set
            {
                SetProperty(ref minDist, value);
            }
        }

        [Slider(0, 500)]
        public double Param1
        {
            get { return param1; }
            set
            {
                SetProperty(ref param1, value);
            }
        }

        [Slider(0, 500)]
        public double Param2
        {
            get { return param2; }
            set
            {
                SetProperty(ref param2, value);
            }
        }

        [Slider(0, 100)]
        public int MinRadius
        {
            get { return minRadius; }
            set
            {
                SetProperty(ref minRadius, value);
            }
        }

        [Slider(0, 500)]
        public int MaxRadius
        {
            get { return maxRadius; }
            set
            {
                SetProperty(ref maxRadius, value);
            }
        }

        [Enum(typeof(HoughMethods))]
        public HoughMethods HoughMethod
        {
            get { return method; }
            set
            {
                SetProperty(ref method, value);
            }
        }

        [RGBColor]
        public Color Color
        {
            get { return clr; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref clr, value);
                }
            }
        }
    }
}
