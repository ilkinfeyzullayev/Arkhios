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
        private VariableDeclaration ParseVariableDeclaration()
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

            Expression expression = ParseExpression();

            ExpectSymbol(SymbolType.Semicolon);

            return new VariableDeclaration(type, name, expression);
        }
    }
}
