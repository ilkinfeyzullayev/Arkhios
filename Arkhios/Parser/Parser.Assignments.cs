using Arkhios.AST.Expressions;
using Arkhios.AST.Expressions.LiteralExpressions;
using Arkhios.AST.Statements;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        private Statement ParseAssignment(bool expectSemicolon = true)
        {
            string name = ((Identifier)Current).Value;

            if (Next is Symbol { SymbolType: SymbolType.Increment })
            {
                Advance();
                Advance();

                var value = new BinaryExpression(
                    new IdentifierExpression(name),
                    SymbolType.Plus,
                    new NumberLiteralExpression("1"));

                if (expectSemicolon)
                    ExpectSymbol(SymbolType.Semicolon);

                return new AssignmentStatement(name, value);
            }
            else if (Next is Symbol { SymbolType: SymbolType.Decrement })
            {
                Advance();
                Advance();

                var value = new BinaryExpression(
                    new IdentifierExpression(name),
                    SymbolType.Minus,
                    new NumberLiteralExpression("1"));

                if (expectSemicolon)
                    ExpectSymbol(SymbolType.Semicolon);

                return new AssignmentStatement(name, value);
            }

            Advance();
            ExpectSymbol(SymbolType.Assign);

            Expression expression = ParseExpression();

            if (expectSemicolon)
                ExpectSymbol(SymbolType.Semicolon);

            return new AssignmentStatement(name, expression);
        }
    }
}
