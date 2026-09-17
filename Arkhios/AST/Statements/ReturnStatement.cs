using Arkhios.AST.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class ReturnStatement : Statement
    {
        public Expression Expression { get; }

        public ReturnStatement(Expression expression)
        {
            Expression = expression;
        }
    }
}
