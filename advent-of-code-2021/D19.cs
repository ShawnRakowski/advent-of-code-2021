namespace advent_of_code_2021;

using Microsoft.Xna.Framework;
using System;

static partial class Solutions
{
    public class BeaconRelationship
    {
        public BeaconRelationship(Beacon beacon, float distance, Vector3 vector)
        {
            Beacon = beacon;
            Distance = distance;
        }

        public Beacon Beacon { get; }
        public float Distance { get; }

        public bool IsSimilarTo(BeaconRelationship other)
        {
            return Distance == other.Distance;
        }
    }

    public class Beacon
    {
        private readonly List<BeaconRelationship> _relationships = new List<BeaconRelationship>();

        public Beacon(Vector3 logicalPosition)
        {
            LogicalPosition = logicalPosition;
        }

        public Vector3 LogicalPosition { get; }

        public IEnumerable<BeaconRelationship> Relationships => _relationships;

        public void AddRelationship(Beacon b2, float distance, Vector3 vector)
        {
            _relationships.Add(new BeaconRelationship(b2, distance, vector));
        }

        public bool LikelyMatches(Beacon other)
        {
            var matches = Relationships
                .Select(r1 => (r1, r2: other.Relationships.SingleOrDefault(r2 => r1.IsSimilarTo(r2))))
                .Where(s => s.r2 != null)
                .ToArray();

            var count = matches.Count();

            return count >= 11;
        }
    }

    public class Scanner
    {
        public Scanner(IEnumerable<Beacon> beaconNodes)
        {
            Beacons = beaconNodes;
            BuildRelationships();
        }

        public IEnumerable<Beacon> Beacons { get; }

        private void BuildRelationships()
        {
            var nodes = Beacons.ToList();
            var i = 1;
            nodes.ForEach((b1) =>
            {
                nodes
                    .Skip(i)
                    .ToList()
                    .ForEach((b2) =>
                    {
                        b1.AddRelationship(b2, Vector3.Distance(b1.LogicalPosition, b2.LogicalPosition), b2.LogicalPosition - b1.LogicalPosition);
                        b2.AddRelationship(b1, Vector3.Distance(b2.LogicalPosition, b1.LogicalPosition), b1.LogicalPosition - b2.LogicalPosition);
                    });
                i++;
            });
        }
        
        public IEnumerable<(Beacon ScannerABeacon, Beacon ScannerBBeacon)> FindMatchingBeacons(Scanner other)
        {
            var candidateBeacons = Beacons
                .Select(ba => (ScannerABeacon: ba, ScannerBBeacon: other.Beacons.SingleOrDefault(bb => bb.LikelyMatches(ba))))
                .Where(p => p.ScannerBBeacon != null)
                .ToArray();

            return candidateBeacons;
        }
    }

    public static string D_19_1(string[] input)
    {
        var endState = input
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .Aggregate(
                (ScannerSets: new List<Scanner>(), CurrentSet: (List<Beacon>)null),
                (acc, c) =>
                {
                    var (scannerSets, currentSet) = acc;
                    if (c.StartsWith("---"))
                    {
                        if (currentSet != null)
                        {
                            scannerSets.Add(new Scanner(currentSet));
                        }
                        currentSet = new List<Beacon>();
                    }
                    else
                    {
                        var points = c.Split(',').Select(float.Parse).ToArray();
                        currentSet.Add(new Beacon(new Vector3(
                            points[0],
                            points[1],
                            points[2]
                        )));
                    }
                    return (ScannerSets: scannerSets, CurrentSet: currentSet);
                });

        endState.ScannerSets.Add(new Scanner(endState.CurrentSet));
        var scannerSets = endState.ScannerSets;

        var a = scannerSets[0];
        var b = scannerSets[1];


        return "";// allBeacons.ToString();
    }

    public static string D_19_2(string[] input)
    {
        return "";
    }
}