namespace Nadeko.Calc.Expressions
{
    public sealed class BracketExpression : Expression
    {
        public Expression Expression { get; }

        public BracketExpression(Expression expression)
        {
            Expression = expression;
        }
    }
}