using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Transform;
namespace Comora
{
    public enum AspectMode
    {
        // Token: 0x0400003B RID: 59
        Expand,
        // Token: 0x0400003C RID: 60
        FillStretch,
        // Token: 0x0400003D RID: 61
        FillUniform
    }

    public class Camera 
    {
        private float? width;

        private float? height;

        private AspectMode aspectMode;

        private Transform2D viewportOffset;

        private Transform2D Transform;

        private GraphicsDevice device;

        private Vector2 viewport;

        public AspectMode ResizeMode
        {
            get
            {
                return this.aspectMode;
            }
            set
            {
                if (this.aspectMode != value)
                {
                    this.aspectMode = value;
                    if (value == AspectMode.Expand)
                    {
                        this.viewportOffset.Scale = Vector2.One;
                    }
                    this.UpdateOffset();
                }
            }
        }

        public float Width
        {
            get
            {
                if (this.ResizeMode == AspectMode.Expand)
                {
                    return this.viewport.X;
                }
                float? num = this.width;
                if (num == null)
                {
                    return this.viewport.X;
                }
                return num.GetValueOrDefault();
            }
            set
            {
                this.width = new float?(value);
            }
        }

        public float Height
        {
            get
            {
                if (this.ResizeMode == AspectMode.Expand)
                {
                    return this.viewport.Y;
                }
                float? num = this.height;
                if (num == null)
                {
                    return this.viewport.Y;
                }
                return num.GetValueOrDefault();
            }
            set
            {
                this.height = new float?(value);
            }
        }

        public float Zoom
        {
            get
            {
                return 1f / this.Transform.Scale.X;
            }
            set
            {
                this.Transform.Scale = new Vector2(1f / value, 1f / value);
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.Transform.AbsolutePosition;
            }
            set
            {
                this.Transform.Position = value;
            }
        }

        public Camera(GraphicsDevice device)
        {
            this.device = device;
            this.Transform = new Transform2D();
            this.viewport = new Vector2((float)this.device.Viewport.Width, (float)this.device.Viewport.Height);
            this.viewportOffset = new Transform2D
            {
                Parent = this.Transform
            };
            this.UpdateOffset();
        }

        private void UpdateOffset()
        {
            float num = this.Width;
            float num2 = this.Height;
            AspectMode resizeMode = this.ResizeMode;
            if (resizeMode != AspectMode.FillStretch)
            {
                if (resizeMode == AspectMode.FillUniform)
                {
                    float num3 = Mathf.Min(num / this.viewport.X, num2 / this.viewport.Y);
                    this.viewportOffset.Scale = new Vector2(num3, num3);
                    num = this.viewport.X * this.viewportOffset.Scale.X;
                    num2 = this.viewport.Y * this.viewportOffset.Scale.Y;
                }
            }
            else
            {
                this.viewportOffset.Scale = new Vector2(num / this.viewport.X, num2 / this.viewport.Y);
                num = this.viewport.X * this.viewportOffset.Scale.X;
                num2 = this.viewport.Y * this.viewportOffset.Scale.Y;
            }
            this.viewportOffset.Position = new Vector2(-num * 0.5f, -num2 * 0.5f);
        }

        public void Update(GameTime time)
        {
            if (this.viewport.X != (float)this.device.Viewport.Width || this.viewport.X != (float)this.device.Viewport.Height)
            {
                this.viewport = new Vector2((float)this.device.Viewport.Width, (float)this.device.Viewport.Height);
                this.UpdateOffset();
            }
            
        }
        public Rectangle GetBounds()
        {
            Vector2 absolutePosition = this.Transform.AbsolutePosition;
            float num = this.Width * this.Transform.AbsoluteScale.X;
            float num2 = this.Height * this.Transform.AbsoluteScale.Y;
            float num3 = Mathf.Max(num, num2);
            float num4 = absolutePosition.X - num3 / 2f;
            float num5 = absolutePosition.Y - num3 / 2f;
            return new Rectangle((int)num4, (int)num5, (int)num, (int)num2);
        }
    }
  
}
