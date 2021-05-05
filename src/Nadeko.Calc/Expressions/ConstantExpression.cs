namespace Nadeko.Calc.Expressions
{
    public sealed class ConstantExpression : Expression
    {
        public string Constant { get; }

        public ConstantExpression(string constant)
        {
            Constant = constant;
        }
    }
}