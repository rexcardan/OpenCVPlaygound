using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionsModule.Attributes;

namespace ActionsModule.Actions
{
    [Category("Preprocessing")]
    public class UnsharpMaskImage : ImageAction
    {
        public UnsharpMaskImage()
        {
            this.Name = "Unsharp Mask";
            this.Action = (m) =>
            {
                var gauss = m.GaussianBlur(new Size(this.GaussSize, this.GaussSize), this.GaussSigmaX);

                Cv2.AddWeighted(m, this.Alpha, gauss, -1 * this.Beta, 0, m);
                gauss.Dispose();

                HasError = false;
                return m;
            };
        }
        
        double gaussSigmaX = 3;
        [ImportExport]
        [Slider(0.01, 30, 2, isIntegerType: false)]
        public double GaussSigmaX
        {
            get
            {
                return gaussSigmaX;
            }

            set
            {
                SetProperty(ref gaussSigmaX, value);
            }
        }

        double gaussSize = 0.0;
        [ImportExport]
        [Slider(0.00, 0.99, 2, isIntegerType: false)]
        public double GaussSize
        {
            get
            {
                return gaussSize;
            }

            set
            {
                SetProperty(ref gaussSize, value);
            }
        }
        
        double beta = 0.5;
        [ImportExport]
        [Slider(0.00, 10.0, 2, isIntegerType: false)]
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
        
        double alpha = 1.5;
        [ImportExport]
        [Slider(0.00, 10.0, 2, isIntegerType: false)]
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
    }
}
