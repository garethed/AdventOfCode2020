using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day6 : Day
    {
        [Test(11, testData)]
        public override long Part1(string input)
        {
            return Calc(input, true);
        }

        public int Calc(string input, bool combine)
        {
            HashSet<char> s = null;
            var c = 0;

            foreach (var l in Utils.splitLines(input))
            {
                if (l == "")
                {
                    c += s.Count;
                    s = null;
                }
                else 
                {
                    var s2 = l.ToArray().ToHashSet();
                    if (s == null)
                    {
                        s = s2;
                    }
                    else
                    {
                        if (combine) {
                            s.UnionWith(s2);
                        }
                        else 
                        {
                            s.IntersectWith(s2);
                        }

                    }
                }
            }

            if (s != null)
            {
                c += s.Count;
            }

            return c;            
        }

        public override long Part2(string input)
        {
            return Calc(input, false);
        }

        const string testData = 
@"abc

a
b
c

ab
ac

a
a
a
a

b";
    }
}