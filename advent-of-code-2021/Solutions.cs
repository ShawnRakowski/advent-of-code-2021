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
}
