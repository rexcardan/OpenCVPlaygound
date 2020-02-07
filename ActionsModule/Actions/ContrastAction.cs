using ActionsModule.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Actions
{
    public class ContrastAction : ImageAction
    {
        private double alpha = 1;
        private double beta = 0;

        public ContrastAction()
        {
            this.Name = "Contrast";
            this.Action = (m) =>
            {
                var contrast = m.EmptyClone();
                m.ConvertTo(contrast, MatType.CV_8UC1, Alpha, Beta);
                m.Dispose();
                HasError = false;
                return contrast;
            };
        }

        [Slider(0, 10, 0.1)]
        public double Alpha
        {
            get
            {
                return alpha;
            }

            set
            {
                SetProperty(ref alpha, value);
            }
        }

        [Slider(-1000, 1000, 2)]
        public double Beta
        {
            get
            {
                return beta;
            }

            set
            {
                SetProperty(ref beta, value);
            }
        }
    }
}
