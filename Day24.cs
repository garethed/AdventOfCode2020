using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day24 : Day
    {
        HashSet<HexPoint> black = null;

        [Test(1, "nwwswee")]
        [Test(10, testData)]
        public override long Part1(string input)
        {
            
            black = new HashSet<HexPoint>();
            var start = new HexPoint(0,0,0);

            foreach (var line in Utils.splitLines(input))
            {
                var line2 = line.Replace("se", "a").Replace("sw", "b").Replace("ne", "c").Replace("nw", "d");
                var pos = start;

                foreach (var move in line2)
                {
                    switch (move)
                    {
                        case 'e':
                          pos = pos.E();
                          break;
                        case 'w':
                          pos = pos.W();
                          break;
                        case 'a':
                          pos = pos.SE();
                          break;
                        case 'b':
                          pos = pos.SW();
                          break;
                        case 'c':
                          pos = pos.NE();
                          break;
                        case 'd':
                          pos = pos.NW();
                          break;
                    }
                }

                if (black.Contains(pos)) 
                {
                    black.Remove(pos);
                }
                else 
                {
                    black.Add(pos);
                }

            }

            return black.Count;

        }

        [Test(2208, testData)]
        public override long Part2(string input)
        {
            Part1(input);
            
            for (int cycles = 0; cycles < 100; cycles++)
            {
                black = iterate(black);
            }

            return black.Count;
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
                        if (active == 1 || active == 2)
                        {
                            next.Add(neighbour);
                        }
                    }
                    else if (active == 2)
                    {
                        next.Add(neighbour);
                    }
                }
            }

            return next;           
        }        

        const string testData =
@"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew";        
    }
}