using System;
using System.Collections.Generic;
using System.Linq;
using Nadeko.Calc.Expressions;

namespace Nadeko.Calc
{
    public sealed class Evaluator
    {
        private readonly Dictionary<string, Func<double, double>> _functions;

        private readonly Dictionary<string, double> _constants;

        public Evaluator()
        {
            _functions = new Dictionary<string, Func<double, double>>()
            {
                {"abs", Math.Abs},
                
                {"round", Math.Round},
                {"ceil", Math.Ceiling},
                {"floor", Math.Floor},
                
                {"sign", x => Math.Sign(x)},
                {"trunc", Math.Truncate},
                
                {"sin", x => Math.Sin(x * Math.PI / 180)},
                {"asin", x => Math.Asin(x * Math.PI / 180)},
                {"sinh", x => Math.Sinh(x * Math.PI / 180)},
                {"asinh", x => Math.Asinh(x * Math.PI / 180)},
                {"cos", x => Math.Cos(x * Math.PI / 180)},
                {"acos", x => Math.Acos(x * Math.PI / 180)},
                {"cosh", x => Math.Cosh(x * Math.PI / 180)},
                {"acosh", x => Math.Acosh(x * Math.PI / 180)},
                {"tan", x => Math.Tan(x * Math.PI / 180)},
                {"atan", x => Math.Atan(x * Math.PI / 180)},
                {"tanh", x => Math.Tanh(x * Math.PI / 180)},
                {"atanh", x => Math.Atanh(x * Math.PI / 180)},
            };

            _constants = new[] {Constants.Pi, Constants.E, Constants.Phi, Constants.Gamma}
                .SelectMany(constant => constant.Names.Select(name => (name, constant.Value)))
                .ToDictionary(x => x.name, x => x.Value);
        }

        public Evaluator AddFunction(string name, Func<double, double> func, out bool success)
        {
            name = name.ToLowerInvariant();
            success = _functions.TryAdd(name, func);
            return this;
        }

        public Evaluator RemoveFunction(string name, out bool success)
        {
            name = name.ToLowerInvariant();
            success = _functions.ContainsKey(name);
            _functions.Remove(name);
            return this;
        }

        public Evaluator AddConstant(string name, double value, out bool success)
        {
            name = name.ToLowerInvariant();
            success = _constants.TryAdd(name, value);
            return this;
        }

        public Evaluator RemoveConstant(string name, out bool success)
        {
            name = name.ToLowerInvariant();
            success = _constants.ContainsKey(name);
            _constants.Remove(name);
            return this;
        }

        public IReadOnlyDictionary<string, Func<double, double>> GetFunctions()
            => _functions;
        
        public (bool Success, string Error) TryEvaluate(string input, out double result)
        {
            result = 0;
            
            // lex
            var lexer = new Lexer(input);
            var lexerResult = lexer.Lex();
            if (lexerResult.Error != null)
            {
                return (false, lexerResult.Error);
            }
            
            // parse
            var parser = new Parser(input);
            var (expr, error) = parser.Parse();
            if (!(error is null))
                return (false, error);
            
            var evalResult = Evaluate(expr);
            result = evalResult.Value;
            return (true, null);
        }
        
        public ValueExpression Evaluate(Expression expr)
            => expr switch
            {
                ValueExpression vex => vex,
                UnaryExpression uex => EvaluateUnaryExpression(uex),
                BinaryExpression bex => EvaluateBinaryExpression(bex),
                BracketExpression bex => Evaluate(bex.Expression),
                FunctionExpression fex => EvaluateFunction(fex),
                ConstantExpression cex => _constants.TryGetValue(cex.Constant.ToLowerInvariant(), out var val)
                    ? new ValueExpression(val)
                    : throw new ArgumentOutOfRangeException(cex.Constant, "Invalid constant name."),
                _ => throw new ArgumentOutOfRangeException(nameof(expr))
            };
        
        private ValueExpression EvaluateFunction(FunctionExpression fex)
        {
            if (!_functions.TryGetValue(fex.Name.ToLowerInvariant(), out var function))
                throw new NotSupportedException($"Function {fex.Name} is not supported.");

            return new ValueExpression(function(Evaluate(fex.Expression).Value));
        }

        private ValueExpression EvaluateUnaryExpression(UnaryExpression uex)
            => uex switch
            {
                PlusUnaryExpression mux => Evaluate(mux.Expression),
                MinusUnaryExpression pux => Negate(Evaluate(pux.Expression)),
                _ => throw new ArgumentOutOfRangeException(nameof(uex))
            };

        private ValueExpression EvaluateBinaryExpression(BinaryExpression bex)
        {
            var left = Evaluate(bex.Left);
            var right = Evaluate(bex.Right);
            return bex switch
            {
                DivisionBinaryExpression _ => Divide(left, right),
                MinusBinaryExpression _ => Subtract(left, right),
                MultiplyBinaryExpression _ => Multiply(left, right),
                PlusBinaryExpression _ => Add(left, right),
                PowerBinaryExpression _ => Power(left, right),
                LeftShiftExpression _ => LeftShift(left, right),
                RightShiftExpression _ => RightShift(left, right),
                LogicalAndExpression _ => LogicalAnd(left, right),
                LogicalOrExpression _ => LogicalOr(left, right),
                LogicalXorExpression _ => LogicalXor(left, right),
                _ => throw new ArgumentOutOfRangeException(nameof(bex))
            };
        }
        
        private ValueExpression LogicalXor(ValueExpression left, ValueExpression right)
        {
            if (left.Value % 1 != 0 || right.Value % 1 != 0)
                throw new ArgumentException("You can only perform bitwise operators on whole numbers.");
            
            var result = (long)left.Value ^ (long) right.Value;
            return new ValueExpression(result);
        }
        
        private ValueExpression LogicalOr(ValueExpression left, ValueExpression right)
        {
            if (left.Value % 1 != 0 || right.Value % 1 != 0)
                throw new ArgumentException("You can only perform bitwise operators on whole numbers.");
            
            var result = (long)left.Value | (long) right.Value;
            return new ValueExpression(result);
        }

        private ValueExpression LogicalAnd(ValueExpression left, ValueExpression right)
        {
            if (left.Value % 1 != 0 || right.Value % 1 != 0)
                throw new ArgumentException("You can only perform bitwise operators on whole numbers.");
            
            var result = (long)left.Value & (long)right.Value;
            return new ValueExpression(result);
        }

        private ValueExpression LeftShift(ValueExpression left, ValueExpression right)
        {
            if (left.Value % 1 != 0 || right.Value % 1 != 0)
                throw new ArgumentException("You can only shift whole numbers.");
                
            return new ValueExpression((long)left.Value << (int)right.Value);
        }
        
        private ValueExpression RightShift(ValueExpression left, ValueExpression right)
        {
            if (left.Value % 1 != 0 || right.Value % 1 != 0)
                throw new ArgumentException("You can only shift whole numbers.");
                
            return new ValueExpression((long)left.Value >> (int)right.Value);
        }

        private ValueExpression Power(ValueExpression left, ValueExpression right)
            => new ValueExpression(Math.Pow(left.Value, right.Value));

        private ValueExpression Divide(ValueExpression left, ValueExpression right)
            => new ValueExpression(left.Value / right.Value);

        private ValueExpression Subtract(ValueExpression left, ValueExpression right)
            => new ValueExpression(left.Value - right.Value);

        private ValueExpression Multiply(ValueExpression left, ValueExpression right)
            => new ValueExpression(left.Value * right.Value);

        private ValueExpression Add(ValueExpression left, ValueExpression right)
            => new ValueExpression(left.Value + right.Value);

        private ValueExpression Negate(ValueExpression ex)
            => new ValueExpression(-ex.Value);
    }
}