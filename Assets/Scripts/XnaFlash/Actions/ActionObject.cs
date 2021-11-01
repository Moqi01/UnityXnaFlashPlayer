using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaFlash.Actions
{
    public class ActionObject
    {
        private Dictionary<string, Variable> _namedVars = new Dictionary<string, Variable>();
        private Dictionary<int, Variable> _indexedVars = new Dictionary<int, Variable>();

        public virtual ActionObject MakeInstance(string name, params ActionVar[] args) 
        {
            if (name == "Array") return new Objects.Array();
            return new ActionObject();
        }

        public ActionVar Value 
        {
            get { return (CallValueOf() ?? CallToString() ?? new ActionVar(AsString())).String; }
        }
        public ActionContext Context { get; protected set; }
        public virtual ActionObject Prototype { get; set; }
        public virtual ActionFunc Invokable { get { return this["__constructor__"].Func; } }        
        public virtual IEnumerable<string> VariableNames
        {
            get
            {
                return _indexedVars
                    .Where(v => (v.Value.Flags & VarFlags.DontEnum) == 0)
                    .Select(v => v.Key.ToString())
                    .Concat(_namedVars.Where(v => (v.Value.Flags & VarFlags.DontEnum) == 0).Select(v => v.Key));
            }
        }

        public virtual ActionVar this[int index] 
        {
            get
            {
                Variable var;
                if (!_indexedVars.TryGetValue(index, out var))
                    return new ActionVar();
                return new ActionVar(var.Value);
            }
            set 
            {
                Variable var;
                if (!_indexedVars.TryGetValue(index, out var))
                    _indexedVars.Add(index, new Variable { Flags = VarFlags.None, Value = new ActionVar(value) });
                else if ((var.Flags & VarFlags.ReadOnly) == 0)
                    _indexedVars[index].Value.SetValue(value);
            } 
        }
        public virtual ActionVar this[string name]
        {
            get
            {
                Variable var;
                if (!_namedVars.TryGetValue(name, out var))
                {
                    int index;
                    if (int.TryParse(name, out index))
                        return this[index];
                    return new ActionVar();
                }
                return new ActionVar(var.Value);
            }
            set
            {
                Variable var;
                if (!_namedVars.TryGetValue(name, out var))
                    _namedVars.Add(name, new Variable { Flags = VarFlags.None, Value = new ActionVar(value) });
                else if ((var.Flags & VarFlags.ReadOnly) == 0)
                    _namedVars[name].Value.SetValue(value);
            }
        }

        public ActionObject() 
        {
            Prototype = null;
            ClearVariables();
            this["this"] = this;
        }

        public void ClearVariables() 
        {
            _namedVars.Clear();
            _indexedVars.Clear();
        }
        public bool HasVariable(string name) 
        {
            return _namedVars.ContainsKey(name); 
        }       
        
        protected virtual string AsString() { return "[object]"; }
        public sealed override string ToString() 
        {
            return (CallToString() ?? CallValueOf() ?? new ActionVar(AsString())).String;
        }        
        protected virtual ActionVar CallToString() 
        {
            var func = this["toString"].Func;
            if (func == null) return null;
            var res = func.Invoke(Context);
            if (res.IsValue) return res;
            return null;
        }
        protected virtual ActionVar CallValueOf() 
        {
            var func = this["valueOf"].Func;
            if (func == null) return null;
            var res = func.Invoke(Context);
            if (res.IsValue) return res;
            return null;
        }

        #region Helper Types

        protected enum VarFlags
        {
            None = 0,
            ReadOnly = 0x01,
            DontEnum = 0x02
        }

        protected struct Variable
        {
            public ActionVar Value;
            public VarFlags Flags;
        }

        #endregion
    }
}