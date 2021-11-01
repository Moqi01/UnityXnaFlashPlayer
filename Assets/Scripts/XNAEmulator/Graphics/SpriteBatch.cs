using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comora;
using UnityEngine;

namespace Microsoft.Xna.Framework.Graphics
{
    public class SpriteBatch : GraphicsResource
    {
        //private new GraphicsDevice graphicsDevice;
        private bool _beginCalled;
        public static bool NeedsHalfPixelOffset;
        Vector2 texCoordTL = new Vector2 (0,0);
		Vector2 texCoordBR = new Vector2 (0,0);
		Rectangle tempRect = new Rectangle (0,0,0,0);

        public SpriteBatch(GraphicsDevice graphicsDevice)
        {
			if (graphicsDevice == null) {
				throw new ArgumentException ("graphicsDevice");
			}
            UnityEngine.Debug.Log("new SpriteBatch");
            // TODO: Complete member initialization
            this.graphicsDevice = graphicsDevice;
        }

        public void Begin(Comora.Camera camera)
        {

        }

        public void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
        {
            if (this._beginCalled)
            {
                //throw new InvalidOperationException("Begin cannot be called again until End has been successfully called.");
                return;
            }
            this._sortMode = sortMode;
            //this._blendState = (blendState ?? BlendState.AlphaBlend);
            //this._samplerState = (samplerState ?? SamplerState.LinearClamp);
            //this._depthStencilState = (depthStencilState ?? DepthStencilState.None);
            //this._rasterizerState = (rasterizerState ?? RasterizerState.CullCounterClockwise);
            //this._effect = effect;
            this._matrix = transformMatrix;
            if (sortMode == SpriteSortMode.Immediate)
            {
                this.Setup();
            }
            this._beginCalled = true;
            UnityEngine.Debug.Log("Begin");
        }
        // private readonly EffectParameter _matrixTransform;
        //private readonly EffectPass _spritePass;
        private SpriteSortMode _sortMode;
        private Matrix? _matrix;

        // Token: 0x04000543 RID: 1347
        private Viewport _lastViewport;

        // Token: 0x04000544 RID: 1348
        private Matrix _projection;
        private void Setup()
        {
            GraphicsDevice graphicsDevice = base.GraphicsDevice;
            //graphicsDevice.BlendState = this._blendState;
            //graphicsDevice.DepthStencilState = this._depthStencilState;
            //graphicsDevice.RasterizerState = this._rasterizerState;
            //graphicsDevice.SamplerStates[0] = this._samplerState;
            Viewport viewport = graphicsDevice.Viewport;
            if (viewport.Width != this._lastViewport.Width || viewport.Height != this._lastViewport.Height)
            {
                Matrix.CreateOrthographicOffCenter(0f, (float)viewport.Width, (float)viewport.Height, 0f, 0f, -1f, out this._projection);
                if (SpriteBatch.NeedsHalfPixelOffset)
                {
                    this._projection.M41 = this._projection.M41 + -0.5f * this._projection.M11;
                    this._projection.M42 = this._projection.M42 + -0.5f * this._projection.M22;
                }
                this._lastViewport = viewport;
            }
            //if (this._matrix != null)
            //{
            //    this._matrixTransform.SetValue(this._matrix.GetValueOrDefault() * this._projection);
            //}
            //else
            //{
            //    this._matrixTransform.SetValue(this._projection);
            //}
            //this._spritePass.Apply();
        }

        public void Begin ()
		{
            Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);	
		}

		public void Begin (SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
		{
            /*
                        // defaults
                        _sortMode = sortMode;
                        _blendState = blendState ?? BlendState.AlphaBlend;
                        _samplerState = samplerState ?? SamplerState.LinearClamp;
                        _depthStencilState = depthStencilState ?? DepthStencilState.None;
                        _rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

                        _effect = effect;

                        _matrix = transformMatrix;


                        if (sortMode == SpriteSortMode.Immediate) {
                            //setup things now so a user can chage them
                            Setup();
                        }
            */
            //Debug.Log("SpriteBatch");
        }

		public void Begin (SpriteSortMode sortMode, BlendState blendState)
		{
			Begin (sortMode, blendState, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);			
		}

		public void Begin (SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
		{
			Begin (sortMode, blendState, samplerState, depthStencilState, rasterizerState, null, Matrix.Identity);	
		}

		public void Begin (SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
		{
			Begin (sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);			
		}

        public void End()
        {
        }

        public void Draw(Texture2D texture2D, Vector2 position, Nullable<Rectangle> source, Color color, float p, Vector2 Origin, float p_2, SpriteEffects spriteEffects, float p_3)
        {
			graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture2D, position, source, color.ToVector4(), Origin, spriteEffects));
        }

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 Origin, Vector2 scale, SpriteEffects spriteEffects, float layerDepth)
        {
            UnityEngine.Debug.Log("scale");
            graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture, position, sourceRectangle, color.ToVector4(), Origin, spriteEffects));
        }

        //TODO: Draw stretching
        public void Draw(Texture2D texture2D, Nullable<Rectangle> source, Color color)
        {
			graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture2D, Vector2.Zero, source, color.ToVector4(), Vector2.Zero, SpriteEffects.None));
        }
		
		//Draw texture section
		public void Draw(Texture2D texture2D, Vector2 position, Nullable<Rectangle> source, Color color)
        {
			graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture2D, position, source, color.ToVector4(), Vector2.Zero, SpriteEffects.None));
        }
		
		//TODO:
		public void Draw (Texture2D texture, Nullable<Rectangle> source, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
		{
            Debug.Log("Draw");
			if (sourceRectangle.HasValue) {
				tempRect = sourceRectangle.Value;
			} else {
				tempRect.X = 0;
				tempRect.Y = 0;
				tempRect.Width = texture.Width;
				tempRect.Height = texture.Height;				
			}
			
			texCoordTL.X = tempRect.X / (float)texture.Width;
			texCoordTL.Y = tempRect.Y / (float)texture.Height;
			texCoordBR.X = (tempRect.X + tempRect.Width) / (float)texture.Width;
			texCoordBR.Y = (tempRect.Y + tempRect.Height) / (float)texture.Height;

			if ((effect & SpriteEffects.FlipVertically) != 0) {
				float temp = texCoordBR.Y;
				texCoordBR.Y = texCoordTL.Y;
				texCoordTL.Y = temp;
			}
			if ((effect & SpriteEffects.FlipHorizontally) != 0) {
				float temp = texCoordBR.X;
				texCoordBR.X = texCoordTL.X;
				texCoordTL.X = temp;
			}

			graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture, Vector2.Zero, source, color.ToVector4(), Vector2.Zero, SpriteEffects.None));
		}

		//TODO:
		public void Draw (Texture2D texture, Nullable<Rectangle> source, Rectangle? sourceRectangle, Color color)
		{
			graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture, Vector2.Zero, source, color.ToVector4(), Vector2.Zero, SpriteEffects.None));
		}

        public void Draw(Texture2D texture2D, Vector2 position, Color color)
        {
            graphicsDevice.DrawQueue.EnqueueSprite(new DrawSpriteCall(texture2D, position, null, color.ToVector4(), Vector2.Zero, SpriteEffects.None));
        }

        public void DrawString(SpriteFont font, string value, Vector2 position, Color color)
        {
            graphicsDevice.DrawQueue.EnqueueString(new DrawStringCall(font, value, position, color.ToVector4()));
        }

        public void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            Vector2 scale2 = new Vector2(scale, scale);
            UnityEngine.Debug.Log("DrawStringBuilder");
            graphicsDevice.DrawQueue.EnqueueString(new DrawStringCall(spriteFont, text.ToString(), position, color.ToVector4()));
        }

        public void DrawString(SpriteFont spriteFont, String text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            Vector2 scale2 = new Vector2(scale, scale);
            UnityEngine.Debug.Log("DrawString");
            graphicsDevice.DrawQueue.EnqueueString(new DrawStringCall(spriteFont, text, position, color.ToVector4()));
        }
    }
}
