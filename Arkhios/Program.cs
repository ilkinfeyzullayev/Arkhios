using Arkhios.Errors;
using Arkhios.Lexer;
using Arkhios.Parser;
using System.Reflection;

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
    PrintTree(statement);
}

static void PrintTree(object? node, string prefix = "", bool isLast = true)
{
    if (node == null)
        return;

    string branch = isLast ? "└── " : "├── ";

    Console.WriteLine($"{prefix}{branch}{node.GetType().Name}");

    var properties = node.GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

    for (int i = 0; i < properties.Length; i++)
    {
        var property = properties[i];

        if (!property.CanRead)
            continue;

        object? value = property.GetValue(node);

        if (value == null)
            continue;

        bool lastProperty = i == properties.Length - 1;

        if (value is string ||
            value.GetType().IsPrimitive ||
            value.GetType().IsEnum)
        {
            string propertyBranch = lastProperty ? "└── " : "├── ";
            Console.WriteLine(
                $"{prefix}{(isLast ? "    " : "│   ")}{propertyBranch}" +
                $"{property.Name}: {value}");
        }
        else
        {
            Console.WriteLine(
                $"{prefix}{(isLast ? "    " : "│   ")}" +
                $"{(lastProperty ? "└── " : "├── ")}" +
                $"{property.Name}:");

            PrintTree(
                value,
                prefix + (isLast ? "    " : "│   "),
                lastProperty);
        }
    }
}