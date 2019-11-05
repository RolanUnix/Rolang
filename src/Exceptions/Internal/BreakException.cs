using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rolang.Exceptions.Internal
{
    public class BreakException : Exception
    {
        public readonly int CodeLine;

        public BreakException(int codeLine)
        {
            CodeLine = codeLine;
        }
    }
}
