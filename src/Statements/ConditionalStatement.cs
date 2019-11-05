using System.Collections.Generic;
using Rolang.Expressions;

namespace Rolang.Statements
{
    public class ConditionalStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly Dictionary<IExpression, List<IStatement>> _conditionBlocks;
        private readonly List<IStatement> _statementsElse;

        public ConditionalStatement(Parser parser, Dictionary<IExpression, List<IStatement>> conditionBlocks, List<IStatement> statementsElse)
        {
            _parser = parser;
            _conditionBlocks = conditionBlocks;
            _statementsElse = statementsElse;
        }

        public void Execute()
        {
            foreach (var condition in _conditionBlocks)
            {
                if (condition.Key.Compute().AsBoolean(condition.Key.GetLine()))
                {
                    new StartBlockStatement(_parser).Execute();

                    foreach (var statement in condition.Value)
                    {
                        statement.Execute();
                    }

                    new EndBlockStatement(_parser).Execute();

                    return;
                }
            }

            if (_statementsElse != null)
            {
                new StartBlockStatement(_parser).Execute();

                foreach (var statement in _statementsElse)
                {
                    statement.Execute();
                }

                new EndBlockStatement(_parser).Execute();
            }
        }
    }
}