using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day10 : Day
    {
        [Test(220, testData)]
        public override long Part1(string input)
        {
            var all = Utils.splitLines(input).Select(s => int.Parse(s)).ToArray();
            var used = new Stack<int>();
            Array.Sort(all);

            return recurseBuildChain(all, used, 0, -1);
        }

        long total = 0;

        private long recurseBuildChain(int[] all, Stack<int> used, int startIndex, int target)
        {
            if (target <= 0 && used.Count == all.Length)
            {
                var ones = 0;
                var threes = 0;
                var prev = 0;

                foreach (var i in used.Reverse()) 
                {
                    var diff = i - prev;
                    if (diff == 1) 
                    {
                        ones++;
                    }
                    if (diff == 3)
                    {
                        threes++;
                    }
                    prev = i;
                }

                return (ones * (threes + 1));
            }
            else if (target > 0 && used.Any() && used.Peek() == target)
            {
                total++;
                return -1;
            }
            else
            {
                var current = used.Any() ? used.Peek() : 0;

                for (var index = startIndex; index < all.Length && all[index] <= current + 3; index++)
                {
                    var candidate = all[index];
                    if (candidate <= current)
                    {
                        startIndex = index;
                    }
                    else if (!used.Contains(candidate))
                    {
                        used.Push(candidate);
                        var result = recurseBuildChain(all, used, startIndex, target);
                        if (result > 0) 
                        {
                            return result;
                        }
                        used.Pop();
                    }
                }
            }

            return -1;
        }

        [Test(19208, testData)]
        public override long Part2(string input)
        {
            var all = Utils.splitLines(input).Select(s => int.Parse(s)).ToArray();
            Array.Sort(all);

            var grandtotal = 1L;

            var targets = new List<int>();
            
            for (int i = 0; i < all.Length - 1; i++)
            {
                if (all[i+1] - all[i] == 3) 
                {
                    targets.Add(all[i]);
                }
            }

            targets.Add(all.Max());


            var prev = 0;

            foreach (var target in targets)
            {
                var subset = all.Where(i => i > prev && i <= target).ToArray();
                total = 0;
                var used= new Stack<int>();
                used.Push(prev);
                recurseBuildChain(subset, used , 0, target);

                grandtotal *= total;
                prev = target;
            }






            return grandtotal;
            
        }

        const string testData =
@"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
    }
}