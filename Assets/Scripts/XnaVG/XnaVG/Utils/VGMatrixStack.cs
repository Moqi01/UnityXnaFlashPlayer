using System.Collections.Generic;
using System.Linq;

namespace XnaVG.Utils
{
    public sealed class VGMatrixStack
    {
        private Stack<VGMatrix> Matrices;

        public VGMatrix Matrix
        {
            get { return Matrices.Peek(); } 
        }

        public VGMatrixStack(VGMatrixStack other)
        {
            Matrices = new Stack<VGMatrix>(other.Matrices.Reverse());
        }
        public VGMatrixStack(int capacity)
        {
            Matrices = new Stack<VGMatrix>(capacity);
            Clear();
        }

        public void Clear()
        {
            Matrices.Clear();
            Matrices.Push(VGMatrix.Identity);
        }
        internal void Clear(VGMatrix baseMatrix)
        {
            Matrices.Clear();
            Matrices.Push(baseMatrix);
        }
        public void Push()
        {
            Matrices.Push(Matrices.Peek());
        }
        public void Push(VGMatrix matrix)
        {
            Matrices.Push(matrix);
        }
        public void PushCombineRight(VGMatrix matrix)
        {
            Matrices.Push(Matrices.Peek() * matrix);
        }
        public void PushCombineLeft(VGMatrix matrix)
        {
            VGMatrix m = matrix * Matrices.Peek();
            Matrices.Push(m);
        }
        public bool Pop()
        {
            if (Matrices.Count < 2)
                return false;

            Matrices.Pop();
          
            return true;
        }                 
    }
}
