using System.Globalization;
using Rolang.Exceptions;

namespace Rolang.Values
{
    public class BooleanValue : IValue
    {
        private readonly bool _value;

        public BooleanValue(bool value)
        {
            _value = value;
        }

        public string AsString(int codeLine)
        {
            return _value.ToString().ToLower();
        }

        public double AsNumber(int codeLine)
        {
            return _value ? 1 : 0;
        }

        public bool AsBoolean(int codeLine)
        {
            return _value;
        }

        public bool IsReferential()
        {
            return false;
        }

        public ValueType GetValueType()
        {
            return ValueType.Boolean;
        }

        public IValue Plus(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for +: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Minus(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for -: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Star(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for *: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Slash(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for /: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Percent(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for %: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue DoubleSlash(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for //: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public override string ToString()
        {
            return _value.ToString().ToLower();
        }
    }
}