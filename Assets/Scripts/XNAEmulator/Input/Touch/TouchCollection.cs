using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Microsoft.Xna.Framework.Input.Touch
{
    public struct TouchCollection : IList<TouchLocation>, ICollection<TouchLocation>, IEnumerable<TouchLocation>, IEnumerable
    {
        public TouchLocation this[int index] { get { return new TouchLocation(); } set {  } }
        public int Count
        {
            get { return 0; }
        }
        public bool IsReadOnly
        {
            get { return true; }
        }
        public int IndexOf(TouchLocation item)
        {
            UnityEngine.Debug.Log("?");
            return 0;
        }
        public void Insert(int index, TouchLocation item)
        {
            UnityEngine.Debug.Log("?");
        }
        public void RemoveAt(int index)
        {
            UnityEngine.Debug.Log("?");
        }

        public void Add(TouchLocation item)
        {
            UnityEngine.Debug.Log("?");
        }
        public void Clear()
        {
            UnityEngine.Debug.Log("?");
        }
        public bool Contains(TouchLocation item)
        {
            UnityEngine.Debug.Log("?");
            return true;
        }
        public void CopyTo(TouchLocation[] array, int arrayIndex)
        {
            UnityEngine.Debug.Log("?");
        }
        public bool Remove(TouchLocation item)
        {
            UnityEngine.Debug.Log("?");
            return true;
        }

        public IEnumerator<TouchLocation> GetEnumerator()
        {
            return (IEnumerator<TouchLocation>)new TouchCollectionEnum(new TouchLocation[0]);
            // TODO:    
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
            // UnityEngine.Debug.Log("?");
        }
    }

    public class TouchCollectionEnum : IEnumerator<TouchLocation>
    {
        int position = -1;
        public TouchLocation[] _touches;


        public TouchCollectionEnum(TouchLocation[] list)
        {
            _touches = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _touches.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public TouchLocation Current
        {
            get
            {
                try
                {
                    return _touches[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {}
    }
}
