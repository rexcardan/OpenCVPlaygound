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

                var alpha = 1 + this.Intensity;
                var beta = -1 * (this.Intensity + this.Gamma);

                Cv2.AddWeighted(m, alpha, gauss, beta, 0, m);
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
        
        double intensity = 1;
        [ImportExport]
        [Slider(0.00, 30.0, 2, isIntegerType: false)]
        public double Intensity
        {
            get
            {
                return intensity;
            }

            set
            {
                SetProperty(ref intensity, value);
            }
        }
        
        double gamma = 0.0;
        [ImportExport]
        [Slider(0.00, 5.0, 2, isIntegerType: false)]
        public double Gamma
        {
            get
            {
                return gamma;
            }

            set
            {
                SetProperty(ref gamma, value);
            }
        }
        
    }
}
