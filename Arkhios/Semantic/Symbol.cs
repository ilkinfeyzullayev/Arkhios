using Arkhios.AST.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Semantic
{
    internal class Symbol
    {
        public string Name { get; }
        public Statement Declaration { get; }

        public Symbol(string name, Statement declaration)
        {
            Name = name;
            Declaration = declaration;
        }
    }
}
