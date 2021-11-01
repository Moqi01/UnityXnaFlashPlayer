using Microsoft.Xna.Framework;

namespace XnaVG
{
    public class VGCxForm
    {
        public static VGCxForm Identity { get { return new VGCxForm(); } }

        public Vector4 AddTerm;
        public Vector4 MulTerm;

        public float AddRed { get { return AddTerm.X; } set { AddTerm.X = value; } }
        public float AddGreen { get { return AddTerm.Y; } set { AddTerm.Y = value; } }
        public float AddBlue { get { return AddTerm.Z; } set { AddTerm.Z = value; } }
        public float AddAlpha { get { return AddTerm.W; } set { AddTerm.W = value; } }

        public float MulRed { get { return MulTerm.X; } set { MulTerm.X = value; } }
        public float MulGreen { get { return MulTerm.Y; } set { MulTerm.Y = value; } }
        public float MulBlue { get { return MulTerm.Z; } set { MulTerm.Z = value; } }
        public float MulAlpha { get { return MulTerm.W; } set { MulTerm.W = value; } }

        public bool IsIdentity { get { return AddTerm == Vector4.Zero && MulTerm == Vector4.One; } }

        public VGCxForm()
            : this(0f, 0f, 0f, 0f, 1f, 1f, 1f, 1f)
        { }

        public VGCxForm(Vector4 add, Vector4 mul)
        {
            AddTerm = add;
            MulTerm = mul;
        }

        public VGCxForm(float radd, float gadd, float badd, float aadd, float rmul, float gmul, float bmul, float amul)
        {
            AddTerm = new Vector4(radd, gadd, badd, aadd);
            MulTerm = new Vector4(rmul, gmul, bmul, amul);
        }

        public VGCxForm(VGCxForm other)
        {
            AddTerm = other.AddTerm;
            MulTerm = other.MulTerm; 
        }

        public VGColor TransformColor(VGColor color)
        {
            return this * color;
        }

        public static Vector4 operator *(VGCxForm cxform, Vector4 v)
        {
            return Vector4.Clamp(v * cxform.MulTerm + cxform.AddTerm, Vector4.Zero, Vector4.One);
        }

        public static VGCxForm operator *(VGCxForm a, VGCxForm b)
        {
            return new VGCxForm(b.AddTerm + (a.AddTerm * b.MulTerm), a.MulTerm * b.MulTerm);
        }

        public static bool operator == (VGCxForm a, VGCxForm b)
        {
            if (a is VGCxForm && b is VGCxForm)
                return a.MulTerm == b.MulTerm && a.AddTerm == b.AddTerm;
            return object.ReferenceEquals(a, b);
        }
        
        public static bool operator !=(VGCxForm a, VGCxForm b)        
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VGCxForm)) return false;
            return (obj as VGCxForm) == this;
        }

        public override int GetHashCode()
        {
            return AddTerm.GetHashCode() ^ MulTerm.GetHashCode();
        }
    }
}
