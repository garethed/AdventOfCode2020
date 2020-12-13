using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day13 : Day
    {
        public override long Part1(string input)
        {
            var data = Utils.splitLines(input).ToArray();
            int departureTime = int.Parse(data[0]);
            var buses = data[1].Split(',').Where (s => s != "x").Select(s => int.Parse(s));

            var minWait = int.MaxValue;
            var bus = 0;

            foreach (var b in buses)
            {
                var wait = b - (departureTime % b);
                if (wait < minWait)
                {
                    minWait = wait;
                    bus = b;
                }

                
            }

            return bus * minWait;

        }

        [Test(1068781, testData)]
        public override long Part2(string input)
        {
            var data = Utils.splitLines(input).ToArray();
            var buses = data[1].Split(',').Select(s => s == "x" ? 0 : long.Parse(s)).ToArray();

            var x = buses[0]; // 5
            var dx = buses[0]; // 5

            for (int i = 1; i < buses.Length; i++)
            {
                var bus = buses[i];   // 7

                if (buses[i] != 0)
                {   
                    var next = x;

                    while (bus - (next % bus) !=  i % bus)  
                    {
                        next += dx;
                    }

                    x = next;
                    dx *= bus;
                }
            }

            return x;



        }

        const string testData =
@"939
7,13,x,x,59,x,31,19";        
    }
}