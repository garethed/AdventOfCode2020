using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2020 {

    class Day7 : Day
    {        

        [Test(4, testData)]
        public override int Part1(string input)
        {
            parseBags(input);

            var gold = BagType.GetBagType("shiny gold");
            var possibleParents = new HashSet<BagType>();

            addParents(possibleParents, gold);

            return possibleParents.Count;
            
        }

        private void addParents(HashSet<BagType> possibleParents, BagType bag)
        {
            foreach (var parent in bag.PossibleParents)
            {
                if (!possibleParents.Contains(parent))
                {
                    possibleParents.Add(parent);
                    addParents(possibleParents, parent);
                }
            }            
        }

        private void parseBags(string input)
        {
            BagType.AllTypes.Clear();
            foreach (var line in Utils.splitLines(input))
            {
                BagType.parseDefinition(line);
            }
        }

        [Test(32, testData)]
        public override int Part2(string input)
        {
            parseBags(input);
            var gold = BagType.GetBagType("shiny gold");

            return countChildren(gold) - 1;
            
        }

        private int countChildren(BagType bag)
        {
            return 1 + bag.Children.Select(kv => kv.Value * countChildren(kv.Key)).Sum();
        }

        class BagType
        {            
            public string Color;
            public Dictionary<BagType, int> Children = new Dictionary<BagType, int>();
            public List<BagType> PossibleParents = new List<BagType>();

            public static void parseDefinition(string definition)
            {
                var bt = GetBagType(definition.Substring(0, definition.IndexOf("contain") - 6));
                foreach (Match child in Regex.Matches(definition, @"(\d) (.*?) bag"))
                {
                    var childType = GetBagType(child.Groups[2].Value); 
                    bt.Children[childType] = int.Parse(child.Groups[1].Value);
                    childType.PossibleParents.Add(bt);
                }
            }

            public static BagType GetBagType(string color) {
                if (!AllTypes.ContainsKey(color)) {
                    AllTypes.Add(color, new BagType() { Color = color});
                }
                return AllTypes[color];
            }

            public static Dictionary<string,BagType> AllTypes = new Dictionary<string, BagType>();

        }

        const string testData =
@"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";
    }    
}