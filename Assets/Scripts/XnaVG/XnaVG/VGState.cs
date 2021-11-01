using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaVG.Paints;
using XnaVG.Rendering.States;
using XnaVG.Utils;

namespace XnaVG
{
    public sealed class VGState
    {
        private float _strokeExpand;
        private VGStencilMasks _readMask, _writeMask;
        private VGFillRule _fillRule;
        private Rectangle? _scissor;
        
        public VGMatrixStack ImageToSurface { get; private set; }
        public VGMatrixStack GlyphToSurface { get; private set; }
        public VGMatrixStack Projection { get; private set; }     
        public VGMatrixStack PathToSurface { get; private set; }        
        public VGMatrixStack PathToFillPaint { get; private set; }
        public VGMatrixStack PathToTextPaint { get; private set; }
        public VGMatrixStack PathToStrokePaint { get; private set; }
        public VGCxFormStack ColorTransformation { get; private set; }

        public bool ColorTransformationEnabled { get; set; }
        public bool MaskingEnabled { get; set; }
        public bool NonScalingStroke { get; set; }
        public float StrokeThickness { get { return _strokeExpand * 2f; } set { _strokeExpand = value / 2f; } }
        public float StrokeMiterLimit { get; set; }
        public VGFont Font { get; set; }
        public VGColor ClearColor { get; set; }        
        public VGImage Mask { get; set; }        
        public Vector4 MaskChannels { get; set; }
        public Vector2 TabSize { get; set; }        
        public VGLineCap StrokeStartCap { get; set; }
        public VGLineCap StrokeEndCap { get; set; }        
        public VGLineJoin StrokeJoin { get; set; }
        public Rectangle? ScissorRectangle { get { return _scissor; } set { _scissor = value; UpdateRasterizerState(); } }
        public VGFillRule FillRule 
        {
            get { return _fillRule; }
            set
            {
                _fillRule = value;
                FillRuleState = (_fillRule == VGFillRule.EvenOdd) ? Stencils.EvenOdd : Stencils.NonZero;
            }
        }
        public VGBlendMode BlendMode { get; set; }
        public VGStencilMasks StencilMask 
        {
            get { return _readMask; }
            set
            {
                _readMask = value;
                Stencils = new StencilStates(_writeMask, _readMask);
                FillRule = FillRule;
            }
        }
        public VGStencilMasks WriteStencilMask 
        {
            get { return _writeMask; }
            set
            {
                _writeMask = value;
                Stencils = new StencilStates(_writeMask, _readMask);
                FillRule = FillRule;
            }
        }                               
        public ColorWriteChannels ColorChannels { get; set; }
                
        public VGDevice Device { get; private set; }
        public VGAntialiasing Antialiasing { get; private set; }
        public VGPaint FillPaint { get; private set; }
        public VGPaint GlyphPaint { get; private set; }
        public VGPaint StrokePaint { get; private set; }

        internal bool IsActive { get; set; }
        internal float StrokeExpand { get { return _strokeExpand; } }
        internal StencilStates Stencils { get; private set; }
        internal DepthStencilState FillRuleState { get; private set; }
        internal RasterizerState RasterizerState { get; private set; }

        private VGState() { IsActive = false; }

        internal VGState(VGDevice device) 
        {
            IsActive = false;
            Device = device;
            Projection = new VGMatrixStack(2);
            ImageToSurface = new VGMatrixStack(2);
            GlyphToSurface = new VGMatrixStack(3);
            PathToSurface = new VGMatrixStack(4);
            PathToFillPaint = new VGMatrixStack(4);
            PathToTextPaint = new VGMatrixStack(4);
            PathToStrokePaint = new VGMatrixStack(4);
            ColorTransformation = new VGCxFormStack(4);

            SetAntialiasing(VGAntialiasing.Better);
            ResetDefaultValues();
        }

        public void ResetDefaultValues()
        {
            PathToSurface.Clear();
            ImageToSurface.Clear();
            GlyphToSurface.Clear();
            PathToFillPaint.Clear();
            PathToTextPaint.Clear();
            PathToStrokePaint.Clear();            
            ColorTransformation.Clear();

            NonScalingStroke = false;
            ColorTransformationEnabled = false;
            MaskingEnabled = false;
            StrokeThickness = 1f;
            StrokeMiterLimit = 4f;
            ClearColor = Color.Transparent;            
            StrokeStartCap = StrokeEndCap = VGLineCap.Butt;
            StrokeJoin = VGLineJoin.Miter;
            BlendMode = VGBlendMode.SrcOver;
            Mask = null;
            Font = null;
            MaskChannels = Vector4.UnitW;
            ColorChannels = ColorWriteChannels.All;
            TabSize = Vector2.One;
            FillPaint = StrokePaint = GlyphPaint = new VGColorPaint(Device, Color.White);

            // Not create states twice
            _fillRule = VGFillRule.EvenOdd;
            _readMask = VGStencilMasks.None; 
            WriteStencilMask = VGStencilMasks.None;
        }

        public VGState Clone()
        {
            return new VGState
            {
                Device = Device,
                Projection = new VGMatrixStack(Projection),
                ImageToSurface = new VGMatrixStack(ImageToSurface),
                GlyphToSurface = new VGMatrixStack(GlyphToSurface),
                PathToSurface = new VGMatrixStack(PathToSurface),
                PathToFillPaint = new VGMatrixStack(PathToFillPaint),
                PathToTextPaint = new VGMatrixStack(PathToTextPaint),
                PathToStrokePaint = new VGMatrixStack(PathToStrokePaint),
                ColorTransformation = new VGCxFormStack(ColorTransformation),
                Antialiasing = Antialiasing,

                NonScalingStroke = NonScalingStroke,
                ColorTransformationEnabled = ColorTransformationEnabled,
                MaskingEnabled = MaskingEnabled,
                StrokeMiterLimit = StrokeMiterLimit,
                ClearColor = ClearColor,
                StrokeStartCap = StrokeStartCap,
                StrokeEndCap = StrokeEndCap,
                StrokeJoin = StrokeJoin,
                FillRule = FillRule,
                BlendMode = BlendMode,
                Mask = Mask,
                Font = Font,
                MaskChannels = MaskChannels,
                ColorChannels = ColorChannels,
                TabSize = TabSize,
                FillPaint = FillPaint,
                GlyphPaint = GlyphPaint,
                StrokePaint = StrokePaint,
                
                _strokeExpand = _strokeExpand,
                _fillRule = _fillRule,
                _readMask = _readMask,
                WriteStencilMask = _writeMask
            };
        }

        public void SetProjection(float canvasWidth, float canvasHeight)
        {
            SetProjection(canvasWidth, canvasHeight, 0f, 0f, 1f, 1f);
        }
        public void SetProjection(float canvasWidth, float canvasHeight, float screenX, float screenY, float screenWidth, float screenHeight)
        {
            // Scaling
            screenWidth *= 2f;
            screenHeight *= -2f;
            screenWidth /= canvasWidth;
            screenHeight /= canvasHeight;

            // Translation
            screenX *= 2f;
            screenY *= 2f;
            screenX -= 1f;
            screenY = 1f - screenY;

            Projection.Clear(new VGMatrix(screenWidth, 0f, 0f, screenHeight, screenX, screenY));
        }
        public void SetAntialiasing(VGAntialiasing antialiasing)
        {
            if (antialiasing == VGAntialiasing.Best && Device.GraphicsDevice.PresentationParameters.MultiSampleCount < 16)
                antialiasing = VGAntialiasing.Better;

            if (antialiasing == VGAntialiasing.Better && Device.GraphicsDevice.PresentationParameters.MultiSampleCount < 8)
                antialiasing = VGAntialiasing.Faster;

            if (antialiasing == VGAntialiasing.Faster && Device.GraphicsDevice.PresentationParameters.MultiSampleCount < 4)
                antialiasing = VGAntialiasing.None;

            Antialiasing = antialiasing;
            UpdateRasterizerState();
        }

        public void SetLineStyle(ref VGLineStyle style)
        {
            PathToStrokePaint.Push(style.PathToStrokePaint);
            StrokeThickness = style.StrokeThickness;
            StrokeMiterLimit = style.StrokeMiterLimit;
            StrokeStartCap = style.StrokeStartCap;
            StrokeEndCap = style.StrokeEndCap;
            StrokeJoin = style.StrokeJoin;
        }
        public void SetFillPaint(VGPaint paint)
        {
            if (paint == null)
                throw new ArgumentNullException("paint");
            
            FillPaint = paint;
        }
        public void SetGlyphPaint(VGPaint paint)
        {
            if (paint == null)
                throw new ArgumentNullException("paint");

            GlyphPaint = paint;
        }
        public void SetStrokePaint(VGPaint paint)
        {
            if (paint == null)
                throw new ArgumentNullException("paint");

            StrokePaint = paint;
        }

        private void UpdateRasterizerState()
        {
            RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid,
                MultiSampleAntiAlias = Antialiasing != VGAntialiasing.None,
                ScissorTestEnable = _scissor.HasValue
            };

            if (IsActive)
            {
                if (_scissor.HasValue)
                    Device.GraphicsDevice.ScissorRectangle = _scissor.Value;
                Device.GraphicsDevice.RasterizerState = RasterizerState;
            }
        }
    }
}
