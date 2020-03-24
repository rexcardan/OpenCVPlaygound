using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ActionsModule.DrawHelpers
{
    public static class LineSegmentPolarExtensions
    {
        public static void Draw(this LineSegmentPolar line, Mat image, Scalar color)
        {
            var xz = line.XPosOfLine(0);
            var yz = line.YPosOfLine(0);

            if (xz.HasValue && !yz.HasValue)
                image.Line(new Point(xz.Value, 0), new Point(xz.Value, image.Height), color);
            else if (!xz.HasValue && yz.HasValue)
                image.Line(new Point(0, yz.Value), new Point(image.Width, yz.Value), color);
            else
            {
                var seg = line.ToSegmentPointX(0, image.Width);
                image.Line(seg.P1, seg.P2, color);
            }
        }
    }
}
