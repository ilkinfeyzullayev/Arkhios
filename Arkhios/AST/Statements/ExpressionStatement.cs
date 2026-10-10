using Arkhios.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class ExpressionStatement : Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }
    }
}
