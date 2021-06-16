using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            var openCount = 0;
            while (true)
            {
                var maybeChar = GetNext();
                if (!(maybeChar is char c))
                    break;

                if (char.IsWhiteSpace(c))
                    continue;

                if (char.IsLetter(c))
                {
                    
                    var startPos = _position - 1;
                    while (Peek() is char ch && (char.IsLetter(ch) || ch == '.'))
                    {
                        GetNext();
                    }
                
                    if (startPos != _position)
                    {
                        var name = _input.Substring(startPos, _position - startPos);
                        if(name.ToLowerInvariant() == "xor")
                            tokens.Add(new LogicalXorToken());
                        else
                        {
                            // if previous token is number token
                            // we can inject a multiply token in here
                            // to simulate 2pi => 2*pi
                            if (tokens.LastOrDefault() is NumberToken)
                                tokens.Add(new MultiplyToken());
                                    
                            tokens.Add(new NameToken(name));
                        }

                        continue;
                    }
                }

                switch (c)
                {
                    case '0': case '1': case '3': case '4':
                    case '5': case '6': case '7': case '8':
                    case '9': case '.': case '2':
                        var startPos = _position - 1;
                        while (Peek() is char ch && (char.IsDigit(ch) || ch == '.'))
                        {
                            GetNext();
                        }
                    
                        var numberStr = _input.Substring(startPos, _position - startPos);
                        if (!double.TryParse(numberStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
                        {
                            return new LexResult(tokens, $"Invalid number '{numberStr}' at position {startPos}");
                        }
                
                        tokens.Add(new NumberToken(val));
                        break;
                    case '+':
                        tokens.Add(new PlusToken());
                        break;
                    case '-':
                        tokens.Add(new MinusToken());
                        break;
                    case '/':
                        tokens.Add(new DivideToken());
                        break;
                    case '*':
                        tokens.Add(new MultiplyToken());
                        break;
                    case '^':
                        tokens.Add(new PowerToken());
                        break;
                    case '&':
                        tokens.Add(new LogicalAndToken());
                        break;
                    case '|':
                        tokens.Add(new LogicalOrToken());
                        break;
                    case '(':
                        tokens.Add(new OpenBracketToken());
                        ++openCount;
                        break;
                    case ')' when --openCount < 0:
                        return new LexResult
                        (
                            tokens, 
                            $"Closed bracket token '{c}' at position {_position} doesn't have a matching open bracket."
                        );
                    case ')':
                        tokens.Add(new ClosedBracketToken());
                        break;
                    case '<':
                    {
                        var next = GetNext();
                        if (next is '<')
                        {
                            tokens.Add(new LeftShiftToken());
                        }
                        else
                        {
                            return new LexResult(tokens,
                                $"Invalid token '<' at position {_position}. Did you mean '<<' ?");
                        }

                        break;
                    }
                    case '>':
                    {
                        var next = GetNext();
                        if (next is '>')
                        {
                            tokens.Add(new RightShiftToken());
                        }
                        else
                        {
                            return new LexResult(tokens,
                                $"Invalid token '>' at position {_position}. Did you mean '>>' ?");
                        }

                        break;
                    }
                    default:
                        return new LexResult(tokens, $"Invalid token '{c}' at position {_position}");
                }
            }

            tokens.Add(new EndOfFileToken());
            if (openCount > 0)
            {
                return new LexResult(tokens, "Not all open brackets have matching closed brackets.");
            }
            
            return new LexResult(tokens);
        }
    }
}