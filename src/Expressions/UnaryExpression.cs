using Rolang.Values;

namespace Rolang.Expressions
{
    public class UnaryExpression : IExpression
    {
        private readonly IExpression _expression;
        private readonly int _codeLine;
        
        public UnaryExpression(IExpression expression, int codeLine)
        {
            _expression = expression;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            return new NumberValue(-1 * _expression.Compute().AsNumber(_codeLine));
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}