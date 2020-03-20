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
    [Category("HighLevel")]
    public class ExtractRectangleAction : ImageAction
    {
        public ExtractRectangleAction()
        {
            Name = "Extract rectangle";
            Action = (input) =>
             {
                 input.FindContours(out var contours, out var hierarchy, RetrievalModes.List, ContourApproximationModes.ApproxNone);

                 var m = input.CvtColor(ColorConversionCodes.GRAY2BGR);

                 var imageArea = m.Width * m.Height;

                 var allContoursSelect = from contour in contours
                                         let boundingRect = Cv2.MinAreaRect(contour)
                                         let boundingRectArea = boundingRect.Size.Width * boundingRect.Size.Height
                                         let convexHull = Cv2.ConvexHull(contour)
                                         let hullArea = Cv2.ContourArea(convexHull)
                                         let contourPercentage = hullArea / boundingRectArea
                                         let wtoh = boundingRect.Size.Width / boundingRect.Size.Height
                                         let htow = boundingRect.Size.Height / boundingRect.Size.Width
                                         select new
                                         {
                                             boundingRect.Center,
                                             Wtoh = wtoh,
                                             Htow = htow,
                                             ImageArea = hullArea / imageArea,
                                             BoundingRect = boundingRect,
                                             Pixels = boundingRect.Size.Width * boundingRect.Size.Height,
                                             ContourPerc = contourPercentage,
                                             ConvexHull = convexHull,
                                             HullArea = hullArea,
                                             RectArea = boundingRect.Size.Width * boundingRect.Size.Height
                                         };

                 if (this.EnableFilters)
                 {
                     allContoursSelect = allContoursSelect
                                            .Where(i => i.Pixels >= this.MinRectPixels && i.Pixels <= this.MaxRectPixels
                                                        && i.ContourPerc >= this.MinRectShapeArea
                                                        && (Math.Abs(i.Wtoh - this.WidthToHeightRatio) <= this.WidthToHeightRatioDeviation
                                                            || Math.Abs(i.Htow - this.WidthToHeightRatio) <= this.WidthToHeightRatioDeviation));

                     switch (this.SelectionMode)
                     {
                         case RectangleSelectionMode.MostNorth:
                             allContoursSelect = allContoursSelect.OrderByDescending(c => c.Center.Y);
                             break;
                         case RectangleSelectionMode.MostSouth:
                             allContoursSelect = allContoursSelect.OrderBy(c => c.Center.Y);
                             break;
                         case RectangleSelectionMode.MostEast:
                             allContoursSelect = allContoursSelect.OrderByDescending(c => c.Center.Y);
                             break;
                         case RectangleSelectionMode.MostWest:
                             allContoursSelect = allContoursSelect.OrderBy(c => c.Center.Y);
                             break;
                         case RectangleSelectionMode.BiggestBoundingRectArea:
                             allContoursSelect = allContoursSelect.OrderBy(c => c.RectArea);
                             break;
                         case RectangleSelectionMode.SmallestBoundingRectArea:
                             allContoursSelect = allContoursSelect.OrderByDescending(c => c.RectArea);
                             break;
                         case RectangleSelectionMode.BiggestConvexHullArea:
                             allContoursSelect = allContoursSelect.OrderBy(c => c.HullArea);
                             break;
                         case RectangleSelectionMode.SmallestConvexHullArea:
                             allContoursSelect = allContoursSelect.OrderByDescending(c => c.HullArea);
                             break;
                         default:
                             throw new Exception($"Missing mode handling: {this.SelectionMode}");
                     }
                 }

                 var allContours = allContoursSelect
                                        .ToArray();

                 this.Status = $"{allContours.Length} found ({contours.Length} unfiltered)";
                 this.DetectedRect = string.Empty;

                 if (allContours.Length == 0)
                     throw new Exception("No contours found that match the criteria.");

                 var contourOfInterest = allContours.Last();
                 this.DetectedRect = $"ImageArea: {contourOfInterest.ImageArea}\n" +
                                     $"Pixels: {contourOfInterest.Pixels:F0}\n" +
                                     $"RectShapeArea: {contourOfInterest.ContourPerc:P2}\n" +
                                     $"WidthToHeight: {contourOfInterest.Wtoh}\n" +
                                     $"HeightToWidth: {contourOfInterest.Htow}";

                 if (!this.Crop)
                 {
                     // Draw contours.
                     foreach (var contour in allContours)
                     {
                         var color = contour == contourOfInterest ? Scalar.Green : Scalar.Red;

                         m.Polylines(new[] { contour.BoundingRect.Points().Select(p => new Point(p.X, p.Y)) }, isClosed: true, color);
                         m.DrawContours(new[] { contour.ConvexHull }, 0, color);
                     }

                     input.Dispose();
                     return m;
                 }

                 // Crop!
                 var angle = contourOfInterest.BoundingRect.Angle;
                 var size = contourOfInterest.BoundingRect.Size;
                 if (angle < -45)
                 {
                     angle += 90.0f;
                     size = new Size2f(size.Height, size.Width);
                 }

                 var rotMatrix = Cv2.GetRotationMatrix2D(contourOfInterest.BoundingRect.Center, angle, 1.0);
                 var warped = m.WarpAffine(rotMatrix, m.Size(), InterpolationFlags.Cubic);
                 input.Dispose();
                 m.Dispose();
                 return warped.GetRectSubPix(new Size(size.Width, size.Height), contourOfInterest.BoundingRect.Center);
             };
        }

        [Label]
        public string Status { get { return status; } set { SetPropertyNoChange(ref status, value); } }
        private string status;

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

        [ImportExport]
        [Enum(typeof(RectangleSelectionMode))]
        public RectangleSelectionMode SelectionMode { get { return selectionMode; } set { SetProperty(ref selectionMode, value); } }
        private RectangleSelectionMode selectionMode = RectangleSelectionMode.BiggestBoundingRectArea;
        
        [ImportExport]
        [CheckBox]
        public bool EnableFilters { get { return enableFilters; } set { SetProperty(ref enableFilters, value); } }
        private bool enableFilters = true;

        [ImportExport]
        [TextBox]
        public long MinRectPixels { get { return minRectPixels; } set { SetProperty(ref minRectPixels, value); } }
        private long minRectPixels = 1000;
        
        [ImportExport]
        [TextBox]
        public long MaxRectPixels { get { return maxRectPixels; } set { SetProperty(ref maxRectPixels, value); } }
        private long maxRectPixels = 3000;

        [ImportExport]
        [CheckBox]
        public bool Crop { get { return crop; } set { SetProperty(ref crop, value); } }
        private bool crop = true;

        [Label]
        public string DetectedRect { get { return detectedRect; } set { SetPropertyNoChange(ref detectedRect, value); } }
        private string detectedRect = "";

        public enum RectangleSelectionMode
        {
            BiggestBoundingRectArea,
            SmallestBoundingRectArea,
            BiggestConvexHullArea,
            SmallestConvexHullArea,
            MostNorth,
            MostSouth,
            MostEast,
            MostWest
        }
    }
}
