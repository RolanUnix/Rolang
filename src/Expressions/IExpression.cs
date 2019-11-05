using Rolang.Values;

namespace Rolang.Expressions
{
    public interface IExpression
    {
        IValue Compute();
        int GetLine();
    }
}