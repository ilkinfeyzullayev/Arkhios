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
        private IfStatement ParseIfStatement()
        {
            Advance();

            ExpectSymbol(SymbolType.LeftParen);

            Expression condition = ParseExpression();

            ExpectSymbol(SymbolType.RightParen);

            BlockStatement body = ParseBlockStatement();

            Statement? @else = null;

            if (Current is Keyword { KeywordType: KeywordType.Else })
            {
                Advance();

                if (Current is Keyword { KeywordType: KeywordType.If })
                {
                    @else = ParseIfStatement();
                }
                else
                {
                    @else = ParseBlockStatement();
                }
            }

            return new IfStatement(condition, body, @else);
        }
    }
}
