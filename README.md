# CampfireInterpreters

This is my work-in-progress implementation of a toy programming language and compilation pipeline (as presented in [Crafting Interpreters](https://craftinginterpreters.com/)) with a C# port of the Tree-Walk Interpreter.

You can also find the author's implementation (as well as a suite of other ports) [here](https://github.com/munificent/craftinginterpreters).

# Repository Layout
*   `campfire/` – Ad-Hoc CLI to trigger the various stages
*   `TreeWalkInterpreter/` – C# Tree-Walk Interpreter Implementation (Manual Lexer, Recursive Descent Parser, and Interpreter)
*   `TreeWalkInterpreter/campfire_grammar.g` – BNF of the language's grammar
*   `TreeWalkInterpreter/TestScript.cf` – De facto test script in toy language
*   `CVM/` - The bytecode and Virtual Machine, written in C.

