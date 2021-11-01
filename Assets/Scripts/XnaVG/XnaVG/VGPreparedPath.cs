using System;
using Microsoft.Xna.Framework;
using XnaVG.Rendering.Tesselation;

namespace XnaVG
{
    public sealed class VGPreparedPath : IDisposable, IRenderable
    {
        public object Tag { get; set; }

        public Vector4 Extents { get; private set; }
        public VGLineCap StartCap { get; private set; }
        public VGLineCap EndCap { get; private set; }
        public VGLineJoin Join { get; private set; }
        public float MiterLimit { get; private set; }

        public bool HasFill { get { return Fill != null; } }
        public bool HasStroke { get { return Stroke != null; } }
        bool IRenderable.HasSolidStroke { get { return Stroke != null && Stroke.HasSolid; } }
        bool IRenderable.HasRadialStroke { get { return Stroke != null && Stroke.HasRadial; } }

        internal FillMesh Fill { get; private set; }
        internal StrokeMesh Stroke { get; private set; }

        internal VGPreparedPath(FillMesh fill, StrokeMesh stroke, VGLineCap startCap, VGLineCap endCap, VGLineJoin join, float miterLimit, Vector4 extents)
        {
            Fill = fill;
            Stroke = stroke;
            StartCap = startCap;
            EndCap = endCap;
            Join = join;
            MiterLimit = miterLimit;
            Extents = extents;
        }

        void IRenderable.RenderFill()
        {
            Fill.Render();
        }
        void IRenderable.Render()
        {
            if (HasFill)
            {
                Fill.Activate();
                Fill.Render();
            }

            if (HasStroke)
            {
                Stroke.ActivateRadial();
                Stroke.RenderRadial();
                Stroke.ActivateSolid();
                Stroke.RenderSolid();
            }
        }
        void IRenderable.RenderSolidStroke()
        {
            Stroke.RenderSolid();
        }
        void IRenderable.RenderRadialStroke()
        {
            Stroke.RenderRadial();
        }
        void IRenderable.BeforeRenderFill()
        {
            Fill.Activate();
        }
        void IRenderable.BeforeRenderSolidStroke()
        {
            Stroke.ActivateSolid();
        }
        void IRenderable.BeforeRenderRadialStroke()
        {
            Stroke.ActivateRadial();
        }

        public void Dispose()
        {
            if (Fill != null)
            {
                Fill.Dispose();
                Fill = null;
            }

            if (Stroke != null)
            {
                Stroke.Dispose();
                Stroke = null;
            }
        }
    }
}
