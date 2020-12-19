using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020
{

    public struct Point4 : IEquatable<Point4>, PointN<Point4>
    {
        public Point4(int x, int y, int z, int w) 
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Point4 Offset(int dx, int dy, int dz, int dw)
        {
            return new Point4(x + dx, y + dy, z + dz, w + dw);
        }

        public int x;
        public int y;
        public int z;
        public int w;

        public IEnumerable<Point4> SelfAndNeighbours 
        { 
            get 
            {
                foreach (var dx in deltas)
                {
                    foreach (var dy in deltas)
                    {
                        foreach (var dz in deltas)
                        {
                            foreach (var dw in deltas)
                            {
                                yield return Offset(dx, dy, dz, dw);
                            }
                        }
                    }
                }

            }
        }

        public IEnumerable<Point4> Neighbours 
        { 
            get 
            { 
                foreach (var dx in deltas)
                {
                    foreach (var dy in deltas)
                    {
                        foreach (var dz in deltas)
                        {
                            foreach (var dw in deltas)
                            {
                                if (dx != 0 || dy != 0 || dz != 0 || dw != 0) 
                                {
                                    yield return Offset(dx, dy, dz, dw);
                                }
                            }
                        }
                    }
                }                
            } 
        }

        private static int[] deltas = new[] { -1, 0, 1};

        public override string ToString()
        {
            return $"<x={x}, y={y}, z={z}, w={w}>";
        }

        public bool Equals(Point4 other)
        {
            return other.x == x && other.y == y && other.z == z && other.w == w;
        }
    }

    public interface PointN<T> where T : PointN<T>  {
        IEnumerable<T> Neighbours { get; }
        IEnumerable<T> SelfAndNeighbours { get;} 
    }
}