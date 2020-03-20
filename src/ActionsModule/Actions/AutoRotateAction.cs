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
                this.Status = "Processing...";
                var maxRotation = this.MaxRotationDegrees.DegToRad();
                var clusterThreshold = this.ClusterThresholdDegrees.DegToRad();
                var zeroRotation = this.searchFor == SearchForEnum.HorizontalLines
                                                ? 90.0
                                                : 0.0;

                var canny = input.Canny(this.Threshold1, this.Threshold2, 3);

                // Apply Hough Lines.
                var lines = canny.HoughLines(this.HoughRho, this.HoughTheta, (int)this.HoughThreshold);
                var thetaValues = lines
                                    .Select(l => (double)l.Theta)
                                    .Where(t => t.DistanceToRad(zeroRotation, ignoreRotationDirection: true) < maxRotation);

                // Detect biggest cluster.
                var cluster = ClusterDetection1D.Detect(thetaValues, clusterThreshold);
                var (important, nonImportant) = lines.Split(line => cluster.Contains(line.Theta));

                // Draw lines.
                Mat image;

                if (this.Debug)
                {
                    if (input.Channels() == 1)
                    {
                        image = input.CvtColor(ColorConversionCodes.GRAY2BGR);
                        input.Dispose();
                    }
                    else
                        image = input;

                    foreach (var line in nonImportant)
                        line.Draw(image, Scalar.Red);
                    foreach (var line in important)
                        line.Draw(image, Scalar.Green);
                }
                else
                {
                    image = input;
                }

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

        private double threshold1 = 80.0;
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

        private double threshold2 = 160.0;
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

        [ImportExport]
        [CheckBox]
        public bool Debug { get { return debug; } set { SetProperty(ref debug, value); } }
        private bool debug = true;

        public enum SearchForEnum
        {
            HorizontalLines,
            VerticalLines
        }
    }
}
