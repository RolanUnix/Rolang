using System.Linq;
using System.Runtime.InteropServices;
using Rolang.Exceptions;
using Rolang.Values;

namespace Rolang.Expressions
{
    public class ListValueExpression : IExpression
    {
        private readonly Parser _parser;
        private readonly IExpression _list;
        private readonly IExpression _index;
        private readonly int _codeLine;

        public ListValueExpression(Parser parser, IExpression list, IExpression index, int codeLine)
        {
            _parser = parser;
            _list = list;
            _index = index;
            _codeLine = codeLine;
        }

        public IValue Compute()
        {
            var list = GetList();
            
            if (list is ListValue)
            {
                var val = (ListValue)_list.Compute();
                return val[_index.Compute(), _codeLine];
            }

            if (list is StringValue)
            {
                var str = list.AsString(_codeLine).ToList();
                var index = _index.Compute().AsNumber(_codeLine);
                if (index < str.Count) return new StringValue(str[(int)index].ToString());
                
                throw new RuntimeException("key is not found", _codeLine);
            }
            
            throw new RuntimeException("this type does not have an indexer", _codeLine);
        }

        public IValue GetList()
        {
            if (_list is VariableExpression)
            {
                var block = _parser.CurrentBlock;

                while (block != null)
                {
                    if (block.Space.Variables.Any(variable => variable.Name == ((VariableExpression)_list).GetVariableName()))
                    {
                        return block.Space.Variables.First(variable => variable.Name == ((VariableExpression)_list).GetVariableName()).Value;
                    }

                    block = block.LastBlock;
                }
            
                throw new RuntimeException("variable " + ((VariableExpression)_list).GetVariableName() + " is not initialized", _codeLine);
            }

            return _list.Compute();
        }

        public IValue GetIndex()
        {
            return _index.Compute();
        }

        public string GetVariableName()
        {
            return ((VariableExpression) _list).GetVariableName();
        }
        
        public int GetLine()
        {
            throw new System.NotImplementedException();
        }
    }
}