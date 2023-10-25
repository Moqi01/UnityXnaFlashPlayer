// #region License
// /*
// Microsoft Public License (Ms-PL)
// MonoGame - Copyright © 2009 The MonoGame Team
// 
// All rights reserved.
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
// accept the license, do not use the software.
// 
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
// U.S. copyright law.
// 
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
// your patent license from such contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
// notices that are present in the software.
// (D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
// a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
// code form, you may only do so under a license that complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
// or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
// permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
// purpose and non-infringement.
// */
// #endregion License
// 
using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Graphics
{
	public class BlendState : GraphicsResource
	{
        // TODO: We should be asserting if the state has
        // been changed after it has been bound to the device!

		public BlendFunction AlphaBlendFunction { get; set; }
		public Blend AlphaDestinationBlend { get; set; }
		public Blend AlphaSourceBlend { get; set; }
		public Color BlendFactor { get; set; }
		public BlendFunction ColorBlendFunction { get; set; }
		public Blend ColorDestinationBlend { get; set; }
		public Blend ColorSourceBlend { get; set; }
		public ColorWriteChannels ColorWriteChannels { get; set; }
		public ColorWriteChannels ColorWriteChannels1 { get; set; }
		public ColorWriteChannels ColorWriteChannels2 { get; set; }
		public ColorWriteChannels ColorWriteChannels3 { get; set; }
		public int MultiSampleMask { get; set; }

		public static readonly BlendState Additive;
		public static readonly BlendState AlphaBlend;
		public static readonly BlendState NonPremultiplied;
		public static readonly BlendState Opaque;
		
		public BlendState() 
        {
			AlphaBlendFunction = BlendFunction.Add;
			AlphaDestinationBlend = Blend.Zero;
			AlphaSourceBlend = Blend.One;
			BlendFactor = Color.White;
			ColorBlendFunction = BlendFunction.Add;
			ColorDestinationBlend = Blend.Zero;
			ColorSourceBlend = Blend.One;
			ColorWriteChannels = ColorWriteChannels.All;
			ColorWriteChannels1 = ColorWriteChannels.All;
			ColorWriteChannels2 = ColorWriteChannels.All;
			ColorWriteChannels3 = ColorWriteChannels.All;
			MultiSampleMask = Int32.MaxValue;
		}
		
		static BlendState() 
        {
			Additive = new BlendState() 
            {
				ColorSourceBlend = Blend.SourceAlpha,
				AlphaSourceBlend = Blend.SourceAlpha,
    			ColorDestinationBlend = Blend.One,	
				AlphaDestinationBlend = Blend.One
			};
			
			AlphaBlend = new BlendState()
            {
				ColorSourceBlend = Blend.One,
				AlphaSourceBlend = Blend.One,
				ColorDestinationBlend = Blend.InverseSourceAlpha,
				AlphaDestinationBlend = Blend.InverseSourceAlpha
			};
			
			NonPremultiplied = new BlendState() 
            {
				ColorSourceBlend = Blend.SourceAlpha,
				AlphaSourceBlend = Blend.SourceAlpha,
				ColorDestinationBlend = Blend.InverseSourceAlpha,
				AlphaDestinationBlend = Blend.InverseSourceAlpha
			};
			
			Opaque = new BlendState()
            {
				ColorSourceBlend = Blend.One,
				AlphaSourceBlend = Blend.One,			    
				ColorDestinationBlend = Blend.Zero,
				AlphaDestinationBlend = Blend.Zero
			};
		}

        public override string ToString ()
        {
            string blendStateName;

            if(this == BlendState.Additive)
                blendStateName = "Additive";
            else if (this == BlendState.AlphaBlend)
                blendStateName = "AlphaBlend";
            else if (this == BlendState.NonPremultiplied)
                blendStateName = "NonPremultiplied";
            else
                blendStateName = "Opaque";


            return string.Format("{0}.{1}", base.ToString(), blendStateName);
        }
	}

	public struct RenderTargetBinding
	{
		/// <summary>Creates an instance of this object.</summary>
		/// <param name="renderTarget">Identifies a cubemap render target.</param>
		/// <param name="cubeMapFace">Cubemap face.</param>
		// Token: 0x06000090 RID: 144 RVA: 0x00012D2C File Offset: 0x0001212C
		//public RenderTargetBinding(RenderTargetCube renderTarget, CubeMapFace cubeMapFace)
		//{
		//	if (renderTarget == null)
		//	{
		//		throw new ArgumentNullException("renderTarget");
		//	}
		//	this._renderTarget = renderTarget;
		//	this._cubeMapFace = cubeMapFace;
		//}

		/// <summary>Creates an instance of this object.</summary>
		/// <param name="renderTarget">Identifies a 2D render target.</param>
		// Token: 0x06000091 RID: 145 RVA: 0x00012CF8 File Offset: 0x000120F8
		public RenderTargetBinding(RenderTarget2D renderTarget)
		{
			if (renderTarget == null)
			{
				throw new ArgumentNullException("renderTarget");
			}
			this._renderTarget = renderTarget;
			this._cubeMapFace = CubeMapFace.PositiveX;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00001008 File Offset: 0x00000408
		public static implicit operator RenderTargetBinding(RenderTarget2D renderTarget)
		{
			RenderTargetBinding result = new RenderTargetBinding(renderTarget);
			return result;
		}

		/// <summary>Gets a 2D texture.</summary>
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00001024 File Offset: 0x00000424
		public Texture RenderTarget
		{
			get
			{
				return this._renderTarget;
			}
		}

		/// <summary>Gets one face of a cubemap.</summary>
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000103C File Offset: 0x0000043C
		public CubeMapFace CubeMapFace
		{
			get
			{
				return this._cubeMapFace;
			}
		}

		// Token: 0x040000AF RID: 175
		internal Texture _renderTarget;

		// Token: 0x040000B0 RID: 176
		internal CubeMapFace _cubeMapFace;
	}
	public class VertexBufferBinding
	{

	}
}

