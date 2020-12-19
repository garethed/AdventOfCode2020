using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020 {

    class Day19 : Day
    {
        [Test(2, testData)]
        public override long Part1(string input)
        {
            input = Utils.SanitizeInput(input);
            var parts = input.Split("\n\n");

            var rules = Utils.splitLines(parts[0]).Select(l => l.Split(":")).ToDictionary(l => int.Parse(l[0]), l => l[1].Split(" "));

            var regex = "^" + getRegex(rules[0], rules) + "$";

            var parsed = new Regex(regex, RegexOptions.Compiled);

            if (rules.ContainsKey(42))
            {
                Console.WriteLine("42: "+ getRegex(rules[42], rules));
                Console.WriteLine("31: "+ getRegex(rules[31], rules));
            }
            
            return Utils.splitLines(parts[1]).Count(l => parsed.IsMatch(l));            
            
        }

        private string getRegex(string[] rule, Dictionary<int, string[]> rules)
        {
            var builder = new StringBuilder();
            bool needsBracket = false;
            int nextId;

            foreach (var element in rule)
            {
                if (element == "|")
                {
                      needsBracket = true;
                      builder.Append("|");                    
                }
                else if (int.TryParse(element, out nextId))
                {
                    builder.Append(getRegex(rules[nextId], rules));
                }
                else 
                {
                    builder.Append(element.Trim('"'));
                }
            }

            if (needsBracket) 
            {
                return "(" + builder.ToString() + ")";
            }
            else
            {
                return builder.ToString();
            }            
        }

        [Test(12, testData2)]
        public override long Part2(string input)
        {
            input = Utils.SanitizeInput(input);
            var parts = input.Split("\n\n");

            var rules = Utils.splitLines(parts[0]).Select(l => l.Split(":")).ToDictionary(l => int.Parse(l[0]), l => l[1].Split(" "));
            rules[8] = new string[] { "(" + getRegex(rules[42], rules) + ")+"};
            rules[11] = new string[] { "(?<left>" + getRegex(rules[42], rules) + ")+(?<-left>" + getRegex(rules[31], rules) + ")+(?(left)(?!))"};            

            var regex = "^" + getRegex(rules[0], rules) + "$";

            var parsed = new Regex(regex, RegexOptions.Compiled);
            
            var matches = Utils.splitLines(parts[1]).Select(l => parsed.IsMatch(l));   
            return matches.Count(m => m);

        }

        const string testData =
@"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb";        

const string testData2 = 
@"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";
    }
}