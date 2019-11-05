using System;
using Rolang.Expressions;

namespace Rolang.Exceptions.Internal
{
    public class ReturnException : Exception
    {
        public readonly IExpression ReturnExpression;

        public ReturnException(IExpression returnExpression)
        {
            ReturnExpression = returnExpression;
        }
    }
}