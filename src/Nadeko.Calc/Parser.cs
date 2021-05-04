using System;
using System.Collections.Generic;
using System.Linq;
using Nadeko.Calc.Tokens;

namespace Nadeko.Calc
{
    public class Parser
    {
        private readonly IReadOnlyList<Token> _tokens;
        private int current = 0;

        public Parser(string input)
        {
            var lexer = new Lexer(input);
            var lexerResult = lexer.Lex();
            if (lexerResult.Error != null)
                throw new InvalidOperationException(lexerResult.Error);
            
            _tokens = lexerResult.Tokens.ToList();
        }
        
        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.ToList();
        }
        
        public (Expression expression, string error) Parse()
        {
            var expr = ParseExpression1();
            if (expr is null)
                return (null, $"Unexpected {Peek(0).GetType().Name}, token #{current}");
            return (expr, null);
        }

        private Expression ParseExpression1()
        {
            var left = ParseExpression2();
            if (left is null)
                return null;
            while (Peek(0) is PlusToken || Peek(0) is MinusToken)
            {
                var token = GetNext();
                var right = ParseExpression2();
                if (right is null)
                    return null;
                left = CreateBinaryExpression(left, right, token);
            }

            return left;
        }
        
        private Expression ParseExpression2()
        {
            var left = ParseExpression3();
            if (left is null)
                return null;
            while (Peek(0) is MultiplyToken || Peek(0) is DivideToken)
            {
                var token = GetNext();
                var right = ParseExpression3();
                if (right is null)
                    return null;
                left = CreateBinaryExpression(left, right, token);
            }

            return left;
        }
        
        private Expression ParseExpression3()
        {
            var left = ParseValue();
            if (left is null)
                return null;
            while (Peek(0) is PowerToken)
            {
                GetNext();
                var right = ParseValue();
                if (right is null)
                    return null;
                left = new PowerBinaryExpression(left, right);
            }

            return left;
        }

        private Expression ParseValue()
        {
            while (true)
            {
                var token = GetNext();
                
                if (token is PlusToken)
                    continue;
                
                if (token is MinusToken)
                {
                    var value = ParseValue();
                    if (value is null)
                        return null;
                    
                    return new MinusUnaryExpression(value);
                }

                if (token is NumberToken nt)
                    return new ValueExpression(nt.Value);

                return null;
            }
        }

        private BinaryExpression CreateBinaryExpression(Expression left, Expression right, Token token)
            => token switch
            {
                DivideToken _ => new DivisionBinaryExpression(left, right),
                MultiplyToken _ => new MultiplicationBinaryExpression(left, right),
                PlusToken _ => new PlusBinaryExpression(left, right),
                MinusToken _ => new MinusBinaryExpression(left, right),
                PowerToken _ => new PowerBinaryExpression(left, right),
                _ => throw new ArgumentOutOfRangeException(nameof(token))
            };

        private Token Peek(int offset = 0)
        {
            if (_tokens.Count <= current + offset)
                return new EndOfFileToken();
            
            return _tokens[current + offset];
        }

        private Token GetNext()
        {
            return _tokens[current++];
        }
    }
}