using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day18 : Day
    {
        [Test(51, "1 + (2 * 3) + (4 * (5 + 6))")]
        public override long Part1(string input)
        {          
            return Utils.splitLines(input).Sum(l => evaluate(tokenise(l)));
        }

        Queue<string> tokenise(string input) 
        {
            input = input.Replace("(", " ( ").Replace(")", " ) ");
            return new Queue<string>(input.Split(" ").Where(t => !string.IsNullOrWhiteSpace(t)));

        }

        long evaluate (Queue<string> tokens)
        {
            var acc = getNextValue(tokens);

            while (tokens.Count > 0)
            {
                switch (tokens.Dequeue())
                {
                    case "+":
                        acc += getNextValue(tokens);
                        break;
                    case "*":
                        acc *= getNextValue(tokens);
                        break;
                    case ")":
                        return acc;
                }
            }

            return acc;
        }

        private long getNextValue(Queue<string> tokens)
        {
            var next = tokens.Dequeue();
            if (next == "(")
            {
                return p2 ? evaluate2(tokens) : evaluate(tokens);
            }
            return long.Parse(next);
        }

        [Test(1445, "5 + (8 * 3 + 9 + 3 * 4 * 3)")]
        public override long Part2(string input)
        {
            p2 = true;
            return Utils.splitLines(input).Sum(l => evaluate2(tokenise(l)));
        
        }

        long evaluate2 (Queue<string> tokens)
        {
            var product = 1L;
            var acc = getNextValue(tokens);

            while (tokens.Count > 0)
            {
                switch (tokens.Dequeue())
                {
                    case "+":                        
                        acc += getNextValue(tokens);
                        break;
                    case "*":
                        product *= acc;
                        acc = getNextValue(tokens);
                        break;
                    case ")":
                        return product * acc;
                }
            }

            return product * acc;
        }        

        bool p2;        

        const string testData =
@"";        
    }
}