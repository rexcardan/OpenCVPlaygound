using ActionsModule.Attributes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Actions
{
    public class BlurAction : ImageAction
    {
        int size = 3;
        [Slider(2,30)]
        public int Size
        {
            get { return size; }
            set { SetProperty(ref size, value); }
        }

        BorderTypes bTypes = BorderTypes.Default;

        [Enum(typeof(BorderTypes))]
        public BorderTypes BorderType
        {
            get { return bTypes; }
            set { SetProperty(ref bTypes, value); }
        }
        public BlurAction()
        {
            this.Name = "Blur";
            this.Action = (m) =>
            {
                var blur = m.Blur(new Size(size, size), null, BorderType);
                m.Dispose();
                return blur;
            };
        }
    }
}
