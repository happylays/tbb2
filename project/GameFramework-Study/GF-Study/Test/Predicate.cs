
public class Example
{
    public static void Main()
    {
        Point[] points = { new Point(100, 200), new Point(); }
    Prediacte<Point> predicate = FindPoints;

    Point first = Array.Find(points, predicate);

    Point first = Array.Find(points, x => x.X * x.Y > 10000);

    teams.FindAll( x => x.Founded <= foundedBeforeYear))

    }

private static bool FindPoints(Point obj)
{
    return obj.X * obj.Y > 10000;
}
}