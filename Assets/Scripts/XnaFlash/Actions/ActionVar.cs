using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XnaFlash.Actions.Objects;
using XnaFlash.Movie;

namespace XnaFlash.Actions
{
    public class ActionVar : IConvertible
    {
        private object _value;

        public bool IsNull { get { return _value == null; } }
        public bool IsUndef { get { return _value is Undef; } }        
        public bool IsString { get { return _value is string; } }        
        public bool IsFunc { get { return _value is ActionFunc; } }
        public bool IsObject { get { return _value is ActionObject; } }
        public bool IsBoolean { get { return _value is bool; } }
        public bool IsDouble { get { return _value is double; } }
        public bool IsInteger { get { return _value is long; } }
        public bool IsValid { get { return !IsNull && !IsUndef; } }
        public bool IsValue { get { return !IsObject && !IsFunc; } }
        public bool IsNumeric { get { return _value is double || _value is long || _value is bool; } }

        public MovieClip MovieClip 
        {
            get { return _value as MovieClip; }
            set { _value = value; }
        }
        public ActionObject Object 
        {
            get { return _value as ActionObject; } 
            set { _value = value; } 
        }
        public ActionObject WrapperObject
        {
            get
            {
                if (!IsValid) return null;
                if (IsObject) return Object;
                if (IsString) return new Objects.String((string)_value);
                if (IsNumeric) return new Objects.Number(Double);
                if (IsFunc) return new Objects.Function(Func);
                if (IsBoolean) return new Objects.Boolean(Boolean);
                return null;
            }
        }
        public ActionFunc Func 
        {
            get { return _value as ActionFunc; }
            set { _value = value; } 
        }
        public string String 
        {
            get
            {
                if (IsNull) return "[null]";
                return _value.ToString();
            }
            set { _value = value; } 
        }
        public string NullableString
        {
            get
            {
                if (!IsValid) return null;
                return _value.ToString();
            }
            set { _value = value; }
        }
        public double Double 
        {
            get
            {
                if (IsDouble) return (double)_value;
                if (IsString)
                {
                    double f;
                    double.TryParse(_value.ToString(), out f);
                    return f;
                }
                if (IsBoolean) return ((bool)_value) ? 1f : 0f;
                if (IsInteger) return (long)_value;
                if (IsObject) return (_value as ActionObject).Value.Double;
                
                return 0f;
            }
            set { _value = value; } 
        }
        public bool Boolean 
        {
            get
            {
                if (IsBoolean) return (bool)_value;
                if (IsDouble) return 0f != (double)_value;
                if (IsInteger) return 0 != (long)_value;
                if (IsString)
                {
                    var s = _value as string;
                    if (string.Equals(s, "true", StringComparison.InvariantCultureIgnoreCase)) return true;
                    if (string.Equals(s, "1", StringComparison.InvariantCultureIgnoreCase)) return true;
                    return false;
                }                
                if (IsObject) return (_value as ActionObject).Value.Boolean;

                return false;
            }
            set { _value = value; } 
        }
        public long Integer
        {
            get
            {
                if (IsInteger) return (long)_value;
                if (IsBoolean) return ((bool)_value) ? 1 : 0;
                if (IsDouble) return (long)(double)_value;
                if (IsString)
                {
                    long i;
                    long.TryParse(_value as string, out i);
                    return i;
                }
                if (IsObject) return (_value as ActionObject).Value.Integer;

                return 0;
            }
            set { _value = value; }
        }
        public char FirstChar
        {
            get
            {
                var s = String;
                if (s.Length == 0) return '\0';
                return s[0];
            }
        }

        public string TypeOf
        {
            get
            {
                if (IsNull) return "null";
                if (IsUndef) return "undefined";
                if (IsString) return "string";
                if (IsFunc) return "function";
                if (IsBoolean) return "boolean";
                if (IsNumeric) return "number";
                if (_value is MovieClip) return "movieclip";
                if (IsObject) return "object";

                throw new InvalidOperationException("Unexpected variable value!");
            }
        }
        
        public ActionVar() { SetUndef(); }
        public ActionVar(ActionVar var) { _value = var._value; }
        public ActionVar(MovieClip mc) { MovieClip = mc; }
        public ActionVar(ActionObject o) { Object = o; }
        public ActionVar(ActionFunc f) { Func = f; }
        public ActionVar(string s) { String = s; }
        public ActionVar(double d) { Double = d; }
        public ActionVar(bool b) { Boolean = b; }
        public ActionVar(long i) { Integer = i; }

        public bool IsIdentical(ActionVar other)
        {
            if (IsNull && other.IsNull) return true;
            if (IsNull || other.IsNull) return false;
            if (_value.GetType() != other._value.GetType()) return false;
            return object.Equals(_value, other._value);
        }
        public bool IsEqual(ActionVar other)
        {
            if (IsNull || IsUndef) return other.IsNull || other.IsUndef;
            if (other.IsNull || other.IsUndef) return IsNull || IsUndef;           
            if (_value.GetType() == other._value.GetType()) return object.Equals(_value, other._value);                       
            if (IsFunc) return object.ReferenceEquals(_value, other._value);
            if (IsInteger) return Integer == other.Integer;
            if (IsDouble) return Double == other.Double;
            if (IsBoolean) return Boolean == other.Boolean;
            if (IsString || IsObject) return other.IsEqual(this);

            return false;
        }

        public void SetNull() 
        {
            _value = null;
        }
        public void SetUndef() 
        {
            _value = Undef.Instance;
        }
        public void SetValue(ActionVar other)
        {
            _value = other._value;
        }

        public override string ToString()
        {
            return String;
        }

        public static implicit operator ActionVar(ActionObject o) { return new ActionVar(o); }
        public static implicit operator ActionVar(ActionFunc f) { return new ActionVar(f); }
        public static implicit operator ActionVar(MovieClip mc) { return new ActionVar(mc); }
        public static implicit operator ActionVar(string s) { return new ActionVar(s); }
        public static implicit operator ActionVar(double d) { return new ActionVar(d); }
        public static implicit operator ActionVar(float f) { return new ActionVar((double)f); }
        public static implicit operator ActionVar(bool b) { return new ActionVar(b); }
        public static implicit operator ActionVar(long i) { return new ActionVar(i); }
        public static implicit operator ActionVar(int i) { return new ActionVar((long)i); }

        public static explicit operator ActionObject(ActionVar var) { return var.Object; }
        public static explicit operator ActionFunc(ActionVar var) { return var.Func; }
        public static explicit operator MovieClip(ActionVar var) { return var.MovieClip; }
        public static explicit operator string(ActionVar var) { return var.String; }
        public static explicit operator double(ActionVar var) { return var.Double; }
        public static explicit operator Single(ActionVar var) { return (float)var.Double; }
        public static explicit operator bool(ActionVar var) { return var.Boolean; }
        public static explicit operator long(ActionVar var) { return var.Integer; }
        public static explicit operator int(ActionVar var) { return (int)var.Integer; }

        public static ActionVar FromNativeValue(object obj)
        {
            if (obj == null) return new ActionVar((string)null);
            if (obj is ActionVar) return obj as ActionVar;
            if (obj is double || obj is double || obj is decimal) return new ActionVar(Convert.ToDouble(obj));
            if (obj is long || obj is long || obj is short || obj is byte) return new ActionVar(Convert.ToInt64(obj));
            if (obj is string) return new ActionVar(obj as string);
            if (obj is bool) return new ActionVar((bool)obj);
            if (obj is ActionObject) return new ActionVar(obj as ActionObject);
            if (obj is ActionFunc) return new ActionVar(obj as ActionFunc);
            return new ActionVar();
        }        

        #region IConvertible

        TypeCode IConvertible.GetTypeCode()
        {
            var value = _value as IConvertible;
            return (value != null) ? value.GetTypeCode() : TypeCode.Object;
        }
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Boolean;
        }
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return (byte)Integer;
        }
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (char)Integer;
        }
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return new DateTime(Integer);
        }
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return (decimal)Double;
        }
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Double;
        }
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return (short)Integer;
        }
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return (int)Integer;
        }
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Integer;
        }
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return (sbyte)Integer;
        }
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return (float)Double;
        }
        string IConvertible.ToString(IFormatProvider provider)
        {
            return String;
        }
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return (ushort)Integer;
        }
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return (uint)Integer;
        }
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return (ulong)Integer;
        }

        #endregion

        #region Helper classes

        private class Undef
        {
            public static readonly Undef Instance = new Undef();
            private Undef() { }
            public override string ToString() { return "[undefined]"; }
        }
        public class IndexActionVar : ActionVar
        {
            public IndexActionVar(int index) : base(index) { }
        }

        #endregion
    }
}