using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Arkhios.AST.Statements.FunctionDeclarations
{
    internal class FunctionDeclaration : Statement
    {
        public string ReturnType { get; }
        public string Name { get; }
        public List<Parameter> Parameters { get; }
        public BlockStatement Body { get; }

        public FunctionDeclaration(
            string returnType,
            string name,
            List<Parameter> parameters,
            BlockStatement body)
        {
            ReturnType = returnType;
            Name = name;
            Parameters = parameters;
            Body = body;
        }
    }
}
