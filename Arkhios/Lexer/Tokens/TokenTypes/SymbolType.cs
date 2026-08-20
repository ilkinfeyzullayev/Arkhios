using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Lexer.Tokens.TokenTypes
{
    internal enum SymbolType
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Power,

        Assign,
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,

        LeftParen,
        RightParen,
        LeftBracket,
        RightBracket,
        LeftBrace,
        RightBrace,

        Comma,

        Arrow
    }
}
