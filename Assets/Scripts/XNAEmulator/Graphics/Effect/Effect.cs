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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Graphics
{

    public class Effect : GraphicsResource
    {
        public Effect(GraphicsDevice graphicsDevice, byte[] v) : this(graphicsDevice)
        {
            Parameters.Add("MatrixTransform", new EffectParameter());
            Parameters.Add("WorldMatrix", new EffectParameter());
            Parameters.Add("Viewport", new EffectParameter());
            //Techniques.Add("HueTechnique", new EffectTechnique());
            this.graphicsDevice = graphicsDevice;

        }
        protected Effect(Effect cloneSource)
        {
            this.graphicsDevice = cloneSource. graphicsDevice;


        }
        public Effect(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
                throw new ArgumentNullException("Graphics Device Cannot Be Null");

            this.graphicsDevice = graphicsDevice;
           // CurrentTechnique = new EffectTechnique("test",IntPtr .Zero,new EffectPassCollection());
            pParamCollection = new EffectParameterCollection();
            pParamCollection["xMatrix"] = new EffectParameter();
            pParamCollection["xRotation"] = new EffectParameter();
            pParamCollection["xBlendWeight"] = new EffectParameter();

            pParamCollection["xPixels"] = new EffectParameter();
            pParamCollection["xPalette"] = new EffectParameter();
            pParamCollection["xFontColorIndex"] = new EffectParameter();
            pParamCollection["xFontTotalColors"] = new EffectParameter();
            pParamCollection["xPalFx_Use"] = new EffectParameter();
            pParamCollection["xPalFx_Add"] = new EffectParameter();
            pParamCollection["xPalFx_Mul"] = new EffectParameter();
            pParamCollection["xPalFx_Invert"] = new EffectParameter();
            pParamCollection["xPalFx_Color"] = new EffectParameter();
            pParamCollection["xPalFx_SinMath"] = new EffectParameter();
            pParamCollection["xPalFx_Use"] = new EffectParameter();
            pParamCollection["xAI_Use"] = new EffectParameter();
            pParamCollection["xAI_Invert"] = new EffectParameter();
            pParamCollection["xAI_color"] = new EffectParameter();
            pParamCollection["xAI_preadd"] = new EffectParameter();
            pParamCollection["xAI_contrast"] = new EffectParameter();
            pParamCollection["xAI_postadd"] = new EffectParameter();
            pParamCollection["xAI_paladd"] = new EffectParameter();
            pParamCollection["xAI_palmul"] = new EffectParameter();
            pParamCollection["xAI_number"] = new EffectParameter();

            EffectTechnique[] techniques = new EffectTechnique[] {
                new EffectTechnique("DrawOLD",IntPtr .Zero,new EffectPassCollection()),
                 new EffectTechnique("Draw",IntPtr .Zero,new EffectPassCollection()),
                  new EffectTechnique("FontDraw",IntPtr .Zero,new EffectPassCollection()),
            };
            Techniques = new EffectTechniqueCollection(techniques);
            _currentTechnique = techniques[1];
            //pParamCollection["doctorColor"] =
        }
        internal EffectTechnique _currentTechnique;
        private EffectParameterCollection pParamCollection;
        public EffectParameterCollection Parameters
        {
            get
            {
                return this.pParamCollection;
            }
        }

        //public EffectParameter Parameters
        //{
        //    get
        //    {
        //        UnityEngine.Debug.Log("Effect  Parameters");
        //        return new EffectParameter();
        //    }
        //}

        public  EffectTechnique CurrentTechnique
        {
            get
            {
                
                return this._currentTechnique;
            }
            set
            {
                this._currentTechnique = value;
                if(_currentTechnique!=null&&_currentTechnique.Name == "FontDraw")
                {
                    DrawGame.instance.SetPass(1);
                }
                else
                {
                    DrawGame.instance.SetPass(0);
                }
            }
        }

        protected internal virtual void OnApply()
        {
        }
        public EffectTechniqueCollection Techniques { get; private set; }
    }

    public class EffectTechniqueCollection : IEnumerable<EffectTechnique>, IEnumerable
    {
        // Token: 0x17000205 RID: 517
        // (get) Token: 0x0600092D RID: 2349 RVA: 0x0000A370 File Offset: 0x00008570
        public int Count
        {
            get
            {
                return this._techniques.Length;
            }
        }

        // Token: 0x0600092E RID: 2350 RVA: 0x0000A37A File Offset: 0x0000857A
        internal EffectTechniqueCollection(EffectTechnique[] techniques)
        {
            this._techniques = techniques;
        }

        // Token: 0x0600092F RID: 2351 RVA: 0x0002E998 File Offset: 0x0002CB98
        internal EffectTechniqueCollection Clone(Effect effect)
        {
            EffectTechnique[] array = new EffectTechnique[this._techniques.Length];
            for (int i = 0; i < this._techniques.Length; i++)
            {
                array[i] = new EffectTechnique(effect, this._techniques[i]);
            }
            return new EffectTechniqueCollection(array);
        }

        // Token: 0x17000206 RID: 518
        public EffectTechnique this[int index]
        {
            get
            {
                return this._techniques[index];
            }
        }

        // Token: 0x17000207 RID: 519
        public EffectTechnique this[string name]
        {
            get
            {
                foreach (EffectTechnique effectTechnique in this._techniques)
                {
                    if (effectTechnique.Name == name)
                    {
                        return effectTechnique;
                    }
                }
                return null;
            }
        }

        // Token: 0x06000932 RID: 2354 RVA: 0x0000A393 File Offset: 0x00008593
        public IEnumerator<EffectTechnique> GetEnumerator()
        {
            return ((IEnumerable<EffectTechnique>)this._techniques).GetEnumerator();
        }

        // Token: 0x06000933 RID: 2355 RVA: 0x0000A3A5 File Offset: 0x000085A5
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._techniques.GetEnumerator();
        }

        // Token: 0x040003CC RID: 972
        private readonly EffectTechnique[] _techniques;
    }

    public sealed class EffectTechnique
    {
        //internal Effect _parent = effect;
        private string name;
        public string Name
        {

            get
            {
                return name;
            }
            
            private set
            {
                name = value;
            }
        }
        // Token: 0x04000169 RID: 361
        internal EffectPassCollection pPasses;

        public EffectPassCollection Passes
        {
            get
            {
                if (pPasses == null)
                    pPasses = new EffectPassCollection();
                return this.pPasses;
            }
            private set
            {
                this.pPasses = value;
            }
        }
        private IntPtr t;
        internal IntPtr TechniquePointer
        {
            
            get
            {
                return this.t;
            }
            
            private set
            {
                this.t = value;
            }
        }
        public EffectAnnotationCollection Annotations { get; private set; }
        public  EffectTechnique(string name, IntPtr pointer, EffectPassCollection passes)
        {
            Name = name;
            Passes = passes;
            TechniquePointer = pointer;
        }

        internal EffectTechnique(Effect effect, EffectTechnique cloneSource)
        {
            this.Name = cloneSource.Name;
            this.Annotations = cloneSource.Annotations;
            this.Passes = cloneSource.Passes;
        }
    }


    public sealed class EffectPassCollection : IEnumerable<EffectPass>
    {
        private List<EffectPass> pPass=new List<EffectPass> ();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<EffectPass>)pPass).GetEnumerator();

        }

        public List<EffectPass>.Enumerator GetEnumerator()
        {
            return this.pPass.GetEnumerator();
        }

        IEnumerator<EffectPass> IEnumerable<EffectPass>.GetEnumerator()
        {
            return ((IEnumerable<EffectPass>)pPass).GetEnumerator();
        }

       

        public int Count
        {
            get
            {
                return this.pPass.Count;
            }
        }

        // Token: 0x17000049 RID: 73
        public EffectPass this[int index]
        {
            get
            {
                if (index >= 0 && index < this.pPass.Count)
                {
                    return this.pPass[index];
                }
                return null;
            }
        }

        // Token: 0x1700004A RID: 74
        public EffectPass this[string name]
        {
            get
            {
                List<EffectPass>.Enumerator enumerator = this.pPass.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    do
                    {
                        EffectPass effectPass = enumerator.Current;
                        if (effectPass.Name == name)
                        {
                            return effectPass;
                        }
                    }
                    while (enumerator.MoveNext());
                }
                return null;
            }
        }
    }

    public sealed class EffectParameterCollection : IEnumerable<EffectParameter>
    {
        public List<EffectParameter>.Enumerator GetEnumerator()
        {
            return this.pParameter.GetEnumerator();
        }

        IEnumerator<EffectParameter> IEnumerable<EffectParameter>.GetEnumerator()
        {
            return ((IEnumerable<EffectParameter>)pParameter).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<EffectParameter>)pParameter).GetEnumerator();
        }

        internal void Add(string v, EffectParameter effectParameter)
        {
            pParameter.Add(effectParameter);
        }

        public int Count
        {
            get
            {
                return this.pParameter.Count;
            }
        }

        // Token: 0x17000056 RID: 86
        public EffectParameter this[int index]
        {
            get
            {
                if (index >= 0 && index < this.pParameter.Count)
                {
                    return this.pParameter[index];
                }
                return null;
            }
            set
            {
                pParameter[index] = value;
            }
        }

        // Token: 0x17000057 RID: 87
        public EffectParameter this[string name]
        {
            get
            {
                if (pParameter != null)
                {
                    List<EffectParameter>.Enumerator enumerator = this.pParameter.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        do
                        {
                            EffectParameter effectParameter = enumerator.Current;
                            if (effectParameter._name == name)
                            {
                                return effectParameter;
                            }
                        }
                        while (enumerator.MoveNext());
                    }
                }
                return new EffectParameter();
            }
            set
            {
                
                EffectParameter effectParameter = value;
                effectParameter._name = name;
                pParameter.Add(effectParameter);



            }
        }

        private List<EffectParameter> pParameter=new List<EffectParameter> ();
    }
    public sealed class EffectParameter
    {
        public string Name { get { return _name; } }
        public string _name;

        public int _rows;

        public int _columns;

        public EffectParameterCollection pElementCollection;

        public EffectParameterClass _paramClass;

        public EffectParameter()
        {

        }

        public void SetValue(float value)
        {
            DrawGame.instance.SetFloatValue(_name, value);
        }

        public void SetValue(bool value)
        {
            //UnityEngine.Debug.Log("?");
            //// if (this.pElementCollection.Count != 0)
            //{
            //    UnityEngine.Debug.Log("Effect!");
            //    //throw new InvalidCastException();
            //}
            //EffectParameterClass paramClass = this._paramClass;
            //if (paramClass == EffectParameterClass.Vector && this._rows == 1)
            //{
            //    int columns = this._columns;
            //    if (columns != 2)
            //    {
            //        if (columns != 3)
            //        {
            //            if (columns != 4)
            //            { }
            //        }
            //    }
            //}
            DrawGame.instance.SetIntValue(_name, value?1:0);

        }

        public void SetValue(Vector2 value)
        {
            UnityEngine.Vector2 v = new UnityEngine.Vector2(value.X, value.Y);
            DrawGame.instance.SetVector2(_name, v);

        }


        public void SetValue(Vector4 value)
        {
            UnityEngine.Vector4 v = new UnityEngine.Vector4(value.X, value.Y,value .Z,value.W);
            DrawGame.instance.SetVector2(_name, v);
        }

        public void SetValue(Vector3 value)
        {
            UnityEngine.Vector3 v = new UnityEngine.Vector3(value.X, value.Y, value.Z);
            DrawGame.instance.SetVector2(_name, v);
        }

        public void SetValue(Matrix value)
        {
            UnityEngine.Matrix4x4 v = new UnityEngine.Matrix4x4();
            v.m00 = value.M11;
            v.m01 = value.M12;
            v.m02 = value.M13;
            v.m03 = value.M14;
            v.m10 = value.M21;
            v.m11 = value.M22;
            v.m12 = value.M23;
            v.m13 = value.M24;
            v.m20 = value.M31;
            v.m21 = value.M32;
            v.m22 = value.M33;
            v.m23 = value.M34;
            v.m30 = value.M41;
            v.m31 = value.M42;
            v.m32 = value.M43;
            v.m33 = value.M44;
            DrawGame.instance.SetM(_name, v);
        }

        public void SetValue(XnaVG.VGMatrix value)
        {
            UnityEngine.Matrix4x4 v = new UnityEngine.Matrix4x4();
            v.m00 = value.M11;
            v.m01 = value.M12;
            v.m02 = value.M13;
            //v.m03 = value.M14;
            v.m10 = value.M21;
            v.m11 = value.M22;
            v.m12 = value.M23;
            //v.m13 = value.M24;
            v.m20 = value.M31;
            v.m21 = value.M32;
            v.m22 = value.M33;
           // v.m23 = value.M34;
           // v.m30 = value.M41;
           // v.m31 = value.M42;
           // v.m32 = value.M43;
           // v.m33 = value.M44;
            DrawGame.instance.SetM(_name, v);
        }

        public void SetValue(Texture2D value)
        {
            DrawGame.instance.SetTexture(_name, value.unityTexture2D);
        }
    }

    public enum EffectParameterClass
    {
        // Token: 0x040003D7 RID: 983
        Scalar,
        // Token: 0x040003D8 RID: 984
        Vector,
        // Token: 0x040003D9 RID: 985
        Matrix,
        // Token: 0x040003DA RID: 986
        Object,
        // Token: 0x040003DB RID: 987
        Struct
    }


    public interface IGraphicsDeviceService
    {
        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000112 RID: 274
        GraphicsDevice GraphicsDevice { get; }

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x06000113 RID: 275
        // (remove) Token: 0x06000114 RID: 276
        event EventHandler<EventArgs> DeviceCreated;

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x06000115 RID: 277
        // (remove) Token: 0x06000116 RID: 278
        event EventHandler<EventArgs> DeviceResetting;

        // Token: 0x14000009 RID: 9
        // (add) Token: 0x06000117 RID: 279
        // (remove) Token: 0x06000118 RID: 280
        event EventHandler<EventArgs> DeviceReset;

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x06000119 RID: 281
        // (remove) Token: 0x0600011A RID: 282
        event EventHandler<EventArgs> DeviceDisposing;
    }

    public interface IVertexType
    {
        // Token: 0x1700008D RID: 141
        // (get) Token: 0x060002F0 RID: 752
        VertexDeclaration VertexDeclaration { get; }
    }

    public class VertexDeclaration : GraphicsResource
    {
        #region Public Properties

        public int VertexStride
        {
            get;
            private set;
        }

        #endregion

        #region Internal Variables

        internal VertexElement[] elements;

        #endregion

        #region Public Constructors

        public VertexDeclaration(
            params VertexElement[] elements
        ) : this(GetVertexStride(elements), elements)
        {
        }

        public VertexDeclaration(
            int vertexStride,
            params VertexElement[] elements
        )
        {
            if ((elements == null) || (elements.Length == 0))
            {
                throw new ArgumentNullException("elements", "Elements cannot be empty");
            }

            this.elements = (VertexElement[])elements.Clone();
            VertexStride = vertexStride;
        }

        #endregion

        #region Public Methods

        public VertexElement[] GetVertexElements()
        {
            return (VertexElement[])elements.Clone();
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Returns the VertexDeclaration for Type.
        /// </summary>
        /// <param name="vertexType">A value type which implements the IVertexType interface.</param>
        /// <returns>The VertexDeclaration.</returns>
        /// <remarks>
        /// Prefer to use VertexDeclarationCache when the declaration lookup
        /// can be performed with a templated type.
        /// </remarks>
        internal static VertexDeclaration FromType(Type vertexType)
        {
            if (vertexType == null)
            {
                throw new ArgumentNullException("vertexType", "Cannot be null");
            }

            if (!vertexType.IsValueType)
            {
                throw new ArgumentException("vertexType", "Must be value type");
            }

            IVertexType type = Activator.CreateInstance(vertexType) as IVertexType;
            if (type == null)
            {
                throw new ArgumentException("vertexData does not inherit IVertexType");
            }

            VertexDeclaration vertexDeclaration = type.VertexDeclaration;
            if (vertexDeclaration == null)
            {
                throw new ArgumentException("vertexType's VertexDeclaration cannot be null");
            }

            return vertexDeclaration;
        }

        #endregion

        #region Private Static VertexElement Methods

        private static int GetVertexStride(VertexElement[] elements)
        {
            int max = 0;

            for (int i = 0; i < elements.Length; i += 1)
            {
                int start = elements[i].Offset + GetTypeSize(elements[i].VertexElementFormat);
                if (max < start)
                {
                    max = start;
                }
            }

            return max;
        }

        private static int GetTypeSize(VertexElementFormat elementFormat)
        {
            switch (elementFormat)
            {
                case VertexElementFormat.Single:
                    return 4;
                case VertexElementFormat.Vector2:
                    return 8;
                case VertexElementFormat.Vector3:
                    return 12;
                case VertexElementFormat.Vector4:
                    return 16;
                case VertexElementFormat.Color:
                    return 4;
                case VertexElementFormat.Byte4:
                    return 4;
                case VertexElementFormat.Short2:
                    return 4;
                case VertexElementFormat.Short4:
                    return 8;
                case VertexElementFormat.NormalizedShort2:
                    return 4;
                case VertexElementFormat.NormalizedShort4:
                    return 8;
                case VertexElementFormat.HalfVector2:
                    return 4;
                case VertexElementFormat.HalfVector4:
                    return 8;
            }
            return 0;
        }

        #endregion
    }



    public struct VertexPositionTexture : IVertexType
    {
        // Token: 0x0600051C RID: 1308 RVA: 0x0003C648 File Offset: 0x0003BA48
        public VertexPositionTexture(Vector3 position, Vector2 textureCoordinate)
        {
            this.Position = position;
            this.TextureCoordinate = textureCoordinate;
        }

        // Token: 0x1700011D RID: 285
        // (get) Token: 0x0600051D RID: 1309 RVA: 0x0003C664 File Offset: 0x0003BA64
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexPositionTexture.VertexDeclaration;
            }
        }

        // Token: 0x0600051E RID: 1310 RVA: 0x0003C678 File Offset: 0x0003BA78
        public override int GetHashCode()
        {
            return 0;
            // return Helpers.SmartGetHashCode(this);
        }

        // Token: 0x0600051F RID: 1311 RVA: 0x0003C698 File Offset: 0x0003BA98
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Position:{0} TextureCoordinate:{1}}}", new object[]
            {
                this.Position,
                this.TextureCoordinate
            });
        }

        // Token: 0x06000520 RID: 1312 RVA: 0x0003C6D8 File Offset: 0x0003BAD8
        public static bool operator ==(VertexPositionTexture left, VertexPositionTexture right)
        {
            return left.Position == right.Position && left.TextureCoordinate == right.TextureCoordinate;
        }

        // Token: 0x06000521 RID: 1313 RVA: 0x0003C710 File Offset: 0x0003BB10
        public static bool operator !=(VertexPositionTexture left, VertexPositionTexture right)
        {
            return !(left == right);
        }

        // Token: 0x06000522 RID: 1314 RVA: 0x0003C728 File Offset: 0x0003BB28
        public override bool Equals(object obj)
        {
            return obj != null && !(obj.GetType() != base.GetType()) && this == (VertexPositionTexture)obj;
        }

        // Token: 0x06000523 RID: 1315 RVA: 0x0003C76C File Offset: 0x0003BB6C
        // Note: this type is marked as 'beforefieldinit'.
        static VertexPositionTexture()
        {
        }

        // Token: 0x04000316 RID: 790
        //[SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector3 Position;

        // Token: 0x04000317 RID: 791
        // [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields")]
        public Vector2 TextureCoordinate;

        // Token: 0x04000318 RID: 792
        //[SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(new VertexElement[]
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        })
        {
            Name = "VertexPositionTexture.VertexDeclaration"
        };
    }
    public sealed class EffectPass
    {
        public string Name
        {
            
            get
            {
                return this.name;
            }
            
            private set
            {
                this.name = value;
            }
        }

        // Token: 0x170001C6 RID: 454
        // (get) Token: 0x06000B9E RID: 2974 RVA: 0x0002A379 File Offset: 0x00028579
        // (set) Token: 0x06000B9F RID: 2975 RVA: 0x0002A381 File Offset: 0x00028581
        //public EffectAnnotationCollection Annotations
        //{
           
        //    get
        //    {
        //        return this. k__BackingField;
        //    }
          
        //    private set
        //    {
        //        this.k__BackingField = value;
        //    }
        //}

        // Token: 0x06000BA0 RID: 2976 RVA: 0x0002A38A File Offset: 0x0002858A
        internal EffectPass(string name, EffectAnnotationCollection annotations, Effect parent, uint passIndex)
        {
            this.Name = name;
           // this.Annotations = annotations;
            this.parentEffect = parent;
            this.pass = passIndex;
        }

        // Token: 0x06000BA1 RID: 2977 RVA: 0x0002A3AF File Offset: 0x000285AF
        public void Apply()
        {
            this.parentEffect.OnApply();
           // this.parentEffect.INTERNAL_applyEffect(this.pass);
        }

        // Token: 0x04000648 RID: 1608
        private Effect parentEffect;

        // Token: 0x04000649 RID: 1609
        private uint pass;

        // Token: 0x0400064A RID: 1610
       
        private string name;

		// Token: 0x0400064B RID: 1611
		
       // private EffectAnnotationCollection k__BackingField;
    }

    public sealed class EffectAnnotationCollection 
    {
        // Token: 0x170001B6 RID: 438
        // (get) Token: 0x06000B4F RID: 2895 RVA: 0x0002850C File Offset: 0x0002670C
        //public int Count
        //{
        //    get
        //    {
        //        return this.elements.Count;
        //    }
        //}

        // Token: 0x170001B7 RID: 439
        //public EffectAnnotation this[int index]
        //{
        //    get
        //    {
        //        return this.elements[index];
        //    }
        //}

        //// Token: 0x170001B8 RID: 440
        //public EffectAnnotation this[string name]
        //{
        //    get
        //    {
        //        foreach (EffectAnnotation effectAnnotation in this.elements)
        //        {
        //            if (name.Equals(effectAnnotation.Name))
        //            {
        //                return effectAnnotation;
        //            }
        //        }
        //        return null;
        //    }
        //}

        //// Token: 0x06000B52 RID: 2898 RVA: 0x0002858C File Offset: 0x0002678C
        //internal EffectAnnotationCollection(List<EffectAnnotation> value)
        //{
        //    this.elements = value;
        //}

        //// Token: 0x06000B53 RID: 2899 RVA: 0x0002859B File Offset: 0x0002679B
        //public List<EffectAnnotation>.Enumerator GetEnumerator()
        //{
        //    return this.elements.GetEnumerator();
        //}

        // Token: 0x06000B54 RID: 2900 RVA: 0x000285A8 File Offset: 0x000267A8
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return this.elements.GetEnumerator();
        //}

        // Token: 0x06000B55 RID: 2901 RVA: 0x000285BA File Offset: 0x000267BA
        //IEnumerator<EffectAnnotation> IEnumerable<EffectAnnotation>.GetEnumerator()
        //{
        //    return this.elements.GetEnumerator();
        //}

        //// Token: 0x0400062A RID: 1578
        //private List<EffectAnnotation> elements;
    }


    public interface IGraphicsResource
    {
        // Token: 0x0600011B RID: 283
        void ReleaseNativeObject([MarshalAs(UnmanagedType.U1)] bool disposeManagedResource);

        // Token: 0x0600011C RID: 284
        int SaveDataForRecreation();

        // Token: 0x0600011D RID: 285
        int RecreateAndPopulateObject();
    }

    public class IndexBuffer : GraphicsResource, IGraphicsResource
    {
        int IGraphicsResource.RecreateAndPopulateObject()
        {
            throw new NotImplementedException();
        }

        void IGraphicsResource.ReleaseNativeObject(bool disposeManagedResource)
        {
            throw new NotImplementedException();
        }

        int IGraphicsResource.SaveDataForRecreation()
        {
            throw new NotImplementedException();
        }
        public IndexBuffer(GraphicsDevice graphicsDevice, IndexElementSize indexElementSize, int indexCount, BufferUsage usage)
        {
        }
        public void SetData<T>(T[] data) where T : struct
        {
            int elementCount;
            if (data != null)
            {
                elementCount = data.Length;
            }
            else
            {
                elementCount = 0;
            }
           // this.SetData<T>(0, data, 0, elementCount);
        }
    }
}
