using System.Collections.Generic;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class ListExpression : IExpression
    {
        private readonly Dictionary<IExpression, IExpression> _expressions;
        private readonly int _codeLine;

        public ListExpression(Dictionary<IExpression, IExpression> expressions, int codeLine)
        {
            _expressions = expressions;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            var values = new Dictionary<IValue, IValue>();

            foreach (var expression in _expressions)
            {
                values.Add(expression.Key.Compute(), expression.Value.Compute());
            }

            return new ListValue(values);
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}