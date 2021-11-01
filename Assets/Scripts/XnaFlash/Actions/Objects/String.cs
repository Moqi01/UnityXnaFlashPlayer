using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions.Objects
{
    public class String : ActionObject
    {
        public String(string str)
        { }

        protected override string AsString()
        {
            return ""; //value
        }
    }
}
