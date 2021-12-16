
namespace advent_of_code_2021;

using System.Collections;

static partial class Solutions
{       
    private static IEnumerable<bool> GetBits(IEnumerable<byte> bytes)
    {
        foreach (var by in bytes)
        {
            var ba = new BitArray(new[] { by });
            var s = new Stack<bool>();
            for (var i = 0; i < ba.Length; i++)
            {
                s.Push(ba[i]);
            }
            while (s.Any())
            {
                yield return s.Pop();
            }
        }
    }

    private static int GetValue(IEnumerable<bool> bits)
    {
        var i = 0;
        foreach (var bit in bits)
        {
            if (i != 0)
            {
                i <<= 1;
            }
            i |= bit ? 1 : 0;
        }
        return i;
    }

    private static (int Value, IEnumerable<bool> Remaining) GetChunk(IEnumerable<bool> set, int len)
    {
        return (Value: GetValue(set.Take(len)), Remaining: set.Skip(len));
    }

    private class BitsConsumed
    {
        public int Total = 0;
        public void Add(int t) => Total += t;
    }

    private class VersionCount
    {
        public int Total = 0;
        public void Add(int t) => Total += t;
    }

    private static IEnumerable<long> GetTokens(IEnumerable<bool> bits, int max = int.MaxValue, BitsConsumed consumed = null, VersionCount vc = null)
    {
        var end = false;
        var count = 0;
        while (!end)
        {
            var (value, newBits) = GetChunk(bits, 3);
            consumed?.Add(3);
            bits = newBits;
            //yield return value;
            var version = value;
            vc?.Add(version);

            (value, bits) = GetChunk(bits, 3);
            consumed?.Add(3);
            //yield return value;
            var typeId = value;

            if (typeId == 4)
            {                
                long literal = 0;
                var finished = false;
                do
                {
                    if (literal != 0)
                    {
                        literal <<= 4;
                    }
                    (value, bits) = GetChunk(bits, 5);
                    consumed?.Add(5);
                    finished = ((byte)value & 0b0001_0000) == 0;
                    literal |= ((byte)value & 0b0000_1111);
                }
                while (!finished);
                yield return literal;
            }
            else
            {
                (value, bits) = GetChunk(bits, 1);
                consumed?.Add(1);
                //yield return value;
                if (value == 0)
                {
                    (value, bits) = GetChunk(bits, 15);
                    consumed?.Add(15);
                    var subPacketsLen = value;

                    if (subPacketsLen == 0) break;

                    //yield return subPacketsLen;

                    var subPacket = bits.Take(subPacketsLen);
                    bits = bits.Skip(subPacketsLen);

                    var op = typeId;
                    if (op == 0) yield return GetTokens(subPacket, vc: vc).Sum();
                    if (op == 1) yield return GetTokens(subPacket, vc: vc).Multiply();
                    if (op == 2) yield return GetTokens(subPacket, vc: vc).Min();
                    if (op == 3) yield return GetTokens(subPacket, vc: vc).Max();
                    if (op == 5) yield return GetTokens(subPacket, vc: vc).Aggregate((a, c) => a > c ? 1 : 0);
                    if (op == 6) yield return GetTokens(subPacket, vc: vc).Aggregate((a, c) => a < c ? 1 : 0);
                    if (op == 7) yield return GetTokens(subPacket, vc: vc).Aggregate((a, c) => a == c ? 1 : 0);

                    consumed?.Add(subPacketsLen);
                }
                else
                {
                    (value, bits) = GetChunk(bits, 11);
                    consumed?.Add(11);
                    var subPacketCount = value;

                    if (subPacketCount == 0) break;

                    // yield return subPacketCount;
                    var subConsumed = new BitsConsumed();

                    var op = typeId;
                    if (op == 0) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Sum();
                    if (op == 1) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Multiply();
                    if (op == 2) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Min();
                    if (op == 3) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Max();
                    if (op == 5) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Aggregate((a, c) => a > c ? 1 : 0);
                    if (op == 6) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Aggregate((a, c) => a < c ? 1 : 0);
                    if (op == 7) yield return GetTokens(bits, subPacketCount, subConsumed, vc: vc).Aggregate((a, c) => a == c ? 1 : 0);

                    bits = bits.Skip(subConsumed.Total);
                    consumed?.Add(subConsumed.Total);
                }
            }

            count++;
            end = !bits.Any() || count >= max;
        }
    }

    public static string D_16_1(string[] input)
    {
        var hex = input.First();
        
        var bits = GetBits(hex
            .Chunk(2)
            .Select(h => string.Join("", h))
            .Select(h => Convert.ToByte(h, 16))
            .ToArray()
        );

        var v = new VersionCount();
        foreach (var token in GetTokens(bits, consumed: new BitsConsumed(), vc: v))
        {
            // Console.WriteLine(token);
        }

        return v.Total.ToString();
    }

    public static string D_16_2(string[] input)
    {
        var hex = input.First();

        var bits = GetBits(hex
            .Chunk(2)
            .Select(h => string.Join("", h))
            .Select(h => Convert.ToByte(h, 16))
            .ToArray()
        );

        var v = new VersionCount();
        return GetTokens(bits, consumed: new BitsConsumed(), vc: v).First().ToString();
    }
}