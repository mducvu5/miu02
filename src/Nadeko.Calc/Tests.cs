using System;
using System.Linq;
using Nadeko.Calc.Expressions;
using Nadeko.Calc.Tokens;
using NUnit.Framework;

namespace Nadeko.Calc
{
    public class Tests
    {
        private Evaluator _eval;

        [SetUp]
        public void Setup()
        {
            _eval = new Evaluator();
        }
        
        [Test]
        public void TestValueExpressions()
        {
            var result = _eval.Evaluate(new ValueExpression(5));
            
            Assert.AreEqual(5, result.Value);
        }
        
        [Test]
        public void TestUnaryExpressions()
        {
            var result = _eval.Evaluate(
                new MinusUnaryExpression(
                    new ValueExpression(123.456)));
            
            Assert.AreEqual(-123.456, result.Value);
        }

        [Test]
        public void TestPlusBinaryExpression()
        {
            var left = new ValueExpression(5);
            var right = new ValueExpression(2);
            var add = new PlusBinaryExpression(left, right);
            var result = _eval.Evaluate(add);
            
            Assert.AreEqual(7, result.Value);
        }
        
        [Test]
        public void TestMinusBinaryExpression()
        {
            var left = new ValueExpression(5);
            var right = new ValueExpression(2);
            var add = new MinusBinaryExpression(left, right);
            var result = _eval.Evaluate(add);
            
            Assert.AreEqual(3, result.Value);
        }
        
        [Test]
        public void TestMultiplyBinaryExpression()
        {
            var left = new ValueExpression(5);
            var right = new ValueExpression(2);
            var add = new MultiplyBinaryExpression(left, right);
            var result = _eval.Evaluate(add);
            
            Assert.AreEqual(10, result.Value);
        }
        
        [Test]
        public void TestDivideBinaryExpression()
        {
            var left = new ValueExpression(5);
            var right = new ValueExpression(2);
            var add = new DivisionBinaryExpression(left, right);
            var result = _eval.Evaluate(add);
            
            Assert.AreEqual(2.5, result.Value);
        }

        [Test]
        public void TestParserExpression()
        {
            var parser = new Parser(new Token[]
            {
                new NumberToken(152),
                new PlusToken(),
                new NumberToken(15),
                new MultiplyToken(),
                new NumberToken(2),
                new MinusToken(),
                new MinusToken(),
                new MinusToken(),
                new PlusToken(),
                new NumberToken(5),
                new MinusToken(),
                new NumberToken(3.3),
                new PowerToken(),
                new NumberToken(5),
                new PlusToken(),
                new NumberToken(3.3),
                new PowerToken(),
                new NumberToken(5),
            });

            // - - - + 5 = -5
            
            var (expr, error) = parser.Parse();
            var result = _eval.Evaluate(expr);
            
            Assert.AreEqual(177, result.Value);
        }
        
        [Test]
        public void TestInvalidParserExpression()
        {
            var parser = new Parser(new Token[]
            {
                new NumberToken(152),
                new PlusToken(),
                new NumberToken(15),
                new MultiplyToken(),
                new NumberToken(2),
                new MinusToken(),
                new MinusToken(),
                new MinusToken(),
                new PlusToken(),
                new NumberToken(5),
                new MinusToken(),
                new NumberToken(3.3),
                new PowerToken(),
                new NumberToken(5),
                new PlusToken(),
                new NumberToken(3.3),
                new PowerToken(),
                new PowerToken(),
                new NumberToken(5),
            });

            // - - - + 5 = -5
            
            var (expr, error) = parser.Parse();
            Assert.IsNull(expr);
            Assert.IsNotEmpty(error);
            TestContext.Out.WriteLine(error);
        }

        [Test]
        public void TestValidLexer()
        {
            var lexer = new Lexer("1");

            var result = lexer.Lex();
            var tokens = result.Tokens.ToList();

            Assert.IsNull(result.Error, result.Error);
            Assert.AreEqual(2, tokens.Count); // end of file token at the end
            
            
            lexer = new Lexer("1 + 2 ^ 3");
            
            result = lexer.Lex();
            tokens = result.Tokens.ToList();

            Assert.IsNull(result.Error, result.Error);
            Assert.AreEqual(6, tokens.Count);
            
            lexer = new Lexer("1 + 2 ^ 3 * 4");
            
            result = lexer.Lex();
            tokens = result.Tokens.ToList();

            Assert.IsNull(result.Error, result.Error);
            Assert.AreEqual(8, tokens.Count);
            Assert.IsInstanceOf<NumberToken>(tokens[0]);
            Assert.IsInstanceOf<PlusToken>(tokens[1]);
            Assert.IsInstanceOf<NumberToken>(tokens[2]);
            Assert.IsInstanceOf<PowerToken>(tokens[3]);
            Assert.IsInstanceOf<NumberToken>(tokens[4]);
            Assert.IsInstanceOf<MultiplyToken>(tokens[5]);
            Assert.IsInstanceOf<NumberToken>(tokens[6]);
            Assert.IsInstanceOf<EndOfFileToken>(tokens[7]);
        }

        [Test]
        public void TestLexerImplicitMultiplication()
        {
            var lexer = new Lexer("123abc");
            var result = lexer.Lex();
            
            Assert.IsNull(result.Error, result.Error);
            TestContext.Out.WriteLine(result.Error);
            Assert.AreEqual(4, result.Tokens.Count());
            Assert.IsInstanceOf<NumberToken>(result.Tokens.First());
        }
        
        [Test]
        public void TestFullEvalAdditionSubtraction()
        {
            var (succ, err) = _eval.TryEvaluate("5", out var result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(5, result);

            (succ, err) = _eval.TryEvaluate("5 + 3", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(8, result);
            
            (succ, err) = _eval.TryEvaluate("5 + 3 - 2", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(6, result);
            
            (succ, err) = _eval.TryEvaluate("5 + 3 - 4 + 3 - 3", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(4, result);
            
            (succ, err) = _eval.TryEvaluate("- 5 + 3 - - 3 - 5 + 3", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(-1, result);
        }
        
        [Test]
        public void TestFullEvalMultDiv()
        {
            var (succ, err) = _eval.TryEvaluate("5 * 3", out var result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(15, result);

            (succ, err) = _eval.TryEvaluate("5 / 2", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(2.5, result);
            
            (succ, err) = _eval.TryEvaluate("15 / 3 * 2 * 1 / 2", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(5, result);
            
            (succ, err) = _eval.TryEvaluate("50 * 5 * 5 / 10", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(125, result);
            
            (succ, err) = _eval.TryEvaluate("10 / 10 / 10 / 10", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(0.01, result);
        }

        [Test]
        public void TestFullEvalAddSubMultDiv()
        {
            var (succ, err) = _eval.TryEvaluate("5 * 3 + 2", out var result);

            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(17, result);

            (succ, err) = _eval.TryEvaluate("5 - 5 / 2", out result);

            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(2.5, result);

            (succ, err) = _eval.TryEvaluate("15 / 3 + 2 + 1 * 2", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(9, result);
        }
        
        [Test]
        public void TestFullEvalAddSubMultDivPower()
        {
            var (succ, err) = _eval.TryEvaluate("15 ^ 2 - 15 * 15 + 1 * 15", out var result);

            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(15, result);

            (succ, err) = _eval.TryEvaluate("5 - 2 ^ 3 * 4", out result);

            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(-27, result);

            (succ, err) = _eval.TryEvaluate("25 / 5 ^ 2 + 2 + 1 * 2", out result);
            
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void TestConstants()
        {
            var (succ, err) = _eval.TryEvaluate("pi", out var result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(Math.PI, result);
            
            (succ, err) = _eval.TryEvaluate("pi * e", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(Math.PI * Math.E, result);
            
            (succ, err) = _eval.TryEvaluate("3pi + 2e - 6", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(3 * Math.PI + 2 * Math.E - 6, result);
        }
        
        [Test]
        public void TestFunctions()
        {
            var (succ, err) = _eval.TryEvaluate("abs(-5) + abs(3) * 3", out var result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(14, result);
            
            (succ, err) = _eval.TryEvaluate("abs(  -  pi) * (e + 1   )", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(Math.Abs(Math.PI) * (Math.E + 1), result);
            
            (succ, err) = _eval.TryEvaluate("3abs(pi + 2e) - 6", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(3 * Math.Abs(Math.PI + 2 * Math.E) - 6, result);
            
            (succ, err) = _eval.TryEvaluate("round(3.4) + round(3.5) + round(3.6)", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(11, result);
            
            (succ, err) = _eval.TryEvaluate("ceil(3.4) + sign(-531) * sin(90)", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(3, result);
            
            (succ, err) = _eval.TryEvaluate("abs((sin(33) / cos(33)) - tan(33))", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.Less(result, 0.000000001);
        }
        
        [Test]
        public void TestShifts()
        {
            var (succ, err) = _eval.TryEvaluate("1 + 123456 << 2", out var result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(493828, result);
            
            (succ, err) = _eval.TryEvaluate("153 << 2 >> 1", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(306, result);
            
            (succ, err) = _eval.TryEvaluate("153 << 100", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(10514079940608, result);
            
            (succ, err) = _eval.TryEvaluate("153 >> 100", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(0, result);
        }
        
        [Test]
        public void TestBitwise()
        {
            var (succ, err) = _eval.TryEvaluate("1234 | 547 << 2", out var result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(3294, result);
            
            (succ, err) = _eval.TryEvaluate("2 * 1234 & 547 << 2", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(2180, result);
            
            (succ, err) = _eval.TryEvaluate("547 << 1 xor 4444 / 4", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(17, result);
            
            (succ, err) = _eval.TryEvaluate("1 | 2 xor 3", out result);
                        
            Assert.IsNull(err, err);
            Assert.IsTrue(succ);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void Lulz()
        {
            var (succ, err) = _eval.TryEvaluate("2pi-3", out var result);
            
            TestContext.WriteLine(err);
            TestContext.WriteLine($"Result: {result}");
        }
    }
}