using Arkhios.Diagnostics;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Lexer.Tokens
{
    internal abstract record Token(TokenType Type);

    internal record Identifier(string Value, SourceSpan Span)
        : Token(TokenType.Identifier);

    internal record Number(string Value, SourceSpan Span)
        : Token(TokenType.Number);

    internal record Symbol(SymbolType SymbolType, SourceSpan Span)
        : Token(TokenType.Symbol);

    internal record Keyword(KeywordType KeywordType, SourceSpan Span)
        : Token(TokenType.Keyword);

    internal record String(string Value, SourceSpan Span)
        : Token(TokenType.String);

    internal record EndOfFile()
        : Token(TokenType.EndOfFile);
}
