using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020
{

    [RegexDeserializable(@"\<x=(?<x>\-?\d+), y=(?<y>\-?\d+), z=(?<z>\-?\d+)\>")]
    public struct Point3 : IEquatable<Point3>, PointN<Point3>
    {
        public Point3(int x, int y, int z) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point3 Offset(int dx, int dy, int dz)
        {
            return new Point3(x + dx, y + dy, z + dz);
        }

        public int x;
        public int y;
        public int z;

        internal Point3 Offset(Point3 adjustment)
        {
            return Offset(adjustment.x, adjustment.y, adjustment.z);
        }

        internal Point3 Scale(int v)
        {
            return new Point3(x * v, y * v, z * v);
        }

        public int Magnitude => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public IEnumerable<Point3> SelfAndNeighbours 
        { 
            get 
            {
                foreach (var dx in deltas)
                {
                    foreach (var dy in deltas)
                    {
                        foreach (var dz in deltas)
                        {
                            yield return Offset(dx, dy, dz);
                        }
                    }
                }

            }
        }

        public IEnumerable<Point3> Neighbours 
        { 
            get 
            { 
                foreach (var dx in deltas)
                {
                    foreach (var dy in deltas)
                    {
                        foreach (var dz in deltas)
                        {
                            if (dx != 0 || dy != 0 || dz != 0) 
                            {
                                yield return Offset(dx, dy, dz);
                            }
                        }
                    }
                }                
            } 
        }

        private static int[] deltas = new[] { -1, 0, 1};

        public override string ToString()
        {
            return $"<x={x}, y={y}, z={z}>";
        }

        public bool Equals(Point3 other)
        {
            return other.x == x && other.y == y && other.z == z;
        }
    }
}