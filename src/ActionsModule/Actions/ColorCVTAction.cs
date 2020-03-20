using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using ActionsModule.Attributes;

namespace ActionsModule.Actions
{
    [Category("Preprocessing")]
    public class ColorCVTAction : ImageAction
    {
        private ColorConversionCodes cc = ColorConversionCodes.BGR2GRAY;

        public ColorCVTAction()
        {
            this.Name = "Convert Color";
            this.Action = (m) =>
            {
                var cvt = m.CvtColor(ColorConversion);
                m.Dispose();
                HasError = false;
                return cvt;
            };
        }

        [ImportExport]
        [Enum(typeof(ColorConversionCodes))]
        public ColorConversionCodes ColorConversion
        {
            get { return cc; }
            set
            {
                SetProperty(ref cc, value);
            }
        }
    }
}
