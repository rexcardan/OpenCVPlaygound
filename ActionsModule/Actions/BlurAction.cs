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
        [SliderAttribute(2,30)]
        public int Size
        {
            get { return size; }
            set { SetProperty(ref size, value); }
        }

        BorderTypes bTypes = BorderTypes.Default;

        [EnumAttribute(typeof(BorderTypes))]
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
                try
                {
                    var blur =  m.Blur(new OpenCvSharp.Size(size, size),null, BorderType);
                    m.Dispose();
                    HasError = false;
                    return blur;
                }
                catch (Exception)
                {
                    HasError = true;
                }
                return m;
            };
        }
    }
}
