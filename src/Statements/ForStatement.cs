using System;
using System.Collections.Generic;
using Rolang.Exceptions.Internal;
using Rolang.Expressions;

namespace Rolang.Statements
{
    public class ForStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly IStatement _preStatement;
        private readonly IExpression _conditionLoop;
        private readonly IExpression _stepExpression;
        private readonly List<IStatement> _statements;

        public ForStatement(Parser parser, IStatement preStatement, IExpression conditionLoop, IExpression stepExpression, List<IStatement> statements)
        {
            _parser = parser;
            _preStatement = preStatement;
            _conditionLoop = conditionLoop;
            _stepExpression = stepExpression;
            _statements = statements;
        }

        public void Execute()
        {
            for (_preStatement.Execute(); _conditionLoop.Compute().AsBoolean(_conditionLoop.GetLine()); _stepExpression.Compute())
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