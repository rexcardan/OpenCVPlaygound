using ActionsModule.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Actions
{
    public class AdaptiveThresholdAction : ImageAction
    {
        private double maxVal = 255;
        private AdaptiveThresholdTypes at = AdaptiveThresholdTypes.GaussianC;
        private ThresholdTypes tt = ThresholdTypes.Binary;
        private int blockSize = 3;
        private int subtraction = 0;

        public AdaptiveThresholdAction()
        {
            this.Name = "Adaptive Threshold";
            this.Action = (m) =>
            {
                try
                {
                    var adp= m.AdaptiveThreshold(MaxVal, AdaptiveThresholdType, ThresholdType,BlockSize, Subtraction);
                    m.Dispose();
                    HasError = false;
                    return adp;
                }
                catch (Exception)
                {
                    HasError = true;
                }
                return m;
            };
        }

        [Slider(0, 255)]
        public double MaxVal
        {
            get { return maxVal; }
            set
            {
                SetProperty(ref maxVal, value);
            }
        }

        [Enum(typeof(AdaptiveThresholdTypes))]
        public AdaptiveThresholdTypes AdaptiveThresholdType
        {
            get { return at; }
            set
            {
                SetProperty(ref at, value);
            }
        }

        [Enum(typeof(ThresholdTypes))]
        public ThresholdTypes ThresholdType
        {
            get { return tt; }
            set
            {
                SetProperty(ref tt, value);
            }
        }

        [Slider(3, 25, 2)]
        public int BlockSize
        {
            get { return blockSize; }
            set
            {
                SetProperty(ref blockSize, value);
            }
        }

        [Slider(-100, 255)]
        public int Subtraction
        {
            get { return subtraction; }
            set
            {
                SetProperty(ref subtraction, value);
            }
        }
    }
}
