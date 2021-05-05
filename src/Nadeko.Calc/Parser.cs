using System;
using System.Collections.Generic;
using System.Linq;
using Nadeko.Calc.Expressions;
using Nadeko.Calc.Tokens;

namespace Nadeko.Calc
{
    public class Parser
    {
        private readonly IReadOnlyList<Token> _tokens;
        private int current = 0;
        private readonly List<List<Type>> _binaryOperators;

        private Parser()
        {
            _binaryOperators = new List<List<Type>>()
            {
                new List<Type>() { typeof(LogicalAndToken), typeof(LogicalOrToken), typeof(LogicalXorToken)},
                new List<Type>() { typeof(LeftShiftToken), typeof(RightShiftToken)},
                new List<Type>() { typeof(PlusToken), typeof(MinusToken)},
                new List<Type>() { typeof(MultiplyToken), typeof(DivideToken)},
                new List<Type>() { typeof(PowerToken)},
            };
        }
        public Parser(string input) : this()
        {
            var lexer = new Lexer(input);
            var lexerResult = lexer.Lex();
            if (lexerResult.Error != null)
                throw new InvalidOperationException(lexerResult.Error);
            
            _tokens = lexerResult.Tokens.ToList();
        }
        
        public Parser(IEnumerable<Token> tokens) : this()
        {
            _tokens = tokens.ToList();
        }

        private IReadOnlyList<Type> BinaryOperatorsFor(int precedence = 0)
            => precedence >= _binaryOperators.Count
                ? Enumerable.Empty<Type>().ToList()
                : _binaryOperators[precedence];
        
        public (Expression expression, string error) Parse()
        {
            var expr = ParseExpression();
            
            if (!(Peek() is EndOfFileToken) || expr is null)
                return (null, $"Unexpected {Peek(0).GetType().Name}, token #{current}");
            
            return (expr, null);
        }

        private Expression ParseExpression(int precedence = 0)
        {
            var binaryOperators = BinaryOperatorsFor(precedence);

            if (!binaryOperators.Any())
            {
                return ParseValue();
            }

            var left = ParseExpression(precedence + 1);
            if (left is null)
                return null;
            
            while (binaryOperators.Contains(Peek(0).GetType()))
            {
                var token = Consume();
                var right = ParseExpression(precedence + 1);
                if (right is null)
                    return null;
                left = CreateBinaryExpression(left, right, token);
            }

            return left;
        }

        private Expression ParseValue()
        {
            while (true)
            {
                var token = Consume();

                switch (token)
                {
                    case OpenBracketToken _:
                    {
                        var expression = ParseExpression();
                        if (expression is null)
                            return null;

                        token = Consume();
                        if (token is ClosedBracketToken)
                            return new BracketExpression(expression);
                    
                        // unmatched closing bracket?!
                        return null;
                    }
                    case PlusToken _:
                        continue;
                    case MinusToken _:
                    {
                        var value = ParseValue();
                        if (value is null)
                            return null;
                    
                        return new MinusUnaryExpression(value);
                    }
                    case NumberToken nt:
                        return new ValueExpression(nt.Value);
                    case NameToken nameToken:
                        if (Peek() is OpenBracketToken)
                        {
                            Consume();
                            var expression = ParseExpression();

                            token = Consume();
                            if (token is ClosedBracketToken)
                                return new FunctionExpression(nameToken.Name, expression);

                            // Function call with unmatched closing bracket?!
                            return null;
                        }
                        
                        return new ConstantExpression(nameToken.Name);
                    default:
                        return null;
                }
            }
        }

        private BinaryExpression CreateBinaryExpression(Expression left, Expression right, Token token)
            => token switch
            {
                DivideToken _ => new DivisionBinaryExpression(left, right),
                MultiplyToken _ => new MultiplyBinaryExpression(left, right),
                PlusToken _ => new PlusBinaryExpression(left, right),
                MinusToken _ => new MinusBinaryExpression(left, right),
                PowerToken _ => new PowerBinaryExpression(left, right),
                LeftShiftToken _ => new LeftShiftExpression(left, right),
                RightShiftToken _ => new RightShiftExpression(left, right),
                LogicalAndToken _ => new LogicalAndExpression(left, right),
                LogicalOrToken _ => new LogicalOrExpression(left, right),
                LogicalXorToken _ => new LogicalXorExpression(left, right),
                _ => throw new ArgumentOutOfRangeException(nameof(token))
            };

        private Token Peek(int offset = 0)
        {
            if (_tokens.Count <= current + offset)
                return new EndOfFileToken();
            
            return _tokens[current + offset];
        }

        private Token Consume()
        {
            return _tokens[current++];
        }
    }
}