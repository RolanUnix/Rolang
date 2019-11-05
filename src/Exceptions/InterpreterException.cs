using System;

namespace Rolang.Exceptions
{
    public class InterpreterException : Exception
    {
        public InterpreterException(string errorName, string description, int line) : base(errorName + " on line " + line + ": " + description) {}
    }
}