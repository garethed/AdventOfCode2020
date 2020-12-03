using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day3 : Day
    {
        [Test(7, testData)]
        public override int Part1(string input)
        {
            var grid = Utils.stringToGrid(input, c => c == '#');
            
            //Utils.debug(grid);

            return CountTrees(grid, 3, 1);
        }

        int CountTrees(bool[,] grid, int dx, int dy)
        {
            var w = grid.GetLength(0);
            return Enumerable.Range(0, grid.GetLength(1) / dy).Select(i => grid[(dx * i) % w, dy * i]).Count(b => b);

        }

        [Test(336, testData)]
        public override int Part2(string input)
        {

/*
    Right 1, down 1.
    Right 3, down 1. (This is the slope you already checked.)
    Right 5, down 1.
    Right 7, down 1.
    Right 1, down 2.
*/
            var grid = Utils.stringToGrid(input, c => c == '#');
            return CountTrees(grid, 1, 1) * CountTrees(grid, 3, 1) * CountTrees(grid, 5, 1) * CountTrees(grid, 7, 1) * CountTrees(grid, 1, 2);
        }


        const string testData = 
@"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
    }
}