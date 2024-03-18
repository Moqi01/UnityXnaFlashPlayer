using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Content;
using XnaFlash.Swf.Tags;
using XnaVG;


namespace XnaFlash.Movie
{
    public class DisplayList
    {
        private LinkedList<DisplayObject> _displayList = new LinkedList<DisplayObject>();
        
        internal DisplayList() { }

        public DisplayObject Get(ushort depth)
        {
            var n = _displayList.Last;
            for (; n != null; n = n.Previous)
            {
                if (n.Value.Depth == depth)
                    return n.Value;
            }
            return null;
        }
        public void Set(PlaceObject2Tag tag, MovieClip clip) 
        {
            DisplayObject obj;

            var n = _displayList.Last;
            for (; n != null && n.Value.Depth > tag.Depth; n = n.Previous) ;

            if (n == null)
            {
                obj = DisplayObject.CreateAndPlace(tag, clip);
                if (obj != null) _displayList.AddFirst(obj);
                return;
            }

            if (n.Value.Depth == tag.Depth)
            {
                if (!n.Value.SetPlacement(tag, clip))
                    _displayList.Remove(n);
                return;
            }

            obj = DisplayObject.CreateAndPlace(tag, clip);
            if (obj != null)
                _displayList.AddAfter(n, obj);
        }
        public void Remove(ushort depth) 
        {
            var n = _displayList.Last;
            for (; n != null; n = n.Previous)
            {
                if (n.Value.Depth != depth)
                    continue;

                n.Value.Removed();
                _displayList.Remove(n);
                return;
            }
        }

        public void Clear() 
        {
            foreach (var n in _displayList) n.Removed();
            _displayList.Clear();
        }
        public void ProcessSpriteFrame(SpriteFrame frame, MovieClip clip)
        {
            DisplayObject obj;

            if(frame!=null)
            {
                var n = _displayList.First;
                foreach (var r in frame.RemovedObjects)
                {
                    for (; n != null && n.Value.Depth < r; n = n.Next) ;
                    if (n == null) break;
                    if (n.Value.Depth == r)
                    {
                        if (n.Next == null)
                        {
                            n.Value.Removed();
                            _displayList.RemoveLast();
                            n = null;
                        }
                        else
                        {
                            n = n.Next;
                            n.Previous.Value.Removed();
                            _displayList.Remove(n.Previous);
                        }
                        continue;
                    }
                }

                n = _displayList.First;

                foreach (var m in frame.ModifiedObjects)
                {
                    for (; n != null && n.Value.Depth < m.Depth; n = n.Next) ;
                    if (n == null)
                    {
                        obj = DisplayObject.CreateAndPlace(m, clip);
                        if (obj != null) _displayList.AddLast(obj);
                    }
                    else if (n.Value.Depth == m.Depth)
                    {
                        if (!n.Value.SetPlacement(m, clip))
                        {
                            n = n.Next;
                            _displayList.Remove(n);
                        }
                    }
                    else
                    {
                        obj = DisplayObject.CreateAndPlace(m, clip);
                        if (obj != null)
                            _displayList.AddBefore(n, obj);
                    }
                }
            }
           
        }
        public void ProcessButtonParts(IEnumerable<ButtonPart> parts, Button button)
        {
            DisplayObject obj;
            var n = _displayList.First;
            var p = parts.GetEnumerator();
            if (p.MoveNext())
            {
                while (n != null)
                {
                    if (n.Value.Depth != p.Current.Depth)
                    {
                        var next = n.Next;
                        n.Value.Removed();
                        _displayList.Remove(n);
                        n = next;
                    }
                    else
                    {
                        n = n.Next;
                        if (p.MoveNext())
                            continue;

                        while (n != _displayList.Last)
                        {
                            _displayList.Last.Value.Removed();
                            _displayList.RemoveLast();
                        }
                        break;
                    }
                }
            }

            n = _displayList.First;
            foreach (var m in parts)
            {                
                for (; n != null && n.Value.Depth < m.Depth; n = n.Next) ;
                if (n != null && n.Value.Depth == m.Depth)
                {
                    n.Value.SetPlacement(m, button);
                    continue;
                }

                obj = DisplayObject.CreateAndPlace(m, button);
                if (obj == null) continue;

                if (n == null)
                    _displayList.AddLast(obj);
                else
                    _displayList.AddBefore(n, obj);
            }
        }

        public void OnNextFrame()
        {
            foreach (var n in _displayList)
                n.OnNextFrame();
        }
        public bool OnMouseMove()
        {
            for (var n = _displayList.Last; n != null; n = n.Previous)
                if (n.Value.Object is IInstanceable && (n.Value.Object as IInstanceable).OnMouseMove())
                    return true;
            return false;
        }
        public void Draw(IVGRenderContext<DisplayState> target)
        {
            foreach (var n in _displayList)
                n.Draw(target);
        }
    }
}
