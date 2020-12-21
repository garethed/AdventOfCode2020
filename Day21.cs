using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day21 : Day
    {

            Dictionary<string, HashSet<string>> possibleAllergens;
            List<HashSet<string>> ingredients; 
            List<HashSet<string>> allergens;
            HashSet<string> allIngredients;
            HashSet<string> allAllergens;
            HashSet<string> nonAllergens;

        [Test(5, testData)]
        public override long Part1(string input)
        {
            possibleAllergens = new Dictionary<string, HashSet<string>>();
            ingredients = new List<HashSet<string>>();            
            allergens = new List<HashSet<string>>();
            allIngredients = new HashSet<string>();
            allAllergens = new HashSet<string>();

            foreach (var line in Utils.splitLines(input))
            {
                var parts = line.TrimEnd(')').Split(" (contains ");
                var thisIngredients = parts[0].Split(' ').ToHashSet();
                var thisAllergens = parts[1].Split(", ").ToHashSet();
                ingredients.Add(thisIngredients);
                allergens.Add(thisAllergens);                

                allIngredients.UnionWith(thisIngredients);
                allAllergens.UnionWith(thisAllergens);
            
                foreach (var allergen in thisAllergens)
                {
                    if (possibleAllergens.ContainsKey(allergen))
                    {
                        possibleAllergens[allergen].IntersectWith(thisIngredients);
                    }
                    else 
                    {
                        possibleAllergens[allergen] = new HashSet<string>(thisIngredients);
                    }
                }
            }

            nonAllergens = allIngredients.Except(possibleAllergens.Values.SelectMany(v => v).ToHashSet()).ToHashSet();

            return ingredients.SelectMany(i => i.Intersect(nonAllergens)).Count();
            
        }

        [Test("mxmxvkd,sqjhc,fvjkl", testData)]
        public override string Part2S(string input)
        {       
                        
            while (possibleAllergens.Values.Any(v => v.Count > 1))
            {
                var identifiedAllergens = new HashSet<string>(possibleAllergens.Values.Where(v => v.Count == 1).Select(v => v.First()) );

                foreach (var allergens in possibleAllergens.Values)
                {
                    if (allergens.Count > 1)
                    {
                        allergens.ExceptWith(identifiedAllergens);
                    }
                }                
            }

            var allergenMap =  new SortedDictionary<string, string>(possibleAllergens.ToDictionary(kv => kv.Key, kv => kv.Value.First()));

            return string.Join(',', allergenMap.Values);


            
        }

        const string testData =
@"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)";        
    }
}