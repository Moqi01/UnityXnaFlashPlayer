using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using Microsoft.Xna.Framework;
namespace Transform
{
    public class Transform2D
    {
        // Token: 0x06000016 RID: 22 RVA: 0x00002350 File Offset: 0x00000550
        public Transform2D()
        {
            this.Position = Vector2.Zero;
            this.Rotation = 0f;
            this.Scale = Vector2.One;
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000017 RID: 23 RVA: 0x0000239D File Offset: 0x0000059D
        // (set) Token: 0x06000018 RID: 24 RVA: 0x000023A8 File Offset: 0x000005A8
        public Transform2D Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (this.parent != value)
                {
                    if (this.parent != null)
                    {
                        this.parent.children.Remove(this);
                    }
                    this.parent = value;
                    if (this.parent != null)
                    {
                        this.parent.children.Add(this);
                    }
                    this.SetNeedsAbsoluteUpdate();
                }
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000019 RID: 25 RVA: 0x000023FE File Offset: 0x000005FE
        public IEnumerable<Transform2D> Children
        {
            get
            {
                return this.children;
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x0600001A RID: 26 RVA: 0x00002406 File Offset: 0x00000606
        public float AbsoluteRotation
        {
            get
            {
                return this.UpdateAbsoluteAndGet<float>(ref this.absoluteRotation);
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600001B RID: 27 RVA: 0x00002414 File Offset: 0x00000614
        public Vector2 AbsoluteScale
        {
            get
            {
                return this.UpdateAbsoluteAndGet<Vector2>(ref this.absoluteScale);
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600001C RID: 28 RVA: 0x00002422 File Offset: 0x00000622
        public Vector2 AbsolutePosition
        {
            get
            {
                return this.UpdateAbsoluteAndGet<Vector2>(ref this.absolutePosition);
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600001D RID: 29 RVA: 0x00002430 File Offset: 0x00000630
        // (set) Token: 0x0600001E RID: 30 RVA: 0x00002438 File Offset: 0x00000638
        public float Rotation
        {
            get
            {
                return this.localRotation;
            }
            set
            {
                if (this.localRotation != value)
                {
                    this.localRotation = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x0600001F RID: 31 RVA: 0x00002450 File Offset: 0x00000650
        // (set) Token: 0x06000020 RID: 32 RVA: 0x00002458 File Offset: 0x00000658
        public Vector2 Position
        {
            get
            {
                return this.localPosition;
            }
            set
            {
                if (this.localPosition != value)
                {
                    this.localPosition = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000021 RID: 33 RVA: 0x00002475 File Offset: 0x00000675
        // (set) Token: 0x06000022 RID: 34 RVA: 0x0000247D File Offset: 0x0000067D
        public Vector2 Scale
        {
            get
            {
                return this.localScale;
            }
            set
            {
                if (this.localScale != value)
                {
                    this.localScale = value;
                    this.SetNeedsLocalUpdate();
                }
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000023 RID: 35 RVA: 0x0000249A File Offset: 0x0000069A
        public Matrix Local
        {
            get
            {
                return this.UpdateLocalAndGet<Matrix>(ref this.absolute);
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000024 RID: 36 RVA: 0x000024A8 File Offset: 0x000006A8
        public Matrix Absolute
        {
            get
            {
                return this.UpdateAbsoluteAndGet<Matrix>(ref this.absolute);
            }
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000025 RID: 37 RVA: 0x000024B6 File Offset: 0x000006B6
        public Matrix InvertAbsolute
        {
            get
            {
                return this.UpdateAbsoluteAndGet<Matrix>(ref this.invertAbsolute);
            }
        }

        // Token: 0x06000026 RID: 38 RVA: 0x000024C4 File Offset: 0x000006C4
        public void ToLocalPosition(ref Vector2 absolute, out Vector2 local)
        {
            Vector2.Transform(ref absolute, ref this.invertAbsolute, out local);
        }

        // Token: 0x06000027 RID: 39 RVA: 0x000024D3 File Offset: 0x000006D3
        public void ToAbsolutePosition(ref Vector2 local, out Vector2 absolute)
        {
            Vector2.Transform(ref local, ref this.absolute, out absolute);
        }

        // Token: 0x06000028 RID: 40 RVA: 0x000024E4 File Offset: 0x000006E4
        public Vector2 ToLocalPosition(Vector2 absolute)
        {
            Vector2 result;
            this.ToLocalPosition(ref absolute, out result);
            return result;
        }

        // Token: 0x06000029 RID: 41 RVA: 0x000024FC File Offset: 0x000006FC
        public Vector2 ToAbsolutePosition(Vector2 local)
        {
            Vector2 result;
            this.ToAbsolutePosition(ref local, out result);
            return result;
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00002514 File Offset: 0x00000714
        private void SetNeedsLocalUpdate()
        {
            this.needsLocalUpdate = true;
            this.SetNeedsAbsoluteUpdate();
        }

        // Token: 0x0600002B RID: 43 RVA: 0x00002524 File Offset: 0x00000724
        private void SetNeedsAbsoluteUpdate()
        {
            this.needsAbsoluteUpdate = true;
            foreach (Transform2D transform2D in this.children)
            {
                transform2D.SetNeedsAbsoluteUpdate();
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x0000257C File Offset: 0x0000077C
        private void UpdateLocal()
        {
            Matrix matrix = Matrix.CreateScale(this.Scale.X, this.Scale.Y, 1f);
            matrix *= Matrix.CreateRotationZ(this.Rotation);
            matrix *= Matrix.CreateTranslation(this.Position.X, this.Position.Y, 0f);
            this.local = matrix;
            this.needsLocalUpdate = false;
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000025F4 File Offset: 0x000007F4
        private void UpdateAbsolute()
        {
            if (this.Parent == null)
            {
                this.absolute = this.local;
                this.absoluteScale = this.localScale;
                this.absoluteRotation = this.localRotation;
                this.absolutePosition = this.localPosition;
            }
            else
            {
                Matrix matrix = this.Parent.Absolute;
                Matrix.Multiply(ref this.local, ref matrix, out this.absolute);
                this.absoluteScale = this.Parent.AbsoluteScale * this.Scale;
                this.absoluteRotation = this.Parent.AbsoluteRotation + this.Rotation;
                this.absolutePosition = Vector2.Zero;
                this.ToAbsolutePosition(ref this.absolutePosition, out this.absolutePosition);
            }
            Matrix.Invert(ref this.absolute, out this.invertAbsolute);
            this.needsAbsoluteUpdate = false;
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000026C3 File Offset: 0x000008C3
        private T UpdateLocalAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }
            return field;
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000026D9 File Offset: 0x000008D9
        private T UpdateAbsoluteAndGet<T>(ref T field)
        {
            if (this.needsLocalUpdate)
            {
                this.UpdateLocal();
            }
            if (this.needsAbsoluteUpdate)
            {
                this.UpdateAbsolute();
            }
            return field;
        }

        // Token: 0x0400000D RID: 13
        private Transform2D parent;

        // Token: 0x0400000E RID: 14
        private List<Transform2D> children = new List<Transform2D>();

        // Token: 0x0400000F RID: 15
        private Matrix absolute;

        // Token: 0x04000010 RID: 16
        private Matrix invertAbsolute;

        // Token: 0x04000011 RID: 17
        private Matrix local;

        // Token: 0x04000012 RID: 18
        private float localRotation;

        // Token: 0x04000013 RID: 19
        private float absoluteRotation;

        // Token: 0x04000014 RID: 20
        private Vector2 localScale;

        // Token: 0x04000015 RID: 21
        private Vector2 absoluteScale;

        // Token: 0x04000016 RID: 22
        private Vector2 localPosition;

        // Token: 0x04000017 RID: 23
        private Vector2 absolutePosition;

        // Token: 0x04000018 RID: 24
        private bool needsAbsoluteUpdate = true;

        // Token: 0x04000019 RID: 25
        private bool needsLocalUpdate = true;
    }
}
