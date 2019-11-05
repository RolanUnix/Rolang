using System;
using System.Collections.Generic;
using System.Linq;
using Rolang.Exceptions;
using Rolang.Expressions;

namespace Rolang.Values
{
    public class ListValue : IValue
    {
        private readonly Dictionary<IValue, IValue> _values;
        
        public ListValue(Dictionary<IValue, IValue> values)
        {
            _values = values;
        }

        public IValue this[IValue index, int codeLine]
        {
            get
            {
                if (index.IsReferential())
                {
                    if (!_values.ContainsKey(index)) throw new RuntimeException("key is not found", codeLine);
                    return _values[index];
                }
                else
                {
                    var val = _values.Where(obj => obj.Key.ToString() == index.ToString()).ToList();
                    if (!val.Any()) throw new RuntimeException("key is not found", codeLine);
                    return val.First().Value;
                }
            }

            set
            {
                if (index.IsReferential())
                {
                    if (_values.ContainsKey(index)) _values[index] = value;
                    _values.Add(index, value);   
                }
                else
                {
                    var valList = _values.Where(obj => obj.Key.ToString() == index.ToString()).ToList();

                    if (valList.Any()) _values[valList.First().Key] = value;
                    else _values.Add(index, value);
                }
            }
        }
        
        public string AsString(int codeLine)
        {
            throw new RuntimeException(GetValueType() + " cannot be represented as a string", codeLine);
        }

        public double AsNumber(int codeLine)
        {
            throw new RuntimeException(GetValueType() + " cannot be represented as a number", codeLine);
        }

        public bool AsBoolean(int codeLine)
        {
            throw new RuntimeException(GetValueType() + " cannot be represented as a boolean", codeLine);
        }

        public override string ToString()
        {
            return "[" + string.Join(", ", _values.Select(obj => "[" + obj.Key.ToString() + ", " + obj.Value.ToString() +  "]")) + "]";
        }

        public bool IsReferential()
        {
            return false;
        }

        public ValueType GetValueType()
        {
            return ValueType.List;
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