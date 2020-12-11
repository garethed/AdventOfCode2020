using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2020 {

    class Day4 : Day
    {
        [Test(2, testData)]
        public override long Part1(string input)
        {
            return parseItems(input).Count(i => validate(i));
        }

        private bool validate(Dictionary<string, string> i)
        {
            foreach (var required in new[] { "ecl", "pid", "eyr", "hcl", "byr", "iyr", "hgt"})
            {
                if (!i.ContainsKey(required)) {
                    return false;
                }
            }

            return true;
        }

        private IEnumerable<Dictionary<string,string>> parseItems(string input)
        {
            return Utils.SanitizeInput(input).Split("\n\n").Select(i => parseItem(i));
        }

        private Dictionary<string, string> parseItem(string item)
        {
            item = item.Replace("\n", " ");
            return item.Split(" ").ToDictionary(i => i.Split(":")[0], i => i.Split(":")[1]);
        }
        

        [Test(4, testData2)]
        public override long Part2(string input)
        {
            var count = parseItems(input).Count(i => validate(i) && validate2(i));
            /*foreach (var kv in validOutputs) 
            {
                Utils.WriteLine(kv.Key, ConsoleColor.Red);
                foreach (var s in kv.Value)
                {
                    Utils.Write(s + " ", ConsoleColor.White);
                }
                Console.WriteLine();

            }*/

            return count;
        }

        private bool validate2(Dictionary<string,string> item) 
        {
            foreach (var kv in item)
            {
                if (regexvalidators.ContainsKey(kv.Key) && !Regex.IsMatch(kv.Value, "^" + regexvalidators[  kv.Key ] + "$"))
                {
                    return false;
                }
                if (intvalidators.ContainsKey(kv.Key))
                {
                    int value;
                    if (!int.TryParse(kv.Value, out value) || !intvalidators[kv.Key](value))
                    {
                        return false;
                    }
                }
                if (kv.Key == "hgt")
                {
                    int value = int.Parse(kv.Value.Substring(0, kv.Value.Length - 2));
                    if (kv.Value.EndsWith("cm") && (value <150 || value > 193))
                    {
                        return false;
                    }
                    
                    if (kv.Value.EndsWith("in") && (value <59 || value > 76))
                    {
                        return false;
                    }                    
                } 

                if (!validOutputs.ContainsKey(kv.Key)) {
                    validOutputs[kv.Key] = new HashSet<string>();
                }
                validOutputs[kv.Key].Add(kv.Value);

            }
            return true;            

        }

        private Dictionary<string,HashSet<string>> validOutputs = new Dictionary<string, HashSet<string>>();

        private Dictionary<string,string> regexvalidators = new Dictionary<string,string>
        {
            { "byr", @"\d{4}" },
            { "iyr", @"\d{4}" },
            { "eyr", @"\d{4}" },
            { "hgt", @"\d+(cm|in)"},
            { "hcl", @"#[0-9a-f]{6}" },
            { "ecl", @"(amb|blu|brn|gry|grn|hzl|oth)" },
            { "pid", @"\d{9}" }
        };

        private Dictionary<string,Func<int,bool>> intvalidators = new Dictionary<string,Func<int,bool>>
        {
            { "byr", i => i >= 1920 && i <= 2002 },
            { "iyr", i => i >= 2010 && i <= 2020 },
            { "eyr", i => i >= 2020 && i <= 2030 }
        };

/*

    byr (Birth Year) - four digits; at least 1920 and at most 2002.
    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    hgt (Height) - a number followed by either cm or in:
        If cm, the number must be at least 150 and at most 193.
        If in, the number must be at least 59 and at most 76.
    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    pid (Passport ID) - a nine-digit number, including leading zeroes.
    cid (Country ID) - ignored, missing or not.
*/

        private const string testData = 
@"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";

private const string testData2 = 
@"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186in iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59in ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007

pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
    }
}