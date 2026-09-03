using System;
using System.Collections.Generic;
using System.Text;
using Arkhios.AST.Expressions;

namespace Arkhios.AST.Statements
{
    internal class VariableDeclaration : Statement
    {
        public string? Type { get; set; }
        public string Name { get; set; }
        public Expression Expression { get; set; }

        public VariableDeclaration(string type, string name, Expression expression)
        {
            Type = type;
            Name = name;
            Expression = expression;
        }
    }
}
