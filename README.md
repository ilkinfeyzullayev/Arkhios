# Arkhios

Arkhios is an experimental, math-focused programming language designed around expressing mathematical formulas and structured computations in a clear and concise way.

The language and its implementation are currently under active development, so **syntax, semantics, types, and other language features are subject to change**.

## Status

**Early development**

The project currently has an initial lexer capable of:

* Reading `.ark` source files
* Recognizing identifiers and language keywords
* Recognizing numeric literals
* Recognizing single- and multi-character symbols
* Producing structured tokens
* Reporting basic lexical errors

The parser, semantic analysis, execution model, and other major parts of the language are still under development.

## Example

A small example of the current syntax:

```arkhios
var x = 123;
float y = 57.25;

var result = x + y * 2;
var comparison = x >= 100;

int square(x) => x ^ 2;
```

This syntax is **not considered stable** and may change as Arkhios develops.

## Project Structure

The project is currently organized around the main stages of the language implementation:

```text
Arkhios/
├── Errors/
├── Lexer/
│   └── Tokens/
├── Parser/
└── ...
```

The structure will evolve as new compiler/interpreter components are introduced.

## Goals

Arkhios aims to explore:

* Mathematical and formula-oriented syntax
* A clear and predictable type system
* Numerical types suitable for both ordinary and high-precision computation
* Readable mathematical expressions
* A straightforward language design
* A complete language implementation built from the ground up

These goals are still being refined.

## Development

Arkhios is being developed in C# and .NET.

The project is currently focused on building the language implementation incrementally:

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

Not all stages are implemented yet.

## Current Philosophy

Arkhios is intentionally being designed from the ground up rather than attempting to follow an existing language specification exactly.

That means some ideas will inevitably be discarded, redesigned, or replaced during development. The current implementation should therefore be treated as experimental rather than a stable language standard.

## License

Not yet decided.
