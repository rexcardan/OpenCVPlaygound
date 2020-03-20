using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using ActionsModule.Attributes;

namespace ActionsModule.Actions
{
    [Category("Transformations")]
    public class FlipAction : ImageAction
    {
        private FlipMode flipMode = OpenCvSharp.FlipMode.X;

        public FlipAction()
        {
            this.Name = "Flip";
            this.Action = (m) =>
            {
                var flip = m.Flip(this.FlipMode);
                m.Dispose();
                HasError = false;
                return flip;
            };
        }

        [ImportExport]
        [Enum(typeof(FlipMode))]
        public FlipMode FlipMode
        {
            get
            {
                return flipMode;
            }

            set
            {
                SetProperty(ref flipMode, value);
            }
        }
    }
}
