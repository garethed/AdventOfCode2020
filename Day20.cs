using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day20 : Day
    {
        Grid grid;

        [Test(20899048083289, testData)]
        public override long Part1(string input)
        {
            var map = new Dictionary<string,HashSet<Tile>>();
            var tileData = Utils.SanitizeInput(input).Split("\n\n");
            foreach (var tile in tileData)
            {
                new Tile(tile, map);
            }

            grid = new Grid(map);
            grid.Assign(map.First().Value.First(), new Point(0,0));

            grid.Extend();



            var xmin = grid.grid.Keys.Select(p => p.x).Min();
            var xmax = grid.grid.Keys.Select(p => p.x).Max();
            var ymin = grid.grid.Keys.Select(p => p.y).Min();
            var ymax = grid.grid.Keys.Select(p => p.y).Max();

            var prod = 1L;

            foreach( var x in new[] { xmin, xmax}) 
            {
                foreach( var y in new[] { ymin, ymax}) 
                {
                    prod *= grid.grid[new Point(x,y)].id;
                }

            }

            grid.debug();

            return prod;            
            
        }

        [Test(273, testData)]
        public override long Part2(string input)
        {
            Part1(input);

            var xmin = grid.grid.Keys.Select(p => p.x).Min();
            var xmax = grid.grid.Keys.Select(p => p.x).Max();
            var ymin = grid.grid.Keys.Select(p => p.y).Min();
            var ymax = grid.grid.Keys.Select(p => p.y).Max();

            var image = new bool[(xmax - xmin + 1) * 8, (ymax - ymin + 1) * 8];

            for (int tx = 0; tx <= xmax - xmin; tx++)
            {
                for (int ty = 0; ty <= ymax - ymin; ty++)
                {
                    var tile = grid.grid[new Point(xmin + tx, ymin + ty)];

                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            image[8 * tx + x, 8 * ty + y] = tile.data[x + 1,y + 1];
                        }                        
                        
                    }

                }
            }

            var seaMonsters = new bool[(xmax - xmin + 1) * 8, (ymax - ymin + 1) * 8];

            var seaMonster = Utils.stringToGrid(seaMonsterText, c => c == '#');

            Func<bool[,], bool[,]> reflect = (a => Utils.ReflectX(a));
            Func<bool[,], bool[,]> rotate = (a => Utils.Rotate(a));

            findSeaMonster(seaMonster, image, seaMonsters);

            foreach (var op in new[] { rotate, rotate, rotate, reflect, rotate, rotate, rotate })
            {
                seaMonster = op(seaMonster);

                findSeaMonster(seaMonster, image, seaMonsters);

            }

            Utils.debug(Utils.Rotate(Utils.Rotate(Utils.Rotate(image))));
            Utils.debug(Utils.Rotate(Utils.Rotate(Utils.Rotate(seaMonsters))));

            return image.flatten().Count(b => b) -  seaMonsters.flatten().Count(b => b);            
        }

        private void findSeaMonster(bool[,] seaMonster, bool[,] image, bool[,] seaMonsters)
        {
            for (int x = 0; x <= image.GetLength(0) - seaMonster.GetLength(0); x++)
            {
                for (int y = 0; y <= image.GetLength(1) - seaMonster.GetLength(1); y++)
                {
                    bool match = true;

                    for (var x2 = 0; match && x2 < seaMonster.GetLength(0); x2++)
                    {
                        for (var y2 = 0; match && y2 < seaMonster.GetLength(1); y2++)
                        {
                            match &= (image[x + x2, y + y2] || !seaMonster[x2,y2]);
                        }
                        
                    }

                    if (match) 
                    {
                        for (var x2 = 0; match && x2 < seaMonster.GetLength(0); x2++)
                        {
                            for (var y2 = 0; match && y2 < seaMonster.GetLength(1); y2++)
                            {
                                if (seaMonster[x2,y2])
                                {
                                    seaMonsters[x + x2, y + y2] = true;

                                }
                            }                            
                        }
                        
                    }
                }

            }
        }

        const string seaMonsterText = 
@"                  # 
#    ##    ##    ###
 #  #  #  #  #  #   ";            

        class Grid 
        {
            private readonly Dictionary<string, HashSet<Tile>> map;
            public Dictionary<Point, Tile> grid = new Dictionary<Point, Tile>();

            public Grid(Dictionary<string, HashSet<Tile>> map)
            {
                this.map = map;
            }

            internal void Assign(Tile tile, Point location)
            {
                grid[location] = tile;
                tile.UpdateGrid();
                Extend(tile, location);
                                
            }

            internal void Extend()
            {
                var tiles = map.Values.SelectMany(s => s).Distinct().Count();

                while (grid.Count < tiles)
                {
                    foreach (var kv in grid)
                    {
                        Extend(kv.Value, kv.Key);
                    }
                }
            }

            void Extend(Tile tile, Point location)
            {
                TryExtend(tile, 0, location.Move(0, -1));
                TryExtend(tile, 1, location.Move(1, 0));
                TryExtend(tile, 2, location.Move(0, 1));
                TryExtend(tile, 3, location.Move(-1, 0));                                
            }

            private void TryExtend(Tile tile, int direction, Point point)
            {
                if (grid.ContainsKey(point)) {
                    return;
                }

                
                var side = tile.sides[direction].Reverse();
                var matches = map[side].Except(grid.Values).ToArray();
                if (matches.Length == 1)
                {
                    var match = matches[0];
                    var matchingDirection = (direction + 2) % 4;
                    for (int r = 0; r < 4; r++)
                    {
                        if (match.sides[matchingDirection] == side)
                        {
                            Assign(match, point);
                            return;
                        }
                        match.Rotate();
                    }

                    match.Flip();

                    for (int r = 0; r < 4; r++)
                    {
                        if (match.sides[matchingDirection] == side)
                        {
                            Assign(match, point);
                            return;
                        }
                        match.Rotate();
                    }                    
                }
            }

            public void debug() {

                var xmin = grid.Keys.Select(p => p.x).Min();
                var xmax = grid.Keys.Select(p => p.x).Max();
                var ymin = grid.Keys.Select(p => p.y).Min();
                var ymax = grid.Keys.Select(p => p.y).Max();

                for (int ty = 0; ty <= ymax - ymin; ty++)
                {
                    Console.Write("\n");

                    for (int y = 0; y < 10; y++)
                    {
                        Console.Write("\n");

                        for (int tx = 0; tx <= xmax - xmin; tx++)
                        {
                            Console.Write(" ");

                            var tile = grid[new Point(xmin + tx, ymin + ty)];

                            for (int x = 0; x < 10; x++)
                            {
                                var data = tile.data[x,y];
                                Console.Write(data ? '#' : '.');                                
                            }
                        }

                    }
                }

            }
        }


        class Tile
        {
            public int id;
            public bool[,] data;

            private string[] unflippedSides;
            private string[] flippedSides;
            private int rotation;
            private bool flip;
            public string[] sidesBeforeRotation { get => flip ? flippedSides : unflippedSides;}

            public string N { get => sidesBeforeRotation[ (0 + rotation) % 4] ; }
            public string E { get => sidesBeforeRotation[ (1 + rotation) % 4] ; }
            public string S { get => sidesBeforeRotation[ (2 + rotation) % 4] ; }
            public string W { get => sidesBeforeRotation[ (3 + rotation) % 4] ; }

            public string[] sides {
                get {
                    return new[] { N, E, S, W };
                }
            }

            public void Rotate() { rotation++; }
            public void Flip() { flip = !flip; }       

            public void UpdateGrid() {

                if (flip)
                {
                    data = Utils.ReflectX(data);
                }



                for (int r = 0; r < rotation; r++)
                {
                    data = Utils.Rotate(data);
                }

            }     

            public Tile(string data, Dictionary<string,HashSet<Tile>> map)
            {
                flippedSides = new string[4];
                unflippedSides = new string[4];
                this.data = new bool[10,10];
                rotation = 0;
                flip = false;

                var lines = Utils.splitLines(data).ToArray();
                id = int.Parse(lines[0].TrimEnd(':').Substring(5));

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        this.data[x,y] = lines[y + 1][x] == '#';
                    }
                }

                for (int r = 0; r < 4; r++)
                {
                    var side = "";

                    for (int x = 0; x < 10; x++)
                    {
                        side = side + (this.data[x,0] ? '#' : ' ');
                    }

                    RotateData();
                    map.Add(side, this);
                    map.Add(side.Reverse(), this);

                    
                    unflippedSides[r] = side;
                    //flippedSides[r] = side;
                    flippedSides[ (r % 2 == 1) ? (4 - r) : r] = side.Reverse();
                }

            }

            private void RotateData()
            {
                bool[,] newData = new bool[10,10];

                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        newData[y, 9 - x] = data[x,y];
                    }
                }

                data = newData;

            }

            public override string ToString()
            {
                return id.ToString();
            }
        }

        const string testData =
@"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###...";        
    }
}