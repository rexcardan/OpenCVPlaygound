using ActionsModule.Attributes;
using ActionsModule.Controls;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Actions
{
    [Category("Preprocessing")]
    public class ThresholdAction : ImageAction
    {
        private double threshold = 0.0;
        private double maxVal = 255;
        private ThresholdTypes tt = ThresholdTypes.Binary;

        public ThresholdAction()
        {
            this.Name = "Threshold";
            this.Action = (m) =>
            {
                var mat = m.Threshold(Threshold, MaxVal, ThresholdType);
                return mat;
            };
        }

        [ImportExport]
        [Slider(0, 255)]
        public double Threshold
        {
            get { return threshold; }
            set
            {
                SetProperty(ref threshold, value);
            }
        }

        [ImportExport]
        [Slider(0, 255)]
        public double MaxVal
        {
            get { return maxVal; }
            set
            {
                SetProperty(ref maxVal, value);
            }
        }

        [ImportExport]
        [Enum(typeof(ThresholdTypes))]
        public ThresholdTypes ThresholdType
        {
            get { return tt; }
            set
            {
                SetProperty(ref tt, value);
            }
        }
    }
}
