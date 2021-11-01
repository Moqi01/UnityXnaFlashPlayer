using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class DrawSpriteCall
{
    private Texture2D texture2D;
    private Vector2 position;
	private Nullable<Rectangle> source;
    private Vector4 color;
	private Vector2 origin;
	private SpriteEffects spriteEffects;
    private Vector2 Scale;
    public Texture2D Texture2D {
		get {
			return this.texture2D;
		}
	}

	public Vector2 Position {
		get {
			return this.position;
		}
	}

	public Nullable<Rectangle> Source {
		get {
			return this.source;
		}
	}

	public Vector4 Color {
		get {
			return this.color;
		}
	}
	
	public Vector2 Origin {
		get {
			return this.origin;
		}
	}
	
	public SpriteEffects SpriteEffects {
		get {
			return this.spriteEffects;
		}
	}

    public const int x = 1, y = 1;

    public DrawSpriteCall(Texture2D texture2D, Vector2 position, Nullable<Rectangle> source, Vector4 color, Vector2 origin, SpriteEffects spriteEffects,Vector2 scale=new  Vector2())
    {
        // TODO: Complete member initialization
        this.texture2D = texture2D;
        this.position = position;
		this.source = source;
        this.color = color;
		this.origin = origin;
		this.spriteEffects = spriteEffects;
        Scale = scale;
       // texture2D.Width = (int)(scale.X*texture2D .Width);
    }

}
