using ActionsModule.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ActionsModule.Actions
{
    [Category("Recognition")]
    public class FindAndDrawContours : ImageAction
    {
        private RetrievalModes rv = RetrievalModes.List;
        private ContourApproximationModes cam = ContourApproximationModes.ApproxNone;
        private Color? clr = Colors.Red;

        public FindAndDrawContours()
        {
            Name = "Find and Draw Contours";
            Action = (im) =>
             {
                 Point[][] ctrs;
                 HierarchyIndex[] hi;
                 if (im.Channels() != 1)
                 {
                     im = im.CvtColor(ColorConversionCodes.BGR2GRAY);
                 }
                 im.FindContours(out ctrs, out hi, RetrievalMode, ContourApproximation);

                 Mat m = im.Clone();
                 m = im.CvtColor(ColorConversionCodes.GRAY2BGR);
                 var clr = (Color)Color;
                 for(int i = 0; i < ctrs.Length; i++)
                 {
                     Cv2.DrawContours(m, ctrs, i, new Scalar(clr.B, clr.G, clr.R));
                 }
                 return m;
             };
        }

        [ImportExport]
        [Enum(typeof(RetrievalModes))]
        public RetrievalModes RetrievalMode { get { return rv; } set { SetProperty(ref rv, value); } }

        [ImportExport]
        [Enum(typeof(ContourApproximationModes))]
        public ContourApproximationModes ContourApproximation { get { return cam; } set { SetProperty(ref cam, value); } }

        [ImportExport]
        [RGBColor]
        public Color? Color
        {
            get { return clr; }
            set
            {
                if(value!= null)
                {
                    SetProperty(ref clr, value);
                }
            }
        }
    }
}
