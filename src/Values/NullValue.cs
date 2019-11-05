using Rolang.Exceptions;

namespace Rolang.Values
{
    public class NullValue : IValue
    {
        public string AsString(int codeLine)
        {
            throw new RuntimeException("null cannot be represented as a string", codeLine);
        }

        public double AsNumber(int codeLine)
        {
            throw new RuntimeException("null cannot be represented as a number", codeLine);
        }

        public bool AsBoolean(int codeLine)
        {
            throw new RuntimeException("null cannot be represented as a boolean", codeLine);
        }

        public override string ToString()
        {
            return "null";
        }
        
        public bool IsReferential()
        {
            return false;
        }

        public ValueType GetValueType()
        {
            return ValueType.Null;
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
    }
}