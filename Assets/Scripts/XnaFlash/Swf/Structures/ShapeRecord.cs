
namespace XnaFlash.Swf.Structures
{
    public class ShapeRecord
    {
        private uint mFlags;

        public ShapeRecordType Type { get; private set; }

        // StyleChange
        public bool NewStyles { get { return (mFlags & 0x10) != 0; } }
        public bool NewLineStyle { get { return (mFlags & 0x08) != 0; } }
        public bool NewFillStyle1 { get { return (mFlags & 0x04) != 0; } }
        public bool NewFillStyle0 { get { return (mFlags & 0x02) != 0; } }
        public bool NewMoveTo { get { return (mFlags & 0x01) != 0; } }
        public int MoveDeltaX { get; private set; }
        public int MoveDeltaY { get; private set; }
        public FillStyle FillStyle0 { get; private set; }
        public FillStyle FillStyle1 { get; private set; }
        public LineStyle LineStyle { get; private set; }

        // StraightEdge        
        public int DrawDeltaX { get; private set; }
        public int DrawDeltaY { get; private set; }

        // CurvedEdge
        public int DrawControlX { get; private set; }
        public int DrawControlY { get; private set; }
        public ShapeState State;

        public ShapeRecord(SwfStream swf, bool hasAlpha, bool isExtended, bool extendedStyles, ShapeState state)
        {
            int f0 = 0, f1 = 0, l = 0;
            State = state;
            mFlags = swf.ReadBitUInt(6);
            
            Type = ConvertType(mFlags);
            switch (Type)
            {
                case ShapeRecordType.StyleChange:
                    {                       
                        if (NewMoveTo)
                        {
                            int bits = (int)swf.ReadBitUInt(5);
                            MoveDeltaX = swf.ReadBitInt(bits);
                            MoveDeltaY = swf.ReadBitInt(bits);
                        }
                        if (NewFillStyle0) f0 = ((int)swf.ReadBitUInt(state.FillBits));
                        if (NewFillStyle1) f1 = ((int)swf.ReadBitUInt(state.FillBits));
                        if (NewLineStyle) l = ((int)swf.ReadBitUInt(state.LineBits));
                        if (NewStyles && extendedStyles)
                        {
                            state.FillStyles = new FillStyleArray(swf, hasAlpha);
                            state.LineStyles = new LineStyleArray(swf, hasAlpha, isExtended);
                            state.FillBits = (int)swf.ReadBitUInt(4);
                            state.LineBits = (int)swf.ReadBitUInt(4);
                        }
                        if (NewFillStyle0) FillStyle0 = state.GetFill(f0);
                        if (NewFillStyle1) FillStyle1 = state.GetFill(f1);
                        if (NewLineStyle) LineStyle = state.GetLine(l);
                    }
                    break;
                case ShapeRecordType.StraightEdge:
                    {
                        int bits = 2 + (int)(mFlags & 0x0F);
                        bool general = swf.ReadBit();
                        bool vert = general || swf.ReadBit();
                        DrawDeltaX = (general || !vert) ? swf.ReadBitInt(bits) : 0;
                        DrawDeltaY = (general ||  vert) ? swf.ReadBitInt(bits) : 0;
                    }
                    break;
                case ShapeRecordType.CurvedEdge:
                    {
                        int bits = 2 + (int)(mFlags & 0x0F);
                        DrawControlX = swf.ReadBitInt(bits);
                        DrawControlY = swf.ReadBitInt(bits);
                        DrawDeltaX = swf.ReadBitInt(bits);
                        DrawDeltaY = swf.ReadBitInt(bits);
                    }
                    break;
            }
        }

        private ShapeRecordType ConvertType(uint flags)
        {
            if (flags == 0) return ShapeRecordType.EndOfShape;
            if ((flags & 0x20) == 0) return ShapeRecordType.StyleChange;
            if ((flags & 0x10) == 0) return ShapeRecordType.CurvedEdge;
            return ShapeRecordType.StraightEdge;
        }

        public enum ShapeRecordType
        {
            EndOfShape,
            StyleChange,
            StraightEdge,
            CurvedEdge
        }
    }
}
