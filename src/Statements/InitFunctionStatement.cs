using System.Collections.Generic;
using System.Linq;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Statements
{
    public class InitFunctionStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly string _name;
        private readonly List<string> _nameArguments;
        private readonly IEnumerable<IStatement> _statements;
        private readonly int _codeLine;

        public InitFunctionStatement(Parser parser, string name, List<string> nameArguments, IEnumerable<IStatement> statements, int codeLine)
        {
            _parser = parser;
            _name = name;
            _nameArguments = nameArguments;
            _statements = statements;
            _codeLine = codeLine;
        }

        public void Execute()
        {
            var block = _parser.CurrentBlock;

            while (block != null)
            {
                if (block.Space.Variables.Any(variable => variable.Name == _name)) throw new RuntimeException("variable or function " + _name + " already exists", _codeLine);
                block = block.LastBlock;
            }

            _parser.CurrentBlock.Space.Variables.Add(new Variable(_name, new FunctionValue(_nameArguments, _statements)));
        }
    }
}