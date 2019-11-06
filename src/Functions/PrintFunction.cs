using System;
using Rolang.Expressions;
using Rolang.Statements;

namespace Rolang.Functions
{
    public class PrintFunction : IStatement
    {
        private readonly Parser _parser;

        public PrintFunction(Parser parser)
        {
            _parser = parser;
        }

        public void Execute()
        {
            Console.WriteLine(new VariableExpression(_parser, "$1", 0).Compute().ToString());
        }
    }
}