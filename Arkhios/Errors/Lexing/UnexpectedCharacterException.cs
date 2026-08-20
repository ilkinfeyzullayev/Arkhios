using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class UnexpectedCharacterException : ArkhiosException
    {
        public UnexpectedCharacterException(
            char character,
            int line,
            int column)
            : base(
                $"Unexpected character '{character}' in line {line}, column {column}")
        {
        }
    }
}
