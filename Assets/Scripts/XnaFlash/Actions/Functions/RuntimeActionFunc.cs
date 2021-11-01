using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Actions.Objects;
using XnaFlash.Actions.Records;
using XnaFlash.Movie;

namespace XnaFlash.Actions.Functions
{
    public class RuntimeActionFunc : ActionFunc
    {        
        private ActionObject _prototype = null;
        private int _registerCount = 4;
        private FuncFlags _flags = FuncFlags.None;
        private ActionContext _context;
        private RegisterParam[] _parameters;
        private ActionBlock _code;

        internal RuntimeActionFunc(ActionContext context, ActionBlock codeBlock)
        {
            _parameters = new RegisterParam[0];
            _code = codeBlock;
            _context = new ActionContext
            {
                Constants = context.Constants,
                DefaultTarget = context.DefaultTarget,
                RootClip = context.RootClip,
                Scope = new LinkedList<ActionObject>(context.Scope)
            };
        }
        internal RuntimeActionFunc(ActionContext context, DefineFunctionAction info)
        {
            _parameters = info.Params.Select(s => new RegisterParam(s)).ToArray();
            _code = info.Actions;
            _context = new ActionContext
            {
                Constants = context.Constants,
                DefaultTarget = context.DefaultTarget,
                RootClip = context.RootClip,
                Scope = new LinkedList<ActionObject>(context.Scope)
            };
        }
        internal RuntimeActionFunc(ActionContext context, DefineFunction2Action info)
        {
            _flags = info.Flags;
            _registerCount = info.NumRegisters;
            _parameters = info.Params;
            _code = info.Actions;
            _context = new ActionContext
            {
                Constants = context.Constants,
                DefaultTarget = context.DefaultTarget,
                RootClip = context.RootClip,
                Scope = new LinkedList<ActionObject>(context.Scope)
            };
        }

        public override ActionObject Prototype { get { return _prototype; } set { _prototype = value; } }
        public override ActionVar Invoke(ActionContext context, params ActionVar[] parameters)
        {
            var scope = context.This ?? context.GlobalScope;
            var newContext = context.MakeLocalScope(_registerCount, parameters.Length);
            var locals = newContext.Scope.Last.Value;

            int i, l = Math.Min(_parameters.Length, parameters.Length);
            for (i = 0; i < _parameters.Length; i++)
            {
                newContext.Stack.Push(parameters[i]);
                if (_parameters[i].Register > 0 && _parameters[i].Register < _registerCount)
                    newContext.Registers[_parameters[i].Register] = parameters[i];
                locals[_parameters[i].Name] = parameters[i];
            }

            for (; i < parameters.Length; i++)
                newContext.Stack.Push(parameters[i]);

            for (; i < _parameters.Length; i++)
            {
                if (_parameters[i].Register > 0 && _parameters[i].Register < _registerCount)
                    newContext.Registers[_parameters[i].Register] = new ActionVar();
                locals[_parameters[i].Name] = new ActionVar();
            }

            newContext.Stack.Push(parameters.Length);

            int preloadReg = 1;
            if ((_flags & FuncFlags.PreloadThis) != 0)
                newContext.Registers[preloadReg++] = scope;
            if ((_flags & FuncFlags.SupressThis) == 0)
                locals["this"] = scope;

            if ((_flags & FuncFlags.PreloadArguments) != 0)
                newContext.Registers[preloadReg++] = new Objects.Array(parameters);
            if ((_flags & FuncFlags.SupressArguments) == 0)
                locals["arguments"] = new Objects.Array(parameters);

            if ((_flags & FuncFlags.PreloadSuper) != 0)
                newContext.Registers[preloadReg++] = scope.Prototype;
            if ((_flags & FuncFlags.SupressSuper) == 0)
                locals["super"] = scope.Prototype;

            if ((_flags & FuncFlags.PreloadRoot) != 0)
                newContext.Registers[preloadReg++] = context.RootClip;
            if ((_flags & FuncFlags.PreloadParent) != 0)
                newContext.Registers[preloadReg++] = (scope is MovieClip) ? new ActionVar((scope as MovieClip).Parent) : new ActionVar((string)null);
            if ((_flags & FuncFlags.PreloadGlobal) != 0)
                newContext.Registers[preloadReg++] = context.GlobalScope;

            return _code.RunSafe(newContext);
        }
    }
}