using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal class UnterminatedCommentException : ArkhiosException
    {
        public UnterminatedCommentException(int line, int column)
            : base($"Unterminated comment at line {line}, column {column}.")
        {
        }
    }
}
