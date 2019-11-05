using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters;
using Rolang.Data;
using Rolang.Exceptions;
using Rolang.Expressions;
using Rolang.Statements;
using Rolang.Values;
using BinaryExpression = Rolang.Expressions.BinaryExpression;
using UnaryExpression = Rolang.Expressions.UnaryExpression;

namespace Rolang
{
    public class Parser
    {
        public Block CurrentBlock;
        
        private readonly List<Token> _tokens;
        private int _readerPosition;
        
        public Parser(Block block, IEnumerable<Token> tokens)
        {
            _tokens = tokens.ToList();
            _readerPosition = 0;
            CurrentBlock = block;
        }

        public IEnumerable<IStatement> Parse()
        {
            var statements = new List<IStatement>();

            while (_readerPosition < _tokens.Count - 1)
            {   
                statements.Add(Statement());
            }

            return statements;
        }

        private void CheckSemicolon()
        {
            var currentToken = ReadToken();
            if (currentToken.Type != TokenType.Semicolon) throw new SyntaxException("a semicolon is required after the action completes", currentToken.Line);
        }
        
        private IStatement Statement()
        {
            var currentToken = ReadToken();

            if (currentToken.Type == TokenType.Function)
            {
                var nameFunctionToken = ReadToken();
                if (nameFunctionToken.Type != TokenType.Word) throw new SyntaxException("invalid syntax", currentToken.Line);

                currentToken = ReadToken();
                if (currentToken.Type != TokenType.LParen) throw new SyntaxException("invalid syntax", currentToken.Line);

                currentToken = ReadToken();
                var nameArguments = new List<string>();

                while (currentToken.Type != TokenType.RParen)
                {
                    if (currentToken.Type != TokenType.Word) throw new SyntaxException("invalid syntax", currentToken.Line);
                    
                    if (nameArguments.Contains(currentToken.Value)) throw new SyntaxException("duplicate argument '" + currentToken.Value + "' in function definition", currentToken.Line);
                    nameArguments.Add(currentToken.Value);
                    
                    currentToken = ReadToken();
                }

                var statements = new List<IStatement>();

                var statement = Statement();

                if (statement is StartBlockStatement)
                {
                    do
                    {
                        statements.Add(statement);
                        statement = Statement();

                        if (!(statement is EndBlockStatement)) continue;

                        statements.Add(statement);
                        break;
                    } while (true);
                }
                else
                {
                    statements.Add(statement);
                }

                return new InitFunctionStatement(this, nameFunctionToken.Value, nameArguments, statements);
            }

            if (currentToken.Type == TokenType.If)
            {
                var conditionBlocks = new Dictionary<IExpression, List<IStatement>>();

                currentToken = ReadToken();
                if (currentToken.Type != TokenType.LParen) throw new SyntaxException("invalid syntax", currentToken.Line);
                var conditionExpression = Expression();
                currentToken = ReadToken();
                if (currentToken.Type != TokenType.RParen) throw new SyntaxException("invalid syntax", currentToken.Line);

                var statements = new List<IStatement>();

                var statement = Statement();
                
                if (statement is StartBlockStatement)
                {
                    do
                    {
                        statements.Add(statement);
                        statement = Statement();

                        if (!(statement is EndBlockStatement)) continue;
                        
                        statements.Add(statement);
                        break;
                    } while (true);
                }
                else
                {
                    statements.Add(statement);
                }

                conditionBlocks.Add(conditionExpression, statements);

                currentToken = ReadToken();

                while (currentToken.Type == TokenType.Else)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.If)
                    {
                        currentToken = ReadToken();
                        if (currentToken.Type != TokenType.LParen) throw new SyntaxException("invalid syntax", currentToken.Line);
                        conditionExpression = Expression();
                        currentToken = ReadToken();
                        if (currentToken.Type != TokenType.RParen) throw new SyntaxException("invalid syntax", currentToken.Line);

                        statements = new List<IStatement>();

                        statement = Statement();

                        if (statement is StartBlockStatement)
                        {
                            do
                            {
                                statements.Add(statement);
                                statement = Statement();

                                if (!(statement is EndBlockStatement)) continue;

                                statements.Add(statement);
                                break;
                            } while (true);
                        }
                        else
                        {
                            statements.Add(statement);
                        }

                        conditionBlocks.Add(conditionExpression, statements);

                        currentToken = ReadToken();
                    }
                    else
                    {
                        _readerPosition--;

                        statements = new List<IStatement>();

                        statement = Statement();

                        if (statement is StartBlockStatement)
                        {
                            do
                            {
                                statements.Add(statement);
                                statement = Statement();

                                if (!(statement is EndBlockStatement)) continue;

                                statements.Add(statement);
                                break;
                            } while (true);
                        }
                        else
                        {
                            statements.Add(statement);
                        }

                        return new ConditionalStatement(this, conditionBlocks, statements);
                    }
                }

                _readerPosition--;

                return new ConditionalStatement(this, conditionBlocks, null);
            }

            if (currentToken.Type == TokenType.Loop)
            {
                var statements = new List<IStatement>();

                var statement = Statement();

                if (statement is StartBlockStatement)
                {
                    do
                    {
                        statements.Add(statement);
                        statement = Statement();

                        if (!(statement is EndBlockStatement)) continue;

                        statements.Add(statement);
                        break;
                    } while (true);
                }
                else
                {
                    statements.Add(statement);
                }

                return new WhileStatement(this, new BooleanExpression(new BooleanValue(true), currentToken.Line), statements);
            }

            if (currentToken.Type == TokenType.While)
            {
                currentToken = ReadToken();
                if (currentToken.Type != TokenType.LParen) throw new SyntaxException("invalid syntax", currentToken.Line);
                var conditionExpression = Expression();
                currentToken = ReadToken();
                if (currentToken.Type != TokenType.RParen) throw new SyntaxException("invalid syntax", currentToken.Line);

                var statements = new List<IStatement>();

                var statement = Statement();

                if (statement is StartBlockStatement)
                {
                    do
                    {
                        statements.Add(statement);
                        statement = Statement();

                        if (!(statement is EndBlockStatement)) continue;

                        statements.Add(statement);
                        break;
                    } while (true);
                }
                else
                {
                    statements.Add(statement);
                }

                return new WhileStatement(this, conditionExpression, statements);
            }

            if (currentToken.Type == TokenType.For)
            {
                currentToken = ReadToken();
                if (currentToken.Type != TokenType.LParen) throw new SyntaxException("invalid syntax", currentToken.Line);
                
                var preStatement = Statement();
                var conditionLoop = Expression();
                CheckSemicolon();
                var stepExpression = Expression();

                currentToken = ReadToken();
                if (currentToken.Type != TokenType.RParen) throw new SyntaxException("invalid syntax", currentToken.Line);

                var statements = new List<IStatement>();

                var statement = Statement();

                if (statement is StartBlockStatement)
                {
                    do
                    {
                        statements.Add(statement);
                        statement = Statement();

                        if (!(statement is EndBlockStatement)) continue;

                        statements.Add(statement);
                        break;
                    } while (true);
                }
                else
                {
                    statements.Add(statement);
                }

                return new ForStatement(this, preStatement, conditionLoop, stepExpression, statements);
            }

            if (currentToken.Type == TokenType.LBrace)
            {
                return new StartBlockStatement(this);
            }
            
            if (currentToken.Type == TokenType.RBrace)
            {
                return new EndBlockStatement(this);
            }
            
            if (currentToken.Type == TokenType.InitVariable)
            {
                var nameVariableToken = ReadToken();
                if (nameVariableToken.Type != TokenType.Word) throw new SyntaxException("invalid syntax", nameVariableToken.Line);

                var equalToken = ReadToken();

                if (equalToken.Type == TokenType.Equal)
                {
                    var expression = Expression();
                    var statementWithEqual = new InitVariableStatement(this, nameVariableToken.Value, expression, nameVariableToken.Line);
                    CheckSemicolon();
                    return statementWithEqual;
                }

                _readerPosition--;

                var statement = new InitVariableStatement(this, nameVariableToken.Value, new NullExpression(currentToken.Line), nameVariableToken.Line);
                CheckSemicolon();
                return statement;
            }
            
            if (currentToken.Type == TokenType.Print)
            {
                var statement = new PrintStatement(Expression());
                CheckSemicolon();
                return statement;
            }

            if (currentToken.Type == TokenType.Return)
            {
                currentToken = ReadToken();

                if (currentToken.Type == TokenType.Semicolon)
                {
                    return new ReturnStatement(new NullExpression(currentToken.Line));
                }

                _readerPosition--;

                var statement = new ReturnStatement(Expression());
                CheckSemicolon();
                return statement;
            }

            if (currentToken.Type == TokenType.Break)
            {
                CheckSemicolon();
                return new BreakStatement();
            }

            if (currentToken.Type == TokenType.Continue)
            {
                CheckSemicolon();
                return new ContinueStatement();
            }

            {
                _readerPosition--;
                var expression = Expression();
                currentToken = ReadToken();

                if (currentToken.Type == TokenType.Plus)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        var statement = new EqualStatement(this, expression, Expression(), TokenType.Plus, currentToken.Line);
                        CheckSemicolon();
                        return statement;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Minus)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        var statement = new EqualStatement(this, expression, Expression(), TokenType.Minus, currentToken.Line);
                        CheckSemicolon();
                        return statement;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Star)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        var statement = new EqualStatement(this, expression, Expression(), TokenType.Star, currentToken.Line);
                        CheckSemicolon();
                        return statement;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Slash)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        var statement = new EqualStatement(this, expression, Expression(), TokenType.Slash, currentToken.Line);
                        CheckSemicolon();
                        return statement;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Percent)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        var statement = new EqualStatement(this, expression, Expression(), TokenType.Percent, currentToken.Line);
                        CheckSemicolon();
                        return statement;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Slash)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Slash)
                    {
                        currentToken = ReadToken();
                        
                        if (currentToken.Type == TokenType.Equal)
                        {
                            var statement = new EqualStatement(this, expression, Expression(), TokenType.DoubleSlash, currentToken.Line);
                            CheckSemicolon();
                            return statement;
                        }
                        
                        _readerPosition -= 2;   
                    }
                    else
                    {
                        _readerPosition--;
                    }
                }
                
                if (currentToken.Type == TokenType.Equal)
                {
                    var statement = new EqualStatement(this, expression, Expression(), TokenType.Equal, currentToken.Line);
                    CheckSemicolon();
                    return statement;
                }

                _readerPosition--;
                CheckSemicolon();
                
                return new EmptyStatement(expression);
            }
        }

        private IExpression Expression()
        {            
            return ConditionalLogicExpression();
        }
        
        private IExpression ConditionalLogicExpression()
        {
            var expression = ConditionalEqualExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.Or)
                {
                    expression = new LogicalExpression(expression, ConditionalEqualExpression(), TokenType.Or, currentToken.Line);
                    continue;
                }

                if (currentToken.Type == TokenType.And)
                {
                    expression = new LogicalExpression(expression, ConditionalEqualExpression(), TokenType.And, currentToken.Line);
                    continue;
                }
                
                _readerPosition--;
                break;
            }
            
            return expression;
        }
        
        private IExpression ConditionalEqualExpression()
        {
            var expression = ConditionalComparisonExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.Equal)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        expression = new LogicalExpression(expression, ConditionalComparisonExpression(), TokenType.Equal, currentToken.Line);
                        continue;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Exclamation)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        expression = new LogicalExpression(expression, ConditionalComparisonExpression(), TokenType.NotEqual, currentToken.Line);
                        continue;
                    }

                    _readerPosition--;
                }
                
                _readerPosition--;
                break;
            }
            
            return expression;
        }

        private IExpression ConditionalComparisonExpression()
        {
            var expression = AddictiveExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.LessThan)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        expression = new LogicalExpression(expression, AddictiveExpression(), TokenType.LessThanOrEqual, currentToken.Line);
                    }
                    else
                    {
                        _readerPosition--;
                        expression = new LogicalExpression(expression, AddictiveExpression(), TokenType.LessThan, currentToken.Line);
                    }
                    
                    continue;
                }
                
                if (currentToken.Type == TokenType.MoreThan)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Equal)
                    {
                        expression = new LogicalExpression(expression, AddictiveExpression(), TokenType.MoreThanOrEqual, currentToken.Line);
                    }
                    else
                    {
                        _readerPosition--;
                        expression = new LogicalExpression(expression, AddictiveExpression(), TokenType.MoreThan, currentToken.Line);
                    }
                    
                    continue;
                }
                
                _readerPosition--;
                break;
            }
            
            return expression;
        }
        
        private IExpression AddictiveExpression()
        {
            var expression = MultiplicativeExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.Plus || currentToken.Type == TokenType.Minus)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type != TokenType.Equal)
                    {
                        _readerPosition--;
                        expression = new BinaryExpression(currentToken.Type, expression, MultiplicativeExpression(), currentToken.Line);
                        continue;   
                    }

                    _readerPosition--;
                }

                _readerPosition--;
                break;
            }
            
            return expression;
        }

        private IExpression MultiplicativeExpression()
        {
            var expression = CrementExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.Slash)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type == TokenType.Slash)
                    {
                        currentToken = ReadToken();

                        if (currentToken.Type != TokenType.Equal)
                        {
                            _readerPosition--;
                            expression = new BinaryExpression(TokenType.DoubleSlash, expression, CrementExpression(), currentToken.Line);
                            continue;   
                        }

                        _readerPosition--;
                    }

                    _readerPosition--;
                }
                
                if (currentToken.Type == TokenType.Star || currentToken.Type == TokenType.Slash || currentToken.Type == TokenType.Percent)
                {
                    currentToken = ReadToken();

                    if (currentToken.Type != TokenType.Equal)
                    {
                        _readerPosition--;
                        expression = new BinaryExpression(currentToken.Type, expression, CrementExpression(), currentToken.Line);
                        continue;   
                    }

                    _readerPosition--;
                }
                
                _readerPosition--;
                break;
            }
            
            return expression;
        }

        private IExpression CrementExpression()
        {
            var expression = GetterExpression();

            while (true)
            {
                var currentToken = ReadToken();

                if (currentToken.Type == TokenType.Plus)
                {
                    currentToken = ReadToken();
                    
                    if (currentToken.Type == TokenType.Plus) return new IncrementExpression(this, expression, true, currentToken.Line);
                    
                    _readerPosition--;
                }

                if (currentToken.Type == TokenType.Minus)
                {
                    currentToken = ReadToken();
                    
                    if (currentToken.Type == TokenType.Minus) return new DecrementExpression(this, expression, true, currentToken.Line);

                    _readerPosition--;
                }
                
                _readerPosition--;
                break;
            }
            
            return expression;
        }
        
        private IExpression GetterExpression()
        {
            var expression = UnaryExpression();
            
            while (true)
            {
                var currentToken = ReadToken();
                
                if (currentToken.Type == TokenType.LSquare)
                {
                    expression = new ListValueExpression(this, expression, UnaryExpression(), currentToken.Line);
                    currentToken = ReadToken();
                    if (currentToken.Type != TokenType.RSquare) throw new SyntaxException("invalid syntax", currentToken.Line);
                    continue;
                }

                _readerPosition--;
                break;
            }

            return expression;
        }
        
        private IExpression UnaryExpression()
        {
            var currentToken = ReadToken();

            if (currentToken.Type == TokenType.Minus)
            {
                currentToken = ReadToken();

                if (currentToken.Type == TokenType.Minus)
                {
                    return new DecrementExpression(this, Expression(), false, currentToken.Line);
                }
                
                return new UnaryExpression(UnaryExpression(), currentToken.Line); 
            }
            
            if (currentToken.Type == TokenType.Plus)
            {
                currentToken = ReadToken();

                if (currentToken.Type == TokenType.Plus)
                {
                    return new IncrementExpression(this, Expression(), false, currentToken.Line);
                }

                _readerPosition--;
            }
            
            _readerPosition--;
            return PrimaryExpression();
        }
        
        private IExpression PrimaryExpression()
        {
            var currentToken = ReadToken();

            if (currentToken.Type == TokenType.Not)
            {
                var expression = Expression();
                return new LogicalExpression(expression, expression, TokenType.Not, currentToken.Line);
            }
            
            if (currentToken.Type == TokenType.String)
            {
                return new StringExpression(new StringValue(currentToken.Value), currentToken.Line);
            }
            
            if (currentToken.Type == TokenType.LParen)
            {
                var expression = Expression();
                var parenToken = ReadToken();
                if (parenToken.Type != TokenType.RParen) throw new SyntaxException("invalid syntax", currentToken.Line);
                return expression;
            }
            
            if (currentToken.Type == TokenType.LSquare)
            {
                var indexes = new Dictionary<IExpression, IExpression>();
                var index = 0;
                
                currentToken = ReadToken();

                while (currentToken.Type != TokenType.RSquare)
                {
                    _readerPosition--;
                    indexes.Add(new NumberExpression(new NumberValue(index), currentToken.Line), Expression());
                    currentToken = ReadToken();
                    if (currentToken.Type != TokenType.Comma && currentToken.Type != TokenType.RSquare) throw new SyntaxException("invalid syntax", currentToken.Line);
                    if (currentToken.Type == TokenType.Comma) _readerPosition++;
                    index++;
                }
                
                return new ListExpression(indexes, currentToken.Line);
            }
            
            if (currentToken.Type == TokenType.Number)
            {
                return new NumberExpression(new NumberValue(currentToken.Value, currentToken.Line), currentToken.Line);
            }
            
            if (currentToken.Type == TokenType.Word)
            {
                var token = ReadToken();

                if (token.Type == TokenType.Plus)
                {
                    token = ReadToken();
                    
                    if (token.Type != TokenType.Plus)
                    {
                        _readerPosition -= 2;
                        return new VariableExpression(this, currentToken.Value, currentToken.Line);   
                    }
                    
                    return new IncrementExpression(this, new VariableExpression(this, currentToken.Value, token.Line), true, token.Line);   
                }
                
                if (token.Type == TokenType.Minus)
                {
                    token = ReadToken();
                    
                    if (token.Type != TokenType.Minus)
                    {
                        _readerPosition -= 2;
                        return new VariableExpression(this, currentToken.Value, currentToken.Line);   
                    }
                    
                    return new DecrementExpression(this, new VariableExpression(this, currentToken.Value, token.Line), true, token.Line);   
                }
                
                _readerPosition--;
                return new VariableExpression(this, currentToken.Value, currentToken.Line);   
            }

            if (currentToken.Type == TokenType.Null)
            {
                return new NullExpression(currentToken.Line);
            }

            if (currentToken.Type == TokenType.Boolean)
            {
                return new BooleanExpression(new BooleanValue(currentToken.Value == "true"), currentToken.Line);   
            }
            
            throw new SyntaxException("invalid syntax", currentToken.Line);
        }
        
        private Token ReadToken()
        {
            _readerPosition++;
            return _readerPosition - 1 >= _tokens.Count ? new Token(TokenType.EndStream, null, _tokens.Last().Line) : _tokens[_readerPosition - 1];   
        }
    }
}