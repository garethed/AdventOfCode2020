using System;

public struct Point {

    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }
    public int x;
    public int y;
    public int d => Math.Abs(x) + Math.Abs(y);

    internal Point Move(int dx, int dy)
    {
        return new Point(x + dx, y + dy);
    }

    public static bool operator ==(Point p1, Point p2)
    {
        return p1.Equals(p2);
    }
    public static bool operator !=(Point p1, Point p2)
    {
        return !p1.Equals(p2);
    }

    public override bool Equals(object obj)
    {
        return  obj is Point && x == ((Point)obj).x && y == ((Point)obj).y;
    }

    public override int GetHashCode() {
        return 486187739 * x + y;
    }

    internal Point RotateRight()
    {
        return new Point(y, -x);
    }

    internal Point RotateLeft()
    {
        return new Point(-y, x);
    }

    internal Point RotateClockwise(int degrees)
    {
        var p = this;

        while (Math.Abs(degrees) % 360 != 0)
        {
            p = p.RotateRight();
            degrees -= 90;            
        }

        return p;
    }

    public static Point operator +(Point location, Point direction)
    {
        return new Point(location.x + direction.x, location.y + direction.y);
    }

    public static Point operator *(Point direction, int value)
    {
        return new Point(direction.x * value, direction.y * value);
    }


    public override string ToString() {
        return $"{x},{y}";
    }

    public System.Collections.Generic.IEnumerable<Point> GetNeighbours(int xmin, int xmax, int ymin, int ymax)
    {
            if (x > xmin) yield return new Point(x-1, y);
            if (x < xmax) yield return new Point(x+1, y);
            if (y > ymin) yield return new Point(x, y-1);
            if (y < ymax) yield return new Point(x, y+1);
    }

    public System.Collections.Generic.IEnumerable<Point> Neighbours
    {
        get
        {
            yield return new Point(x-1, y);
            yield return new Point(x+1, y);
            yield return new Point(x, y-1);
            yield return new Point(x, y+1);
        }
    }

    public static Point East => new Point(1, 0);
    public static Point West => new Point(-1, 0);
    public static Point North => new Point(0, 1);
    public static Point South => new Point(0, -1);
}