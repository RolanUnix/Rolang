using System;
using Rolang.Expressions;

namespace Rolang.Statements
{
    public class PrintStatement : IStatement
    {
        private readonly IExpression _outputExpression;

        public PrintStatement(IExpression outputExpression)
        {
            _outputExpression = outputExpression;
        }
        
        public void Execute()
        {
            Console.WriteLine(_outputExpression.Compute().ToString());
        }
    }
}