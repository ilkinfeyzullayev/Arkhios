using Arkhios.AST.Statements;
using Arkhios.AST.Statements.FunctionDeclarations;
using Arkhios.Lexer.Tokens;
using Arkhios.Lexer.Tokens.TokenTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        private FunctionDeclaration ParseFunctionDeclaration()
        {
            string returnType;

            if (Current is Keyword { KeywordType: KeywordType.Void })
            {
                returnType = "void";
            }
            else
            {
                returnType = ((Identifier)Current).Value;
            }

            Advance();
            string name = ((Identifier)Current).Value;
            Advance();
            ExpectSymbol(SymbolType.LeftParen);

            List<Parameter> parameters = ParseFunctionParameters();
            ExpectSymbol(SymbolType.RightParen);

            BlockStatement blockStatement = ParseBlockStatement();

            return new FunctionDeclaration(returnType, name, parameters, blockStatement);
        }

        private List<Parameter> ParseFunctionParameters()
        {
            List<Parameter> parameters = new();

            while (Current is not Symbol { SymbolType: SymbolType.RightParen })
            {
                string type = TypeList.Contains(((Identifier)Current).Value)
                    ? ((Identifier)Current).Value
                    : throw new Exception("Expected a valid type.");

                if (Next is not Identifier identifier)
                {
                    throw new Exception("Expected a valid argument.");
                }

                parameters.Add(new Parameter(type, identifier.Value));

                Advance();
                Advance();

                if (Current is Symbol { SymbolType: SymbolType.Comma })
                {
                    Advance();
                }
                else
                {
                    break;
                }
            }

            return parameters;
        }
    }
}
