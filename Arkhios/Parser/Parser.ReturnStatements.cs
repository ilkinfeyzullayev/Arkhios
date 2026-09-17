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
        private ReturnStatement ParseReturnStatement()
        {
            Advance();

            Expression expression = ParseExpression();

            ExpectSymbol(SymbolType.Semicolon);

            return new ReturnStatement(expression);
        }
    }
}
