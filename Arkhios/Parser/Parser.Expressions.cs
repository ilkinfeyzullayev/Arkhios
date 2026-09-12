using Arkhios.AST.Expressions.IdentifierExpressions;
using Arkhios.AST.Expressions.LiteralExpressions;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;
using String = Arkhios.Lexer.Tokens.String;
using Expression = Arkhios.AST.Expressions.Expression;

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

        private Expression ParseExpression()
        {
            return ParsePrimary();
        }
    }
}
