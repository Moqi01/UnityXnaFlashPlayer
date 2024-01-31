using System.Collections.Generic;

namespace XnaFlash.Swf.Structures
{
    public static class ShapeInfo
    {
        public static FillStyleArray fillStyleArrays;

        public static IEnumerable<ShapeRecord> ReadShape(SwfStream swf, bool hasAlpha, bool isExtended, bool hasStyle, bool extendedStyles)
        {
            ShapeState state = new ShapeState();

            swf.Align();
            if (hasStyle)
            {
                fillStyleArrays = new FillStyleArray(swf, hasAlpha);
                state.FillStyles = fillStyleArrays;
                state.LineStyles = new LineStyleArray(swf, hasAlpha, isExtended);
            }
            else
            {
                fillStyleArrays = new FillStyleArray();
                state.FillStyles = fillStyleArrays;
                state.LineStyles = new LineStyleArray();
            }
            
            state.FillBits = (int)swf.ReadBitUInt(4);
            state.LineBits = (int)swf.ReadBitUInt(4);

            ShapeRecord rec;

            while (true)
            {
                rec = new ShapeRecord(swf, hasAlpha, isExtended, extendedStyles, state);
                if (rec.Type == ShapeRecord.ShapeRecordType.EndOfShape)
                    break;
             
                yield return rec;
            }
        }        
    }
}
