namespace Nadeko.Calc
{
    public class Constant
    {
        public Constant(double value, params string[] nameses)
        {
            Names = nameses;
            Value = value;
        }

        public string[] Names { get; }
        public double Value { get; }
    }
}