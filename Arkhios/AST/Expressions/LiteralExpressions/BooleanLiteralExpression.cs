using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.LiteralExpressions
{
    class BooleanLiteralExpression : LiteralExpression
    {

        public BooleanLiteralExpression(bool value)
        : base(value)
        {
        }
    }
}
