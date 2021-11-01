using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Swf;

namespace XnaFlash.Actions
{
    public abstract class ActionFunc
    {
        public virtual ActionObject Prototype { get { return null; } set { } }
        public virtual int ParameterCount { get { return -1; } }

        public abstract ActionVar Invoke(ActionContext context, params ActionVar[] parameters);
        public override string ToString() { return "[function]"; }

        public class RegisterParam
        {
            public byte Register { get; private set; }
            public string Name { get; private set; }

            internal RegisterParam(SwfStream stream)
            {
                Register = stream.ReadByte();
                Name = stream.ReadString();
            }

            internal RegisterParam(string name)
            {
                Register = 0;
                Name = name;
            }

            public override string ToString()
            {
                return string.Format("{0} [{1:X2}]", Name, Register);
            }
        }
    }
}