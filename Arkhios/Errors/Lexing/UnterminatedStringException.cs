using Arkhios.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class UnterminatedStringException : LexingException
    {
        public SourceSpan Span { get; }

        public UnterminatedStringException(
            SourceSpan span,
            string source)
            : base(DiagnosticFormatter.Format(
                "Unterminated string literal",
                span,
                source))
        {
            Span = span;
        }
    }
}
