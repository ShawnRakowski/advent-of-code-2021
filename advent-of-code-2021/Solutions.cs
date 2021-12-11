namespace advent_of_code_2021;

static partial class Solutions
{
    public static void Execute(int day, int part)
    {
        var method = typeof(Solutions)
            .GetMethods()
            .SingleOrDefault(m => m.Name == $"D_{day}_{part}");

        if (method != null)
        {
            Console.WriteLine(method.Invoke(null, new[] { File.ReadAllLines($"./data/D_{day}/test.txt") }));
            Console.WriteLine(method.Invoke(null, new[] { File.ReadAllLines($"./data/D_{day}/input.txt") }));
        }
    }

    public static string D_1_1(string[] input)
    {
        return input
            .Select(int.Parse)
            .Aggregate(
                (Prev: int.MaxValue, Cnt: 0),
                (acc, curr) => (Prev: curr, Cnt: acc.Cnt + (curr > acc.Prev ? 1 : 0))
            ).Cnt.ToString();
    }

    public static string D_1_2(string[] input)
    {
        var values = input.Select(int.Parse).ToArray();
        return D_1_1(Enumerable
            .Range(0, values.Length - 2)
            .Select(i => values[i..(i + 3)].Sum())
            .Select(i => i.ToString())
            .ToArray()
        );
    }

    public static string D_2_1(string[] input)
    {
        char DirSet(string d) => d == "forward" ? 'h' : 'v';

        int DeltaMult(string d) => d == "up" ? -1 : 1;

        return input.Select(s => s.Trim().Split())
            .Select(s => (Dir: s.First(), Chg: int.Parse(s.Last())))
            .Select(d => (Set: DirSet(d.Dir), Chg: d.Chg * DeltaMult(d.Dir)))
            .GroupBy(d => d.Set)
            .Select(g => g.Sum(d => d.Chg))
            .Aggregate(1, (a, c) => a * c)
            .ToString();
    }

    public static string D_2_2(string[] input)
    {
        char DirCmd(string d) => d == "forward" ? 'm' : 'a';

        int DirMulti(string d) => d == "up" ? -1 : 1;

        var state = input.Select(s => s.Trim().Split())
            .Select(s => (Dir: s.First(), Chg: int.Parse(s.Last())))
            .Select(d => (Cmd: DirCmd(d.Dir), Chg: d.Chg * DirMulti(d.Dir)))
            .Aggregate((H: 0, D: 0, A: 0), (a, c) => (
                H: a.H + (c.Cmd is 'm' ? c.Chg : 0),
                D: a.D + (c.Cmd is 'm' ? (c.Chg * a.A) : 0),
                A: a.A + (c.Cmd is 'a' ? c.Chg : 0)
            ));

        return (state.H * state.D).ToString();
    }

    public static string D_3_1(string[] input)
    {
        var ge = input
            .Select(i => i.Trim())
            .Aggregate(
                new int[input[0].Length],
                (acc, curr) =>
                {
                    var vals = curr.Select(c => c.ToString()).Select(int.Parse).ToArray();
                    for (var i = 0; i < vals.Length; i++)
                        acc[i] += vals[i];
                    return acc;
                }
            )
            .Select(i => i >= input.Length - i ? 1 : 0)
            .Aggregate(
                (G: 0, E: 0),
                (acc, curr) =>
                    (G: (acc.G << 1) | curr,
                     E: (acc.E << 1) | (curr == 0 ? 1 : 0))
            );

        return (ge.G * ge.E).ToString();
    }

    public static string D_3_2(string[] input)
    {
        string Find(int pos, IEnumerable<string> remaining, Func<int, int, char> cmp)
        {
            if (remaining.Count() == 1)
            {
                return remaining.Single();
            }

            var ones = remaining.Count(r => r[pos] == '1');
            var zeros = remaining.Count(r => r[pos] == '0');
            var val = cmp(ones, zeros);

            return Find(
                pos + 1,
                remaining.Where(c => c[pos] == val),
                cmp
            );
        }

        var ov = Find(0, input, (a, b) => a >= b ? '1' : '0');
        var cv = Find(0, input, (a, b) => a < b ? '1' : '0');

        var o = Convert.ToInt32(ov, 2);
        var c = Convert.ToInt32(cv, 2);

        return (o * c).ToString();
    }

    public class Cell
    {
        public int Board;
        public int Row;
        public int Column;
        public int Value;
        public bool Checked;
    }

    public static string D_4_1(string[] input)
    {
        var numbers = input.First().Split(',').Select(int.Parse);

        var cells = input
            .Select(x => x.Trim())
            .Where(x => x != string.Empty)
            .Skip(1)
            .SelectMany((x, r) => x.Split()
                .Where(x => x != string.Empty)
                .Select(int.Parse)
                .Select((x, c) => new Cell
                {
                    Board = r / 5,
                    Row = r % 5,
                    Column = c,
                    Value = x,
                    Checked = false
                })
            )
            .ToArray();

        var rows = cells.GroupBy(cells => (cells.Board, cells.Row));
        var cols = cells.GroupBy(cells => (cells.Board, cells.Column));
        var paths = rows.Concat(cols).ToArray();

        foreach (var i in numbers)
        {
            cells
                .Where(c => c.Value == i)
                .ToList()
                .ForEach(x => x.Checked = true);

            var winningBoard =
                paths
                    .Where(p => p.All(v => v.Checked))
                    .Select(p => (int?)p.Key.Board)
                    .FirstOrDefault();

            if (winningBoard.HasValue)
            {
                return (cells.Where(c => c.Board == winningBoard.Value)
                    .Where(c => !c.Checked)
                    .Select(c => c.Value)
                    .Sum() * i).ToString();
            }
        }

        return String.Empty;
    }

    public class BoardState { public int Round; public int Value; }

    public static string D_4_2(string[] input)
    {
        var numbers = input.First().Split(',').Select(int.Parse);

        var cells = input
            .Select(x => x.Trim())
            .Where(x => x != string.Empty)
            .Skip(1)
            .SelectMany((x, r) => x.Split()
                .Where(x => x != string.Empty)
                .Select(int.Parse)
                .Select((x, c) => new Cell
                {
                    Board = r / 5,
                    Row = r % 5,
                    Column = c,
                    Value = x,
                    Checked = false
                })
            )
            .ToArray();

        var rows = cells.GroupBy(cells => (cells.Board, cells.Row));
        var cols = cells.GroupBy(cells => (cells.Board, cells.Column));
        var paths = rows.Concat(cols).ToArray();

        var boardCount = cells.Select(c => c.Board).Distinct().Count();
        var boardState = Enumerable.Range(0, boardCount).Select(c => new BoardState { Round = -1, Value = -1 }).ToArray();

        var r = 0;
        foreach (var i in numbers)
        {
            cells
                .Where(c => boardState[c.Board].Round < 0)
                .Where(c => c.Value == i)
                .ToList()
                .ForEach(x => x.Checked = true);

            var newWins = paths
                .Where(p => p.All(v => v.Checked))
                .Select(p => p.Key.Board)
                .Where(b => boardState[b].Round < 0)
                .ToList();

            newWins
                .ForEach(b =>
                {
                    boardState[b].Round = r;
                    boardState[b].Value = i;
                });

            r++;
        }

        var winningBoard = -1;
        var topRound = -1;
        for (var idx = 0; idx < boardCount; idx++)
        {
            var currBoardValue = boardState[idx];
            if (currBoardValue.Round > topRound)
            {
                winningBoard = idx;
                topRound = currBoardValue.Round;
            }
        }

        return (cells.Where(c => c.Board == winningBoard)
            .Where(c => !c.Checked)
            .Select(c => c.Value)
            .Sum() * boardState[winningBoard].Value).ToString();
    }

    public static string D_5_1(string[] input)
    {
        var map = new Dictionary<(int X, int Y), int>();

        input
            .Select(i => i.Trim().Replace(" -> ", ";").Split(";"))
            .Select(i =>
            (
                P1: i.First().Split(',').Select(int.Parse).ToArray(),
                P2: i.Last().Split(',').Select(int.Parse).ToArray()
            ))
            .Select(i =>
            (
                P1: (X: i.P1[0], Y: i.P1[1]),
                P2: (X: i.P2[0], Y: i.P2[1])
            ))
            .Where(p => p.P1.X == p.P2.X || p.P1.Y == p.P2.Y)
            .ToList()
            .ForEach(p =>
            {
                var dx = p.P2.X - p.P1.X;
                var dy = p.P2.Y - p.P1.Y;
                var sl = (
                    X: dx < 0 ? -1 : dx == 0 ? 0 : 1,
                    Y: dy < 0 ? -1 : dy == 0 ? 0 : 1
                );
                var current = p.P1;
                var end = p.P2;
                while (current.X != end.X || current.Y != end.Y)
                {
                    map[current] = map.GetValueOrDefault(current);
                    map[current] += 1;
                    current = (X: current.X + sl.X, Y: current.Y + sl.Y);
                }
                map[current] = map.GetValueOrDefault(current);
                map[current] += 1;
            });

        return map.Values.Count(m => m > 1).ToString();
    }

    public static string D_5_2(string[] input)
    {
        var map = new Dictionary<(int X, int Y), int>();

        input
            .Select(i => i.Trim().Replace(" -> ", ";").Split(";"))
            .Select(i =>
            (
                P1: i.First().Split(',').Select(int.Parse).ToArray(),
                P2: i.Last().Split(',').Select(int.Parse).ToArray()
            ))
            .Select(i =>
            (
                P1: (X: i.P1[0], Y: i.P1[1]),
                P2: (X: i.P2[0], Y: i.P2[1])
            ))
            .ToList()
            .ForEach(p =>
            {
                var dx = p.P2.X - p.P1.X;
                var dy = p.P2.Y - p.P1.Y;
                var sl = (
                    X: dx < 0 ? -1 : dx == 0 ? 0 : 1,
                    Y: dy < 0 ? -1 : dy == 0 ? 0 : 1
                );
                var current = p.P1;
                var end = p.P2;
                while (current.X != end.X || current.Y != end.Y)
                {
                    map[current] = map.GetValueOrDefault(current);
                    map[current] += 1;
                    current = (X: current.X + sl.X, Y: current.Y + sl.Y);
                }
                map[current] = map.GetValueOrDefault(current);
                map[current] += 1;
            });

        return map.Values.Count(m => m > 1).ToString();
    }

    public static string D_6_1(string[] input)
    {
        var period = 18;

        var fish = new Queue<(int Timer, int SpawnDay)>(input
            .First()
            .Split(",")
            .Select(int.Parse)
            .Select(x => (Timer: x, SpawnDay: 0))
        );

        var totalSpawned = fish.Count();

        while (fish.Any())
        {
            var current = fish.Dequeue();
            var totalCreate = ((period - current.SpawnDay - current.Timer) / 7) + 1;
            var firstSpawn = current.SpawnDay + current.Timer + 1;
            for (var i = firstSpawn; i <= period; i += 7)
            {
                totalSpawned++;
                fish.Enqueue((Timer: 8, SpawnDay: i));
            }
        }

        return totalSpawned.ToString();
    }

    public static string D_6_2(string[] input)
    {
        var period = 256;
        var spawnCount = new long[period];

        var init = input
            .First()
            .Split(",")
            .Select(int.Parse)
            .ToList();

        long total = init.Count;
        init.ForEach(i => 
        {
            spawnCount[i /* -1 + 1 */]++;
            var firstChildSpawn = (i - 1) + 8;
            for (var d = firstChildSpawn; d < period; d += 7)
                spawnCount[d]++;
        });
        
        for (var d = 0; d < spawnCount.Length; d++)
        {
            var m = spawnCount[d];
            total += m;
            var firstChildSpawn = d + 9;
            for (var i = firstChildSpawn; i < period; i += 7)
                spawnCount[i] += m;
        }

        return total.ToString();
    }

    public static string D_7_1(string[] input)
    {
        var list = input.First().Split(",").Select(int.Parse);
        var max = list.Max();
        var min = list.Min();
        return list.SelectMany((v) =>
            Enumerable
                .Range(min, max - min)
                .Select((i, set) => (set, (int)Math.Abs(i - v)))
            )
            .GroupBy(i => i.set)
            .Select(i => (set: i.Key, sum: i.Select(j => j.Item2).Sum()))
            .OrderBy(i => i.sum)
            .First()
            .ToString();
    }

    public static string D_7_2(string[] input)
    {
        var list = input.First().Split(",").Select(int.Parse);
        var max = list.Max();
        var min = list.Min();
        return list.SelectMany((v) =>
            Enumerable
                .Range(min, max - min)
                .Select((i, set) => (set, (int)Math.Abs(i - v)))
            )
            .GroupBy(i => i.set)
            .Select(i => (set: i.Key, sum: i.Select(j => Enumerable.Range(1, j.Item2).Aggregate(0, (acc, k) => acc + k)).Sum()))
            .OrderBy(i => i.sum)
            .First()
            .ToString();
    }

    public static string D_8_1(string[] input)
    {
        var i = input.SelectMany(i => i.Split("|").Last().Split(" ")).Where(i => !string.IsNullOrWhiteSpace(i));
        return i
            .Select(i => i.Length)
            .Where(i => i == 2 || i == 4 || i == 3 || i == 7)
            .Count()
            .ToString();
    }

    public static int Solve((string[] Output, IEnumerable<string> All) input)
    {
        var decoder = new Dictionary<char, char>();

        var one = input.All.First(i => i.Length == 2);

        var four = input.All.First(i => i.Length == 4);
        var seven = input.All.First(i => i.Length == 3);
        var eight = input.All.First(i => i.Length == 7);

        var others = input.All.Where(i => i.Length != 2 && i.Length != 4 && i.Length != 3 && i.Length != 7).Distinct();
        
        var a = seven.Single(x => !one.Contains(x));
        decoder[a] = 'a';

        var allExcept8 = one.Concat(four).Concat(seven);
        var eAndG = eight.Where(c => !allExcept8.Contains(c)).ToArray();

        var oneAndSeven = one.Concat(seven);
        var bAndD = eight.Where(c => four.Contains(c) && !oneAndSeven.Contains(c)).ToArray();

        var six = others.Single(x =>
            x.Contains(a) &&
            eAndG.All(x.Contains) &&
            bAndD.All(x.Contains) &&
            x != one && x != four && x != seven && x != eight
        );

        var f = six.Single(one.Contains);
        decoder[f] = 'f';

        var c = one.Single(x => x != f);
        decoder[c] = 'c';

        var two = others.Single(x =>
            x.Length == 5 &&
            eAndG.All(x.Contains) &&
            x.Contains(a) &&
            x.Contains(c)
        );

        var d = two.Single(x => !eAndG.Contains(x) && x != a && x != c);
        decoder[d] = 'd';

        var b = bAndD.Single(x => x != d);
        decoder[b] = 'b';

        var nine = others.Single(x =>
            x.Length == 6 &&
            new[] { a, b, c, d, f }.All(x.Contains)
        );

        var g = nine.Single(x => x != a && x != b && x != c && x != d && x != f);
        decoder[g] = 'g';

        var e = eAndG.Single(x => x != g);
        decoder[e] = 'e';

        for (var i = 0; i < 7; i++)
            if (!decoder.ContainsKey((char)('a' + i)))
                throw new Exception("need to be smarter");

        var sequences = new[]
        {
            "abcefg",
            "cf",
            "acdeg",
            "acdfg",
            "bcdf",
            "abdfg",
            "abdefg",
            "acf",
            "abcdefg",
            "abcdfg"
        };

        // input.Output.Select(i => lookup[i])
        return int.Parse(string.Join("", input.Output
            .Select(x => String.Join("", x.Select(y => decoder[y]).OrderBy(x => x)))
            .Select(x => Array.IndexOf(sequences, x).ToString())));
    }

    public static string D_8_2(string[] input)
    {
        return input
            .Select(i =>
            {
                var split = i.Split("|").Select(i => i.Trim());
                return
                (
                    Output: split.Last().Split(' '),
                    All: split.First().Split(' ')
                );
            })
            .Select(i => Solve(i))
            .Sum()
            .ToString();
    }

    public static string D_9_1(string[] input)
    {
        var rows = input.Length;
        var cols = input[0].Length;

        var points = input
            .Select(i => i.Select(c => int.Parse(c.ToString())))
            .SelectMany((l, r) => l.Select((v, c) => (Value: v, Row: r, Col: c)))
            .ToDictionary(
                k => (Row: k.Row, Col: k.Col),
                v => v.Value + 1
            );

        var riskLevel = 0;
        for (var r = 0; r < rows; r++)
            for (var c = 0; c < cols; c++)
            {
                var point = (Row: r, Col: c);
                var up = (Row: r - 1, Col: c);
                var down = (Row: r + 1, Col: c);
                var left = (Row: r, Col: c - 1);
                var right = (Row: r, Col: c + 1);

                var value = points[point];

                var adjPoints = new[]
                {
                    points.GetValueOr(up, int.MaxValue),
                    points.GetValueOr(down, int.MaxValue),
                    points.GetValueOr(left, int.MaxValue),
                    points.GetValueOr(right, int.MaxValue),
                };

                if (adjPoints.Any(a => a < value) ||
                    adjPoints.Where(a => a != int.MaxValue).All(a => a == value))
                    continue;

                riskLevel += value;
            }

        return riskLevel.ToString();
    }

    public static string D_9_2(string[] input)
    {
        var rows = input.Length;
        var cols = input[0].Length;

        var points = input
            .Select(i => i.Select(c => int.Parse(c.ToString())))
            .SelectMany((l, r) => l.Select((v, c) => (Value: v, Row: r, Col: c)))
            .ToDictionary(
                k => (Row: k.Row, Col: k.Col),
                v => v.Value
            );

        var lowPoints = new List<(int Row, int Col)>();
        for (var r = 0; r < rows; r++)
            for (var c = 0; c < cols; c++)
            {
                var point = (Row: r, Col: c);
                var up = (Row: r - 1, Col: c);
                var down = (Row: r + 1, Col: c);
                var left = (Row: r, Col: c - 1);
                var right = (Row: r, Col: c + 1);

                var value = points[point];

                var adjPoints = new[]
                {
                    points.GetValueOr(up, int.MaxValue),
                    points.GetValueOr(down, int.MaxValue),
                    points.GetValueOr(left, int.MaxValue),
                    points.GetValueOr(right, int.MaxValue),
                };

                if (adjPoints.Any(a => a < value) ||
                    adjPoints.Where(a => a != int.MaxValue).All(a => a == value) ||
                    value == 9)
                    continue;

                lowPoints.Add(point);
            }

        return lowPoints
            .Select((lp, i) => 
            {
                var r = FindBasinPoints(null, lp, points, new HashSet<(int Row, int Col)>());
                // Console.WriteLine($"Set {i + 1} -> {r}");
                return r;
            })
            .OrderByDescending(v => v)
            // .OutputToConsole()
            .Take(3)
            .Multiply()
            .ToString();
    }

    private static int FindBasinPoints(
        (int Row, int Col)? fromPoint,
        (int Row, int Col) lowPoint,
        Dictionary<(int Row, int Col), int> points,
        HashSet<(int Row, int Col)> alreadyFactored)
    {
        if (!points.ContainsKey(lowPoint))
            return 0;

        var pv = points[lowPoint];

        if (pv == 9)
        {
            return 0;
        }

        if (fromPoint.HasValue)
        {
            var fromPointValue = points[fromPoint.Value];
            if (pv <= fromPointValue)
                return 0;
        }

        var r = lowPoint.Row;
        var c = lowPoint.Col;

        var adjPoints = new[]
        {
            (Row: r - 1, Col: c),
            (Row: r + 1, Col: c),
            (Row: r, Col: c - 1),
            (Row: r, Col: c + 1),
        };

        var value = 0;
        if (!alreadyFactored.Contains(lowPoint))
        {
            alreadyFactored.Add(lowPoint);
            value = 1;
        }

        // Console.WriteLine($"{lowPoint}:{(fromPoint.HasValue ? points[fromPoint.Value] : "None")}->{pv};+{value}");
        return value + adjPoints.Select(p => FindBasinPoints(lowPoint, p, points, alreadyFactored)).Sum();
    }
}
