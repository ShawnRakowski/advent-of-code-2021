namespace advent_of_code_2021;

static class Solutions
{
    public static string D_1_1(string[] input)
    {
        return input
            .Select(int.Parse)
            .Aggregate(
                (Last: int.MaxValue, Count: 0),
                (acc, curr) => (Last: curr, Count: acc.Count + (curr > acc.Last ? 1 : 0))
            ).Count.ToString();
    }

    public static string D_1_2(string[] input)
    {
        var intInput = input.Select(int.Parse).ToArray();
        return D_1_1(Enumerable
            .Range(0, intInput.Length - 2)
            .Select(i => intInput[i..(i + 3)].Sum())
            .Select(i => i.ToString())
            .ToArray()
        );
    }

    public static string D_2_1(string[] input)
    {
        char DirSet(string direction) =>
            direction == "forward" ? 'x' : 'y';

        int DeltaMult(string direction) =>
            direction == "up" ? -1 : 1;

        return input
            .Select(s => s.Trim().Split())
            .Select(s => (Direction: s.First(), Delta: int.Parse(s.Last())))
            .Select(d => (Set: DirSet(d.Direction), Delta: d.Delta * DeltaMult(d.Direction)))
            .GroupBy(d => d.Set)
            .Select(g => g.Sum(d => d.Delta))
            .Aggregate(1, (a, c) => a *= c)
            .ToString();
    }

    public static string D_2_2(string[] input)
    {
        char DirSet(string direction) =>
            direction == "forward" ? 'm' : 'a';

        int DeltaMult(string direction) =>
            direction == "up" ? -1 : 1;

        bool IsMoving(char set) => set == 'm';
        bool IsAiming(char set) => set == 'a';

        var state = input
            .Select(s => s.Trim().Split())
            .Select(s => (Direction: s.First(), Delta: int.Parse(s.Last())))
            .Select(d => (Set: DirSet(d.Direction), Delta: d.Delta * DeltaMult(d.Direction)))
            .Aggregate((X: 0, D: 0, A: 0), (a, c) => (
                X: a.X + (IsMoving(c.Set) ? c.Delta : 0),
                D: a.D + (IsMoving(c.Set) ? (c.Delta * a.A) : 0),
                A: a.A + (IsAiming(c.Set) ? c.Delta : 0)
            ));

        return (state.X * state.D).ToString();
    }
}
