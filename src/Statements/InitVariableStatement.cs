using System.Linq;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Expressions;
using Rolang.Values;

namespace Rolang.Statements
{
    public class InitVariableStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly string _name;
        private readonly IExpression _value;
        private readonly int _line;

        public InitVariableStatement(Parser parser, string name, IExpression value, int line)
        {
            _parser = parser;
            _name = name;
            _value = value;
            _line = line;
        }

        public void Execute()
        {
            var block = _parser.CurrentBlock;
            
            while (block != null)
            {
                if (block.Space.Variables.Any(variable => variable.Name == _name)) throw new RuntimeException("variable " + _name +" already exists", _line);
                block = block.LastBlock;
            }
            
            _parser.CurrentBlock.Space.Variables.Add(new Variable(_name, _value.Compute()));
        }
    }
}