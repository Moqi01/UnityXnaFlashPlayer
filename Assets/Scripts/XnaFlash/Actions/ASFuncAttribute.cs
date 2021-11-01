using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions
{
    public class ASFuncAttribute : Attribute
    {
        public string Name { get; private set; }
        public ASFuncAttribute(string name) { Name = name; }
        public ASFuncAttribute() : this(null) { }
    }
}
