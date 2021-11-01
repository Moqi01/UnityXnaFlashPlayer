using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class ConstantPoolAction : ActionRecord
    {
        public ConstantPool Pool { get; private set; }

        protected override void Load(SwfStream stream, ushort length)
        {
            var array = new string[stream.ReadUShort()];
            for (int i = 0; i < array.Length; i++)
                array[i] = stream.ReadString();
            Pool = new ConstantPool(array);
        }
    }
}
