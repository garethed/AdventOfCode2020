using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day23 : Day
    {
        public override string Input => "523764819";

        [Test(67384529, testData)]
        public override long Part1(string input)
        {
            var cups = new LinkedList<long>();
            foreach (var c in input)
            {
                cups.AddLast(long.Parse(c.ToString()));                
            }

            iterate(cups, 100);

            var output = 0L;
            var outputNode = cups.Find(1).Next;
            do 
            {
                output = output * 10 + outputNode.Value;
                outputNode = outputNode.Next ?? cups.First;
            } while (outputNode.Value != 1);
            
            return output;                

        }

        void iterate(LinkedList<long> cups, int count)
        {
            var removed = new Stack<LinkedListNode<long>>();
            var lookup = new Dictionary<long, LinkedListNode<long>>();
            var max = cups.Count;

            var node = cups.First;
            while (node != null)            
            {
                lookup[node.Value] = node;
                node = node.Next;
            }

            var current = cups.First;

            for (int i = 0; i < count; i++)
            {
                while (removed.Count < 3)
                {
                    var toRemove = current.Next ?? cups.First;
                    removed.Push(toRemove);
                    cups.Remove(toRemove);
                }

                var target = current.Value;
                LinkedListNode<long> destination = null;

                while (destination == null || removed.Contains(destination))
                {             
                    target--;
                    if (target == 0)
                    {
                        target = max;
                    }
                    destination = lookup[target];
                }

                while (removed.Any())
                {
                    cups.AddAfter(destination, removed.Pop());
                }

                current = current.Next ?? cups.First;                
            }            
        }

        [Test(149245887792, testData)]
        public override long Part2(string input)
        {
            var cups = new LinkedList<long>();
            foreach (var c in input)
            {
                cups.AddLast(long.Parse(c.ToString()));                
            }
            var next = 10;
            while (cups.Count < 1000000)
            {
                cups.AddLast(next);
                next++;
            }

            iterate(cups, 10000000);

            var cup1 = cups.Find(1);
            return circularNext(cup1).Value * circularNext(circularNext(cup1)).Value;         
        }

        private LinkedListNode<T> circularNext<T>(LinkedListNode<T> node)
        {
            return node.Next ?? node.List.First;
        }

        const string testData =
@"389125467";        
    }
}