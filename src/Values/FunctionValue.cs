using System.Collections.Generic;
using Rolang.Exceptions;
using Rolang.Exceptions.Internal;
using Rolang.Expressions;
using Rolang.Statements;

namespace Rolang.Values
{
    public class FunctionValue : IValue
    {
        public readonly List<string> NameArguments;
        private readonly IEnumerable<IStatement> _statements;

        public FunctionValue(List<string> nameArguments, IEnumerable<IStatement> statements)
        {
            NameArguments = nameArguments;
            _statements = statements;
        }

        public IValue Execute(Parser parser, List<IValue> arguments, int codeLine)
        {
            if (NameArguments.Count != arguments.Count) throw new RuntimeException("function takes " + NameArguments.Count + " positional arguments but " + arguments.Count + " were given", codeLine);

            new StartBlockStatement(parser).Execute();

            for (var i = 0; i < NameArguments.Count; i++) new InitVariableStatement(parser, NameArguments[i], new PlugExpression(arguments[i], codeLine), codeLine).Execute();

            try
            {
                foreach (var statement in _statements)
                {
                    statement.Execute();
                }

                new EndBlockStatement(parser).Execute();
            }
            catch (ReturnException e)
            {
                new EndBlockStatement(parser).Execute();
                return e.ReturnExpression.Compute();
            }

            return new NullValue();
        }

        public string AsString(int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public double AsNumber(int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public bool AsBoolean(int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public bool IsReferential()
        {
            throw new System.NotImplementedException();
        }

        public ValueType GetValueType()
        {
            throw new System.NotImplementedException();
        }

        public IValue Plus(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public IValue Minus(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public IValue Star(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public IValue Slash(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public IValue Percent(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }

        public IValue DoubleSlash(IValue value, int codeLine)
        {
            throw new System.NotImplementedException();
        }
    }
}