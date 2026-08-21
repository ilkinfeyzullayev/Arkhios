using Arkhios.Errors;
using Arkhios.Errors.Lexing;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using Type = Arkhios.Lexer.Tokens.Type;

namespace Arkhios.Lexer
{
    internal class Lexer
    {
        private List<Token> _tokenList = new();
        public IReadOnlyList<Token> Tokens => _tokenList;

        private readonly string _source;

        private int _currentIndex = 0;
        private int _lexemeStart = 0;

        private int _line = 1;
        private int _lineStart = 0;

        private char _currentChar => _source[_currentIndex];

        private static readonly HashSet<char> _singleSymbols =
        [
            '+', '-', '*', '/', '^',
            '=', '<', '>', '!',
            '(', ')', '[', ']', '{', '}',
            ',', ';'
        ];

        private static readonly HashSet<char> _helperSingleSymbols =
        [
            '&', '|'
        ];

        private static readonly HashSet<string> _doubleSymbols =
        [
            "==", "!=", "<=", ">=", "=>", "&&", "||"
        ];

        public Lexer(string source)
        {
            _source = source.Replace("\r\n", "\n").Replace('\r', '\n');
        }

        public void Lex()
        {
            while (_currentIndex < _source.Length)
            {
                _lexemeStart = _currentIndex;

                if (char.IsWhiteSpace(_currentChar))
                {
                    if (_currentChar == '\n')
                    {
                        _line++;
                        _lineStart = _currentIndex + 1;
                    }

                    _currentIndex++;
                }
                else if (char.IsLetter(_currentChar))
                {
                    LexIdentifier();
                }
                else if (char.IsDigit(_currentChar))
                {
                    LexNumber();
                }
                else if (_singleSymbols.Contains(_currentChar) || _helperSingleSymbols.Contains(_currentChar))
                {
                    LexSymbol();
                }
                else
                {
                    UnexpectedCharacter();
                }
            }
        }

        private void LexIdentifier()
        {
            while (_currentIndex < _source.Length &&
                   char.IsLetterOrDigit(_currentChar))
            {
                _currentIndex++;
            }

            SendLexeme(
                _source.AsSpan(
                    _lexemeStart,
                    _currentIndex - _lexemeStart));
        }

        private void LexNumber()
        {
            while (_currentIndex < _source.Length &&
                   (char.IsLetterOrDigit(_currentChar) || _currentChar == '.'))
            {
                _currentIndex++;
            }

            var lexeme = _source.AsSpan(
                _lexemeStart,
                _currentIndex - _lexemeStart);

            if (lexeme.Count('.') > 1 ||
                ContainsLetter(lexeme) ||
                lexeme[0] == '.' ||
                lexeme[^1] == '.')
            {
                InvalidNumber(lexeme);
            }

            _tokenList.Add(new Number(lexeme.ToString()));
        }

        private void LexSymbol()
        {
            if (_currentIndex + 1 < _source.Length)
            {
                var lexeme = _source[_currentIndex..(_currentIndex + 2)];

                if (_doubleSymbols.Contains(lexeme))
                {
                    SendLexeme(lexeme);
                    _currentIndex += 2;
                    return;
                }
            }

            if (_helperSingleSymbols.Contains(_currentChar))
            {
                UnexpectedCharacter();
            }

            SendLexeme(_source.AsSpan(_currentIndex, 1));
            _currentIndex++;
        }

        private void InvalidNumber(ReadOnlySpan<char> lexeme)
        {
            throw new InvalidNumberException(
                lexeme.ToString(),
                _line,
                _lexemeStart - _lineStart + 1);
        }

        private void UnexpectedCharacter()
        {
            throw new UnexpectedCharacterException(
                _currentChar,
                _line,
                _currentIndex - _lineStart + 1);
        }

        private bool ContainsLetter(ReadOnlySpan<char> lexeme)
        {
            foreach (char ch in lexeme)
            {
                if (char.IsLetter(ch))
                    return true;
            }

            return false;
        }

        private void SendLexeme(ReadOnlySpan<char> lexeme)
        {
            _tokenList.Add((lexeme) switch
            {
                // Keywords
                "var" => new Keyword(KeywordType.Var),
                "if" => new Keyword(KeywordType.If),
                "else" => new Keyword(KeywordType.Else),
                "for" => new Keyword(KeywordType.For),
                "while" => new Keyword(KeywordType.While),
                "func" => new Keyword(KeywordType.Function),
                "return" => new Keyword(KeywordType.Return),
                "true" => new Keyword(KeywordType.True),
                "false" => new Keyword(KeywordType.False),

                // Types
                "int" => new Type(TypeType.Int),
                "float" => new Type(TypeType.Float),
                "BigInt" => new Type(TypeType.BigInt),
                "BigFloat" => new Type(TypeType.BigFloat),
                "BigNum" => new Type(TypeType.BigNum),
                "complex" => new Type(TypeType.Complex),
                "bool" => new Type(TypeType.Boolean),
                "list" => new Type(TypeType.List),
                "vector" => new Type(TypeType.Vector),

                // Arithmetic
                "+" => new Symbol(SymbolType.Plus),
                "-" => new Symbol(SymbolType.Minus),
                "*" => new Symbol(SymbolType.Multiply),
                "/" => new Symbol(SymbolType.Divide),
                "^" => new Symbol(SymbolType.Power),

                // Assignment / comparison
                "=" => new Symbol(SymbolType.Assign),
                "==" => new Symbol(SymbolType.Equal),
                "!=" => new Symbol(SymbolType.NotEqual),
                "<" => new Symbol(SymbolType.LessThan),
                ">" => new Symbol(SymbolType.GreaterThan),
                "<=" => new Symbol(SymbolType.LessThanOrEqual),
                ">=" => new Symbol(SymbolType.GreaterThanOrEqual),

                // Logical
                "&&" => new Symbol(SymbolType.And),
                "||" => new Symbol(SymbolType.Or),
                "!" => new Symbol(SymbolType.Not),

                // Grouping
                "(" => new Symbol(SymbolType.LeftParen),
                ")" => new Symbol(SymbolType.RightParen),
                "[" => new Symbol(SymbolType.LeftBracket),
                "]" => new Symbol(SymbolType.RightBracket),
                "{" => new Symbol(SymbolType.LeftBrace),
                "}" => new Symbol(SymbolType.RightBrace),

                // Other
                "," => new Symbol(SymbolType.Comma),
                ";" => new Symbol(SymbolType.Semicolon),
                "=>" => new Symbol(SymbolType.Arrow),

                _ => new Identifier(lexeme.ToString())
            });
        }
    }
}