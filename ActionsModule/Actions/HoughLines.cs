﻿using ActionsModule.Attributes;
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
    public class HoughLines : ImageAction
    {
        public HoughLines()
        {
            this.Name = "Hough Lines";
            this.Action = (input) =>
            {
                try
                {
                    if (input.Channels() != 1)
                    {
                        throw new Exception($"Image.Channels must be 1");
                    }

                    var image = this.Mode == HoughLinesMode.Standard
                                        ? this.HoughLinesStandard(input)
                                        : this.HoughLinesProbabilistic(input);

                    HasError = false;
                    return image;
                }
                catch (Exception e)
                {
                    HasError = true;
                    this.ErrorMessage = e.Message;
                }

                return input;
            };
        }

        private HoughLinesMode mode = HoughLinesMode.Standard;
        [Enum(typeof(HoughLinesMode))]
        public HoughLinesMode Mode
        {
            get { return mode; }
            set
            {
                SetProperty(ref mode, value);
            }
        }

        private double rho = 1;
        [Slider(1, 100)]
        public double Rho
        {
            get { return this.rho; }
            set
            {
                SetProperty(ref this.rho, value);
            }
        }

        private double theta = Math.PI / 180;
        [Slider(0, 1.0, 0.001)]
        public double Theta
        {
            get { return this.theta; }
            set
            {
                SetProperty(ref this.theta, value);
            }
        }

        private double threshold = 100;
        [Slider(1, 1000)]
        public double Threshold
        {
            get { return this.threshold; }
            set
            {
                SetProperty(ref this.threshold, value);
            }
        }

        private Color color = Colors.Red;
        [RGBColor]
        public Color Color
        {
            get { return color; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref color, value);
                }
            }
        }

        private Mat HoughLinesStandard(Mat input)
        {
            var c = new Scalar(this.Color.B, this.Color.G, this.Color.R);
            var lines = input.HoughLines(this.Rho, this.Theta, (int)this.Threshold);
            var image = input.CvtColor(ColorConversionCodes.GRAY2BGR);
            foreach (var line in lines)
            {
                if (line.Theta != 0)
                {
                    var l = line.ToSegmentPointX(0, image.Width);
                    image.Line(l.P1, l.P2, c);
                }
            }

            return image;
        }
        
        private Mat HoughLinesProbabilistic(Mat input)
        {
            var c = new Scalar(this.Color.B, this.Color.G, this.Color.R);
            var lines = input.HoughLinesP(this.Rho, this.Theta, (int)this.Threshold);
            var image = input.CvtColor(ColorConversionCodes.GRAY2BGR);
            foreach (var line in lines)
            {
                image.Line(line.P1, line.P2, c);
            }

            return image;
        }

        public enum HoughLinesMode
        {
            Standard,
            Probabilistic
        }
    }
}
