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
            Expression left = ParsePrimary();

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
            return ParseComparison();
        }
    }
}