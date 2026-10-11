using Arkhios.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class UnexpectedCommentTerminatorException
    : LexingException
    {
        public SourceSpan Span { get; }

        public UnexpectedCommentTerminatorException(
            SourceSpan span,
            string source)
            : base(DiagnosticFormatter.Format(
                "Unexpected comment terminator '*/'",
                span,
                source))
        {
            Span = span;
        }
    }
}
