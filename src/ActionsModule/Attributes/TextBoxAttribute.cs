using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class TextBoxAttribute : Attribute
    {
        public TextBoxAttribute()
        {
        }
    }
}
