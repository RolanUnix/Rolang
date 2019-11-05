using System;

namespace Rolang.Exceptions.Internal
{
    public class ContinueException : Exception
    {
        public readonly int CodeLine;

        public ContinueException(int codeLine)
        {
            CodeLine = codeLine;
        }
    }
}