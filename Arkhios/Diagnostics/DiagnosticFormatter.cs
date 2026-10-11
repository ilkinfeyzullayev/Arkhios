using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Diagnostics
{
    internal static class DiagnosticFormatter
    {
        public static string Format(
            string message,
            SourceSpan span,
            string source)
        {
            // Find the start of the source line.
            int lineStart = span.Start.Offset;

            while (lineStart > 0 && source[lineStart - 1] != '\n')
            {
                lineStart--;
            }

            // Find the end of the source line.
            int lineEnd = span.Start.Offset;

            while (lineEnd < source.Length && source[lineEnd] != '\n')
            {
                lineEnd++;
            }

            string sourceLine = source[lineStart..lineEnd];

            // Calculate the caret position and underline length.
            int column = span.Start.Offset - lineStart;
            int underlineLength = Math.Max(
                1,
                span.End.Offset - span.Start.Offset);

            // Align the source line and diagnostic marker.
            string lineNumber = span.Start.Line.ToString();
            string gutter = new string(' ', lineNumber.Length);

            string underline =
                new string(' ', column) +
                new string('^', underlineLength);

            return
                $"{message}{Environment.NewLine}" +
                $" --> line {span.Start.Line}, column {span.Start.Column}{Environment.NewLine}" +
                $"{gutter} |{Environment.NewLine}" +
                $"{lineNumber} | {sourceLine}{Environment.NewLine}" +
                $"{gutter} | {underline}";
        }
    }
}
