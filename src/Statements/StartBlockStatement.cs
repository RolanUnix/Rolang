using Rolang.Data;

namespace Rolang.Statements
{
    public class StartBlockStatement : IStatement
    {
        private readonly Parser _parser;

        public StartBlockStatement(Parser parser)
        {
            _parser = parser;
        }

        public void Execute()
        {
            _parser.CurrentBlock = new Block()
            {
                LastBlock = _parser.CurrentBlock
            };
        }
    }
}