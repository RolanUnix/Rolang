using Rolang.Exceptions.Internal;

namespace Rolang.Statements
{
    public class ContinueStatement : IStatement
    {
        public void Execute()
        {
            throw new ContinueException();
        }
    }
}