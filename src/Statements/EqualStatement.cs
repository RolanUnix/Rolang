using System.Linq;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Expressions;
using Rolang.Values;

namespace Rolang.Statements
{
    public class EqualStatement : IStatement
    {
        private readonly Parser _parser;
        private readonly IExpression _var;
        private readonly IExpression _value;
        private readonly TokenType _equalType;
        private readonly int _codeLine;
        
        public EqualStatement(Parser parser, IExpression var, IExpression value, TokenType equalType, int codeLine)
        {
            _parser = parser;
            _var = var;
            _value = value;
            _equalType = equalType;
            _codeLine = codeLine;
        }

        public void Execute()
        {    
            if (_var is VariableExpression)
            {
                var varExpression = (VariableExpression) _var;
                var block = _parser.CurrentBlock;

                while (block != null)
                {
                    if (block.Space.Variables.Any(variable => variable.Name == varExpression.GetVariableName()))
                    {
                        switch (_equalType)
                        {
                            case TokenType.Equal:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = _value.Compute();
                                break;
                            case TokenType.Plus:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.Plus(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Minus:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.Minus(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Star:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.Star(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Slash:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.Slash(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Percent:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.Percent(_value.Compute(), _codeLine);
                                break;
                            case TokenType.DoubleSlash:
                                block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value = block.Space.Variables.First(variable => variable.Name == varExpression.GetVariableName()).Value.DoubleSlash(_value.Compute(), _codeLine);
                                break;
                            default:
                                throw new RuntimeException("invalid syntax", _codeLine);
                        }
                        return;
                    }

                    block = block.LastBlock;
                }
            
                throw new RuntimeException("variable " + varExpression.GetVariableName() + " is not initialized", _codeLine);   
            }

            if (_var is ListValueExpression)
            {
                var block = _parser.CurrentBlock;

                var listValueExpression = (ListValueExpression)_var;
                var listValue = (ListValue) listValueExpression.GetList();
                
                while (block != null)
                {
                    if (block.Space.Variables.Any(variable => variable.Name == listValueExpression.GetVariableName()))
                    {
                        switch (_equalType)
                        {
                            case TokenType.Equal:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = _value.Compute();
                                break;
                            case TokenType.Plus:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].Plus(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Minus:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].Minus(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Star:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].Star(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Slash:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].Slash(_value.Compute(), _codeLine);
                                break;
                            case TokenType.Percent:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].Percent(_value.Compute(), _codeLine);
                                break;
                            case TokenType.DoubleSlash:
                                ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine] = ((ListValue)block.Space.Variables.First(variable => variable.Name == listValueExpression.GetVariableName()).Value)[listValueExpression.GetIndex(), _codeLine].DoubleSlash(_value.Compute(), _codeLine);
                                break;
                            default:
                                throw new RuntimeException("invalid syntax", _codeLine);
                        }
                        return;
                    }

                    block = block.LastBlock;
                }
            
                throw new RuntimeException("variable " + listValueExpression.GetVariableName() + " is not initialized", _codeLine);
            }
            
            throw new RuntimeException("it is impossible to assign a value", _value.GetLine());
        }
    }
}