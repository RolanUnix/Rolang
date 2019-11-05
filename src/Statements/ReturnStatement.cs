using Rolang.Exceptions.Internal;
using Rolang.Expressions;

namespace Rolang.Statements
{
    public class ReturnStatement : IStatement
    {
        private readonly IExpression _expression;

        public ReturnStatement(IExpression expression)
        {
            _expression = expression;
        }

        public void Execute()
        {
            throw new ReturnException(_expression);
        }
    }
}