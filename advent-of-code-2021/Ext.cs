namespace advent_of_code_2021;

public static class Ext
{
    public static U GetValueOr<T, U>(this Dictionary<T, U> self, T k, U dv) =>
        self.ContainsKey(k) ? self[k] : dv;

    public static IEnumerable<U> Tee<U>(this IEnumerable<U> u, Action<U> a) =>
        u.Select(v => { a(v); return v; });

    public static IEnumerable<U> OutputToConsole<U>(this IEnumerable<U> u, Func<U, string>? toString = null) =>
        u.Tee(v => Console.WriteLine(toString == null ? (v is null ? "null" : v.ToString()) : toString(v)));

    public static int Multiply(this IEnumerable<int> u) => u.Aggregate((a, v) => a * v);

    public static IEnumerable<(int Row, int Col)> PointRange(int from, int to) => PointRange(from, to, from, to);

    public static IEnumerable<(int Row, int Col)> PointRange(int fromRow, int toRow, int fromCol, int toCol)
    {
        for (var r = fromRow; r <= toRow; r++)
            for (var c = fromCol; c <= toCol; c++)
            {
                yield return (r, c);
            }
    }
}
