using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day2 : Day
    {
        [Test(2, testInput)]
        public override int Part1(string input)
        {
            var passwords = RegexDeserializable.Deserialize<PasswordPolicy>(input).ToArray();
            return passwords.Count(p => p.IsValid);
        }

        [Test(1, testInput)]
        public override int Part2(string input)
        {
            var passwords = RegexDeserializable.Deserialize<PasswordPolicy>(input).ToArray();
            return passwords.Count(p => p.IsValid2);
        }

        [RegexDeserializable(@"(?<minOccurs>\d+)\-(?<maxOccurs>\d+) (?<requiredChar>\w): (?<password>\w+)")]
        record PasswordPolicy (int minOccurs, int maxOccurs, char requiredChar, string password)
        {
            public bool IsValid {
                get {
                    var count = password.Count(c => c == requiredChar);
                    return count >= minOccurs && count <= maxOccurs;
                }
            }

            public bool IsValid2 {
                get {
                    return password[minOccurs - 1] == requiredChar 
                    ^  password[maxOccurs - 1] == requiredChar;
                }
            }

        }

        const string testInput =
@"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";        
    }
}