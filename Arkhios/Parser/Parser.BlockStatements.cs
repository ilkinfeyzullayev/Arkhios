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
        private BlockStatement ParseBlockStatement()
        {
            ExpectSymbol(SymbolType.LeftBrace);

            List<Statement> statements = new();

            while (Current is not Symbol { SymbolType: SymbolType.RightBrace })
            {
                statements.Add(ParseStatement());
            }

            ExpectSymbol(SymbolType.RightBrace);

            return new BlockStatement(statements);
        }
    }
}
