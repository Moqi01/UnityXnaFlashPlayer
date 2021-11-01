#region License
/*
MIT License
Copyright © 2006 - 2007 The Mono.Xna Team

All rights reserved.

Authors:
 * Rob Loach
 * Olivier Dufour
 * Lars Magnusson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion License

using Microsoft.Xna.Framework;
using System;
using System.Globalization;

namespace Microsoft.Xna.Framework.Graphics
{
    [Serializable]
    public struct Viewport
    {
        #region Fields

        private int _x;
        private int _y;
        private int _width;
        private int _height;
        private float minDepth;
        private float maxDepth;
        private Rectangle titleSafeArea;

        #endregion Fields

        #region Properties

        public float AspectRatio
        {
            get { return _width / _height; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public float MaxDepth
        {
            get { return maxDepth; }
            set { maxDepth = value; }
        }

        public float MinDepth
        {
            get { return minDepth; }
            set { minDepth = value; }
        }

        public Rectangle TitleSafeArea
        {
            get { return titleSafeArea; }
        }

        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion Properties
        public Viewport(Rectangle bounds)
        {
            minDepth = 0;
            maxDepth = 0;
            this._x = bounds.X;
            this._y = bounds.Y;
            this._width = bounds.Width;
            this._height = bounds.Height;
            this._minZ = 0f;
            this._maxZ = 1f;
            titleSafeArea = new Rectangle(_x, _y, _width, _height);
        }

        public Viewport (
         int x,
         int y,
         int width,
         int height
		)
		{
			this._x = x;
			this._y = y;
			this._width = width;
			this._height = height;
            minDepth = 0;
            maxDepth = 0;
            this._minZ = 0f;
            this._maxZ = 1f;
            
            titleSafeArea = new Rectangle(x, y, width, height);
		}
        #region Public Methods

        public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            //TODO Use the ref versions of methods to speed things up

            //done it here is better than use D3DVect3Project in glProject
            // because it is not the same function and not need gl for that
            /*
            Matrix:
            M =
            width/2				0			0			0
                0			-height/2		0			0
                0				0		Max-Min			0
            X + width/2		height + Y		Min			1

            M * P * V * W * S
            S = source, W=world, V=view, P=projection
            */
            //here must do projection
            Vector4 result = Vector4.Transform(source, world);
            result = Vector4.Transform(result, view);
            result = Vector4.Transform(result, projection);
            result.Z = result.Z * (this.maxDepth - this.minDepth);
            result = Vector4.Divide(result, result.W);

            Vector3 finalResult = new Vector3(result.X, result.Y, result.Z);

            finalResult.X = this.X + (1 + finalResult.X) * this._width / 2;
            finalResult.Y = this.Y + (1 - finalResult.Y) * this._height / 2;
            finalResult.Z = finalResult.Z + minDepth;
            return finalResult;
        }

        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            //TODO Use the ref versions of methods to speed things up

            Vector4 result;
            result.X = ((source.X - this.X) * 2 / this._width) - 1;
            result.Y = 1 - ((source.Y - this.Y) * 2 / this._height);
            result.Z = source.Z - minDepth;
            if (this.maxDepth - this.minDepth == 0)
                result.Z = 0;
            else
                result.Z = result.Z / (this.maxDepth - this.minDepth);
            result.W = 1f;
            result = Vector4.Transform(result, Matrix.Invert(projection));
            result = Vector4.Transform(result, Matrix.Invert(view));
            result = Vector4.Transform(result, Matrix.Invert(world));
            result = Vector4.Divide(result, result.W);
            return new Vector3(result.X, result.Y, result.Z);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{X:{0} Y:{1} Width:{2} Height:{3} MinDepth:{4} MaxDepth:{5}}}", new object[] { _x, _y, _width, _height, minDepth, maxDepth });
        }

        internal static Rectangle GetTitleSafeArea(int x, int y, int w, int h)
        {
            return new Rectangle(x, y, w, h);
        }

        public Rectangle Bounds
        {
            get
            {
                Rectangle result;
                result.X = this._x;
                result.Y = this._y;
                result.Width = this._width;
                result.Height = this._height;
                return result;
            }
            set
            {
                this._x = value.X;
                this._y = value.Y;
                this._width = value.Width;
                this._height = value.Height;
            }
        }
        // Token: 0x04000352 RID: 850
        

        // Token: 0x04000356 RID: 854
        private float _minZ;

        // Token: 0x04000357 RID: 855
        private float _maxZ;

        #endregion Methods


    }
}