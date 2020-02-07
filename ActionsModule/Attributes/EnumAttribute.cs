using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.Attributes
{
    [AttributeUsage(System.AttributeTargets.Property)]
    public class EnumAttribute : Attribute
    {
        public EnumAttribute(Type enumClass)
        {
            EnumClass = enumClass;
        }

        public Type EnumClass { get; set; }
    }
}
