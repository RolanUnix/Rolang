using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class BinaryExpression : IExpression
    {
        private readonly TokenType _binaryType;
        private readonly IExpression _leftExpression;
        private readonly IExpression _rightExpression;

        private readonly int _codeLine;
        
        public BinaryExpression(TokenType binaryType, IExpression leftExpression, IExpression rightExpression, int codeLine)
        {
            _binaryType = binaryType;
            _leftExpression = leftExpression;
            _rightExpression = rightExpression;
            _codeLine = codeLine;
        }
        
        public IValue Compute()
        {
            var leftValue = _leftExpression.Compute();
            var rightValue = _rightExpression.Compute();
            
            switch (_binaryType)
            {
                case TokenType.Plus:
                    return leftValue.Plus(rightValue, _codeLine);
                case TokenType.Minus:
                    return leftValue.Minus(rightValue, _codeLine);
                case TokenType.Star:
                    return leftValue.Star(rightValue, _codeLine);
                case TokenType.Slash:
                    return leftValue.Slash(rightValue, _codeLine);
                case TokenType.Percent:
                    return leftValue.Percent(rightValue, _codeLine);
                case TokenType.DoubleSlash:
                    return leftValue.DoubleSlash(rightValue, _codeLine);
                default:
                    throw new RuntimeException("invalid syntax", _codeLine);
            }
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}