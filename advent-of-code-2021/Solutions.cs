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
        return D_1_1(input
            .Select(int.Parse)
            .SelectMany((v, i) => new[]
            {
                (Window: i, Value: v),
                (Window: i + 1, Value: v),
                (Window: i + 2, Value: v)
            })
            .Where(x => x.Window < input.Length - 2)
            .Aggregate(
                new int[input.Length - 2],
                (acc, curr) => 
                {
                    acc[curr.Window] += curr.Value;
                    return acc;
                }
            )
            .Select(i => i.ToString())
            .ToArray()
        );
    }
}
