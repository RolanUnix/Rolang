namespace Rolang.Values
{
    public interface IValue
    {
        string AsString(int codeLine);
        double AsNumber(int codeLine);
        bool AsBoolean(int codeLine);
        bool IsReferential();
        ValueType GetValueType();

        IValue Plus(IValue value, int codeLine);
        IValue Minus(IValue value, int codeLine);
        IValue Star(IValue value, int codeLine);
        IValue Slash(IValue value, int codeLine);
        IValue Percent(IValue value, int codeLine);
        IValue DoubleSlash(IValue value, int codeLine);
    }
}