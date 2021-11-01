
namespace XnaFlash.Actions
{
    public class ConstantPool
    {
        private string[] mValues;

        public ActionVar this[ActionVar i] { get { return mValues[i.Integer]; } }
        public int Length { get { return mValues.Length; } }

        public ConstantPool(params string[] values)
        {
            mValues = values;
        }
    }
}
