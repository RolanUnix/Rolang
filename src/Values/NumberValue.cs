using System;
using System.CodeDom;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using Rolang.Exceptions;

namespace Rolang.Values
{
    public class NumberValue : IValue
    {
        private readonly double _value;
        
        public NumberValue(string value, int codeLine)
        {
            double result;
            
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                _value = result;
                return;
            }

            throw new OverrunException("cannot be represented as a " + GetValueType(), codeLine);
        }

        public NumberValue(double value)
        {
            _value = value;
        }
        
        public string AsString(int codeLine)
        {
            return AsNumber(codeLine).ToString(CultureInfo.InvariantCulture);
        }

        public double AsNumber(int codeLine)
        {
            return _value;
        }

        public bool AsBoolean(int codeLine)
        {
            throw new RuntimeException(GetValueType() + " cannot be represented as a boolean", codeLine);
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
        
        public bool IsReferential()
        {
            return false;
        }

        public ValueType GetValueType()
        {
            return ValueType.Number;
        }

        public IValue Plus(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return new NumberValue(AsNumber(codeLine) + value.AsNumber(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for +: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Minus(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return new NumberValue(AsNumber(codeLine) - value.AsNumber(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for -: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Star(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return value.AsNumber(codeLine) == 0
                    ? new NumberValue(0)
                    : new NumberValue(AsNumber(codeLine) * value.AsNumber(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for *: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Slash(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return value.AsNumber(codeLine) == 0
                    ? new NumberValue(0)
                    : new NumberValue(AsNumber(codeLine) / value.AsNumber(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for /: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue Percent(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return new NumberValue(_value % value.AsNumber(codeLine));
            }
            
            throw new RuntimeException("unsupported operand type(s) for %: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }

        public IValue DoubleSlash(IValue value, int codeLine)
        {
            if (value is NumberValue)
            {
                return new NumberValue((long) (_value / value.AsNumber(codeLine)));
            }
            
            throw new RuntimeException("unsupported operand type(s) for //: '" + GetValueType() + "' and '" + value.GetValueType() + "'", codeLine);
        }
    }
}
