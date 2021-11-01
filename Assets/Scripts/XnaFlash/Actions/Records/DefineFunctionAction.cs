using System;
using XnaFlash.Actions;
using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class DefineFunctionAction : ActionRecord
    {
        public string Name { get; private set; }
        public string[] Params { get; private set; }
        public ActionBlock Actions { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Name = stream.ReadString();
            
            Params = new string[stream.ReadUShort()];
            for (int i = 0; i < Params.Length; i++)
                Params[i] = stream.ReadString();

            Actions = ActionRecord.ReadActions(stream, stream.ReadUShort());
        }
    }

    public class DefineFunction2Action : ActionRecord
    {        
        public string Name { get; private set; }
        public ActionFunc.RegisterParam[] Params { get; private set; }
        public byte NumRegisters { get; private set; }
        public FuncFlags Flags { get; private set; }
        public ActionBlock Actions { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            Name = stream.ReadString();
            Params = new ActionFunc.RegisterParam[stream.ReadUShort()];
            NumRegisters = stream.ReadByte();
            Flags = (FuncFlags)stream.ReadUShort();

            for (int i = 0; i < Params.Length; i++)
                Params[i] = new ActionFunc.RegisterParam(stream);

            Actions = ActionRecord.ReadActions(stream, stream.ReadUShort());
        }
    }
}
