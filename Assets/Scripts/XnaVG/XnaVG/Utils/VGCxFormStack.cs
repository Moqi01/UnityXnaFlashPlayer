using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XnaVG.Utils
{
    public sealed class VGCxFormStack
    {
        private Stack<VGCxForm> CxForms;

        public VGCxForm CxForm
        {
            get { return CxForms.Peek(); } 
        }

        internal VGCxFormStack(VGCxFormStack other)
        {
            CxForms = new Stack<VGCxForm>(other.CxForms.Reverse());
        }
        internal VGCxFormStack(int capacity)
        {
            CxForms = new Stack<VGCxForm>(capacity);
            Clear();
        }

        public void Clear()
        {
            CxForms.Clear();
            CxForms.Push(VGCxForm.Identity);
        }
        internal void Clear(VGCxForm baseForm)
        {
            CxForms.Clear();
            CxForms.Push(baseForm);
        }
        public void Push()
        {
            CxForms.Push(CxForms.Peek());
        }
        public void Push(VGCxForm cxform)
        {
            CxForms.Push(cxform);
        }
        public void PushCombineLeft(VGCxForm cxform)
        {
            CxForms.Push(cxform * CxForms.Peek());
        }
        public void PushCombineRight(VGCxForm cxform)
        {
            CxForms.Push(CxForms.Peek() * cxform);
        }
        public bool Pop()
        {
            if (CxForms.Count < 2)
                return false;

            CxForms.Pop();
            return true;
        }             
    }
}
