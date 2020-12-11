using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day11 : Day
    {
        [Test(37, testData)]
        public override long Part1(string input)
        {
            var grid = Utils.stringToGrid(input, c => c);

            while (iterate(grid)) ;

            return grid.flatten().Count(c => c == '#');


        }

        private bool iterate(char[,] grid)
        {
            var prev = (char[,]) grid.Clone();
            var changed = false;

            for( int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    var occupied = Utils.neighbours(prev, x, y).Count(c => c == '#');
                    if (prev[x,y] == 'L' && occupied == 0)
                    {
                        grid[x,y] = '#';
                        changed = true;
                    }
                    else if (prev[x,y] == '#' && occupied >= 4)
                    {
                        grid[x,y] = 'L';
                        changed = true;
                    }

                }
            }

            //Utils.describe(grid);

            return changed;
        }

        [Test(26,testData)]
        public override long Part2(string input)
        {
            var grid = Utils.stringToGrid(input, c => c);

            while (iterate2(grid)) ;

            return grid.flatten().Count(c => c == '#');            
        }

        private bool iterate2(char[,] grid)
        {
            var prev = (char[,]) grid.Clone();
            var changed = false;

            for( int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    var occupied = longNeighbours(prev, x, y).Count(c => c == '#');
                    if (prev[x,y] == 'L' && occupied == 0)
                    {
                        grid[x,y] = '#';
                        changed = true;
                    }
                    else if (prev[x,y] == '#' && occupied >= 5)
                    {
                        grid[x,y] = 'L';
                        changed = true;
                    }

                }
            }

            //Utils.describe(grid);

            return changed;
        }      

        public static IEnumerable<char> longNeighbours(char[,] grid, int x, int y)
        {            
            foreach (var dx in new[] {  -1, 0, 1})
            {
                foreach (var dy in new[] { -1, 0, 1})
                {
                    var x2 = x + dx;
                    var y2 = y + dy;

                    while (x2 >= 0 && x2 < grid.GetLength(0) && y2 >= 0 && y2 < grid.GetLength(1) && (x2 != x || y2 != y))
                    {
                        if (grid[x2,y2] != '.')
                        {
                            yield return grid[x2,y2];
                            break;
                        }
                        x2 += dx;
                        y2 += dy;
                    }
                }
            }
        }          

        const string testData =
@"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
    }
}