using System;
using System.Linq;

namespace AdventOfCode2020 {

    class Day1 : Day
    {
        [Test(514579, testInput)]
        public override int Part1(string input)
        {
            var data = Utils.splitLines(input).Select(s => int.Parse(s)).OrderBy(i => i).ToArray();
                      
            return findPair(data, 2020, -1);

            throw new InvalidOperationException();
        }

        private int findPair(int[] data, int target, int excludeIndex) {
            var i = 0;
            var j = data.Length - 1;

            while (true) {
                while(data[i] + data[j] > target)
                {
                    j--;
                    if (j == excludeIndex) {
                        j--;
                    }
                }
                if (data[i] + data[j] == target) 
                {
                    return (data[i] * data[j]);
                }
                
                i++;
                if (i == excludeIndex)
                {
                    i++;
                }

                if (i >= j) 
                {
                    return -1;
                }
            }

        }

        [Test(241861950, testInput)]
        public override int Part2(string input)
        {
            var data = Utils.splitLines(input).Select(s => int.Parse(s)).OrderBy(i => i).ToArray();

            for (int i = 0; i < data.Length; i++)
            {
                var n = data[i];
                var pair = findPair(data, 2020 - n, i);
                if (pair > 0) 
                {
                    return (pair * n);
                }

            }

            return -1;
        }

        const string testInput = 
@"1721
979
366
299
675
1456";
    }
}