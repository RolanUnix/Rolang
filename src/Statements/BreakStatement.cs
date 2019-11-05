using Rolang.Exceptions.Internal;

namespace Rolang.Statements
{
    public class BreakStatement : IStatement
    {
        private readonly int _codeLine;
        public BreakStatement(int codeLine)
        {
            _codeLine = codeLine;
        }

        public void Execute()
        {
            throw new BreakException(_codeLine);
        }
    }
}