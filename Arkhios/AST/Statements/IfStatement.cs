using Arkhios.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class IfStatement : Statement
    {
        public Expression Condition { get; }
        public BlockStatement Body { get; }
        public Statement? Else { get; }

        public IfStatement(
            Expression condition,
            BlockStatement body,
            Statement? @else)
        {
            Condition = condition;
            Body = body;
            Else = @else;
        }
    }
}
