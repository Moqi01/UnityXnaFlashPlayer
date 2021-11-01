using System;
using Microsoft.Xna.Framework;

namespace XnaVG
{
    public class VGMatrix
    {
        public static VGMatrix Identity { get { return new VGMatrix(1f, 0f, 0f, 1f, 0f, 0f); } }
        public static VGMatrix Zero { get { return new VGMatrix(0f, 0f, 0f, 0f, 0f, 0f); } }

        private float[] _m = new float[9];

        public float M11 { get { return _m[0]; } set { _m[0] = value; } }
        public float M12 { get { return _m[1]; } set { _m[1] = value; } }        
        public float M21 { get { return _m[3]; } set { _m[3] = value; } }
        public float M22 { get { return _m[4]; } set { _m[4] = value; } }        
        public float M31 { get { return _m[6]; } set { _m[6] = value; } }
        public float M32 { get { return _m[7]; } set { _m[7] = value; } }
        public float M13 { get { return _m[2]; } set { _m[2] = value; } }
        public float M23 { get { return _m[5]; } set { _m[5] = value; } }
        public float M33 { get { return _m[8]; } set { _m[8] = value; } }

        public bool IsIdentity
        {
            get
            {
                return
                    (M11 == 1f) && (M22 == 1f) && (M33 == 1f) &&
                    (M21 == 0f) && (M32 == 0f) && (M13 == 0f) &&
                    (M31 == 0f) && (M12 == 0f) && (M23 == 0f);
            }
        }
        public bool IsZero
        {
            get
            {
                return
                    (M11 == 0f) && (M22 == 0f) && (M33 == 0f) &&
                    (M21 == 0f) && (M32 == 0f) && (M13 == 0f) &&
                    (M31 == 0f) && (M12 == 0f) && (M23 == 0f);
            }
        }
        public float ScaleX 
        {
            get { return new Vector2(_m[0], _m[3]).Length(); }
            set            
            {
                var current = Scale(Vector2.UnitX);
                if (current.LengthSquared() == 0f)
                {
                    M11 = value;
                    M21 = 0f;
                }
                else
                {
                    M11 = current.X * value;
                    M21 = current.Y * value;
                }
            }
        }
        public float ScaleY 
        {
            get { return new Vector2(_m[1], _m[4]).Length(); }
            set
            {
                var current = Scale(Vector2.UnitY);
                if (current.LengthSquared() == 0f)
                {                    
                    M22 = 0f;
                    M12 = value;
                }
                else
                {
                    M12 = current.X * value;
                    M22 = current.Y * value;
                }
            }
        }
        public float Rotation 
        {
            get
            {
                Vector2 diff = Scale(Vector2.UnitX) - Vector2.UnitX;
                return MathHelper.WrapAngle((float)Math.Atan2(diff.Y, diff.X));
            }
            set
            {
                float xs = ScaleX, ys = ScaleY, sin = (float)Math.Sin(value), cos = (float)Math.Cos(value);
                M11 = cos * xs;
                M12 = -sin * ys;
                M21 = sin * xs;
                M22 = cos * ys;
            }
        }

        public VGMatrix() {  }

        public VGMatrix(float scaleX, float skew0, float skew1, float scaleY, float transX, float transY)
        {
            _m[0] = scaleX;
            _m[1] = skew0;
            _m[2] = 0f;

            _m[3] = skew1;
            _m[4] = scaleY;
            _m[5] = 0f;

            _m[6] = transX;
            _m[7] = transY;
            _m[8] = 1f;
        }

        public VGMatrix(VGMatrix other)
        {
            Array.Copy(other._m, _m, _m.Length);
        }

        public Vector4 TransformExtents(Vector4 extents)
        {
            Vector2 a = this * new Vector2(extents.X, extents.Y);
            Vector2 b = this * new Vector2(extents.Z, extents.W);
            Vector2 c = this * new Vector2(extents.X, extents.W);
            Vector2 d = this * new Vector2(extents.Z, extents.Y);

            return new Vector4(
                Math.Min(Math.Min(a.X, b.X), Math.Min(c.X, d.X)),
                Math.Min(Math.Min(a.Y, b.Y), Math.Min(c.Y, d.Y)),
                Math.Max(Math.Max(a.X, b.X), Math.Max(c.X, d.X)),
                Math.Max(Math.Max(a.Y, b.Y), Math.Max(c.Y, d.Y)));
        }
        public Vector2 Transform(Vector2 vector)
        {
            return new Vector2(
                vector.X * _m[0] + vector.Y * _m[3] + _m[6],
                vector.X * _m[1] + vector.Y * _m[4] + _m[7]);
        }
        public Vector2 Scale(Vector2 vector)
        {
            return new Vector2(
                vector.X * _m[0] + vector.Y * _m[3],
                vector.X * _m[1] + vector.Y * _m[4]);
        }

        public VGMatrix MakeInverse()
        {
            float c = (_m[0] * _m[4] - _m[1] * _m[3]);
            if (Math.Abs(c) < Rendering.Constants.Epsilon)
                return VGMatrix.Zero;
                //throw new ArithmeticException("Matrix is not invertible!");
            
            c = 1f / c;
            return Translate(-_m[6], -_m[7]) * new VGMatrix(c * _m[4], -c * _m[1], -c * _m[3], c * _m[0], 0f, 0f);
        }

        public static VGMatrix operator ~(VGMatrix matrix1)
        {
            return matrix1.MakeInverse();
        }

        public static VGMatrix operator -(VGMatrix matrix1)
        {
            VGMatrix m = new VGMatrix();
            m._m[0] = -matrix1._m[0];
            m._m[1] = -matrix1._m[1];
            m._m[2] = -matrix1._m[2];
            m._m[3] = -matrix1._m[3];
            m._m[4] = -matrix1._m[4];
            m._m[5] = -matrix1._m[5];
            m._m[6] = -matrix1._m[6];
            m._m[7] = -matrix1._m[7];
            m._m[8] = -matrix1._m[8];
            return m;
        }

        public static VGMatrix operator -(VGMatrix matrix1, VGMatrix matrix2)
        {
            VGMatrix m = new VGMatrix();

            m._m[0] = matrix1._m[0] - matrix2._m[0];
            m._m[1] = matrix1._m[1] - matrix2._m[1];
            m._m[2] = matrix1._m[2] - matrix2._m[2];
            m._m[3] = matrix1._m[3] - matrix2._m[3];
            m._m[4] = matrix1._m[4] - matrix2._m[4];
            m._m[5] = matrix1._m[5] - matrix2._m[5];
            m._m[6] = matrix1._m[6] - matrix2._m[6];
            m._m[7] = matrix1._m[7] - matrix2._m[7];
            m._m[8] = matrix1._m[8] - matrix2._m[8];
            return m;
        }

        public static bool operator !=(VGMatrix matrix1, VGMatrix matrix2)
        {
            if (matrix1 is VGMatrix && matrix2 is VGMatrix)
                return
                    matrix1._m[0] != matrix2._m[0] ||
                    matrix1._m[1] != matrix2._m[1] ||
                    matrix1._m[2] != matrix2._m[2] ||
                    matrix1._m[3] != matrix2._m[3] ||
                    matrix1._m[4] != matrix2._m[4] ||
                    matrix1._m[5] != matrix2._m[5] ||
                    matrix1._m[6] != matrix2._m[6] ||
                    matrix1._m[7] != matrix2._m[7] ||
                    matrix1._m[8] != matrix2._m[8];
            
            return object.ReferenceEquals(matrix1, matrix2);
        }

        public static bool operator ==(VGMatrix matrix1, VGMatrix matrix2)
        {
            return !(matrix1 != matrix2);
        }

        public static VGMatrix operator *(float scaleFactor, VGMatrix matrix)
        {
            VGMatrix m = new VGMatrix();

            m._m[0] = matrix._m[0] * scaleFactor;
            m._m[1] = matrix._m[1] * scaleFactor;
            m._m[2] = matrix._m[2] * scaleFactor;
            m._m[3] = matrix._m[3] * scaleFactor;
            m._m[4] = matrix._m[4] * scaleFactor;
            m._m[5] = matrix._m[5] * scaleFactor;
            m._m[6] = matrix._m[6] * scaleFactor;
            m._m[7] = matrix._m[7] * scaleFactor;
            m._m[8] = matrix._m[8] * scaleFactor;
            return m;
        }

        public static VGMatrix operator *(VGMatrix matrix, float scaleFactor)
        {
            return scaleFactor * matrix;
        }

        public static VGMatrix operator *(VGMatrix matrix1, VGMatrix matrix2)
        {
            VGMatrix m = new VGMatrix();

            m._m[0] = matrix1._m[0] * matrix2._m[0] + matrix1._m[1] * matrix2._m[3] + matrix1._m[2] * matrix2._m[6];
            m._m[1] = matrix1._m[0] * matrix2._m[1] + matrix1._m[1] * matrix2._m[4] + matrix1._m[2] * matrix2._m[7];
            m._m[2] = matrix1._m[0] * matrix2._m[2] + matrix1._m[1] * matrix2._m[5] + matrix1._m[2] * matrix2._m[8];
            
            m._m[3] = matrix1._m[3] * matrix2._m[0] + matrix1._m[4] * matrix2._m[3] + matrix1._m[5] * matrix2._m[6];
            m._m[4] = matrix1._m[3] * matrix2._m[1] + matrix1._m[4] * matrix2._m[4] + matrix1._m[5] * matrix2._m[7];
            m._m[5] = matrix1._m[3] * matrix2._m[2] + matrix1._m[4] * matrix2._m[5] + matrix1._m[5] * matrix2._m[8];

            m._m[6] = matrix1._m[6] * matrix2._m[0] + matrix1._m[7] * matrix2._m[3] + matrix1._m[8] * matrix2._m[6];
            m._m[7] = matrix1._m[6] * matrix2._m[1] + matrix1._m[7] * matrix2._m[4] + matrix1._m[8] * matrix2._m[7];
            m._m[8] = matrix1._m[6] * matrix2._m[2] + matrix1._m[7] * matrix2._m[5] + matrix1._m[8] * matrix2._m[8];

            return m;
        }

        public static Vector2 operator *(VGMatrix matrix, Vector2 vector)
        {
            return new Vector2(
                vector.X * matrix._m[0] + vector.Y * matrix._m[3] + matrix._m[6],
                vector.X * matrix._m[1] + vector.Y * matrix._m[4] + matrix._m[7]);
        }

        public static VGMatrix operator /(VGMatrix matrix, float divider)
        {
            VGMatrix m = new VGMatrix();

            divider = 1f / divider;
            m._m[0] = matrix._m[0] * divider;
            m._m[1] = matrix._m[1] * divider;
            m._m[2] = matrix._m[2] * divider;
            m._m[3] = matrix._m[3] * divider;
            m._m[4] = matrix._m[4] * divider;
            m._m[5] = matrix._m[5] * divider;
            m._m[6] = matrix._m[6] * divider;
            m._m[7] = matrix._m[7] * divider;
            m._m[8] = matrix._m[8] * divider;

            return m;
        }

        public static VGMatrix operator /(VGMatrix matrix1, VGMatrix matrix2)
        {
            VGMatrix m = new VGMatrix();

            m._m[0] = matrix1._m[0] / matrix2._m[0];
            m._m[1] = matrix1._m[1] / matrix2._m[1];
            m._m[2] = matrix1._m[2] / matrix2._m[2];
            m._m[3] = matrix1._m[3] / matrix2._m[3];
            m._m[4] = matrix1._m[4] / matrix2._m[4];
            m._m[5] = matrix1._m[5] / matrix2._m[5];
            m._m[6] = matrix1._m[6] / matrix2._m[6];
            m._m[7] = matrix1._m[7] / matrix2._m[7];
            m._m[8] = matrix1._m[8] / matrix2._m[8];
            return m;
        }

        public static VGMatrix operator +(VGMatrix matrix1, VGMatrix matrix2)
        {
            VGMatrix m = new VGMatrix();

            m._m[0] = matrix1._m[0] + matrix2._m[0];
            m._m[1] = matrix1._m[1] + matrix2._m[1];
            m._m[2] = matrix1._m[2] + matrix2._m[2];
            m._m[3] = matrix1._m[3] + matrix2._m[3];
            m._m[4] = matrix1._m[4] + matrix2._m[4];
            m._m[5] = matrix1._m[5] + matrix2._m[5];
            m._m[6] = matrix1._m[6] + matrix2._m[6];
            m._m[7] = matrix1._m[7] + matrix2._m[7];
            m._m[8] = matrix1._m[8] + matrix2._m[8];
            return m;
        }

        internal Vector4 GetTransformation()
        {
            return new Vector4(_m[0], _m[1], _m[3], _m[4]);
        }

        internal Vector2 GetTranslation()
        {
            return new Vector2(_m[6], _m[7]);
        }

        public static implicit operator float[](VGMatrix matrix)
        {
            return matrix._m;
        }

        public bool Equals(VGMatrix other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return (obj is VGMatrix) && ((VGMatrix)obj) == this;
        }

        public override int GetHashCode()
        {
            return 
                _m[0].GetHashCode() ^ _m[1].GetHashCode() ^ _m[2].GetHashCode() ^
                _m[3].GetHashCode() ^ _m[4].GetHashCode() ^ _m[5].GetHashCode() ^
                _m[6].GetHashCode() ^ _m[7].GetHashCode() ^ _m[8].GetHashCode();
        }

        public static VGMatrix Rotate(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new VGMatrix(cos, -sin, sin, cos, 0f, 0f);
        }
        public static VGMatrix Rotate(float angle, float cx, float cy)
        {
            return Translate(-cx, -cy) * Rotate(angle) * Translate(cx, cy);
        }

        public static VGMatrix Scale(float sx, float sy)
        {
            return new VGMatrix(sx, 0f, 0f, sy, 0f, 0f);
        }

        public static VGMatrix Translate(float tx, float ty)
        {
            return new VGMatrix(1f, 0f, 0f, 1f, tx, ty);
        }

        public static VGMatrix Shear(float shx, float shy)
        {
            return new VGMatrix(1f, shx, shy, 1f, 0f, 0f);
        }

        public static VGMatrix PaintToRectangle(Rectangle rect)
        {
            return PaintToRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static VGMatrix PaintToRectangle(float left, float top, float width, float height)
        {
            return new VGMatrix(2f / width, 0f, 0f, 2f / height, -1f - 2f * left / width, 1f - 2f * (top + height) / height);
        }
    }
}
