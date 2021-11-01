using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Swf.Tags;
using XnaVG;
using XnaFlash.Content;

namespace XnaFlash.Movie
{
    public class DisplayObject
    {
        public ICharacter Character { get; private set; }
        public IDrawable Object { get; private set; }
        public ushort Depth { get; private set; }
        public ushort ClipDepth { get; private set; }
        public VGMatrix Matrix { get; private set; }
        public VGCxForm CxForm { get; private set; }

        public float Alpha 
        {
            get { return CxForm != null ? CxForm.MulAlpha : 1f; }
            set
            {
                if (value == Alpha) return;
                CxForm = CxForm ?? VGCxForm.Identity;
                CxForm.MulAlpha = value; 
            }
        }

        private DisplayObject(ushort depth)
        {
            Object = null;
            ClipDepth = 0;
            Depth = depth;
            CxForm = null;
            Matrix = VGMatrix.Identity;
        }
        internal bool SetPlacement(PlaceObject2Tag tag, StageObject parent)
        {            
            bool load = false;

            // TODO: Morph ratio
            if (tag.HasCxForm) CxForm = tag.CxForm;
            if (tag.HasMatrix) Matrix = new VGMatrix(tag.Matrix);
            if (tag.HasCharacter)
            {
                var character = parent.Root.Document[tag.CharacterID];
                if (character == null) return false;

                var newInstance = character.MakeInstance(this, parent.Root);
                if (newInstance == null) return false;

                Removed();
                Character = character;
                Object = newInstance;
                Object.SetParent(parent);                
                load = true;
            }
                        
            if (tag.HasClipDepth) ClipDepth = tag.ClipDepth;
            if (Object is IInstanceable)
            {
                var obj = Object as IInstanceable;
                if (tag.HasName) obj.SetName(tag.Name);
                if (tag.HasActions) obj.SetClipActions(tag.Actions);
                if (load) obj.Load();
            }

            return Object != null;
        }
        internal bool SetPlacement(ButtonPart part, StageObject parent)
        {
            if (part.Character == null) return false;
            if (!part.CxForm.IsIdentity) CxForm = part.CxForm;
            Matrix = new VGMatrix(part.Matrix);

            if (part.Character != Character)
            {
                Object = part.Character.MakeInstance(this, parent.Root);
                if (Object == null) return false;                               
                Object.SetParent(parent);
                Character = part.Character;

                if (Object is IInstanceable)
                {
                    var obj = Object as IInstanceable;
                    obj.Load();
                }
            }

            return Object != null;
        }
        public void Removed()
        {
            if (Object is IInstanceable)
                (Object as IInstanceable).Unload();
        }

        public void OnNextFrame()
        {
            if (Object is IInstanceable)
                (Object as IInstanceable).OnNextFrame();
        }
        public void Draw(IVGRenderContext<DisplayState> target)
        {
            target.State.PathToSurface.PushCombineLeft(Matrix);
            if (CxForm != null) target.State.ColorTransformation.PushCombineRight(CxForm);     
            if (ClipDepth > Depth)
            {
                target.State.WriteStencilMask = target.UserState.AddClipping(ClipDepth);
                target.ClearStencilMask(target.State.WriteStencilMask);
            }
            else
                target.State.WriteStencilMask = VGStencilMasks.None;            
       
            Object.Draw(target);

            target.State.StencilMask = target.UserState.ReleaseClippings(Depth);            
            if (CxForm != null) target.State.ColorTransformation.Pop();
            target.State.PathToSurface.Pop();
        }

        internal static DisplayObject CreateAndPlace(PlaceObject2Tag tag, StageObject parent)
        {
            var obj = new DisplayObject(tag.Depth);
            if (!obj.SetPlacement(tag, parent))
                return null;
            return obj;
        }
        internal static DisplayObject CreateAndPlace(ButtonPart buttonPart, StageObject parent)
        {
            var obj = new DisplayObject(buttonPart.Depth);
            if (!obj.SetPlacement(buttonPart, parent))
                return null;
            return obj;
        }
    }
}
