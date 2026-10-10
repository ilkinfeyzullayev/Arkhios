using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Semantic
{
    internal class Scope
    {
        public Scope? Parent { get; }
        public Dictionary<string, Symbol> Symbols { get; }

        public Scope(Scope? parent)
        {
            Parent = parent;
            Symbols = new();
        }
    }
}
