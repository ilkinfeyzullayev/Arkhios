using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements.FunctionDeclarations
{
    internal class Parameter
    {
        public string Type { get; }
        public string Name { get; }

        public Parameter(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
