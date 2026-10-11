using Arkhios.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class InvalidNumberException : LexingException
    {
        public string Number { get; }
        public SourceSpan Span { get; }

        public InvalidNumberException(
            string number,
            SourceSpan span,
            string source)
            : base(DiagnosticFormatter.Format(
                $"Invalid number '{number}'",
                span,
                source))
        {
            Number = number;
            Span = span;
        }
    }
}
