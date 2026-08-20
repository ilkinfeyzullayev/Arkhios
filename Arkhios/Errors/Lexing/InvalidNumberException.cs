using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal sealed class InvalidNumberException : ArkhiosException
    {
        public InvalidNumberException(
            string number,
            int line,
            int column)
            : base(
                $"Invalid number '{number}' in line {line}, column {column}")
        {
        }
    }
}
