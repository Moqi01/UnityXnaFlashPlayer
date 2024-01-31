using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using XnaFlash.Swf.Structures;
using static XnaFlash.Swf.Structures.FillStyle;
using XnaVG;
//using Microsoft.Xna.Framework;
//using SwfLib;
//using SwfLib.Tags.ShapeTags;
//using SwfLib.Shapes.Records;
//using SwfLib.Shapes.FillStyles;

namespace Unity.Flash
{
	public class ShapeParser
	{
		int m_X;
		int m_Y;
		int m_ID;
		int m_Style0;
		int m_Style1;
		Rect m_Bounds;
		List<Style> m_AllStyles;
		Dictionary<int, Style> m_Styles;
		public FillStyle FillStyle;
		public VGPaint Paint;

		public ShapeParser()
        {
			Init();

		}

		//Style fillStyle0 => m_Styles[m_Style0];
		Style fillStyle0 { get {
				if(!m_Styles.ContainsKey(m_Style0))
                {
					return null;
                }
				return m_Styles[m_Style0];
			} }
		Style fillStyle1
		{
			get
			{
				if (!m_Styles.ContainsKey(m_Style1))
				{
					return null;
				}
				return m_Styles[m_Style1];
			}
		}

		public void Init()
		{
			//m_ID = tag.ShapeID;
			//m_Bounds = new Rect(X,Y,X ,Y);
			m_X = 0;
			m_Y = 0;
			m_Style0 = 0;
			m_Style1 = 0;
            m_Styles = new Dictionary<int, Style>();
            m_Styles.Add(0, null);
            m_AllStyles = new List<Style>();

            ParseFillStyles(ShapeInfo.fillStyleArrays.Styles);
            //ParseShapeRecords(ShapeRecords);
            //Tessellate();
        }



		IFill DefaultFill(FillStyleType fillStyle)
		{
            switch (fillStyle)
            {
                case FillStyleType.Solid:
					return new SolidFill()
					{
						Color = Color.white,
						Mode = FillMode.NonZero,
						Opacity = 1
					};
                case FillStyleType.Linear:
     
                case FillStyleType.Radial:
   
                case FillStyleType.Focal:
					return new GradientFill()
					{

						Mode = FillMode.NonZero,
						Opacity = 1
					};
				case FillStyleType.RepeatingBitmap:
        
                case FillStyleType.ClippedBitmap:
        
                case FillStyleType.RepeatingNonsmoothedBitmap:
       
                case FillStyleType.ClippedNonsmoothedBitmap:

				default:
					return new SolidFill()
					{
						Color = Color.white,
						Mode = FillMode.NonZero,
						Opacity = 1
					};
            }
           
		}

		// Parse Styles RGBA

		public void ParseFillStyles(IList<FillStyle> fillStyles)
		{
			if(FillStyle==null&& fillStyles.Count >0)
			FillStyle = fillStyles[0];
			foreach (var fillStyle in fillStyles)
			{

				if (fillStyle.FillType == FillStyleType.Solid)
					m_Styles.Add(m_Styles.Count, Parse(fillStyle));
				else
				{
					if (fillStyle.HasAlpha)
					{
						m_Styles.Add(m_Styles.Count, new Style(DefaultFill(fillStyle.FillType)));

					}
					else
					{
						m_Styles.Add(m_Styles.Count, new Style(DefaultFill(fillStyle.FillType)));
					}
				}

			}
			
		}

		public void AddParseFillStyles(FillStyle fillStyle)
		{
		   m_Styles.Add(m_Styles.Count, Parse(fillStyle));
		}

		Style Parse(FillStyle fillStyle)
		{
			var fill = new SolidFill();
			fill.Color =new Color( fillStyle.Color.R/255f, fillStyle.Color.G / 255f, fillStyle.Color.B / 255f, fillStyle.Color.A / 255f);
			fill.Mode = FillMode.NonZero;
			fill.Opacity = fill.Color.a;
			return new Style(fill);
		}

		Style Parse()
		{
			var fill = new SolidFill();
			fill.Color = Color.white;
			fill.Mode = FillMode.NonZero;
			fill.Opacity = 1;
			return new Style(fill);
		}

		//void Parse(StraightEdgeShapeRecord tag)
		public void Parse(int DeltaX,int DeltaY)
		{
			var begin = new Vector2(m_X, m_Y);
			var end = begin + new Vector2(DeltaX, DeltaY);

			fillStyle0?.Add0(begin, begin, end, end);
			fillStyle1?.Add1(begin, begin, end, end);

			m_X += DeltaX;
			m_Y += DeltaY;
		}

		//void Parse(CurvedEdgeShapeRecord tag)
		public void Parse(int ControlDeltaX,int ControlDeltaY,int AnchorDeltaX,int AnchorDeltaY)
		{
			var begin = new Vector2(m_X, m_Y);
			var control = begin + new Vector2(ControlDeltaX, ControlDeltaY);
			var end = control + new Vector2(AnchorDeltaX, AnchorDeltaY);

			fillStyle0?.Add0(begin, begin, end, end);
			fillStyle1?.Add1(begin, begin, end, end);

			m_X += ControlDeltaX + AnchorDeltaX;
			m_Y += ControlDeltaY + AnchorDeltaY;
		}

		public void ParseClose()
		{
			fillStyle0?.Close();
			fillStyle1?.Close();
			m_AllStyles.AddRange(m_Styles.Values);
			m_Styles.Clear();
		}

		// Parse Shape Records RGBA

		//void ParseShapeRecords(IList<IShapeRecordRGBA> records)
		//{
		//	foreach (var record in records)
		//		if (record is StyleChangeShapeRecordRGBA)
		//			Parse(record as StyleChangeShapeRecordRGBA);
		//		else if (record is StraightEdgeShapeRecord)
		//			Parse(record as StraightEdgeShapeRecord);
		//		else if (record is CurvedEdgeShapeRecord)
		//			Parse(record as CurvedEdgeShapeRecord);
		//		else
		//			Parse(record as EndShapeRecord);
		//}
		
		//void Parse(StyleChangeShapeRecordRGBA tag)
		public void Parse(bool FillStyle0HasValue, bool FillStyle1HasValue ,int MoveDeltaX,int MoveDeltaY,bool StateMoveTo,bool StateNewStyles, FillStyle FillStyle0Value,FillStyle FillStyle1Value, ShapeState State)
		{
			if (StateMoveTo || FillStyle0HasValue)
				fillStyle0?.Close();

			if (StateMoveTo || FillStyle1HasValue)
				fillStyle1?.Close();

			if (StateMoveTo)
            {
				m_X = MoveDeltaX;
				m_Y = MoveDeltaY;

			}
			

			if(StateNewStyles)
				UpdateFillStyles(State.GetFillStyles());

            if (FillStyle0HasValue)
            {
				if (FillStyle0Value == null)
					m_Style0 = 0;
				else
                {
				    m_Style0 = (int)FillStyle0Value.Index;
					if (!m_Styles.ContainsKey(m_Style0))
					{
						AddParseFillStyles(FillStyle0Value);
					}
				}

                //m_Style0 = m_Styles.Count -1;
            }

            if (StateMoveTo || FillStyle0HasValue)
				fillStyle0?.New();

            if (FillStyle1HasValue)
            {
				if(FillStyle1Value==null)
                {
					m_Style1 = 0;

				}
				else
                {
			    	m_Style1 = (int)FillStyle1Value.Index;
					if (!m_Styles.ContainsKey(m_Style1))
					{
						AddParseFillStyles(FillStyle1Value);
					}
				}

				//m_Style1 = m_Styles.Count - 1;
			}
               

            if (StateMoveTo || FillStyle1HasValue)
				fillStyle1?.New();
		}

		
		// Style Override


		void UpdateFillStyles(FillStyle[] Styles=null)
		{
			m_AllStyles.AddRange(m_Styles.Values);
			m_Styles.Clear();
			m_Styles.Add(0, null);
			ParseFillStyles(Styles);
		}

		// Tesselation
		static int num;
		public void Tessellate()
		{
			num++;
			if(num==17)
            {

            }
			// Create Shape List
			shapes = new List<Shape>(m_AllStyles.Count);
			for (int c = 0; c <  m_AllStyles.Count; c++)
				if (m_AllStyles[c] != null)
                {
					if(m_AllStyles[c].GetCan())
					shapes.Add(m_AllStyles[c].ToShape());
                }
			
			// Create Scene Node
			
		}
		public List<Shape> shapes;
	}
}