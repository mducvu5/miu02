namespace Nadeko.Calc.Expressions
{
    public class ValueExpression : Expression
    {
        public ValueExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }
    }
}