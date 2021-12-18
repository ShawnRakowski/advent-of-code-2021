namespace advent_of_code_2021;

static partial class Solutions
{
    public abstract class SFNode
    {
        public SFNumber Parent { get; set; }

        public abstract long Magnitude();
    }

    public class SFLeaf : SFNode
    {
        public SFLeaf(long value)
        {
            Value = value;
        }

        public long Value { get; set; }

        public void Split()
        {
            if (Parent == null) throw new Exception();
            if (Value < 10) throw new Exception();

            var left = new SFLeaf((long)Math.Floor(Value / 2.0));
            var right = new SFLeaf((long)Math.Ceiling(Value / 2.0));
            Parent.Replace(this, new SFNumber(left, right));
        }

        public override string ToString() => Value.ToString();

        public static SFLeaf Parse(string value) => new SFLeaf(long.Parse(value));

        public override long Magnitude() => Value;
    }

    public class SFNumber : SFNode
    {
        public SFNumber(SFNode left, SFNode right)
        {
            Left = left;
            Left.Parent = this;

            Right = right;
            Right.Parent = this;
        }

        public SFNode Left { get; private set; }

        public SFNode Right { get; private set; }

        public static SFNumber operator +(SFNumber left, SFNumber right)
        {
            var newNumber = new SFNumber(left, right);
            //Console.WriteLine($"\t{left}");
            //Console.WriteLine($"+\t{right}");
            // Console.WriteLine($"after addition:\t{newNumber}");            
            return newNumber;
        }

        public override string ToString() => $"[{Left},{Right}]";

        public static SFNumber Parse(string value)
        {
            var normalizedValue = value.Trim().Replace(",", "");
            var charStk = new Stack<char>();
            var sfStk = new Stack<SFNode>();
            foreach (var c in normalizedValue)
            {
                switch (c)
                {
                    case '[':
                        charStk.Push(c);
                        break;
                        
                    case ']':
                        var right = sfStk.Pop();
                        var left = sfStk.Pop();
                        sfStk.Push(new SFNumber(left, right));
                        break;

                    default:
                        sfStk.Push(SFLeaf.Parse(c.ToString()));                       
                        break;
                }
            }
            return sfStk.Pop() as SFNumber;
        }

        public SFNumber Reduce()
        {
            SFNode node;
            do
            {
                node = FindExploder(1);
                if (node != null)
                {
                    (node as SFNumber).Explode();
                    //Console.WriteLine($"after explode:\t{this}");
                }
                else
                {
                    node = FindSplit();
                    if (node != null)
                    {
                        (node as SFLeaf).Split();
                        //Console.WriteLine($"after split:\t{this}");
                    }
                }
            }
            while (node != null);
            //Console.WriteLine($"=\t{this}");
            //Console.WriteLine();
            return this;
        }

        private void Explode()
        {
            if (Parent == null) throw new Exception();
            Parent.HandleChildExplosion(this);
        }

        private void HandleChildExplosion(SFNumber child)
        {
            var replacement = new SFLeaf(0);
            replacement.Parent = this;
            var leftValue = (child.Left as SFLeaf).Value;
            var rightValue = (child.Right as SFLeaf).Value;
            if (Left == child)
            {
                Left = replacement;
                AddRight(rightValue, true);
                var current = this;
                var parent = Parent;
                while (parent != null && parent.Left == current)
                {
                    current = parent;
                    parent = parent.Parent;
                }
                parent?.AddLeft(leftValue, false);
            }
            else if (Right == child)
            {
                Right = replacement;
                AddLeft(leftValue, false);
                var current = this;
                var parent = Parent;
                while (parent != null && parent.Right == current)
                {
                    current = parent;
                    parent = parent.Parent;
                }
                parent?.AddRight(rightValue, true);
            }
            else throw new Exception();
        }

        private void AddRight(long value, bool leftNeighbor)
        {
            if (Right is SFLeaf leaf)
            {
                leaf.Value += value;
            }
            else
            {
                var number = Right as SFNumber;
                if (leftNeighbor)
                {
                    number.AddLeft(value, leftNeighbor);
                }
                else
                {
                    number.AddRight(value, leftNeighbor);
                }
            }
        }

        private void AddLeft(long value, bool leftNeighbor)
        {
            if (Left is SFLeaf leaf)
            {
                leaf.Value += value;
            }
            else
            {
                var number = Left as SFNumber;
                if (leftNeighbor)
                {
                    number.AddLeft(value, leftNeighbor);
                }
                else
                {
                    number.AddRight(value, leftNeighbor);
                }
            }
        }

        private SFNumber FindExploder(int depth)
        {
            SFNumber exploder = null;

            if (Left is SFNumber left)
            {
                exploder = left.FindExploder(depth + 1);
            }

            if (exploder == null && Right is SFNumber right)
            {
                exploder = right.FindExploder(depth + 1);
            }

            if (exploder == null && depth > 4)
            {
                exploder = this;
            }

            return exploder;
        }

        private SFLeaf FindSplit()
        {
            SFLeaf splitter = null;

            if (Left is SFNumber left)
            {
                splitter = left.FindSplit();
            }

            if (splitter == null && (Left is SFLeaf leafLeft && leafLeft.Value >= 10))
            {
                splitter = leafLeft;
            }

            if (splitter == null && Right is SFNumber right)
            {
                splitter = right.FindSplit();
            }

            if (splitter == null && (Right is SFLeaf leafRight && leafRight.Value >= 10))
            {
                splitter = leafRight;
            }

            return splitter;
        }

        internal void Replace(SFLeaf leaf, SFNumber replacement)
        {
            replacement.Parent = this;
            if (Left == leaf) Left = replacement;
            else if (Right == leaf) Right = replacement;
            else throw new Exception();
        }

        public override long Magnitude()
        {
            var left = Left.Magnitude() * 3;
            var right = Right.Magnitude() * 2;
            return left + right; ;
        }
    }

    public static string D_18_1(string[] input)
    {
        var value = input.Select(SFNumber.Parse).Aggregate((a,b) => (a + b).Reduce());
        return value.Magnitude().ToString();
    }

    public static string D_18_2(string[] input)
    {
        var numbers = input.Select(SFNumber.Parse).ToArray();
        var cases = numbers.Length;
        return numbers
            .SelectMany((a, i) =>
                numbers
                    .Skip(i + 1)
                    .Select(b => (A: a, B: b))
            )
            .SelectMany(x =>
            {
                var a1 = SFNumber.Parse(x.A.ToString());
                var b1 = SFNumber.Parse(x.B.ToString());
                var mag1 = (a1 + b1).Reduce().Magnitude();
                var a2 = SFNumber.Parse(x.A.ToString());
                var b2 = SFNumber.Parse(x.B.ToString());
                var mag2 = (b2 + a2).Reduce().Magnitude();
                return new[] { mag1, mag2 };
            })
            .Max()
            .ToString();
    }
}