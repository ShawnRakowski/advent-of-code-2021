
namespace advent_of_code_2021;

//using Roy_T.AStar.Graphs;
//using Roy_T.AStar.Paths;
//using Roy_T.AStar.Primitives;

using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

static partial class Solutions
{        
    public static string D_15_1(string[] input)
    {
        var cols = input[0].Length;
        var rows = input.Length;

        var graph = new Graph<int, string>();

        var data = input
            .SelectMany((l, r) => l.Select((x, c) => (Row: r, Col: c, Cost: int.Parse(x.ToString()))))
            .ToList();

        data.ForEach(d => {
            var idx = (int)CalcIndex((Row: d.Row, Col: d.Col), cols);
            graph.AddNode(idx);
        });

        var costs = data.ToDictionary(
            k => (k.Row, k.Col),
            v => v.Cost
        );

        data.ForEach(n =>
        {
            var start = (n.Row, n.Col);
            var up = (n.Row - 1, n.Col);
            var down = (n.Row + 1, n.Col);
            var left = (n.Row, n.Col - 1);
            var right = (n.Row, n.Col + 1);
            var positions = new[] { up, down, left, right };
            foreach (var position in positions)
            {
                if (!costs.ContainsKey(position)) continue;
                var cost = costs[position];
                graph.Connect(CalcIndex(start, cols), CalcIndex(position, cols), cost, cost.ToString());
            }
        });

        var result = graph.Dijkstra(CalcIndex((Row: 0, Col: 0), cols), CalcIndex((Row: rows - 1, Col: cols - 1), cols));

        return result.Distance.ToString();
    }

    private static uint CalcIndex((int Row, int Col) d, int cols)
    {
        return (uint)(((d.Row * cols) + d.Col) + 1);
    }

    public static string D_15_2(string[] input)
    {
        var cols = input[0].Length;
        var rows = input.Length;

        var graph = new Graph<int, string>();

        var originalData = input
            .SelectMany((l, r) => l.Select((x, c) => (Row: r, Col: c, Cost: int.Parse(x.ToString()))))
            .ToList();

        var data = Enumerable.Range(0, 5)
            .SelectMany(r => Enumerable.Range(0, 5).Select(c => (Row: r, Col: c)))
            .SelectMany(i =>
                originalData
                    .Select(j => (
                        Row: (i.Row * rows) + j.Row,
                        Col: (i.Col * cols) + j.Col,
                        Cost: (i.Row + i.Col) + j.Cost > 9 ? (i.Row + i.Col) + j.Cost - 9 : (i.Row + i.Col) + j.Cost
                    ))
            )
            .ToList();

        cols *= 5;
        rows *= 5;

        data.ForEach(d => {
            var idx = (int)CalcIndex((Row: d.Row, Col: d.Col), cols);
            graph.AddNode(idx);
        });

        var costs = data.ToDictionary(
            k => (k.Row, k.Col),
            v => v.Cost
        );

        data.ForEach(n =>
        {
            var start = (n.Row, n.Col);
            var up = (n.Row - 1, n.Col);
            var down = (n.Row + 1, n.Col);
            var left = (n.Row, n.Col - 1);
            var right = (n.Row, n.Col + 1);
            var positions = new[] { up, down, left, right };
            foreach (var position in positions)
            {
                if (!costs.ContainsKey(position)) continue;
                var cost = costs[position];
                graph.Connect(CalcIndex(start, cols), CalcIndex(position, cols), cost, cost.ToString());
            }
        });

        var result = graph.Dijkstra(CalcIndex((Row: 0, Col: 0), cols), CalcIndex((Row: rows - 1, Col: cols - 1), cols));

        return result.Distance.ToString();
    }
}