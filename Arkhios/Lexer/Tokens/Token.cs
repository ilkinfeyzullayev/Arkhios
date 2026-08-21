using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Lexer.Tokens
{
    internal abstract record Token(TokenType Type);

    internal record Identifier(string Value) : Token(TokenType.Identifier);
    internal record Type(TypeType TypeType) : Token(TokenType.Type);
    internal record Number(string Value) : Token(TokenType.Number);
    internal record Symbol(SymbolType SymbolType) : Token(TokenType.Symbol);
    internal record Keyword(KeywordType KeywordType) : Token(TokenType.Keyword);
}
