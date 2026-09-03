using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.LiteralExpressions
{
    class StringLiteralExpression : LiteralExpression
    {
        public StringLiteralExpression(string value)
        : base(value)
        {
        }
    }
}
