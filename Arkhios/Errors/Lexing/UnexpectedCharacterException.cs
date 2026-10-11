using Arkhios.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class UnexpectedCharacterException : LexingException
    {
        public char Character { get; }
        public SourceSpan Span { get; }

        public UnexpectedCharacterException(
            char character,
            SourceSpan span,
            string source)
            : base(DiagnosticFormatter.Format(
                $"Unexpected character '{character}'",
                span,
                source))
        {
            Character = character;
            Span = span;
        }
    }
}
