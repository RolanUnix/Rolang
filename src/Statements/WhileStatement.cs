using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rolang.Exceptions.Internal;
using Rolang.Expressions;

namespace Rolang.Statements
{
    public class WhileStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly IExpression _conditionExpression;
        private readonly List<IStatement> _statements;

        public WhileStatement(Parser parser, IExpression conditionExpression, List<IStatement> statements)
        {
            _parser = parser;
            _conditionExpression = conditionExpression;
            _statements = statements;
        }

        public void Execute()
        {
            while (_conditionExpression.Compute().AsBoolean(_conditionExpression.GetLine()))
            {
                try
                {
                    new StartBlockStatement(_parser).Execute();

                    foreach (var statement in _statements)
                    {
                        statement.Execute();
                    }

                    new EndBlockStatement(_parser).Execute();
                }
                catch (BreakException)
                {
                    new EndBlockStatement(_parser).Execute();
                    break;
                }
                catch (ContinueException)
                {
                    new EndBlockStatement(_parser).Execute();
                }
            }
        }
    }
}
