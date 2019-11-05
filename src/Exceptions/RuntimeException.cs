using System;

namespace Rolang.Exceptions
{
    public class RuntimeException : InterpreterException
    {
        public RuntimeException(string description, int line) : base("RuntimeError", description, line) {}
    }
}