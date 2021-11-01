using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace XnaFlash.Swf.Structures
{
    public struct KeyCode
    {
        public Keys Key;
        public Keys Alternate;
        public bool? Shift;

        public KeyCode(byte swfKey)
        {
            Shift = null;
            Key = Keys.None;
            Alternate = Keys.None;

            swfKey &= 0x7F;
            switch (swfKey)
            {
                case 1: Key = Keys.Left; break;
                case 2: Key = Keys.Right; break;
                case 3: Key = Keys.Home; break;
                case 4: Key = Keys.End; break;
                case 5: Key = Keys.Insert; break;
                case 6: Key = Keys.Delete; break;
                case 8: Key = Keys.Back; break;
                case 13: Key = Keys.Enter; break;
                case 14: Key = Keys.Up; break;
                case 15: Key = Keys.Down; break;
                case 16: Key = Keys.PageUp; break;
                case 17: Key = Keys.PageDown; break;
                case 18: Key = Keys.Tab; break;
                case 19: Key = Keys.Escape; break;
                case 32: Key = Keys.Space; break;
                case 33: Key = Keys.D1; Shift = true; break;
                case 34: Key = Keys.OemQuotes; Shift = true; break;
                case 35: Key = Keys.D3; Shift = true; break;
                case 36: Key = Keys.D4; Shift = true; break;
                case 37: Key = Keys.D5; Shift = true; break;
                case 38: Key = Keys.D7; Shift = true; break;
                case 39: Key = Keys.OemQuotes; Shift = false; break;
                case 40: Key = Keys.D9; Shift = true; break;
                case 41: Key = Keys.D0; Shift = true; break;
                case 42: Key = Keys.D8; Shift = true; Alternate = Keys.Multiply; break;
                case 43: Key = Keys.OemPlus; Shift = true; Alternate = Keys.Add; break;
                case 44: Key = Keys.OemComma; Shift = false; break;
                case 45: Key = Keys.OemMinus; Shift = false; Alternate = Keys.Subtract; break;
                case 46: Key = Keys.OemPeriod; Shift = false; Alternate = Keys.Decimal; break;
                case 47: Key = Keys.OemQuestion; Shift = false; Alternate = Keys.Divide; break;
                case 58: Key = Keys.OemSemicolon; Shift = true; break;
                case 59: Key = Keys.OemSemicolon; Shift = false; break;
                case 60: Key = Keys.OemComma; Shift = true; break;
                case 61: Key = Keys.OemPlus; Shift = false; break;
                case 62: Key = Keys.OemPeriod; Shift = true; break;
                case 63: Key = Keys.OemQuestion; Shift = true; break;
                case 64: Key = Keys.D2; Shift = true; break;
                case 91: Key = Keys.OemOpenBrackets; Shift = false; break;
                case 92: Key = Keys.OemPipe; Shift = false; break;
                case 93: Key = Keys.OemCloseBrackets; Shift = false; break;
                case 94: Key = Keys.D6; Shift = true; break;
                case 95: Key = Keys.OemMinus; Shift = true; break;
                case 96: Key = Keys.OemTilde; Shift = false; break;
                case 123: Key = Keys.OemOpenBrackets; Shift = true; break;
                case 124: Key = Keys.OemPipe; Shift = true; break;
                case 125: Key = Keys.OemCloseBrackets; Shift = true; break;
                case 126: Key = Keys.OemTilde; Shift = true; break;
                default:
                    if (swfKey >= 48 && swfKey <= 57)
                    {
                        Key = (Keys)swfKey;
                        Alternate = (Keys)(swfKey + 48);
                    }
                    if (swfKey >= 65 && swfKey <= 90)
                    {
                        Shift = true;
                        Key = (Keys)swfKey;
                    }
                    else if (swfKey >= 97 && swfKey <= 122)
                    {
                        Shift = false;
                        Key = (Keys)(swfKey - 32);
                    }
                    break;
            }
        }
    }
}
