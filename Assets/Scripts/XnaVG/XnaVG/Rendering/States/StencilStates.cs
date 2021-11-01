using Microsoft.Xna.Framework.Graphics;

namespace XnaVG.Rendering.States
{
    using DSS = DepthStencilState;

    internal sealed class StencilStates
    {
        public DSS Set { get; private set; }
        public DSS EvenOdd { get; private set; }
        public DSS NonZero { get; private set; }
        public DSS Cover { get; private set; }

        public StencilStates(VGStencilMasks writeMask, VGStencilMasks readMask)
        {
            Set = Clone(_Set);            
            EvenOdd = Clone(_EvenOdd);
            NonZero = Clone(_NonZero);
            Cover = Clone(_Cover);

            if (writeMask != VGStencilMasks.None)
            {
                Set.StencilMask = Set.StencilWriteMask = (int)writeMask;
                EvenOdd.StencilMask = EvenOdd.StencilWriteMask = (int)writeMask;
                NonZero.StencilMask = NonZero.StencilWriteMask = (int)writeMask;
            }

            if (readMask != VGStencilMasks.None)
                Cover.StencilMask |= (int)readMask;

        }

        public static DSS ClearMasks(VGStencilMasks masks)
        {
            var dss = Clone(_Clear);
            dss.StencilMask = dss.StencilWriteMask = (int)masks;
            return dss;
        }
        private static DSS Clone(DSS original)
        {
            return new DSS
            {
                CounterClockwiseStencilDepthBufferFail = original.CounterClockwiseStencilDepthBufferFail,
                CounterClockwiseStencilFail = original.CounterClockwiseStencilFail,
                CounterClockwiseStencilFunction = original.CounterClockwiseStencilFunction,
                CounterClockwiseStencilPass = original.CounterClockwiseStencilPass,
                DepthBufferEnable = original.DepthBufferEnable,
                DepthBufferFunction = original.DepthBufferFunction,
                DepthBufferWriteEnable = original.DepthBufferWriteEnable,
                ReferenceStencil = original.ReferenceStencil,
                StencilDepthBufferFail = original.StencilDepthBufferFail,
                StencilEnable = original.StencilEnable,
                StencilFail = original.StencilFail,
                StencilFunction = original.StencilFunction,
                StencilMask = original.StencilMask,
                StencilPass = original.StencilPass,
                StencilWriteMask = original.StencilWriteMask,
                TwoSidedStencilMode = original.TwoSidedStencilMode                
            };
        }

        #region Templates

        private static readonly DSS _Set = new DSS
        {
            // Disable Depth
            DepthBufferFunction = CompareFunction.Never,
            DepthBufferWriteEnable = false,
            DepthBufferEnable = false,

            // Enable 1-bit Stencil
            StencilEnable = true,
            TwoSidedStencilMode = false,
            ReferenceStencil = 255,
            StencilMask = 1,
            StencilWriteMask = 1,

            // Invert Pixels
            StencilFunction = CompareFunction.Always,
            StencilDepthBufferFail = StencilOperation.Replace,
            StencilFail = StencilOperation.Replace,
            StencilPass = StencilOperation.Replace
        };

        private static readonly DSS _Clear = new DSS
        {
            // Disable Depth
            DepthBufferFunction = CompareFunction.Never,
            DepthBufferWriteEnable = false,
            DepthBufferEnable = false,

            // Enable 1-bit Stencil
            StencilEnable = true,
            TwoSidedStencilMode = false,
            ReferenceStencil = 0,
            StencilMask = 1,
            StencilWriteMask = 1,

            // Invert Pixels
            StencilFunction = CompareFunction.Always,
            StencilDepthBufferFail = StencilOperation.Zero,
            StencilFail = StencilOperation.Zero,
            StencilPass = StencilOperation.Zero
        };

        private static readonly DSS _EvenOdd = new DSS
        {
            // Disable Depth
            DepthBufferFunction = CompareFunction.Never,
            DepthBufferWriteEnable = false,
            DepthBufferEnable = false,

            // Enable 1-bit Stencil
            StencilEnable = true,
            TwoSidedStencilMode = false,
            ReferenceStencil = 0,
            StencilMask = 1,
            StencilWriteMask = 1,

            // Invert Pixels
            StencilFunction = CompareFunction.Always,
            StencilDepthBufferFail = StencilOperation.Invert,
            StencilFail = StencilOperation.Invert,
            StencilPass = StencilOperation.Invert
        };

        private static readonly DSS _NonZero = new DSS
        {
            // Disable Depth
            DepthBufferFunction = CompareFunction.Never,
            DepthBufferWriteEnable = false,
            DepthBufferEnable = false,

            // Enable 1-bit Stencil
            StencilEnable = true,
            TwoSidedStencilMode = true,
            ReferenceStencil = 0xFF,
            StencilMask = 1,
            StencilWriteMask = 1,

            // CW = Set
            StencilFunction = CompareFunction.Always,
            StencilDepthBufferFail = StencilOperation.Replace,
            StencilFail = StencilOperation.Replace,
            StencilPass = StencilOperation.Replace,

            // CCW = Clear
            CounterClockwiseStencilFunction = CompareFunction.Always,
            CounterClockwiseStencilDepthBufferFail = StencilOperation.Zero,
            CounterClockwiseStencilPass = StencilOperation.Zero,
            CounterClockwiseStencilFail = StencilOperation.Zero
        };

        private static readonly DSS _Cover = new DSS
        {
            // Disable Depth
            DepthBufferFunction = CompareFunction.Never,
            DepthBufferWriteEnable = false,
            DepthBufferEnable = false,

            // Enable 1-bit Stencil
            StencilEnable = true,
            TwoSidedStencilMode = false,
            ReferenceStencil = 0xFF,
            StencilMask = 1,
            StencilWriteMask = 1,

            // Render pixels with FILL flag only
            StencilFunction = CompareFunction.Equal,
            StencilDepthBufferFail = StencilOperation.Zero,
            StencilFail = StencilOperation.Zero,
            StencilPass = StencilOperation.Zero
        };

        #endregion
    }
}
