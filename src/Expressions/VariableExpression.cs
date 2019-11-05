using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class VariableExpression : IExpression
    {
        private readonly Parser _parser;
        private readonly string _name;
        private readonly int _codeLine;
        
        public VariableExpression(Parser parser, string name, int codeLine)
        {
            _parser = parser;
            _name = name;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            var block = _parser.CurrentBlock;

            while (block != null)
            {
                if (block.Space.Variables.Any(variable => variable.Name == _name))
                    return block.Space.Variables.First(variable => variable.Name == _name).Value;
                block = block.LastBlock;
            }
            
            throw new RuntimeException("variable " + _name + " is not initialized", _codeLine);
        }

        public string GetVariableName()
        {
            return _name;
        }
        
        public int GetLine()
        {
            return _codeLine;
        }
    }
}