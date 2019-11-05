using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class LogicalExpression : IExpression
    {
        private readonly IExpression _rightExpression;
        private readonly IExpression _leftExpression;
        private readonly TokenType _actionType;
        private readonly int _codeLine;

        public LogicalExpression(IExpression leftExpression, IExpression rightExpression, TokenType actionType, int codeLine)
        {
            _leftExpression = leftExpression;
            _rightExpression = rightExpression;
            _actionType = actionType;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            var leftValue = _leftExpression.Compute();
            var rightValue = _rightExpression.Compute();
            
            if (leftValue.GetValueType() != rightValue.GetValueType()) throw new RuntimeException("it is impossible to compare operands of different types: '" + leftValue.GetValueType() + "' and '" + rightValue.GetValueType() + "'", GetLine());
            
            switch (_actionType)
            {
                case TokenType.Equal:
                    return new BooleanValue(leftValue.ToString() == rightValue.ToString());
                case TokenType.NotEqual:
                    return new BooleanValue(leftValue.ToString() != rightValue.ToString());
                case TokenType.Not:
                    return new BooleanValue(!rightValue.AsBoolean(_codeLine));
                case TokenType.Or:
                    return new BooleanValue(rightValue.AsBoolean(_codeLine) || leftValue.AsBoolean(_codeLine));
                case TokenType.And:
                    return new BooleanValue(rightValue.AsBoolean(_codeLine) && leftValue.AsBoolean(_codeLine));
                case TokenType.LessThan:
                    return new BooleanValue(leftValue.AsNumber(_codeLine) < rightValue.AsNumber(_codeLine));
                case TokenType.LessThanOrEqual:
                    return new BooleanValue(leftValue.AsNumber(_codeLine) <= rightValue.AsNumber(_codeLine));
                case TokenType.MoreThan:
                    return new BooleanValue(leftValue.AsNumber(_codeLine) > rightValue.AsNumber(_codeLine));
                case TokenType.MoreThanOrEqual:
                    return new BooleanValue(leftValue.AsNumber(_codeLine) >= rightValue.AsNumber(_codeLine));
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