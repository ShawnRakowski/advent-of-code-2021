namespace advent_of_code_2021;

using System.Collections.Generic;
using static Ext;

static partial class Solutions
{
    class SquidMoFo
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public int Value { get; set; }
        public bool FlashedThisTurn { get; set; }

        internal int Run(Dictionary<(int Row, int Col), SquidMoFo> floor)
        {
            if (FlashedThisTurn) return 0;
            Value++;
            if (Value < 10) return 0;
            Value = 0;
            FlashedThisTurn = true;
            return 1 + PointRange(-1, 1)
                .Select(p =>
                {
                    var adjPoint = (Row: Row + p.Row, Col: Col + p.Col);
                    return floor.ContainsKey(adjPoint)
                        ? floor[adjPoint].Run(floor)
                        : 0;
                })
                .Sum();
        }
    }

    public static string D_11_1(string[] input)
    {
        var rows = input.Length;
        var cols = input[0].Length;

        var floor = input
            .SelectMany((l, r) => l.Select((v, c) => new SquidMoFo
            {
                Row = r,
                Col = c,
                Value = int.Parse(v.ToString()),
                FlashedThisTurn = false
            }))
            .ToDictionary(
                x => (Row: x.Row, Col: x.Col),
                v => v
            );

        var output = Enumerable.Range(0, 100)
            .Select(i =>
            {
                floor.ToList().ForEach(t => t.Value.FlashedThisTurn = false);
                var flashCount = PointRange(0, rows - 1, 0, cols - 1)
                    .Select(p =>
                    {
                        var state = floor[p];
                        return state.Run(floor);
                    })
                    .Sum();
                return flashCount;
            })
            .Sum();

        return output.ToString();
    }


    public static string D_11_2(string[] input)
    {
        var rows = input.Length;
        var cols = input[0].Length;
        var total = rows * cols;

        var floor = input
            .SelectMany((l, r) => l.Select((v, c) => new SquidMoFo
            {
                Row = r,
                Col = c,
                Value = int.Parse(v.ToString()),
                FlashedThisTurn = false
            }))
            .ToDictionary(
                x => (Row: x.Row, Col: x.Col),
                v => v
            );

        long i = 0;
        var flashCount = 0;
        while (flashCount < total)
        {
            i++;            
            floor.ToList().ForEach(t => t.Value.FlashedThisTurn = false);
            flashCount = PointRange(0, rows - 1, 0, cols - 1)
                .Select(p =>
                {
                    var state = floor[p];
                    return state.Run(floor);
                })
                .Sum();
        }

        return i.ToString();
    }
}