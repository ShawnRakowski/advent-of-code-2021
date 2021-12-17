namespace advent_of_code_2021;

static partial class Solutions
{
    //target area: x=20..30, y=-10..-5
    // target area: x=135..155, y=-102..-78
    public static string D_17_1(string[] input)
    {
        return Enumerable.Range(0, 102).Sum().ToString();
    }

    public static string D_17_2(string[] input)
    {
        //var x1 = 20;
        //var x2 = 30;
        //var y1 = -10;
        //var y2 = -5;
        var x1 = 135;
        var x2 = 155;
        var y1 = -102;
        var y2 = -78;

        // add all the points for direct shot
        // var count = (x2 - x1) * (Math.Abs(y1) - Math.Abs(y2));

        var minX = -1;
        var maxX = x2 + 1;// (int)Math.Floor(x2 / 2.0f);

        var hitsAll = new List<int>();
        var hitsX = Enumerable.Range(minX, (maxX - minX) + 1)
            .SelectMany(x =>
            {
                if (x == 30)
                {
                    Console.Write("");
                }
                var i = 0;
                var v = x;
                var p = 0;
                var hitsAt = new List<(int Inc, int Vel)>();
                for (i = 0; i < 3000 && p <= x2 && v >= 0; i++)
                {
                    p += v;                    
                    if (p >= x1 && p <= x2)
                    {
                        hitsAt.Add((Inc: i, Vel: x));
                        if (v == 0)
                        {
                            hitsAll.Add(x);
                        }
                    }
                    v -= 1;
                }
                return hitsAt;
            })
            .ToArray();

        var minY = y1 - 1;
        var maxY = (int)Math.Abs(y1) + 1;

        var hitY = Enumerable.Range(minY, maxY + Math.Abs(minY))
            .SelectMany(y =>
            {
                var i = 0;
                var v = y;
                var p = 0;
                var hitsAt = new List<(int Inc, int Vel)>();
                for (i = 0; i < 3000 && p >= y1; i++)
                {
                    p += v;                    
                    if (p >= y1 && p <= y2)
                    {
                        hitsAt.Add((Inc: i, Vel: y));
                    }
                    v -= 1;
                }
                return hitsAt;
            })
            .ToArray();

        var distincXInc = hitsX.Select(x => x.Inc).Distinct();
        var missingY = hitY.Distinct().Where(x => !distincXInc.Contains(x.Inc)).ToArray();
        var missing = missingY.Count();

        var d = new HashSet<(int X, int Y)>();
        foreach (var y in hitY)
        {
            var i = 0;
            foreach (var x in hitsX.Where(v => v.Inc == y.Inc))
            {
                var start = (X: x.Vel, Y: y.Vel);
                if (!d.Contains(start))
                {
                    d.Add(start);
                    Console.WriteLine($"{x.Vel},{y.Vel}");
                    i++;
                }
            }
            Console.WriteLine($"{i} at {y.Vel}");
        }

        Console.WriteLine("Missing...");

        foreach (var y in missingY)
        {
            var i = 0;
            foreach (var x in hitsAll)
            {
                var start = (X: x, Y: y.Vel);
                if (!d.Contains(start))
                {
                    d.Add(start);
                    Console.WriteLine($"{x},{y.Vel}");
                    i++;
                }
            }
            Console.WriteLine($"{i} at {y.Vel}");
        }

        var starts = d.Count();

        return starts.ToString();
    }
}