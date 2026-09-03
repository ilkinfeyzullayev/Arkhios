using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors.Lexing
{
    internal class LexingException : ArkhiosException
    {
        public LexingException(string message)
        : base($"LexerError: {message}")
        {
        }
    }
}
