using Arkhios.Errors;
using Arkhios.Lexer;
using Arkhios.Parser;
using System.Collections;
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

for (int i = 0; i < parser.AST.Count; i++)
{
    PrintTree(
        parser.AST[i],
        "",
        i == parser.AST.Count - 1);
}

static void PrintTree(
    object? node,
    string prefix = "",
    bool isLast = true)
{
    if (node == null)
        return;

    string branch = isLast ? "└── " : "├── ";

    Console.WriteLine(
        $"{prefix}{branch}{node.GetType().Name}");

    string childPrefix =
        prefix + (isLast ? "    " : "│   ");

    PrintChildren(node, childPrefix);
}

static void PrintChildren(
    object node,
    string prefix)
{
    var children = node
        .GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(property =>
            property.CanRead &&
            property.GetIndexParameters().Length == 0)
        .Select(property => new
        {
            property.Name,
            Value = property.GetValue(node)
        })
        .Where(child => child.Value != null)
        .ToList();

    for (int i = 0; i < children.Count; i++)
    {
        var child = children[i];

        bool isLast = i == children.Count - 1;

        PrintProperty(
            child.Name,
            child.Value!,
            prefix,
            isLast);
    }
}

static void PrintProperty(
    string name,
    object value,
    string prefix,
    bool isLast)
{
    string branch = isLast ? "└── " : "├── ";

    if (IsSimpleValue(value))
    {
        Console.WriteLine(
            $"{prefix}{branch}{name}: {value}");

        return;
    }

    Console.WriteLine(
        $"{prefix}{branch}{name}:");

    string childPrefix =
        prefix + (isLast ? "    " : "│   ");

    PrintValue(
        value,
        childPrefix);
}

static void PrintValue(
    object value,
    string prefix)
{
    if (value is IEnumerable enumerable &&
        value is not string)
    {
        var items = enumerable
            .Cast<object?>()
            .Where(item => item != null)
            .ToList();

        for (int i = 0; i < items.Count; i++)
        {
            object item = items[i]!;

            PrintTree(
                item,
                prefix,
                i == items.Count - 1);
        }

        return;
    }

    PrintTree(
        value,
        prefix,
        true);
}

static bool IsSimpleValue(object value)
{
    return value is string ||
           value.GetType().IsPrimitive ||
           value.GetType().IsEnum ||
           value is decimal;
}