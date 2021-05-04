using System;
using System.Collections.Generic;
using System.Globalization;
using Nadeko.Calc.Tokens;

namespace Nadeko.Calc
{
    public class Lexer
    {
        private readonly string _input;
        private int _position = 0;

        public Lexer(string input)
        {
            _input = input;
        }
        
        private char? GetNext()
        {
            if (_position >= _input.Length)
                return null;

            return _input[_position++];
        }

        private char? Peek()
        {
            if (_position >= _input.Length)
                return null;

            return _input[_position];
        }
        
        public LexResult Lex()
        {
            var tokens = new List<Token>();
            while (true)
            {
                var maybeChar = GetNext();
                if (!(maybeChar is char c))
                    break;

                if (char.IsWhiteSpace(c))
                    continue;

                var startPos = _position;
                if (char.IsDigit(c) || c == '.')
                {
                    startPos -= 1;
                    while (Peek() is char ch && (char.IsDigit(ch) || ch == '.'))
                    {
                        GetNext();
                    }
                }

                if (startPos != _position)
                {
                    var numberStr = _input.Substring(startPos, _position - startPos);
                    if (!double.TryParse(numberStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
                    {
                        return new LexResult(tokens, $"Invalid number '{numberStr}' at position {startPos}");
                    }
                    
                    tokens.Add(new NumberToken(val));
                    continue;
                }

                // if (char.IsLetter(c))
                // {
                //     ParseWord();
                // }

                if(c == '+')
                    tokens.Add(new PlusToken());
                else if (c == '-')
                    tokens.Add(new MinusToken());
                else if (c == '/')
                    tokens.Add(new DivideToken());
                else if (c == '*')
                    tokens.Add(new MultiplyToken());
                else if (c == '^')
                    tokens.Add(new PowerToken());
                else
                {
                    return new LexResult(tokens, $"Invalid token '{c}' at position {_position}");
                }
            }

            tokens.Add(new EndOfFileToken());
            return new LexResult(tokens);
        }
    }

    public abstract class Expression
    {
    }

    public sealed class ValueExpression : Expression
    {
        public ValueExpression(double value)
        {
            Value = value;
        }

        public double Value { get; }
    }

    public abstract class UnaryExpression : Expression
    {
        public Expression Expression { get; }

        public UnaryExpression(Expression expression)
        {
            Expression = expression;
        }
    }

    public sealed class MinusUnaryExpression : UnaryExpression
    {
        public MinusUnaryExpression(Expression expression) : base(expression)
        {
        }
    }

    public sealed class PlusUnaryExpression : UnaryExpression
    {
        public PlusUnaryExpression(Expression expression) : base(expression)
        {
        }
    }

    public abstract class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }
    }

    public sealed class PlusBinaryExpression : BinaryExpression
    {
        public PlusBinaryExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }

    public sealed class MinusBinaryExpression : BinaryExpression
    {
        public MinusBinaryExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }

    public sealed class DivisionBinaryExpression : BinaryExpression
    {
        public DivisionBinaryExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
    
    public sealed class MultiplicationBinaryExpression : BinaryExpression
    {
        public MultiplicationBinaryExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }
    
    public sealed class PowerBinaryExpression : BinaryExpression
    {
        public PowerBinaryExpression(Expression left, Expression right) : base(left, right)
        {
        }
    }

    public sealed class Evaluator
    {
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
                _ => throw new ArgumentOutOfRangeException(nameof(expr))
            };

        private ValueExpression EvaluateUnaryExpression(UnaryExpression uex)
            => uex switch
            {
                PlusUnaryExpression mux => Evaluate(mux.Expression),
                MinusUnaryExpression pux => Negate(Evaluate(pux.Expression)),
                _ => throw new ArgumentOutOfRangeException(nameof(uex))
            };

        private ValueExpression EvaluateBinaryExpression(BinaryExpression bex)
            => bex switch
            {
                DivisionBinaryExpression ex => Divide(Evaluate(ex.Left), Evaluate(ex.Right)),
                MinusBinaryExpression ex => Subtract(Evaluate(ex.Left), Evaluate(ex.Right)),
                MultiplicationBinaryExpression ex => Multiply(Evaluate(ex.Left), Evaluate(ex.Right)),
                PlusBinaryExpression ex => Add(Evaluate(ex.Left), Evaluate(ex.Right)),
                PowerBinaryExpression ex => Power(Evaluate(ex.Left), Evaluate(ex.Right)),
                _ => throw new ArgumentOutOfRangeException(nameof(bex))
            };
        
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