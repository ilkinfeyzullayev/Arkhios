using Arkhios.Lexer.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Expressions.CallExpressions
{
    internal class CallExpression : Expression
    {
        public string FunctionName { get; }
        public List<Expression> Arguments { get; }

        public CallExpression(
            string functionName,
            List<Expression> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }
    }
}
