# Arkhios

Arkhios is an experimental, math-focused programming language designed around expressing mathematical formulas and structured computations in a clear and concise way.

The language and its implementation are currently under active development, so **syntax, semantics, types, and other language features are subject to change**.

## Status

**Early development — lexer, parser, and AST implementation underway**

The project currently has an initial lexer and parser capable of:

* Reading `.ark` source files
* Recognizing identifiers and language keywords
* Recognizing numeric and string literals
* Recognizing single- and multi-character symbols
* Recognizing single-line and multi-line comments
* Reporting lexical errors, including unterminated multi-line comments
* Producing structured tokens
* Parsing variable declarations
* Parsing literals and identifiers
* Parsing parenthesized expressions
* Parsing unary and binary expressions
* Applying operator precedence and associativity
* Parsing comparison and logical operators
* Parsing function call expressions and argument lists
* Building an initial abstract syntax tree (AST)
* Representing parsed programs as structured AST nodes

The parser and AST are still being expanded. Function declarations, control flow, semantic analysis, execution, and other major language features have not yet been implemented.

## Example

A small example of the **currently supported** syntax:

```arkhios
var x = 123;
float y = 57.25;

var result = x + y * 2;
var valid = x >= 100 && y < 100;

var value = sqrt(x);
var nested = foo(x + y, -(x ^ 2));
```

This syntax is **not considered stable** and may change as Arkhios develops.

Function declarations, control flow, and other higher-level language constructs are planned but are not yet fully supported by the parser.

## Project Structure

The project is currently organized around the main components of the language implementation:

```text
Arkhios/
├── AST/
│   ├── Expressions/
│   └── Statements/
├── Errors/
├── Lexer/
│   └── Tokens/
├── Parser/
│   ├── Parser.cs
│   ├── Parser.Expressions.cs
│   ├── Parser.Statements.cs
│   ├── Parser.Helpers.cs
│   └── Parser.VariableDeclarations.cs
└── ...
```

The structure will evolve as additional compiler/interpreter components are introduced.

## Goals

Arkhios aims to explore:

* Mathematical and formula-oriented syntax
* A clear and predictable type system
* Type inference where appropriate
* Numerical types suitable for both ordinary and high-precision computation
* Readable mathematical expressions
* A straightforward language design
* A complete language implementation built from the ground up

These goals are still being refined as development continues.

## Development

Arkhios is being developed in **C# and .NET**.

The implementation is being built incrementally through separate language-processing stages:

```text
Source Code
    ↓
Lexer
    ↓
Tokens
    ↓
Parser
    ↓
AST
    ↓
Semantic Analysis
    ↓
Execution
```

The lexer and initial parser/AST infrastructure are currently under development. The later stages are planned but are not implemented yet.

## Current Philosophy

Arkhios is intentionally being designed from the ground up rather than attempting to follow an existing language specification exactly.

This means language features and design decisions may be discarded, redesigned, or replaced during development. The current implementation should therefore be treated as experimental rather than as a stable language standard.

## License

Not yet decided.
