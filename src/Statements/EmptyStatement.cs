using Rolang.Expressions;

namespace Rolang.Statements
{
    public class EmptyStatement : IStatement
    {
        private readonly IExpression _expression;

        public EmptyStatement(IExpression expression)
        {
            _expression = expression;
        }

        public void Execute()
        {
            _expression.Compute();
        }
    }
}