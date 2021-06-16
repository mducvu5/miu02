using System;
using System.Collections.Generic;
using System.Linq;

namespace Nadeko.Calc
{
    public static class Constants
    {
        public static Constant Pi { get; } = new Constant(Math.PI, "pi", "math.pi", "π");
        public static Constant E { get; } = new Constant(Math.E, "e", "math.e");
        public static Constant Gamma { get; } = new Constant(0.577_215_664_901_532_860_606_512,
            "gamma", "γ");
        public static Constant Phi { get; } = new Constant(1.618_033_988_749_894_848_204_586, 
            "phi", "fi", "Φ", "φ");
    }
}