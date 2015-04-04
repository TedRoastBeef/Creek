using Creek.Parsing.Generator;
using LinguaDemo.Calculator;

namespace Test.CalculatorParser.Terminals
{
    [Terminal(@"[a-zA-Z][a-zA-Z0-9_]*")]
    public class Variable : CalculatorTerminal
    {
    }
}