using System;

namespace Rolang.Exceptions
{
    public class OverrunException : InterpreterException
    {
        public OverrunException(string description, int line) : base("Overflow", description, line) {}
    }
}