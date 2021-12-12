namespace advent_of_code_2021;

static partial class Solutions
{        
    public class Node
    {               
        public Node(string name)
        {
            Name = name;
            IsStart = name == "start";
            IsEnd = name == "end";
            IsBig = Name.All(x => char.IsUpper(x));
        }

        public string Name { get; }
        public bool IsStart { get; }
        public bool IsEnd { get; }
        public bool IsBig { get; }
        public bool IsSmall => !IsBig;
        public List<Node> Edges { get; } = new List<Node>();
    }

    public static IEnumerable<IEnumerable<Node>> FindPaths(Node node, IEnumerable<Node>? path = null)
    {
        path ??= new[] { node };
        if (node.IsEnd)
        {
            return new[] { path };
        }
        return node.Edges
            .Where(e => !path.Where(e => e.IsSmall).Contains(e))
            .SelectMany(e => FindPaths(e, path.Append(e)))
            //.OutputToConsole(x => string.Join("->", x.Select(x => x.Name)))
            .ToArray();
    }

    public static string D_12_1(string[] input)
    {
        var parsed = input
            .Select(x => x.Split("-"))
            .Select(x => (From: x.First(), To: x.Last()))
            .ToArray();

        var nodes = parsed
            .SelectMany(x => new[] { x.From, x.To })
            .Distinct()
            .Select(x => new Node(x))
            .ToArray();

        parsed
            .ToList()
            .ForEach(x =>
            {
                var from = nodes.Single(n => n.Name == x.From);
                var to = nodes.Single(n => n.Name == x.To);
                from.Edges.Add(to);
                to.Edges.Add(from);
            });

        var starting = nodes.Single(n => n.IsStart);
        return FindPaths(starting).Count().ToString();
    }

    public static IEnumerable<IEnumerable<Node>> FindMorePath(Node node, Node superNode, IEnumerable<Node>? path = null)
    {
        path ??= new[] { node };

        if (node.IsEnd)
        {
            return new[] { path };
        }

        var smallNodesAlreadyInPath = path.Where(e => e.IsSmall).ToArray();

        return node.Edges
            .Where(e => 
                !path.Where(n => n.IsSmall).Contains(e) ||
                (e == superNode && path.Count(n => n == e) < 2)
            )
            .SelectMany(e => FindMorePath(e, superNode, path.Append(e)))
            //.OutputToConsole(x => string.Join("->", x.Select(x => x.Name)))
            .ToArray();
    }

    public static string D_12_2(string[] input)
    {
        var parsed = input
            .Select(x => x.Split("-"))
            .Select(x => (From: x.First(), To: x.Last()))
            .ToArray();

        var nodes = parsed
            .SelectMany(x => new[] { x.From, x.To })
            .Distinct()
            .Select(x => new Node(x))
            .ToArray();

        parsed
            .ToList()
            .ForEach(x =>
            {
                var from = nodes.Single(n => n.Name == x.From);
                var to = nodes.Single(n => n.Name == x.To);
                from.Edges.Add(to);
                to.Edges.Add(from);
            });

        return nodes
            .Where(n => n.IsSmall && !n.IsEnd && !n.IsStart)
            .SelectMany(n => {
                var starting = nodes.Single(n => n.IsStart);
                return FindMorePath(starting, superNode: n);
            })
            .Select(p => string.Join("->", p.Select(x => x.Name)))
            .Distinct()
            .Count()
            .ToString();
    }
}