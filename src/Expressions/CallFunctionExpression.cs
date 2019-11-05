using System.Collections.Generic;
using System.Linq;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class CallFunctionExpression : IExpression
    {
        private readonly Parser _parser;
        private readonly string _name;
        private readonly List<IExpression> _arguments;
        private readonly int _codeLine;

        public CallFunctionExpression(Parser parser, string name, List<IExpression> arguments, int codeLine)
        {
            _parser = parser;
            _name = name;
            _arguments = arguments;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            var block = _parser.CurrentBlock;

            while (block != null)
            {
                if (block.Space.Variables.Any(variable => variable.Name == _name))
                {
                    var value = block.Space.Variables.First(variable => variable.Name == _name).Value;

                    if (value is FunctionValue function)
                    {
                        var argumentValues = new List<IValue>();

                        foreach (var argument in _arguments)
                        {
                            argumentValues.Add(argument.Compute());
                        }

                        return function.Execute(_parser, argumentValues, _codeLine);
                    }

                    throw new RuntimeException("'" + value.GetValueType() + "' is not callable", _codeLine);
                }

                block = block.LastBlock;
            }

            throw new RuntimeException("function " + _name + " is not initialized", _codeLine);
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}