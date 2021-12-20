namespace advent_of_code_2021;

static partial class Solutions
{
    public static string D_20_1(string[] input)
    {
        var iaa = input.First();
        var imageInput = input.Skip(2);

        var output = Enhance(iaa, imageInput, false);
        output = Enhance(iaa, output, true);

        return output.SelectMany(l => l).Count(c => c == '#').ToString();
    }

    private static IEnumerable<string> Enhance(string iaa, IEnumerable<string> imageInput, bool def)
    {
        var image = imageInput
            .SelectMany((s, y) => s.Select((c, x) => (X: x, Y: y, On: c == '#')))
            .ToDictionary(
                k => (X: k.X, Y: k.Y),
                v => v.On
            );

        var minX = image.Keys.Min(k => k.X) - 1;
        var maxX = image.Keys.Max(k => k.X) + 1;
        var minY = image.Keys.Min(k => k.Y) - 1;
        var maxY = image.Keys.Max(k => k.Y) + 1;
        var len = (maxX - minX) + 1;

        var pixels = Ext.PointRange(minY, maxY, minX, maxX)
            .Select(p =>
            {
                var points = new[]
                {
                    (X: p.Col - 1, Y: p.Row - 1),
                    (X: p.Col    , Y: p.Row - 1),
                    (X: p.Col + 1, Y: p.Row - 1),
                    (X: p.Col - 1, Y: p.Row),
                    (X: p.Col    , Y: p.Row),
                    (X: p.Col + 1, Y: p.Row),
                    (X: p.Col - 1, Y: p.Row + 1),
                    (X: p.Col    , Y: p.Row + 1),
                    (X: p.Col + 1, Y: p.Row + 1),
                };

                var outputPoint = points
                    .Select(i => image.GetValueOr(i, def))
                    .Aggregate(
                        0,
                        (acc, pixelOn) =>
                        {
                            acc <<= 1;
                            if (pixelOn)
                            {
                                acc |= 1;
                            }
                            return acc;
                        });

                return iaa[outputPoint];
            });

        var output = new List<string>();
        while (pixels.Any())
        {
            output.Add(new string(pixels.Take(len).ToArray()));
            pixels = pixels.Skip(len);
        }

        return output;
    }

    public static string D_20_2(string[] input)
    {
        var iaa = input.First();
        var imageInput = input.Skip(2);

        var on = false;
        var output = imageInput;
        for (var i = 0; i < 50; i++)
        {
            output = Enhance(iaa, output, on);
            on = !on;
        }

        return output.SelectMany(l => l).Count(c => c == '#').ToString();
    }
}