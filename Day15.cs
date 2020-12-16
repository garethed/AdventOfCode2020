using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day15 : Day
    {
        [Test(436, "0,3,6")]
        [Test(1836, "3,1,2")]
        public override long Part1(string input)
        {
            return calc(input, 2020);
        }
        long calc(string input, int cycles)
        {
            var dict = new Dictionary<int, int>();
            var start = input.Split(",").Select(i => int.Parse(i)).ToArray();

            for (var i = 0; i < start.Length - 1; i++)
            {
                dict[start[i]] = i + 1;
            }

            var prev = start.Last();

            for (var i = start.Length; i < cycles; i++ )
            {
                var next = dict.GetValueOrDefault(prev);
                dict[prev] = i;

                if (next != 0) {
                    next = i - next;
                }

                prev = next;
                //Console.Write(prev + " ");

            }

            return prev;
            
        }

        [Test(175594, "0,3,6")]
        public override long Part2(string input)
        {
            return calc(input, 30000000);
        }

        public override string Input => "2,1,10,11,0,6";

        const string testData =
@"";        
    }
}