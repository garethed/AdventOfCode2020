using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day12 : Day
    {
        [Test(25, testData)]
        public override long Part1(string input)
        {
            var d = Point.East;
            var p = new Point();

            foreach (var line in Utils.splitLines(input)) 
            {
                var value = int.Parse(line.Substring(1));

                switch (line[0])
                {
                    case 'N':
                        p += Point.North * value;
                        break;
                    case 'E':
                        p += Point.East * value;
                        break;
                    case 'S':
                        p += Point.South * value;
                        break;
                    case 'W':
                        p += Point.West * value;
                        break;
                    case 'F':
                        p += d * value;
                        break;
                    case 'L':
                        d = d.RotateClockwise(-value);
                        break;
                    case 'R':
                        d = d.RotateClockwise(value);
                        break;
                }
           }

           return p.d; 
        }

        [Test(286, testData)]
        public override long Part2(string input)
        {
var d = Point.East;
            var w = Point.East * 10 + Point.North;
            var s = new Point();

            foreach (var line in Utils.splitLines(input)) 
            {
                var value = int.Parse(line.Substring(1));

                switch (line[0])
                {
                    case 'N':
                        w += Point.North * value;
                        break;
                    case 'E':
                        w += Point.East * value;
                        break;
                    case 'S':
                        w += Point.South * value;
                        break;
                    case 'W':
                        w += Point.West * value;
                        break;
                    case 'F':
                        s += w * value;
                        break;
                    case 'L':
                        w = w.RotateClockwise(-value);
                        break;
                    case 'R':
                        w = w.RotateClockwise(value);
                        break;
                }
           }

           return s.d;
                   }

        const string testData =
@"F10
N3
F7
R90
F11";        
    }
}