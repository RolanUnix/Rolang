using System.Globalization;
using System.Linq;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class DecrementExpression : IExpression
    {
        private readonly Parser _parser;
        private readonly IExpression _expression;
        private readonly bool _postDecrement;
        private readonly int _codeLine;

        public DecrementExpression(Parser parser, IExpression expression, bool postDecrement, int codeLine)
        {
            _parser = parser;
            _expression = expression;
            _postDecrement = postDecrement;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            if (_expression is VariableExpression)
            {   
                var value = _expression.Compute();

                if (value.GetValueType() == ValueType.Number)
                {
                    var block = _parser.CurrentBlock;
                    while (block != null)
                    {
                        if (block.Space.Variables.Any(variable =>
                            variable.Name == ((VariableExpression) _expression).GetVariableName()))
                        {
                            var newValue =
                                new NumberValue((value.AsNumber(_codeLine) - 1));
                            block.Space.Variables.First(variable =>
                                variable.Name == ((VariableExpression) _expression).GetVariableName()).Value = newValue;
                            return _postDecrement ? value : newValue;
                        }

                        block = block.LastBlock;
                    }

                    throw new RuntimeException(
                        "variable " + ((VariableExpression) _expression).GetVariableName() + " is not initialized",
                        _codeLine);
                }
            }

            if (_expression is ListValueExpression)
            {
                var block = _parser.CurrentBlock;

                var listValueExpression = (ListValueExpression)_expression;
                var listValue = (ListValue) listValueExpression.GetList();
                var oldValue = listValueExpression.Compute();
                var newValue = new NumberValue((oldValue.AsNumber(_codeLine) - 1));
                
                while (block != null)
                {
                    if (block.Space.Variables.Any(variable => variable.Name == listValueExpression.GetVariableName()))
                    {
                        ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = newValue;
                        return _postDecrement ? oldValue : newValue;
                    }

                    block = block.LastBlock;
                }
            
                throw new RuntimeException("variable " + listValueExpression.GetVariableName() + " is not initialized", _codeLine);
            }
            
            if (_expression is NumberExpression)
            {
                return _postDecrement ? _expression.Compute() : new NumberValue((_expression.Compute().AsNumber(_codeLine) - 1));
            }
            
            throw new RuntimeException("this type cannot be decremented", _codeLine);
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}