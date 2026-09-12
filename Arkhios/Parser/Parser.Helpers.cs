using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        private void ExpectSymbol(SymbolType symbolType)
        {
            if (Current is Symbol symbol &&
                symbol.SymbolType == symbolType)
            {
                Advance();
                return;
            }

            throw new Exception($"Expected {symbolType}.");
        }

        private void Advance()
        {
            if (_position < TokenList.Count)
                _position++;
        }
    }
}
