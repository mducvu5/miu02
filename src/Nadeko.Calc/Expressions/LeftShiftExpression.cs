namespace Nadeko.Calc.Expressions
{
    public sealed class LeftShiftExpression : BinaryExpression
    {
        public LeftShiftExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
}