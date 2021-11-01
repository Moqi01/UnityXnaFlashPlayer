using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions.Objects
{
    public class Object : ActionObject
    {
        public Object(params KeyValuePair<string, ActionVar>[] values) { }
        public Object(ActionObject prototype, ActionFunc constructor) { }

        protected override string AsString()
        {
            return "[object]";
        }
    }
}
