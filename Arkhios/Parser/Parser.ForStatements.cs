using Arkhios.AST.Expressions;
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
        private ForStatement ParseForStatement()
        {
            Advance(); // for
            ExpectSymbol(SymbolType.LeftParen);

            Statement? initializer = null;
            Expression? condition = null;
            Statement? iterator = null;

            // Initializer
            if (Current is not Symbol { SymbolType: SymbolType.Semicolon })
            {
                initializer = Current is Identifier identifier &&
                              TypeList.Contains(identifier.Value)
                    ? ParseVariableDeclaration()
                    : ParseAssignment();
            }
            else
            {
                Advance();
            }

            // Condition
            if (Current is not Symbol { SymbolType: SymbolType.Semicolon })
                condition = ParseExpression();

            ExpectSymbol(SymbolType.Semicolon);

            // Iterator
            if (Current is not Symbol { SymbolType: SymbolType.RightParen })
                iterator = ParseAssignment(expectSemicolon: false);

            ExpectSymbol(SymbolType.RightParen);

            Statement body = ParseStatement();

            return new ForStatement(initializer, condition, iterator, body);
        }
    }
}
