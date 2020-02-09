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
    public class FindRectangleAction : ImageAction
    {
        public FindRectangleAction()
        {
            Name = "Find rectangle contours";
            Action = (input) =>
             {
                 input.FindContours(out var contours, out var hierarchy, RetrievalMode, ContourApproximation);

                 Mat m = input.Clone();
                 m = input.CvtColor(ColorConversionCodes.GRAY2BGR);

                 var imageArea = m.Width * m.Height;
                 var minImageArea = imageArea * this.MinAreaOfImagePercent / 100.0;

                 var allContoursSelect = from contour in contours
                                         let boundingRect = Cv2.MinAreaRect(contour)
                                         let boundingRectArea = boundingRect.Size.Width * boundingRect.Size.Height
                                         let convexHull = Cv2.ConvexHull(contour)
                                         let hullArea = Cv2.ContourArea(convexHull)
                                         let contourPercentage = hullArea / boundingRectArea
                                         let wtoh = boundingRect.Size.Width / boundingRect.Size.Height
                                         let htow = boundingRect.Size.Height / boundingRect.Size.Width
                                         where hullArea >= minImageArea
                                            && contourPercentage >= this.MinRectShapeArea
                                            && (Math.Abs(wtoh - this.WidthToHeightRatio) <= this.WidthToHeightRatioDeviation
                                             || Math.Abs(htow - this.WidthToHeightRatio) <= this.WidthToHeightRatioDeviation)
                                         select new
                                         {
                                             Wtoh = wtoh,
                                             ImageArea = hullArea / imageArea,
                                             BoundingRect = boundingRect,
                                             ContourPerc = contourPercentage,
                                             ConvexHull = convexHull,
                                             Sort = boundingRectArea
                                         };

                 var allContours = allContoursSelect
                                        .OrderBy(c => c.Sort)
                                        .ToArray();

                 this.Status = $"{allContours.Length} found ({contours.Length} unfiltered)";
                 this.DetectedRect = string.Empty;

                 if (allContours.Length == 0)
                     throw new Exception("No contours found that match the criteria.");

                 var contourOfInterest = allContours.Last();
                 foreach (var contour in allContours)
                 {
                     var color = contour == contourOfInterest ? Scalar.Green : Scalar.Red;

                     m.Polylines(new[] { contour.BoundingRect.Points().Select(p => new Point(p.X, p.Y)) }, isClosed: true, color);
                     m.DrawContours(new[] { contour.ConvexHull }, 0, color);

                     this.DetectedRect = $"ImageArea: {contour.ImageArea}\n" +
                                         $"RectShapeArea: {contour.ContourPerc:P2}\n" +
                                         $"WidthToHeight: {contour.Wtoh}";
                 }

                 return m;
             };
        }

        [Label]
        public string Status { get { return status; } set { SetPropertyNoChange(ref status, value); } }
        private string status;

        [ImportExport]
        [Enum(typeof(RetrievalModes))]
        public RetrievalModes RetrievalMode { get { return rv; } set { SetProperty(ref rv, value); } }
        private RetrievalModes rv = RetrievalModes.List;

        [ImportExport]
        [Enum(typeof(ContourApproximationModes))]
        public ContourApproximationModes ContourApproximation { get { return cam; } set { SetProperty(ref cam, value); } }
        private ContourApproximationModes cam = ContourApproximationModes.ApproxNone;

        [ImportExport]
        [Slider(0.01, 99.99, 0.01, isIntegerType: false)]
        public double MinAreaOfImagePercent { get { return minAreaOfImagePercent; } set { SetProperty(ref minAreaOfImagePercent, value); } }
        private double minAreaOfImagePercent = 0.05;

        [ImportExport]
        [Slider(0.50, 1.00, 0.01, isIntegerType: false)]
        public double MinRectShapeArea { get { return minRectShapeArea; } set { SetProperty(ref minRectShapeArea, value); } }
        private double minRectShapeArea = 0.90;
        
        [ImportExport]
        [Slider(0.05, 20.00, 0.01, isIntegerType: false)]
        public double WidthToHeightRatio { get { return widthToHeightRatio; } set { SetProperty(ref widthToHeightRatio, value); } }
        private double widthToHeightRatio = 2.0;
        
        [ImportExport]
        [Slider(0.01, 10.0, 0.01, isIntegerType: false)]
        public double WidthToHeightRatioDeviation { get { return widthToHeightRatioDeviation; } set { SetProperty(ref widthToHeightRatioDeviation, value); } }
        private double widthToHeightRatioDeviation = 0.1;
        
        [Label]
        public string DetectedRect { get { return detectedRect; } set { SetPropertyNoChange(ref detectedRect, value); } }
        private string detectedRect = "";
    }
}
