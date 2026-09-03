using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.LiteralExpressions
{
    class NumberLiteralExpression : LiteralExpression
    {

        public NumberLiteralExpression(string value)
        : base(value)
        {
        }
    }
}
