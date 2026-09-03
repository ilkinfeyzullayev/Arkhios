using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.LiteralExpressions
{
    abstract class LiteralExpression : Expression
    {
        public object Value { get; }

        public LiteralExpression(object value)
        {
            Value = value;
        }
    }
}
