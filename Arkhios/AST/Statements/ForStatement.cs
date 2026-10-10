using Arkhios.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class ForStatement : Statement
    {
        public Statement? Initializer { get; }
        public Expression? Condition { get; }
        public Statement? Iterator { get; }
        public Statement Body { get; }

        public ForStatement(
            Statement? initializer,
            Expression? condition,
            Statement? iterator,
            Statement body)
        {
            Initializer = initializer;
            Condition = condition;
            Iterator = iterator;
            Body = body;
        }
    }
}
