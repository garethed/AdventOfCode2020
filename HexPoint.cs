using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020
{

    record HexPoint (int x, int y, int z) : PointN<HexPoint>
    {
        public IEnumerable<HexPoint> Neighbours 
        {
            get 
            {
                return new[] { E(), W(), NE(), NW(), SE(), SW() };
            }
        }

        public IEnumerable<HexPoint> SelfAndNeighbours 
        {
            get
            {
                return Neighbours.Append(this);
            }
        }

        public HexPoint E() => this with { x = x + 1, y = y - 1 };        
        public HexPoint W() => this with { x = x - 1, y = y + 1 };
        public HexPoint NE() => this with { x = x + 1, z = z - 1 };        
        public HexPoint NW() => this with { y = y + 1, z = z - 1 };        
        public HexPoint SE() => this with { y = y - 1, z = z + 1 };        
        public HexPoint SW() => this with { x = x - 1, z = z + 1 };

    }
}