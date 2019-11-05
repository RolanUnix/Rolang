using System;
using System.Globalization;
using System.Text;
using Rolang.Exceptions;

namespace Rolang.Values
{
    public class StringValue : IValue
    {
        private readonly string _string;
        
        public StringValue(string str)
        {
            _string = str;
        }
        
        public string AsString(int codeLine)
        {
            return _string;
        }

        public double AsNumber(int codeLine)
        {
            double number;
            if (double.TryParse(_string, NumberStyles.Any, CultureInfo.InvariantCulture, out number)) return number;
            throw new RuntimeException("cannot convert " + GetValueType() + " to number", codeLine);
        }

        public bool AsBoolean(int codeLine)
        {
            throw new RuntimeException(GetValueType() + " cannot be represented as a boolean", codeLine);
        }

        public bool IsReferential()
        {
            return false;
        }

        public ValueType GetValueType()
        {
            return ValueType.String;
        }

        public IValue Plus(IValue value, int codeLine)
        {
            if (value is StringValue)
            {
                return new StringValue(_string + value.AsString(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for +: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Minus(IValue value, int codeLine)
        {
            throw new RuntimeException("unsupported operand type(s) for -: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Star(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                var val = value.AsNumber(codeLine);
                var stringBuilder = new StringBuilder();

                for (var i = 0; i < val; i++)
                {
                    stringBuilder.Append(_string);
                }
                
                return new StringValue(stringBuilder.ToString());
            }
            
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
            return "\"" + _string + "\"";
        }
    }
}