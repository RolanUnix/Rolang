using Rolang.Exceptions.Internal;

namespace Rolang.Statements
{
    public class BreakStatement : IStatement
    {
        public void Execute()
        {
            throw new BreakException();
        }
    }
}