namespace Nadeko.Calc.Expressions
{
    public sealed class LogicalOrExpression : BinaryExpression
    {
        public LogicalOrExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
}