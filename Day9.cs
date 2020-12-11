using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day9 : Day
    {
        public override long Part1(string input)
        {            
            return Part1(input, 25);
        }

        [Test(127, testInput, 5)]
        public long Part1(string input, int windowSize)
        {
            long[] data = Utils.splitLines(input).Select(l => long.Parse(l)).ToArray();
            int count = windowSize;

            foreach (var valid in TestValidity(data, windowSize))
            {
                if (!valid) 
                {
                    return data[count];
                }
                count++;
            }

            return -1;

        }

        public override long Part2(string input)
        {
            var target = Part1(input);
            long[] data = Utils.splitLines(input).Select(l => long.Parse(l)).ToArray();

            for (int i = 0; ; i++)
            {
                var sum = 0l;
                var min = long.MaxValue;
                var max = long.MinValue;
                for (int l = 0; sum < target; l++)
                {
                    sum += data[i + l];
                    min = Math.Min(min, data[i + l]);
                    max = Math.Max(max, data[i + l]);

                    if (sum == target)
                    {
                        return min + max;
                    }
                }
            }

        }

        IEnumerable<bool> TestValidity(IEnumerable<long> data, int windowSize)
        {
            var previous = new Queue<long>();

            foreach (var number in data)
            {
                if (previous.Count >= windowSize)
                {
                     yield return canMakeTarget(number, previous);
                     previous.Dequeue();
                }

                previous.Enqueue(number);                
            }
        }

        private bool canMakeTarget(long number, Queue<long> previous)
        {
            foreach (var i in previous)
            {
                foreach (var j in previous)
                {
                    if (i + j == number && i != j)
                    {
                        return true;
                    }
                }
            }

            return false;
            
        }

        const string testInput =
@"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";
    }
}