using Arkhios.Diagnostics;
using Arkhios.Errors;
using Arkhios.Errors.Lexing;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using String = Arkhios.Lexer.Tokens.String;

namespace Arkhios.Lexer
{
    internal class Lexer
    {
        public List<Token> TokenList = new();

        private readonly string _source;

        private int _currentIndex = 0;
        private int _lexemeStart = 0;

        private int _line = 1;
        private int _lineStart = 0;

        private char _currentChar => _source[_currentIndex];
        private SourceLocation CurrentLocation => new SourceLocation(_currentIndex, _line, _currentIndex - _lineStart + 1);

        private static readonly HashSet<char> _singleSymbols =
        [
            '+', '-', '*', '/', '^',
            '=', '<', '>', '!',
            '(', ')', '[', ']', '{', '}',
            ',', ';', '%'
        ];

        private static readonly HashSet<char> _helperSingleSymbols =
        [
            '&', '|'
        ];

        private static readonly HashSet<string> _doubleSymbols =
        [
            "==", "!=", "<=", ">=", "=>", "&&", "||", "++", "--", "+=", "-="
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
                else if (_currentChar == '/' &&
                         _currentIndex + 1 < _source.Length &&
                         (_source[_currentIndex + 1] == '/' ||
                          _source[_currentIndex + 1] == '*'))
                {
                    LexComment();
                }
                else if (_currentChar == '*' && _currentIndex + 1 < _source.Length && _source[_currentIndex + 1] == '/')
                {
                    SourceLocation start = CurrentLocation;
                    SourceLocation end = new(
                        start.Offset + 2,
                        start.Line,
                        start.Column + 2);

                    throw new UnexpectedCommentTerminatorException(
                        new SourceSpan(start, end),
                        _source);
                }
                else if (char.IsLetter(_currentChar) || _currentChar == '_')
                {
                    LexIdentifier();
                }
                else if (char.IsDigit(_currentChar))
                {
                    LexNumber();
                }
                else if (_singleSymbols.Contains(_currentChar) ||
                         _helperSingleSymbols.Contains(_currentChar))
                {
                    LexSymbol();
                }
                else if (_currentChar == '\'' || _currentChar == '"')
                {
                    LexString();
                }
                else
                {
                    UnexpectedCharacter();
                }
            }

            TokenList.Add(new EndOfFile());
        }

        private void LexComment()
        {
            if (_source[_currentIndex + 1] == '/')
            {
                _currentIndex += 2;

                while (_currentIndex < _source.Length &&
                       _currentChar != '\n')
                {
                    _currentIndex++;
                }

                return;
            }

            SourceLocation start = CurrentLocation;

            _currentIndex += 2;

            while (_currentIndex < _source.Length)
            {
                if (_currentChar == '*' &&
                    _currentIndex + 1 < _source.Length &&
                    _source[_currentIndex + 1] == '/')
                {
                    _currentIndex += 2;
                    return;
                }

                if (_currentChar == '\n')
                {
                    _line++;
                    _lineStart = _currentIndex + 1;
                }

                _currentIndex++;
            }

            SourceLocation end = new(
                start.Offset + 2,
                start.Line,
                start.Column + 2);

            throw new UnterminatedCommentException(
                new SourceSpan(start, end),
                _source);
        }

        private void LexIdentifier()
        {
            SourceLocation start = CurrentLocation;

            while (_currentIndex < _source.Length &&
                   (char.IsLetterOrDigit(_currentChar) ||
                    _currentChar == '_'))
            {
                _currentIndex++;
            }

            SourceLocation end = CurrentLocation;
            SourceSpan span = new(start, end);

            SendLexeme(
                _source.AsSpan(
                    _lexemeStart,
                    _currentIndex - _lexemeStart),
                span);
        }

        private void LexNumber()
        {
            SourceLocation start = CurrentLocation;

            while (_currentIndex < _source.Length &&
                   (char.IsLetterOrDigit(_currentChar) || _currentChar == '.'))
            {
                _currentIndex++;
            }

            SourceLocation end = CurrentLocation;
            SourceSpan span = new(start, end);

            var lexeme = _source.AsSpan(
                _lexemeStart,
                _currentIndex - _lexemeStart);

            if (lexeme.Count('.') > 1 ||
                ContainsLetter(lexeme) ||
                lexeme[0] == '.' ||
                lexeme[^1] == '.')
            {
                InvalidNumber(lexeme, span);
            }

            TokenList.Add(new Number(lexeme.ToString(), span));
        }

        private void LexSymbol()
        {
            SourceLocation start = CurrentLocation;

            if (_currentIndex + 1 < _source.Length)
            {
                var lexeme = _source[_currentIndex..(_currentIndex + 2)];

                if (_doubleSymbols.Contains(lexeme))
                {
                    _currentIndex += 2;

                    SourceLocation end = CurrentLocation;
                    SourceSpan sourceSpan = new(start, end);

                    SendLexeme(lexeme, sourceSpan);
                    return;
                }
            }

            if (_helperSingleSymbols.Contains(_currentChar))
            {
                UnexpectedCharacter();
            }

            var singleLexeme = _source.AsSpan(_currentIndex, 1);
            _currentIndex++;

            SourceLocation singleEnd = CurrentLocation;
            SourceSpan singleSpan = new(start, singleEnd);

            SendLexeme(singleLexeme, singleSpan);
        }

        private void LexString()
        {
            SourceLocation start = CurrentLocation;
            char quote = _currentChar;

            _currentIndex++;
            _lexemeStart = _currentIndex;

            while (_currentIndex < _source.Length &&
                   _currentChar != quote)
            {
                _currentIndex++;
            }

            if (_currentIndex == _source.Length)
            {
                SourceLocation end = new(
                    start.Offset + 1,
                    start.Line,
                    start.Column + 1);

                throw new UnterminatedStringException(
                    new SourceSpan(start, end),
                    _source);
            }

            string value = _source.AsSpan(
                _lexemeStart,
                _currentIndex - _lexemeStart
            ).ToString();

            _currentIndex++;

            SourceLocation tokenEnd = CurrentLocation;
            SourceSpan sourceSpan = new(start, tokenEnd);

            TokenList.Add(new String(value, sourceSpan));
        }

        private void InvalidNumber(ReadOnlySpan<char> lexeme, SourceSpan span)
        {
            throw new InvalidNumberException(
                lexeme.ToString(),
                span,
                _source);
        }

        private void UnexpectedCharacter()
        {
            SourceLocation start = CurrentLocation;

            SourceLocation end = new(
                start.Offset + 1,
                start.Line,
                start.Column + 1);

            SourceSpan span = new(start, end);

            throw new UnexpectedCharacterException(
                _currentChar,
                span,
                _source);
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

        private void SendLexeme(ReadOnlySpan<char> lexeme, SourceSpan sourceSpan)
        {
            TokenList.Add((lexeme) switch
            {
                // Keywords
                "var" => new Keyword(KeywordType.Var, sourceSpan),
                "void" => new Keyword(KeywordType.Void, sourceSpan),
                "if" => new Keyword(KeywordType.If, sourceSpan),
                "else" => new Keyword(KeywordType.Else, sourceSpan),
                "for" => new Keyword(KeywordType.For, sourceSpan),
                "while" => new Keyword(KeywordType.While, sourceSpan),
                "return" => new Keyword(KeywordType.Return, sourceSpan),
                "true" => new Keyword(KeywordType.True, sourceSpan),
                "false" => new Keyword(KeywordType.False, sourceSpan),

                // Arithmetic
                "+" => new Symbol(SymbolType.Plus, sourceSpan),
                "-" => new Symbol(SymbolType.Minus, sourceSpan),
                "*" => new Symbol(SymbolType.Multiply, sourceSpan),
                "/" => new Symbol(SymbolType.Divide, sourceSpan),
                "^" => new Symbol(SymbolType.Power, sourceSpan),

                // Assignment / comparison
                "=" => new Symbol(SymbolType.Assign, sourceSpan),
                "==" => new Symbol(SymbolType.Equal, sourceSpan),
                "!=" => new Symbol(SymbolType.NotEqual, sourceSpan),
                "<" => new Symbol(SymbolType.LessThan, sourceSpan),
                ">" => new Symbol(SymbolType.GreaterThan, sourceSpan),
                "<=" => new Symbol(SymbolType.LessThanOrEqual, sourceSpan),
                ">=" => new Symbol(SymbolType.GreaterThanOrEqual, sourceSpan),

                // Logical
                "&&" => new Symbol(SymbolType.And, sourceSpan),
                "||" => new Symbol(SymbolType.Or, sourceSpan),
                "!" => new Symbol(SymbolType.Not, sourceSpan),

                // Grouping
                "(" => new Symbol(SymbolType.LeftParen, sourceSpan),
                ")" => new Symbol(SymbolType.RightParen, sourceSpan),
                "[" => new Symbol(SymbolType.LeftBracket, sourceSpan),
                "]" => new Symbol(SymbolType.RightBracket, sourceSpan),
                "{" => new Symbol(SymbolType.LeftBrace, sourceSpan),
                "}" => new Symbol(SymbolType.RightBrace, sourceSpan),

                // Other
                "," => new Symbol(SymbolType.Comma, sourceSpan),
                ";" => new Symbol(SymbolType.Semicolon, sourceSpan),
                "=>" => new Symbol(SymbolType.Arrow, sourceSpan),
                "++" => new Symbol(SymbolType.Increment, sourceSpan),
                "--" => new Symbol(SymbolType.Decrement, sourceSpan),
                "%" => new Symbol(SymbolType.Modulo, sourceSpan),
                "+=" => new Symbol(SymbolType.PlusAssign, sourceSpan),
                "-=" => new Symbol(SymbolType.MinusAssign, sourceSpan),

                _ => new Identifier(lexeme.ToString(), sourceSpan)
            });
        }
    }
}