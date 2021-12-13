using System.Text;

namespace advent_of_code_2021;

static partial class Solutions
{        
    public static string D_13_1(string[] input)
    {
        var points = input
            .Where(i => !i.StartsWith("fold") && !string.IsNullOrWhiteSpace(i))
            .Select(i => i
                .Split(',')
                .Select(i => int.Parse(i))
            )
            .Select(p => (X: p.First(), Y: p.Last()))
            .ToHashSet();

        var maxX = points.Max(i => i.X);
        var maxY = points.Max(i => i.Y);

        var folds = input
            .Where(i => i.StartsWith("fold"))
            .Select(i => i
                .Split(' ')
                .Last()
                .Split('=')
            )
            .Select(i => (Axis: i.First(), Value: int.Parse(i.Last().ToString())))
            .ToArray();

        var firstFold = folds.First();
        
        var count = 0;
        if (firstFold.Axis == "x")
        {
            Console.WriteLine($"TP: {points.Count()}");
            var dupe = 0;
            var moved = 0;
            points
                .Where(p => p.X > firstFold.Value)
                .Select(p => new { OldPoint = p, NewPoint = (X: (maxX - p.X) + 1, Y: p.Y) })
                //.OutputToConsole(p => $"{p.OldPoint.X},{p.OldPoint.Y}->{p.NewPoint.X},{p.NewPoint.Y}")
                .ToList()
                .ForEach(p =>
                {
                    moved++;
                    if (points.Contains(p.NewPoint))
                        dupe++;

                    points.Remove(p.OldPoint);
                    points.Add(p.NewPoint);
                });

            Console.WriteLine($"DUPES: {dupe}");
            Console.WriteLine($"MOVED: {moved}");
            Console.WriteLine($"NEW: {points.Count()}");
            count = points.Count();
        }
        else
        {
            Console.WriteLine($"TP: {points.Count()}");
            var dupe = 0;
            var moved = 0;
            points
                .Where(p => p.Y > firstFold.Value)
                .Select(p => new { OldPoint = p, NewPoint = (X: p.X, Y: maxY - p.Y) })
                .ToList()
                .ForEach(p =>
                {
                    moved++;
                    if (points.Contains(p.NewPoint))
                        dupe++;

                    points.Remove(p.OldPoint);
                    points.Add(p.NewPoint);
                });

            Console.WriteLine($"DUPES: {dupe}");
            Console.WriteLine($"MOVED: {moved}");
            Console.WriteLine($"NEW: {points.Count()}");
            count = points.Count();
        }

        PrintPoints(points, maxX, maxY, "done");
        return count.ToString();
    }

    private static void PrintPoints(HashSet<(int X, int Y)> points, int maxX, int maxY, string fn)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        var lastRow = 0;
        Ext.PointRange(0, maxY, 0, maxX)
            .Tee(p =>
            {
                if (p.Row > lastRow)
                {
                    lastRow = p.Row;
                    sb.AppendLine();
                }

                if (points.Contains((X: p.Col, Y: p.Row)))
                {
                    sb.Append("X");
                }
                else
                {
                    sb.Append(".");
                }
            })
            .ToArray();
        sb.AppendLine();
        File.WriteAllText($"{fn}.txt", sb.ToString());
    }

    public static string D_13_2(string[] input)
    {
        var points = input
            .Where(i => !i.StartsWith("fold") && !string.IsNullOrWhiteSpace(i))
            .Select(i => i
                .Split(',')
                .Select(i => int.Parse(i))
            )
            .Select(p => (X: p.First(), Y: p.Last()))
            .ToHashSet();

        var folds = input
            .Where(i => i.StartsWith("fold"))
            .Select(i => i
                .Split(' ')
                .Last()
                .Split('=')
            )
            .Select(i => (Axis: i.First(), Value: int.Parse(i.Last().ToString())))
            .ToArray();

        var j = 0;
        var maxX = points.Max(i => i.X);
        var maxY = points.Max(i => i.Y);
        foreach (var fold in folds)
        {
            var max = (fold.Value * 2);
            if (fold.Axis == "x")
            {
                points
                    .Where(p => p.X > fold.Value)
                    .Select(p => new { OldPoint = p, NewPoint = (X: (max - p.X), Y: p.Y) })
                    .ToList()
                    .ForEach(p =>
                    {
                        points.Remove(p.OldPoint);
                        points.Add(p.NewPoint);
                    });

                maxX = fold.Value - 1;
            }
            else
            {
                points
                    .Where(p => p.Y > fold.Value)
                    .Select(p => new { OldPoint = p, NewPoint = (X: p.X, Y: (max - p.Y)) })
                    .ToList()
                    .ForEach(p =>
                    {
                        points.Remove(p.OldPoint);
                        points.Add(p.NewPoint);
                    });

                maxY = fold.Value - 1;
            }
            j++;
            PrintPoints(points, maxX, maxY, $"F{j}.txt");
        }
        PrintPoints(points, maxX, maxY, "done");
        return points.Count().ToString();
    }
}