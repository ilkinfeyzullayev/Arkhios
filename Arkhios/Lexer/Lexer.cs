using Arkhios.Errors;
using Arkhios.Errors.Lexing;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;
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

        private static readonly HashSet<char> _singleSymbols = [ '+', '-', '*', '/', '^', '=', '<', '>', '!', '(', ')', '[', ']', '{', '}', ',' ];

        private static readonly HashSet<string> _doubleSymbols = [ "==", "!=", "<=",">=","=>" ];

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
                } else if (char.IsLetter(_currentChar))
                {
                    LexIdentifier();
                } else if (char.IsDigit(_currentChar))
                {
                    LexNumber();
                } else if (_singleSymbols.Contains(_currentChar))
                {
                    LexSymbol();
                } else if (_currentChar == ';')
                {
                    _tokenList.Add(new Semicolon());
                    _currentIndex++;
                } else
                {
                    UnexpectedCharacter();
                }
            }
        }
        private void LexIdentifier()
        {

            while (_currentIndex < _source.Length && char.IsLetterOrDigit(_currentChar))
            {
                _currentIndex++;
            }

            SendLexeme(_source.AsSpan(_lexemeStart, _currentIndex - _lexemeStart));
        }
        
        private void LexNumber()
        {

            while (_currentIndex < _source.Length && (char.IsLetterOrDigit(_currentChar) || _currentChar == '.'))
            {
                _currentIndex++;
            }

            var lexeme = _source.AsSpan(_lexemeStart, _currentIndex - _lexemeStart);

            if (lexeme.Count('.') > 1 || ContainsLetter(lexeme) || lexeme[0] == '.' || lexeme[^1] == '.')
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
                "var" => new Type(TypeType.Var),
                "int" => new Type(TypeType.Int),
                "float" => new Type(TypeType.Float),
                "BigInt" => new Type(TypeType.BigInt),
                "BigFloat" => new Type(TypeType.BigFloat),
                "BigNum" => new Type(TypeType.BigNum),
                "complex" => new Type(TypeType.Complex),
                "list" => new Type(TypeType.List),
                "vector" => new Type(TypeType.Vector),

                "+" => new Symbol(SymbolType.Plus),
                "-" => new Symbol(SymbolType.Minus),
                "*" => new Symbol(SymbolType.Multiply),
                "/" => new Symbol(SymbolType.Divide),
                "^" => new Symbol(SymbolType.Power),

                "=" => new Symbol(SymbolType.Assign),
                "==" => new Symbol(SymbolType.Equal),
                "!=" => new Symbol(SymbolType.NotEqual),
                "<" => new Symbol(SymbolType.LessThan),
                ">" => new Symbol(SymbolType.GreaterThan),
                "<=" => new Symbol(SymbolType.LessThanOrEqual),
                ">=" => new Symbol(SymbolType.GreaterThanOrEqual),

                "(" => new Symbol(SymbolType.LeftParen),
                ")" => new Symbol(SymbolType.RightParen),
                "[" => new Symbol(SymbolType.LeftBracket),
                "]" => new Symbol(SymbolType.RightBracket),
                "{" => new Symbol(SymbolType.LeftBrace),
                "}" => new Symbol(SymbolType.RightBrace),

                "," => new Symbol(SymbolType.Comma),

                "=>" => new Symbol(SymbolType.Arrow),

                _ => new Identifier(lexeme.ToString())
            });
        }
    }
}
