using Rolang.Exceptions.Internal;

namespace Rolang.Statements
{
    public class ContinueStatement : IStatement
    {
        private readonly int _codeLine;
        public ContinueStatement(int codeLine)
        {
            _codeLine = codeLine;
        }

        public void Execute()
        {
            throw new ContinueException(_codeLine);
        }
    }
}