namespace Nadeko.Calc.Expressions
{
    public sealed class LogicalAndExpression : BinaryExpression
    {
        public LogicalAndExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
}