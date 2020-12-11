using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day5 : Day
    {
        public override long Part1(string input)
        {
            return Utils.splitLines(input).Select(l => idFromCode(l)).Max();
        }

        public override long Part2(string input)
        {
            int i1 = -99;

            foreach (var i2 in Utils.splitLines(input).Select(l => idFromCode(l)).OrderBy(i => i)) {
                if (i2 - i1 == 2) 
                {
                    return i1 + 1;
                }
                i1 = i2;


            }

            return -1;
            
        }

/*
    BFFFBBFRRR: row 70, column 7, seat ID 567.
    FFFBBBFRRR: row 14, column 7, seat ID 119.
    BBFFBBFRLL: row 102, column 4, seat ID 820.
*/

        [Test(567, "BFFFBBFRRR")]
        int idFromCode(string seatcode)
        {
            return parseCode(seatcode).id;
        }

        Seat parseCode(string seatcode)
        {
            var r = Convert.ToInt32(seatcode.Substring(0, 7).Replace("B", "1").Replace("F", "0"),2);
            var c = Convert.ToInt32(seatcode.Substring(7).Replace("R", "1").Replace("L", "0"),2);

            return new Seat(r,c);
            
        }

        record Seat(int r, int c)
        {
            public int id = r * 8 + c;        
        }

    }
}