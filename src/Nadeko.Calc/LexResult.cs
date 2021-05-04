using System.Collections.Generic;
using Nadeko.Calc.Tokens;

namespace Nadeko.Calc
{
    public class LexResult
    {
        public LexResult(IEnumerable<Token> tokens, string error = null)
        {
            Error = error;
            Tokens = tokens;
        }

        public IEnumerable<Token> Tokens { get; }
        public string Error { get; }
    }
}