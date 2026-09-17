using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.AST.Statements
{
    internal class BlockStatement : Statement
    {
        public List<Statement> Statements { get; }

        public BlockStatement(List<Statement> statements)
        {
            Statements = statements;
        }
    }
}
