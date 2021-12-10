namespace advent_of_code_2021;

static partial class Solutions
{
    public static string D_10_1(string[] input)
    {
        var bads = new List<char>();
        foreach (var line in input)
        {
            var stk = new Stack<char>();
            foreach (var c in line)
            {
                if (c == '(' || c == '{' || c == '[' || c == '<')
                {
                    stk.Push(c);
                    continue;
                }

                var o = stk.Pop();
                var set = $"{o}{c}";
                if (set != "()" &&
                    set != "[]" &&
                    set != "{}" &&
                    set != "<>")
                {
                    bads.Add(c);
                    break;
                }
            }
        }

        var d1 = bads.Count(c => c == ')') * 3;
        var d2 = bads.Count(c => c == ']') * 57;
        var d3 = bads.Count(c => c == '}') * 1197;
        var d4 = bads.Count(c => c == '>') * 25137;

        static int f(int v) => v == 0 ? 1 : v;

        return (f(d1) + f(d2) + f(d3) + f(d4)).ToString();
    }

    public static string D_10_2(string[] input)
    {
        var incompletes = new List<string>();
        foreach (var line in input)
        {
            var isBad = false;
            var stk = new Stack<char>();
            foreach (var c in line)
            {
                if (c == '(' || c == '{' || c == '[' || c == '<')
                {
                    stk.Push(c);
                    continue;
                }

                var o = stk.Pop();
                var set = $"{o}{c}";
                if (set != "()" &&
                    set != "[]" &&
                    set != "{}" &&
                    set != "<>")
                {
                    isBad = true;
                    break;
                }
            }

            if (stk.Count != 0 && !isBad)
            {
                incompletes.Add(line);
            }
        }

        var scores = new List<long>();

        foreach (var line in incompletes)
        {
            var stk = new Stack<char>();
            var completion = new Queue<char>();
            foreach (var c in line)
            {
                if (c == '(' || c == '{' || c == '[' || c == '<')
                {
                    stk.Push(c);
                }
                else
                {
                    stk.Pop();
                }
            }

            
            while (stk.Any())
            {
                var c = stk.Pop();
                if (c == '(') completion.Enqueue(')');
                if (c == '[') completion.Enqueue(']');
                if (c == '{') completion.Enqueue('}');
                if (c == '<') completion.Enqueue('>');
            }

            long s = 0;
            while (completion.Any())
            {
                var c = completion.Dequeue();
                s *= 5;
                s += c == ')' ? 1 :
                     c == ']' ? 2 :
                     c == '}' ? 3 :
                     c == '>' ? 4 : 0;
            }
            scores.Add(s);
        }

        var idx = (scores.Count / 2);
        var ordered = scores.OrderByDescending(c => c).ToArray();

        return ordered[idx].ToString();
    }
}