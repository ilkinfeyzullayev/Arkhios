using Arkhios.Errors;
using Arkhios.Lexer;

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

Console.WriteLine("Successfully loaded Arkhios code:");

foreach (var token in lexer.Tokens)
{
    Console.WriteLine(token);
}