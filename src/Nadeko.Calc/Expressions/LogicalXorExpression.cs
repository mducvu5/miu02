namespace Nadeko.Calc.Expressions
{
    public sealed class LogicalXorExpression : BinaryExpression
    {
        public LogicalXorExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
}