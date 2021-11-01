using System;
using System.Collections.Generic;
using XnaFlash.Actions.Records;
using XnaFlash.Swf;

namespace XnaFlash.Actions
{
    public class ActionRecord
    {
        public ActionCode Action { get; private set; }
        public int Address { get; private set; }

        protected virtual void Load(SwfStream stream, ushort length) { }
        public override string ToString()
        {
            return Action.ToString();
        }

        public static ActionRecord Read(SwfStream stream)
        {
            byte code = stream.ReadByte();

            if (code == 0)
                return new ActionRecord { Action = ActionCode.End };

            if (!Enum.IsDefined(typeof(ActionCode), code))
                throw new SwfCorruptedException("Unknown action code has been found!");

            if (code < 0x80)         
                return new ActionRecord { Action = (ActionCode)code };                

            ActionRecord r = null;  
            switch ((ActionCode)code)
            {
                // SWF 3
                case ActionCode.GoToFrame: r = new FrameAction(); break;
                case ActionCode.GetURL: r = new GetURLAction(); break;
                case ActionCode.WaitForFrame: r = new WaitForFrameAction(); break;
                case ActionCode.SetTarget: r = new SetTargetAction(); break;
                case ActionCode.GoToLabel: r = new GoToLabelAction(); break;
                
                // SWF 4
                case ActionCode.Push: r = new PushAction(); break;
                case ActionCode.If:
                case ActionCode.Jump: r = new BranchAction(); break;
                case ActionCode.Call: r = new ActionRecord(); break;
                case ActionCode.GetURL2: r = new GetURL2Action(); break;
                case ActionCode.GoToFrame2: r = new GoToFrame2Action(); break;
                case ActionCode.WaitForFrame2: r = new WaitForFrame2Action(); break;

                // SWF 5
                case ActionCode.ConstantPool: r = new ConstantPoolAction(); break;
                case ActionCode.DefineFunction: r = new DefineFunctionAction(); break;
                case ActionCode.With: r = new WithAction(); break;
                case ActionCode.StoreRegister: r = new StoreRegisterAction(); break;

                // SWF 6
                case ActionCode.DefineFunction2: r = new DefineFunction2Action(); break;

                // SWF 7
                case ActionCode.Try: r = new TryAction(); break;
            }

            ushort len = stream.ReadUShort();
            r.Load(stream, len);
            r.Action = (ActionCode)code;
            return r;
        }

        internal static ActionBlock ReadActions(SwfStream stream, uint? length)
        {
            int start = stream.TagPosition;
            long end = start + length ?? 0;                        
            int position = start;
            ActionRecord rec;

            List<ActionRecord> actions = new List<ActionRecord>(256);
            while(!length.HasValue || position < end)
            {
                rec = Read(stream);

                rec.Address = position - start;
                actions.Add(rec);

                position = stream.TagPosition;
                if (rec.Action == ActionCode.End)
                    break;
            }            

            return new ActionBlock(actions.ToArray());
        }
    }
}
