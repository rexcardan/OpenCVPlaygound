using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using ActionsModule.Attributes;

namespace ActionsModule.Actions
{
    public class FlipAction : ImageAction
    {
        private OpenCvSharp.FlipMode flipMode = OpenCvSharp.FlipMode.X;

        public FlipAction()
        {
            this.Name = "Flip";
            this.Action = (m) =>
            {
                try
                {
                    var flip = m.Flip(this.FlipMode);
                    m.Dispose();
                    HasError = false;
                    return flip;
                }
                catch (Exception)
                {
                    HasError = true;
                }
                return m;
            };
        }

        [EnumAttribute(typeof(FlipMode))]
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
