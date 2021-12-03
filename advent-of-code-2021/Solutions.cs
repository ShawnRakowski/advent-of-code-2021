﻿namespace advent_of_code_2021;

static class Solutions
{
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
}
