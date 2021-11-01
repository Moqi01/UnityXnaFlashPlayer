// #region License
// /*
// Microsoft Public License (Ms-PL)
// XnaTouch - Copyright © 2009 The XnaTouch Team
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
	public class SamplerState : GraphicsResource
	{
		static SamplerState () 
        {
			AnisotropicClamp = new SamplerState () 
            {
				Filter = TextureFilter.Anisotropic,
				_addressU = TextureAddressMode.Clamp,
				_addressV = TextureAddressMode.Clamp,
				_addressW = TextureAddressMode.Clamp,
			};
			
			AnisotropicWrap = new SamplerState () 
            {
				Filter = TextureFilter.Anisotropic,
				_addressU = TextureAddressMode.Wrap,
				_addressV = TextureAddressMode.Wrap,
				_addressW = TextureAddressMode.Wrap,
			};
			
			LinearClamp = new SamplerState () 
            {
				Filter = TextureFilter.Linear,
				_addressU = TextureAddressMode.Clamp,
				_addressV = TextureAddressMode.Clamp,
				_addressW = TextureAddressMode.Clamp,
			};
			
			LinearWrap = new SamplerState () 
            {
				Filter = TextureFilter.Linear,
				_addressU = TextureAddressMode.Wrap,
				_addressV = TextureAddressMode.Wrap,
				_addressW = TextureAddressMode.Wrap,
			};
			
			PointClamp = new SamplerState () 
            {
				Filter = TextureFilter.Point,
				_addressU = TextureAddressMode.Clamp,
				_addressV = TextureAddressMode.Clamp,
				_addressW = TextureAddressMode.Clamp,
			};
			
			PointWrap = new SamplerState () 
            {
				Filter = TextureFilter.Point,
				_addressU = TextureAddressMode.Wrap,
				_addressV = TextureAddressMode.Wrap,
				_addressW = TextureAddressMode.Wrap,
			};
		}

        public SamplerState Clone()
        {
            return new SamplerState(this);
        }

        private SamplerState(SamplerState cloneSource)
        {
            base.Name = cloneSource.Name;
            //this._filter = cloneSource._filter;
            this._addressU = cloneSource._addressU;
            this._addressV = cloneSource._addressV;
            this._addressW = cloneSource._addressW;
            //this._borderColor = cloneSource._borderColor;
            this._maxAnisotropy = cloneSource._maxAnisotropy;
            this._maxMipLevel = cloneSource._maxMipLevel;
            this._mipMapLevelOfDetailBias = cloneSource._mipMapLevelOfDetailBias;
            //this._comparisonFunction = cloneSource._comparisonFunction;
            this._filterMode = cloneSource._filterMode;
            Filter = cloneSource.Filter;
        }

        public SamplerState()
        {

        }

		public TextureAddressMode AddressU { get { return _addressU; } set {  _addressU = value; } }
		public TextureAddressMode AddressV { get { return _addressU; } set {  _addressU = value; } }
		public TextureAddressMode AddressW { get { return _addressU; } set {  _addressU = value; } }

		internal void BindToGraphicsDevice(GraphicsDevice device)
        {
           
            base.GraphicsDevice = device;
        }

        public static readonly SamplerState AnisotropicClamp;
		public static readonly SamplerState AnisotropicWrap;
		public static readonly SamplerState LinearClamp;
		public static readonly SamplerState LinearWrap;
		public static readonly SamplerState PointClamp;
		public static readonly SamplerState PointWrap;
		
		public TextureAddressMode _addressU { get; set; }
		public TextureAddressMode _addressV { get; set; }
		public TextureAddressMode _addressW { get; set; }
		public TextureFilter Filter { get; set; }
        public TextureFilter _filterMode { get; set; }
        public int _maxAnisotropy { get; set; }
		public int _maxMipLevel { get; set; }
		public float _mipMapLevelOfDetailBias { get; set; }

        internal SamplerState GetState(GraphicsDevice device)
        {
            if (this._state == null)
            {
                _state = Clone();
            }
            return this._state;
        }
        private SamplerState _state;
    }
}

