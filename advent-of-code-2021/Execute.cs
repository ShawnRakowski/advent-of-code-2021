using System.Text;

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
}