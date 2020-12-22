using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day22 : Day
    {
        [Test(306, testData)]
        public override long Part1(string input)
        {
            var parts = Utils.SanitizeInput(input).Split("\n\n");
            var q1 = new Queue<int>(Utils.splitLines(parts[0]).Skip(1).Select(l => int.Parse(l)));
            var q2 = new Queue<int>(Utils.splitLines(parts[1]).Skip(1).Select(l => int.Parse(l)));


            while (q1.Any() && q2.Any())
            {
                var c1 = q1.Dequeue();
                var c2 = q2.Dequeue();

                if (c1 > c2) 
                {
                    q1.Enqueue(c1);
                    q1.Enqueue(c2);
                }
                else
                {
                    q2.Enqueue(c2);
                    q2.Enqueue(c1);
                }
            }

            var q = q1.Concat(q2).ToList();
            var result = 0;

            while (q.Any())
            {
                result += q[0] * q.Count;
                q.RemoveAt(0);
            }

            return result;

        
        }


        [Test(291, testData)]
        public override long Part2(string input)
        {
            var parts = Utils.SanitizeInput(input).Split("\n\n");
            var q1 = new Queue<int>(Utils.splitLines(parts[0]).Skip(1).Select(l => int.Parse(l)));
            var q2 = new Queue<int>(Utils.splitLines(parts[1]).Skip(1).Select(l => int.Parse(l)));

            var winner = RecursiveCombat(q1, q2);
            var q = (winner == 1 ? q1 : q2).ToList();
        
            var result = 0;

            while (q.Any())
            {
                result += q[0] * q.Count;
                q.RemoveAt(0);
            }

            return result;

        }

        private int RecursiveCombat(Queue<int> q1, Queue<int> q2)
        {
            var previous = new HashSet<string>();

            while (q1.Any() && q2.Any())
            {

                var position = String.Join(',', q1) + "|" + String.Join(',', q2);
                var winner = 1;

                if (previous.Contains(position))
                {
                    return 1;
                }
                else
                {
                    previous.Add(position);

                    var c1 = q1.Dequeue();
                    var c2 = q2.Dequeue();
                    
                    if (q1.Count >= c1 && q2.Count >= c2)
                    {
                        var rq1 = new Queue<int>(q1.Take(c1));
                        var rq2 = new Queue<int>(q2.Take(c2));

                        winner = RecursiveCombat(rq1, rq2);
                    }
                    else 
                    {
                        winner = c1 > c2 ? 1 : 2;
                    }

                    if (winner == 1)
                    {
                        q1.Enqueue(c1);
                        q1.Enqueue(c2);
                    }
                    else
                    {
                        q2.Enqueue(c2);
                        q2.Enqueue(c1);
                    }
                }
            }

            return q1.Count > q2.Count ? 1 : 2;
        }

        const string testData =
@"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10";        
    }
}