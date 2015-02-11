using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OP.General.Attributes
{
    public class EnumOrderAttribute : Attribute
    {
        public int Order { get; set; }

    }
    public class EnumDisplayAttribute : Attribute
    {
        public bool IsHidded { get; set; }
    }
}
