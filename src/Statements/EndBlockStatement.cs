using Rolang.Data;

namespace Rolang.Statements
{
    public class EndBlockStatement : IStatement
    {
        private readonly Parser _parser;

        public EndBlockStatement(Parser parser)
        {
            _parser = parser;
        }

        public void Execute()
        {
            _parser.CurrentBlock = _parser.CurrentBlock.LastBlock;
        }
    }
}