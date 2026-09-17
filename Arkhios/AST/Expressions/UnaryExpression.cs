using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions
{
    internal class UnaryExpression : Expression
    {
        public SymbolType Operator { get; }
        public Expression Operand { get; }

        public UnaryExpression(SymbolType @operator, Expression operand)
        {
            Operator = @operator;
            Operand = operand;
        }
    }
}
