using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.BinaryExpressions
{
    internal class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public SymbolType Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(
            Expression left,
            SymbolType @operator,
            Expression right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }
}
