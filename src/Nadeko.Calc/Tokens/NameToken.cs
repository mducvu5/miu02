namespace Nadeko.Calc.Tokens
{
    public sealed class NameToken : Token
    {
        public string Name { get; }

        public NameToken(string name)
        {
            Name = name;
        }
    }
}