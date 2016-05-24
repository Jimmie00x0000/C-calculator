# C-calculator
This project is a demo for a simple calculator.
The develop environment is in Visual Studio 2013, .NET 4.5.
The parse mechanism have nothing to do with LL/LR parser technique. When I make this calculator I only had little knowledge at compiler principles.
This program will scan all char in a expression, and convert operator into a internal node in a grammar tree, and convert operand(num) into
a leaf node in a grammar tree. At last a postorder traversal will give the final result of expression.
