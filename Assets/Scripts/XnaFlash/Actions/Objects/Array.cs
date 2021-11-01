using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions.Objects
{
    public class Array : ActionObject
    {
        public Array(params ActionVar[] vars) { }

        protected override string AsString()
        {
            return "[array]";
        }
    }
}
