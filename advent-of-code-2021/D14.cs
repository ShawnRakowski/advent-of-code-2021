using System.Text;

namespace advent_of_code_2021;

static partial class Solutions
{        
    public static string D_14_1(string[] input)
    {
        var template = input.First();
        var rules = input
            .Skip(2)
            .Select(i => i.Split(" -> "))
            .Select(i => (Pair: i.First(), Insert: i.Last()))
            .ToDictionary(
                k => k.Pair,
                v => v.Insert
            );

        var final = Run(template, rules, 0)
            .GroupBy(a => a)
            .OrderByDescending(a => a.Count())
            .ToList();

        var most = final.First();
        var least = final.Last();

        return (most.Count() - least.Count()).ToString();
    }

    private static string Run(string template, Dictionary<string, string> rules, int round)
    {
        if (round == 10) return template;

        var newtemplate = template.Aggregate(
            (Last: '\0', Template: ""),
            (acc, curr) =>
            {
                var template = acc.Template;
                if (acc.Last != '\0')
                {
                    var pair = acc.Last.ToString() + curr.ToString();
                    if (rules.ContainsKey(pair))
                    {
                        template += rules[pair];
                    }
                }
                template += curr;
                return (Last: curr, Template: template);
            }
        ).Template;


        return Run(newtemplate, rules, round + 1);
    }

    public static string D_14_2(string[] input)
    {
        var template = input.First();
        var rules = input
            .Skip(2)
            .Select(i => i.Split(" -> "))
            .Select(i => (Pair: i.First(), Insert: i.Last()))
            .ToDictionary(
                k => k.Pair,
                v => v.Insert[0]
            );

        var counts = new Dictionary<char, long>();
        var sets = new Dictionary<string, long>();
        
        var p = template.First();
        AddCount(counts, p);
        foreach (var c in template.Skip(1))
        {
            AddCount(counts, c);
            var key = $"{p}{c}";
            AddSetCount(sets, key, 1);
            p = c;
        }

        for (var i = 0; i < 40; i++)
        {
            var newSets = new Dictionary<string, long>();
            foreach (var set in sets.ToArray())
            {
                var ruleKey = set.Key;
                if (rules.ContainsKey(ruleKey))
                {
                    var insert = rules[ruleKey];
                    var key1 = $"{set.Key[0]}{insert}";
                    var key2 = $"{insert}{set.Key[1]}";
                    var mult = set.Value;
                    AddCount(counts, insert, mult);
                    AddSetCount(newSets, key1, mult);
                    AddSetCount(newSets, key2, mult);
                }
                else
                {
                    AddSetCount(newSets, set.Key, set.Value);
                }
            }
            sets = newSets;
        }

        return (counts.Values.Max() - counts.Values.Min()).ToString();
    }

    private static void AddCount(Dictionary<char, long> counts, char key, long count = 1)
    {
        if (counts.ContainsKey(key))
        {
            counts[key] += count;
        }
        else
        {
            counts[key] = count;
        }
    }

    private static void AddSetCount(Dictionary<string, long> sets, string key, long count)
    {
        if (sets.ContainsKey(key))
        {
            sets[key] += count;
        }
        else
        {
            sets[key] = count;
        }
    }
}