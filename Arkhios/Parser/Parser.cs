using Arkhios.AST.Statements;
using Arkhios.Lexer.Tokens;

namespace Arkhios.Parser
{
    internal partial class Parser
    {
        public List<Statement> AST { get; } = new();
        public readonly List<Token> TokenList;
        private int _position;

        private Token Current => TokenList[_position];
        private bool IsAtEnd => _position >= TokenList.Count;

        private static readonly HashSet<string> TypeList = new() { "int", "float", "complex", "string", "bool" };

        public Parser(List<Token> tokens)
        {
            TokenList = tokens;
            _position = 0;
        }

        public void Parse()
        {
            while (!IsAtEnd)
                AST.Add(ParseStatement());
        }
    }
}
