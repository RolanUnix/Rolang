using System;

namespace Rolang.Exceptions
{
    public class SyntaxException : InterpreterException
    {
        public SyntaxException(string description, int line) : base("SyntaxError", description, line) {}
    }
}