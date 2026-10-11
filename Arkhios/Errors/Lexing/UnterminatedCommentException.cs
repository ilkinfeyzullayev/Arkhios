using Arkhios.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class UnterminatedCommentException : ArkhiosException
    {
        public SourceSpan Span { get; }

        public UnterminatedCommentException(
            SourceSpan span,
            string source)
            : base(DiagnosticFormatter.Format(
                "Unterminated comment",
                span,
                source))
        {
            Span = span;
        }
    }
}
