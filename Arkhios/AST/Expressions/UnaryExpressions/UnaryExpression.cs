using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.UnaryExpressions
{
    internal abstract class UnaryExpression : Expression
    {
        public SymbolType Operator { get; }
        public Expression Operand { get; }

        protected UnaryExpression(SymbolType @operator, Expression operand)
        {
            Operator = @operator;
            Operand = operand;
        }
    }
}
