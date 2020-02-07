using ActionsModule.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Actions
{
    public class CannyAction : ImageAction
    {
        private double threshold1 = 80.0;
        private double threshold2 = 160.0;
        private int apSize = 3;

        public CannyAction()
        {
            Name = "Canny";
            Action = (im) =>
             {
                 var canny = im.Canny(Threshold1, Threshold2, AperatureSize);
                 im.Dispose();
                 return canny;
             };
        }

        [ImportExport]
        [Slider(0, 255)]
        public double Threshold1
        {
            get { return threshold1; }
            set
            {
                SetProperty(ref threshold1, value);
            }
        }

        [ImportExport]
        [Slider(0, 255)]
        public double Threshold2
        {
            get { return threshold2; }
            set
            {
                SetProperty(ref threshold2, value);
            }
        }

        [ImportExport]
        [Slider(3, 7,2)]
        public int AperatureSize
        {
            get { return apSize; }
            set
            {
                SetProperty(ref apSize, value);
            }
        }
    }
}
