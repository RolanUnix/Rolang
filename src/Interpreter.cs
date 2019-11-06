using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Exceptions.Internal;
using Rolang.Functions;
using Rolang.Statements;
using Rolang.Values;

namespace Rolang
{
    public class Interpreter
    {
        private readonly Block _block;

        public IValue this[string name]
        {
            get
            {
                var variable = _block.Space.Variables.Where(obj => obj.Name == name);
                return variable.Any() ? variable.First().Value : null;
            }

            set
            {
                var variable = _block.Space.Variables.Where(obj => obj.Name == name);

                if (variable.Any()) variable.First().Value = value;
                else _block.Space.Variables.Add(new Variable(name, value));
            }
        }

        public Interpreter()
        {
            _block = new Block();
        }

        public IValue ExecuteFile(string pathFile)
        {
            return Execute(File.ReadAllText(pathFile));
        }

        public IValue ExecuteString(string code)
        {
            return Execute(code);
        }

        private IValue Execute(string code)
        {
            try
            {
                var parser = new Parser(_block, new Lexer(code).Tokenize());
                var statements = parser.Parse();

                InitInternalFunction(parser);

                foreach (var statement in statements)
                {
                    statement.Execute();
                }

                return new NullValue();
            }
            catch (ReturnException e)
            {
                return e.ReturnExpression.Compute();
            }
            catch (BreakException e)
            {
                throw new RuntimeException("'break' outside loop", e.CodeLine);
            }
            catch (ContinueException e)
            {
                throw new RuntimeException("'continue' outside loop", e.CodeLine);
            }
        }

        private void InitInternalFunction(Parser parser)
        {
            this["print"] = new FunctionValue(new List<string>()
            {
                "$1"
            }, new List<IStatement>()
            {
                new PrintFunction(parser)
            });
        }
    }
}
