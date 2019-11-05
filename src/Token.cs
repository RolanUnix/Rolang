namespace Rolang
{
    public class Token
    {
        public readonly TokenType Type;
        public readonly string Value;
        public readonly int Line;

        public Token(TokenType type, string value, int line)
        {
            Type = type;
            Value = value;
            Line = line;
        }
    }
}