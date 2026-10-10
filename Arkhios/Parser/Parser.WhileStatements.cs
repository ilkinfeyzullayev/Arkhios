using Arkhios.AST.Expressions;
using Arkhios.AST.Statements;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        private Statement ParseWhileStatement()
        {
            Advance();

            ExpectSymbol(SymbolType.LeftParen);
            Expression condition = ParseExpression();
            ExpectSymbol(SymbolType.RightParen);

            Statement body = ParseStatement();

            return new WhileStatement(condition, body);
        }
    }
}
