using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day17 : Day
    {
        [Test(112,testData)]
        public override long Part1(string input)
        {
            var active = new HashSet<Point3>();

            var parsedInput = Utils.stringToGrid(input, c => c == '#');
            for (int x = 0; x < parsedInput.GetLength(0); x++)
            {
                for (int y = 0; y < parsedInput.GetLength(0); y++)
                {
                    if (parsedInput[x,y])
                    {
                        active.Add(new Point3(x, y, 0));
                    }
                }

            }

            for (int cycles = 0; cycles < 6; cycles++)
            {
                active = iterate(active);
            }

            return active.Count;            
        }

        private HashSet<T> iterate<T>(HashSet<T> input)  where T : PointN<T>
        {
            var next = new HashSet<T>();

            foreach (var point in input)
            {
                foreach (var neighbour in point.SelfAndNeighbours)
                {
                    var active = neighbour.Neighbours.Count(p => input.Contains(p));
                    if (input.Contains(neighbour))
                    {
                        if (active == 2 || active == 3)
                        {
                            next.Add(neighbour);
                        }
                    }
                    else if (active == 3)
                    {
                        next.Add(neighbour);
                    }
                }
            }

            return next;           
        }

       [Test(848,testData)]
        public override long Part2(string input)
        {
            var active = new HashSet<Point4>();

            var parsedInput = Utils.stringToGrid(input, c => c == '#');
            for (int x = 0; x < parsedInput.GetLength(0); x++)
            {
                for (int y = 0; y < parsedInput.GetLength(0); y++)
                {
                    if (parsedInput[x,y])
                    {
                        active.Add(new Point4(x, y, 0, 0));
                    }
                }

            }

            for (int cycles = 0; cycles < 6; cycles++)
            {
                active = iterate(active);
            }

            return active.Count;            
        }
        
        const string testData =
@".#.
..#
###";        
    }
}