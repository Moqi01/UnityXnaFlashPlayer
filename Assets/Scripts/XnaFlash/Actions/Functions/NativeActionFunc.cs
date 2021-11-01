using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace XnaFlash.Actions.Functions
{
    public class NativeActionFunc : ActionFunc
    {
        private MethodInfo _method;
        private int _minParams;
        private bool _void;

        public override int ParameterCount { get { return _minParams; } }

        protected NativeActionFunc() { }
        private NativeActionFunc(MethodInfo method)
        {
            _method = method;
            _void = _method.ReturnType == typeof(void);
            _minParams = method.GetParameters().Length;
        }

        public override ActionVar Invoke(ActionContext context, params ActionVar[] parameters)
        {
            if (parameters.Length < _minParams)
                return new ActionVar();

            var ret = _method.Invoke(null, parameters);
            if (_void) return new ActionVar();
            return ActionVar.FromNativeValue(ret);
        }
        public static IEnumerable<KeyValuePair<string, NativeActionFunc>> CreateFunctions(Assembly assembly)
        {
            foreach (var t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(NativeActionFunc)) && t != typeof(NativeActionFunc))
                {
                    foreach (var attr in t.GetCustomAttributes(typeof(ASFuncAttribute), false).OfType<ASFuncAttribute>())
                    {
                        var f = Activator.CreateInstance(t) as NativeActionFunc;
                        yield return new KeyValuePair<string, NativeActionFunc>(attr.Name ?? t.Name, f);
                    }
                }

                foreach (var m in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    foreach (var attr in m.GetCustomAttributes(typeof(ASFuncAttribute), false).OfType<ASFuncAttribute>())
                    {
                        if (m.GetParameters().All(pi => pi.ParameterType == typeof(ActionVar)))
                            yield return new KeyValuePair<string, NativeActionFunc>(attr.Name ?? m.Name, new NativeActionFunc(m));
                    }
                }
            }
        }
    }
}
