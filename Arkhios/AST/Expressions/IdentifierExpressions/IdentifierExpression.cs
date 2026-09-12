using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.IdentifierExpressions
{
    internal class IdentifierExpression : Expression
    {
        public string Name { get; }

        public IdentifierExpression(string name)
        {
            Name = name;
        }
    }
}
