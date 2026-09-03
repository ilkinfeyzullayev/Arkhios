using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal class UnterminatedStringException : LexingException
    {
        public UnterminatedStringException(
            int line,
            int column)
            : base($"Unterminated string literal at line {line}, column {column}.")
        {
        }
    }
}
