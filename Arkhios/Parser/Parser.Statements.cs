using Arkhios.AST.Statements;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Arkhios.Parser
{
    internal partial class Parser
    {

        private Statement ParseStatement()
        {
            return Current switch
            {
                Keyword { KeywordType: KeywordType.Var } => ParseVariableDeclaration(),
                Keyword { KeywordType: KeywordType.Void } => ParseFunctionDeclaration(),
                Identifier identifier when TypeList.Contains(identifier.Value) => DispatchDeclaration(),
                Keyword { KeywordType: KeywordType.Return } => ParseReturnStatement(),
                Symbol { SymbolType: SymbolType.LeftBrace } => ParseBlockStatement(),
                _ => throw new Exception("Expected a statement.")
            };
        }
    }
}
