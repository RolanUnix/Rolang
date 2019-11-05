using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;
using System.Xml;
using Rolang.Exceptions.Internal;

namespace Rolang
{
    public class Lexer
    {
        private readonly string _code;
        private int _position;
        private readonly Dictionary<char, TokenType> _operators;
        private int _codeLine;
        
        public Lexer(string code)
        {
            _code = code;
            _position = 0;
            _operators = new Dictionary<char, TokenType>
            {
                {'+', TokenType.Plus},
                {'-', TokenType.Minus},
                {'*', TokenType.Star},
                {'/', TokenType.Slash},
                {'{', TokenType.LBrace},
                {'}', TokenType.RBrace},
                {'(', TokenType.LParen},
                {')', TokenType.RParen},
                {'[', TokenType.LSquare},
                {']', TokenType.RSquare},
                {'=', TokenType.Equal},
                {'!', TokenType.Exclamation},
                {',', TokenType.Comma},
                {'%', TokenType.Percent},
                {';', TokenType.Semicolon},
                {'<', TokenType.LessThan},
                {'>', TokenType.MoreThan}
            };
            _codeLine = 1;
        }

        public IEnumerable<Token> Tokenize()
        {
            var tokens = new List<Token>();

            var currentChar = ReadByte();

            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
            while (currentChar != -1)
            {
                if (char.IsDigit((char)currentChar) || currentChar == '.')
                {
                    _position--;
                    tokens.Add(TokenizeNumber());
                }
                else if (currentChar == '"')
                {
                    tokens.Add(TokenizeString());
                }
                else if (_operators.ContainsKey((char)currentChar))
                {
                    if (_operators[(char) currentChar] == TokenType.Slash)
                    {
                        var token = ReadByte();

                        if (_operators.ContainsKey((char) token))
                        {
                            if (_operators[(char) token] == TokenType.Slash)
                            {
                                token = ReadByte();

                                if (_operators.ContainsKey((char) token))
                                {
                                    if (_operators[(char) token] == TokenType.Slash)
                                    {
                                        while (currentChar != '\n' && currentChar != -1)
                                        {
                                            currentChar = ReadByte();
                                        }

                                        continue;
                                    }
                                }

                                _position -= 2;
                                tokens.Add(new Token(_operators[(char)currentChar], null, _codeLine));
                            }
                            else
                            {
                                _position--;
                                tokens.Add(new Token(_operators[(char)currentChar], null, _codeLine));
                            }
                        }
                        else
                        {
                            _position--;
                            tokens.Add(new Token(_operators[(char)currentChar], null, _codeLine));
                        }
                    }
                    else
                    {
                        tokens.Add(new Token(_operators[(char)currentChar], null, _codeLine));   
                    }
                }
                else if (char.IsLetter((char) currentChar) || currentChar == '_')
                {
                    _position--;
                    tokens.Add(TokenizeWord());
                }
                else if (currentChar == '\n')
                {
                    _codeLine++;
                }
                
                currentChar = ReadByte();
            }
            
            return tokens;
        }

        private Token TokenizeString()
        {
            var stringBuilder = new StringBuilder();
            var currentChar = ReadByte();

            while (currentChar != '"' && currentChar != -1)
            {
                stringBuilder.Append((char)currentChar);
                currentChar = ReadByte();
            }
            
            return new Token(TokenType.String, stringBuilder.ToString(), _codeLine);
        }
        
        private Token TokenizeNumber()
        {
            var numberBuilder = new StringBuilder();
            var currentChar = ReadByte();

            while ((char.IsDigit((char)currentChar) || currentChar == '.') && currentChar != -1)
            {
                numberBuilder.Append((char)currentChar);
                currentChar = ReadByte();
            }

            if (currentChar != -1) _position--;

            return new Token(TokenType.Number, numberBuilder.ToString(), _codeLine);
        }

        private Token TokenizeWord()
        {
            var wordBuilder = new StringBuilder();
            var currentChar = ReadByte();

            while (char.IsLetter((char)currentChar) || currentChar == '_')
            {
                wordBuilder.Append((char)currentChar);
                currentChar = ReadByte();
                if (currentChar == -1) break;
            }

            if (currentChar != -1) _position--;

            var word = wordBuilder.ToString();

            switch (word)
            {
                case "null":
                    return new Token(TokenType.Null, null, _codeLine);
                case "not":
                    return new Token(TokenType.Not, null, _codeLine);
                case "and":
                    return new Token(TokenType.And, null, _codeLine);
                case "or":
                    return new Token(TokenType.Or, null, _codeLine);
                case "true":
                    return new Token(TokenType.Boolean, "true", _codeLine);
                case "false":
                    return new Token(TokenType.Boolean, "false", _codeLine);
                case "val":
                    return new Token(TokenType.InitVariable, null, _codeLine);
                case "return":
                    return new Token(TokenType.Return, null, _codeLine);
                case "print":
                    return new Token(TokenType.Print, null, _codeLine);
                case "if":
                    return new Token(TokenType.If, null, _codeLine);
                case "else":
                    return new Token(TokenType.Else, null, _codeLine);
                case "while":
                    return new Token(TokenType.While, null, _codeLine);
                case "break":
                    return new Token(TokenType.Break, null, _codeLine);
                case "continue":
                    return new Token(TokenType.Continue, null, _codeLine);
                case "loop":
                    return new Token(TokenType.Loop, null, _codeLine);
                case "for":
                    return new Token(TokenType.For, null, _codeLine);
                case "fun":
                    return new Token(TokenType.Function, null, _codeLine);
            }
            
            return new Token(TokenType.Word, word, _codeLine);
        }

        private int ReadByte() => _position < _code.Length ? _code[_position++] : -1;
    }
}