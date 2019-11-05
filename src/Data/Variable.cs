using Rolang.Values;

namespace Rolang.Data
{
    public class Variable
    {
        public readonly string Name;
        public IValue Value;

        public Variable(string name, IValue value)
        {
            Name = name;
            Value = value;
        }
    }
}