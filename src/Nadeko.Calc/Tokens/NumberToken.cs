namespace Nadeko.Calc.Tokens
{
    public sealed class NumberToken : Token
    {
        public NumberToken(double value)
        {
            Value = value;
        }

        public double Value { get; }
    }
}