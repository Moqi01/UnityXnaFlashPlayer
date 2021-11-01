using XnaFlash.Swf;

namespace XnaFlash.Actions.Records
{
    public class BranchAction : ActionRecord
    {
        protected int BranchOffset { get; set; }
        public virtual int BranchAddress { get { return Address + BranchOffset + 5; } }

        protected override void Load(SwfStream stream, ushort length)
        {
            BranchOffset = stream.ReadShort();
        }
    }

    public class IndexBranchAction : BranchAction
    {
        public override int BranchAddress { get { return BranchOffset; } }

        public IndexBranchAction(int index)
        {
            BranchOffset = index;
        }
    }
}
