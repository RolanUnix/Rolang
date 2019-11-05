using Rolang.Values;

namespace Rolang.Expressions
{
    public class NullExpression : IExpression
    {
        private readonly int _codeLine;

        public NullExpression(int codeLine)
        {
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            return new NullValue();
        }

        public int GetLine()
        {
            return _codeLine;
        }
    }
}