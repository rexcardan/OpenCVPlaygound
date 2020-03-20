using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class SliderAttribute : Attribute
    {
        public SliderAttribute(double minVal, double maxVal, double increment=1.0, bool isIntegerType = true)
        {
            MaxVal = maxVal;
            MinVal = minVal;
            IsIntegerType = isIntegerType;
            Increment = increment;
        }

        public bool IsIntegerType { get; set; }
        public double MaxVal { get; set; }
        public double MinVal { get; set; }
        public double Increment { get; set; }
    }
}
