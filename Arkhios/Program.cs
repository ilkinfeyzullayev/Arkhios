using Arkhios.AST.Statements;
using Arkhios.Errors;
using Arkhios.Lexer;
using Arkhios.Parser;

string? directory = AppContext.BaseDirectory;

while (directory != null &&
       !File.Exists(Path.Combine(directory, "Arkhios.csproj")))
{
    directory = Directory.GetParent(directory)?.FullName;
}

if (directory == null)
{
    Console.WriteLine("Could not locate the Arkhios project directory.");
    return;
}

string programPath = Path.Combine(directory, "program.ark");

if (!File.Exists(programPath))
{
    Console.WriteLine($"program.ark not found at: {programPath}");
    return;
}

string source = File.ReadAllText(programPath);

Lexer lexer = new(source);

try
{
    lexer.Lex();
}
catch (ArkhiosException ex)
{
    Console.WriteLine(ex.Message);
    return;
}

Parser parser = new(lexer.TokenList);
parser.Parse();

foreach (var statement in parser.AST)
{
    Console.WriteLine(statement);
}


