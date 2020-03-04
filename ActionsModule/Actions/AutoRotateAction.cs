using ActionsModule.Attributes;
using ActionsModule.DrawHelpers;
using ActionsModule.MathHelpers;
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
    [Category("Transformations")]
    public class AutoRotateAction : ImageAction
    {
        public AutoRotateAction()
        {
            this.Name = "AutoRotate";
            this.Action = (input) =>
            {
                if (input.Channels() != 1)
                {
                    throw new Exception($"Image.Channels must be 1");
                }

                this.Status = "Processing...";
                var image = this.HoughLinesStandard(input);

                return image;
            };
        }

        private string status = string.Empty;
        [Label]
        public string Status
        {
            get { return this.status; }
            set
            {
                SetProperty(ref this.status, value);
            }
        }

        private double houghRho = 1;
        [ImportExport]
        [Slider(1, 100)]
        public double HoughRho
        {
            get { return this.houghRho; }
            set
            {
                SetProperty(ref this.houghRho, value);
            }
        }

        private double houghTheta = Math.PI / 180;
        [ImportExport]
        [Slider(0, 1.0, 0.001)]
        public double HoughTheta
        {
            get { return this.houghTheta; }
            set
            {
                SetProperty(ref this.houghTheta, value);
            }
        }

        private double houghThreshold = 150;
        [ImportExport]
        [Slider(1, 1000)]
        public double HoughThreshold
        {
            get { return this.houghThreshold; }
            set
            {
                SetProperty(ref this.houghThreshold, value);
            }
        }

        private SearchForEnum searchFor = SearchForEnum.HorizontalLines;
        [ImportExport]
        [Enum(typeof(SearchForEnum))]
        public SearchForEnum SearchFor
        {
            get { return searchFor; }
            set
            {
                SetProperty(ref searchFor, value);
            }
        }
        
        private double maxRotationDegrees = 45;
        [ImportExport]
        [Slider(0.1, 45.0, increment: 0.01, isIntegerType: false)]
        public double MaxRotationDegrees
        {
            get { return this.maxRotationDegrees; }
            set
            {
                SetProperty(ref this.maxRotationDegrees, value);
            }
        }
        
        private double clusterThresholdDegrees = 3;
        [ImportExport]
        [Slider(0.1, 15.0, increment: 0.01, isIntegerType: false)]
        public double ClusterThresholdDegrees
        {
            get { return this.clusterThresholdDegrees; }
            set
            {
                SetProperty(ref this.clusterThresholdDegrees, value);
            }
        }

        private Mat HoughLinesStandard(Mat input)
        {
            var maxRotation = this.MaxRotationDegrees.DegToRad();
            var clusterThreshold = this.ClusterThresholdDegrees.DegToRad();
            var zeroRotation = this.searchFor == SearchForEnum.HorizontalLines
                                            ? 90.0
                                            : 0.0;

            // Apply Hough Lines.
            var lines = input.HoughLines(this.HoughRho, this.HoughTheta, (int)this.HoughThreshold);
            var thetaValues = lines
                                .Select(l => (double)l.Theta)
                                .Where(t => t.DistanceToRad(zeroRotation, ignoreRotationDirection: true) < maxRotation);

            // Detect biggest cluster.
            var cluster = ClusterDetection1D.Detect(thetaValues, clusterThreshold);
            var (important, nonImportant) = lines.Split(line => cluster.Contains(line.Theta));
            
            // Draw lines.
            var image = input.CvtColor(ColorConversionCodes.GRAY2BGR);
            foreach (var line in nonImportant)
                line.Draw(image, Scalar.Red);
            foreach (var line in important)
                line.Draw(image, Scalar.Green);

            // Rotate.
            var rotation = !cluster.Any() ? 0.0
                            : cluster.Average();
            if (rotation != 0.0)
            {
                var origin = this.SearchFor == SearchForEnum.HorizontalLines
                             ? 270.0 : 180.0;
                var transformMatrix = Cv2.GetRotationMatrix2D(new Point2f(input.Width / 2, input.Height / 2), origin + rotation.RadToDeg(), 1);
                var size = input.Size();
                image = image.WarpAffine(transformMatrix, size);
            }

            this.Status = $"{lines.Length} lines detected ({important.Count} used to determine rotation of {rotation.RadToDeg():F1}°)";

            return image;
        }

        public enum SearchForEnum
        {
            HorizontalLines,
            VerticalLines
        }
    }
}
