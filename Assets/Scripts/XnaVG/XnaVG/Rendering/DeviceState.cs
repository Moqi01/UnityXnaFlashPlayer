using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering
{
    internal sealed class DeviceState
    {
        public GraphicsDevice Device { get; private set; }
        public Color BlendFactor { get; private set; }
        public BlendState BlendState { get; private set; }
        public DepthStencilState DepthStencilState { get; private set; }
        public IndexBuffer Indices { get; private set; }
        public RasterizerState RasterizerState { get; private set; }             
        public Rectangle ScissorRectangle { get; private set; }
        public Viewport Viewport { get; private set; }

        public RenderTargetBinding[] RenderTargets { get; private set; }   
        public VertexBufferBinding[] VertexBuffers { get; private set; }

        public int MultisampleMask { get; private set; }
        public int ReferenceStencil { get; private set; }

        internal DeviceState(GraphicsDevice device)
        {
            Device = device;
            BlendFactor = device.BlendFactor;
            BlendState = device.BlendState;
            DepthStencilState = device.DepthStencilState;
            Indices = device.Indices;
            RasterizerState = device.RasterizerState;
            RenderTargets = device.GetRenderTargets();
            ScissorRectangle = device.ScissorRectangle;
            Viewport = device.Viewport;
            VertexBuffers = device.GetVertexBuffers();
            
            MultisampleMask = device.MultiSampleMask;
            ReferenceStencil = device.ReferenceStencil;
        }

        internal void Restore()
        {
            var device = Device;
            device.BlendFactor = BlendFactor;
            device.BlendState = BlendState;
            device.DepthStencilState = DepthStencilState;
            device.Indices = Indices;
            device.RasterizerState = RasterizerState;
            device.SetRenderTargets(RenderTargets);
            device.ScissorRectangle = ScissorRectangle;
            device.Viewport = Viewport;
            device.SetVertexBuffers(VertexBuffers);
            
            device.MultiSampleMask = MultisampleMask;
            device.ReferenceStencil = ReferenceStencil;
        }
    }
}