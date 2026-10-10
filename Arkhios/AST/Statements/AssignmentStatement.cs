using Arkhios.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class AssignmentStatement : Statement
    {
        public string Name { get; }
        public Expression Value { get; }

        public AssignmentStatement(string name, Expression value)
        {
            Name = name;
            Value = value;
        }
    }
}
