﻿using Rolang.Values;

namespace Rolang.Expressions
{
    public class StringExpression : IExpression
    {
        private readonly IValue _value;
        private readonly int _codeLine;

        public StringExpression(IValue value, int codeLine)
        {
            _value = value;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            return _value;
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}