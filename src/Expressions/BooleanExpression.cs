﻿using Rolang.Values;

namespace Rolang.Expressions
{
    public class BooleanExpression : IExpression
    {
        private readonly IValue _value;
        private readonly int _codeLine;

        public BooleanExpression(IValue value, int codeLine)
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