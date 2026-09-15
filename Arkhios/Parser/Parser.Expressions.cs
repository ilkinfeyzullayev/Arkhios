using Arkhios.AST.Expressions.IdentifierExpressions;
using Arkhios.AST.Expressions.LiteralExpressions;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;
using String = Arkhios.Lexer.Tokens.String;
using Expression = Arkhios.AST.Expressions.Expression;
using UnaryExpression = Arkhios.AST.Expressions.UnaryExpressions.UnaryExpression;
using BinaryExpression = Arkhios.AST.Expressions.BinaryExpressions.BinaryExpression;
using Arkhios.AST.Expressions.CallExpressions;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        private Expression ParsePrimary()
        {
            switch (Current)
            {
                case Number number:
                    Advance();
                    return new NumberLiteralExpression(number.Value);

                case String text:
                    Advance();
                    return new StringLiteralExpression(text.Value);

                case Keyword { KeywordType: KeywordType.True }:
                    Advance();
                    return new BooleanLiteralExpression(true);

                case Keyword { KeywordType: KeywordType.False }:
                    Advance();
                    return new BooleanLiteralExpression(false);

                case Identifier identifier:
                    Advance();
                    return new IdentifierExpression(identifier.Value);

                case Symbol { SymbolType: SymbolType.LeftParen }:
                    Advance();
                    return ParseParenthesizedExpression();

                default:
                    throw new Exception("Expected an expression.");
            }
        }

        private Expression ParseParenthesizedExpression()
        {
            Expression expression = ParseExpression();

            ExpectSymbol(SymbolType.RightParen);

            return expression;
        }

        private Expression ParseUnary()
        {
            switch (Current)
            {
                case Symbol { SymbolType: SymbolType.Minus }:
                    Advance();
                    return new UnaryExpression(
                        SymbolType.Minus,
                        ParseUnary());

                case Symbol { SymbolType: SymbolType.Plus }:
                    Advance();
                    return new UnaryExpression(
                        SymbolType.Plus,
                        ParseUnary());

                case Symbol { SymbolType: SymbolType.Not }:
                    Advance();
                    return new UnaryExpression(
                        SymbolType.Not,
                        ParseUnary());

                default:
                    return ParsePower();
            }
        }

        private Expression ParsePostfix()
        {
            if (Current is Identifier functionName)
            {
                if (Next is Symbol { SymbolType: SymbolType.LeftParen })
                {
                    Advance();
                    List<Expression> arguments = new();

                    if (Next is not Symbol { SymbolType: SymbolType.RightParen })
                    {
                        Advance();
                        arguments = ParseArguments();
                    }
                    else
                    {
                        Advance();
                    }

                    ExpectSymbol(SymbolType.RightParen);

                    return new CallExpression(functionName.Value, arguments);
                }

                return ParsePrimary();
            }

            return ParsePrimary();
        }

        private List<Expression> ParseArguments()
        {
            List<Expression> arguments = new();

            while (Current is not Symbol { SymbolType: SymbolType.RightParen })
            {
                arguments.Add(ParseExpression());

                if (Current is Symbol { SymbolType: SymbolType.Comma })
                {
                    Advance();
                }
                else
                {
                    break;
                }
            }

            return arguments;
        }

        private Expression ParseOr()
        {
            Expression left = ParseAnd();

            while (Current is Symbol { SymbolType: SymbolType.Or })
            {
                Advance();

                Expression right = ParseAnd();

                left = new BinaryExpression(
                    left,
                    SymbolType.Or,
                    right);
            }

            return left;
        }

        private Expression ParseAnd()
        {
            Expression left = ParseComparison();

            while (Current is Symbol { SymbolType: SymbolType.And })
            {
                Advance();

                Expression right = ParseComparison();

                left = new BinaryExpression(
                    left,
                    SymbolType.And,
                    right);
            }

            return left;
        }

        private Expression ParseComparison()
        {
            Expression left = ParseAddition();

            if (Current is Symbol { SymbolType: SymbolType.Equal or SymbolType.NotEqual or SymbolType.GreaterThan or SymbolType.LessThan or SymbolType.GreaterThanOrEqual or SymbolType.LessThanOrEqual })
            {
                SymbolType @operator = ((Symbol)Current).SymbolType;
                Advance();

                Expression right = ParseAddition();

                return new BinaryExpression(
                    left,
                    @operator,
                    right);
            }

            return left;
        }

        private Expression ParsePower()
        {
            Expression left = ParsePostfix();

            if (Current is Symbol { SymbolType: SymbolType.Power })
            {
                Advance();

                Expression right = ParseUnary();

                return new BinaryExpression(
                    left,
                    SymbolType.Power,
                    right);
            }

            return left;
        }

        private Expression ParseMultiplication()
        {
            Expression left = ParseUnary();

            while (Current is Symbol
                {
                    SymbolType: SymbolType.Multiply
                    or SymbolType.Divide
                })
            {
                SymbolType @operator = ((Symbol)Current).SymbolType;
                Advance();

                Expression right = ParseUnary();

                left = new BinaryExpression(
                    left,
                    @operator,
                    right);
            }

            return left;
        }

        private Expression ParseAddition()
        {
            Expression left = ParseMultiplication();

            while (Current is Symbol
                {
                    SymbolType: SymbolType.Plus
                    or SymbolType.Minus
                })
            {
                SymbolType @operator = ((Symbol)Current).SymbolType;
                Advance();

                Expression right = ParseMultiplication();

                left = new BinaryExpression(
                    left,
                    @operator,
                    right);
            }

            return left;
        }

        private Expression ParseExpression()
        {
            return ParseOr();
        }
    }
}