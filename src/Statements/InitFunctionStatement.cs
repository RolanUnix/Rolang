using System.Collections.Generic;

namespace Rolang.Statements
{
    public class InitFunctionStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly string _name;
        private readonly IEnumerable<string> _nameArguments;
        private readonly IEnumerable<IStatement> _statements;

        public InitFunctionStatement(Parser parser, string name, IEnumerable<string> nameArguments, IEnumerable<IStatement> statements)
        {
            _parser = parser;
            _name = name;
            _nameArguments = nameArguments;
            _statements = statements;
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}