using Arkhios.AST.Expressions.LiteralExpressions;
using Arkhios.AST.Statements;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using Expression = Arkhios.AST.Expressions.Expression;
using String = Arkhios.Lexer.Tokens.String;

namespace Arkhios.Parser
{
    internal class Parser
    {
        public List<Statement> AST { get; } = new();
        public readonly List<Token> TokenList;
        private int _position;

        private Token Current => TokenList[_position];
        private bool IsAtEnd => _position >= TokenList.Count;

        private static readonly HashSet<string> TypeList = new() { "int", "float", "complex", "string", "bool" };

        public Parser(List<Token> tokens)
        {
            TokenList = tokens;
            _position = 0;
        }

        public void Parse()
        {
            while (!IsAtEnd)
                ParseStatement();
        }

        private void ParseStatement()
        {
            switch (Current)
            {
                case Keyword { KeywordType: KeywordType.Var }:
                    ParseVariableDeclaration();
                    break;

                case Identifier identifier when TypeList.Contains(identifier.Value):
                    ParseVariableDeclaration();
                    break;

                default:
                    throw new Exception("Expected a statement.");
            }
        }

        public void ParseVariableDeclaration()
        {
            string? type = Current is Identifier identifier
                ? identifier.Value
                : null;

            Advance();

            string name = Current is Identifier identifier1
                ? identifier1.Value
                : throw new Exception("Expected an identifier.");

            Advance();

            ExpectSymbol(SymbolType.Assign);

            Expression expression = ParseLiteral(Current);

            ExpectSymbol(SymbolType.Semicolon);

            AST.Add(new VariableDeclaration(type, name, expression));
        }

        public LiteralExpression ParseLiteral(Token token)
        {

            switch (token)
            {
                case Number number:
                    Advance();
                    return new NumberLiteralExpression(number.Value);

                case String text:
                    Advance();
                    return new StringLiteralExpression(text.Value);

                case Keyword keyword:
                    {
                        var value = keyword.KeywordType switch
                        {
                            KeywordType.True => true,
                            KeywordType.False => false,
                            _ => throw new Exception("Expected a literal.")
                        };

                        Advance();
                        return new BooleanLiteralExpression(value);
                    }

                default:
                    throw new Exception("Expected a literal.");
            }
        }

        private void ExpectSymbol(SymbolType symbolType)
        {
            if (Current is Symbol symbol &&
                symbol.SymbolType == symbolType)
            {
                Advance();
                return;
            }

            throw new Exception($"Expected {symbolType}.");
        }

        private void Advance()
        {
            if (_position < TokenList.Count)
                _position++;
        }
    }
}
