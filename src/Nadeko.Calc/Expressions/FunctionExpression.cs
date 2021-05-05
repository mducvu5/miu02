namespace Nadeko.Calc.Expressions
{
    public sealed class FunctionExpression : Expression
    {
        public string Name { get; }
        public Expression Expression { get; }

        public FunctionExpression(string name, Expression expression)
        {
            Name = name;
            Expression = expression;
        }
    }
}