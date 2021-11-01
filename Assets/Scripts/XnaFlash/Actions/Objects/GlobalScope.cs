using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XnaFlash.Actions.Functions;
using XnaFlash.Movie;

namespace XnaFlash.Actions
{
    public class GlobalScope : ActionObject
    {
        private RootMovieClip _root;

        public GlobalScope(RootMovieClip root)
        {
            _root = root;   

            // Auto native methods
            foreach (var fun in NativeActionFunc.CreateFunctions(GetType().Assembly))
                this[fun.Key] = fun.Value;
            foreach (var fun in NativeActionFunc.CreateFunctions(Assembly.GetExecutingAssembly()))
                this[fun.Key] = fun.Value;

            // Manual native methods
            // Classes
            // Variables
            this["_root"] = root;
            this["_global"] = this;
        }

        public override ActionVar this[string name]
        {
            get
            {
                var v = base[name];
               /* if (v.IsUndef) 
                    _root.Trace("Value '" + name + "' not found in global scope!");*/
                return v;
            }
            set
            {
                base[name] = value;
            }
        }
        protected override string AsString()
        {
            return "[_global]";
        }          
    }
}
