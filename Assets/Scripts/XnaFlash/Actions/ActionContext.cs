using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Actions.Functions;
using XnaFlash.Actions.Objects;
using XnaFlash.Actions.Records;
using XnaFlash.Movie;

namespace XnaFlash.Actions
{
    public class ActionContext
    {
        public RootMovieClip RootClip { get; set; }
        public StageObject DefaultTarget { get; set; }
        public ActionObject This { get; set; }
        public Stack<ActionVar> Stack { get; set; }
        public ConstantPool Constants { get; set; }
        public LinkedList<ActionObject> Scope { get; set; }
        public ActionVar[] Registers { get; set; }

        public ActionObject CurrentScope { get { return Scope.Last.Value; } }
        public ActionObject GlobalScope { get { return Scope.First.Value; } }

        public ActionContext MakeLocalScope(int registerCount, int parameterCount)
        {
            var c = new ActionContext
            {
                Constants = Constants,
                DefaultTarget = DefaultTarget,
                Registers = new ActionVar[registerCount],
                RootClip = RootClip,
                Scope = new LinkedList<ActionObject>(Scope),
                Stack = new Stack<ActionVar>((parameterCount + 1) << 1),
                This = This,
                i= i
            };
            i++;
            //if(i>=73)
            //{
                
            //}
            //if(i<100)
            Scope.AddLast(new ActionObject());
           // This.Context.Scope.AddLast(new ActionObject());
            return c;
        }
        public int i;
    }
}
