using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day16 : Day
    {


        [Test(71, testData)]
        public override long Part1(string input)
        {
            var fields = RegexDeserializable.Deserialize<Field>(input).ToArray();
            var tickets = Utils.splitLines(input).Where(l => l.Length > 0 &&  char.IsDigit(l[0])).Select(l => l.Split(',').Select(n => int.Parse(n)).ToArray()).ToArray();

            return tickets.Sum(t => InvalidFieldSum(t, fields));            
        }

        public override long Part2(string input)
        {
            var fields = RegexDeserializable.Deserialize<Field>(input).ToArray();
            var tickets = Utils.splitLines(input).Where(l => l.Length > 0 &&  char.IsDigit(l[0])).Select(l => l.Split(',').Select(n => int.Parse(n)).ToArray()).ToArray();
            var validTickets = tickets.Where(t => MaybeValid(t, fields)).ToArray();

            var assigned = new Queue<int>();

            foreach (var field in fields)
            {
                field.InitIndices(validTickets[0].Length);
                foreach (var ticket in validTickets)
                {
                    field.eliminateIndices(ticket, assigned);
                }
            }

            while (assigned.Count > 0)
            {
                var index = assigned.Dequeue();

                foreach (var field in fields)
                {
                    if (field.possibleIndices.Count > 1 &&  field.possibleIndices.Contains(index))
                    {
                        field.possibleIndices.Remove(index);
                        if (field.possibleIndices.Count == 1)
                        {
                            assigned.Enqueue(field.possibleIndices.First());
                        }

                    }
                }
            }


            var departureFields = fields.Where(f => f.Name.StartsWith("departure"));
            return departureFields.Aggregate(1L, (a, f) => a * tickets[0][f.possibleIndices.First()]);







            
            
        }

        public int InvalidFieldSum(int[] ticketData, Field[] fields)
        {
            return ticketData.Where(t => !MaybeValid(t, fields)).Sum();
        }

        public bool MaybeValid(int[] ticketData, Field[] fields)
        {
            return ticketData.All(t => MaybeValid(t, fields));
        }

        public bool MaybeValid(int ticketDatum, Field[] fields)
        {
            return fields.Any(f => f.MaybeValid(ticketDatum));
        }

        [RegexDeserializable(@"(?<Name>[a-zA-Z ]+): (?<From1>\d+)-(?<To1>\d+) or (?<From2>\d+)-(?<To2>\d+)")]
        public class Field
        {
            public string Name;
            public int From1;
            public int To1;
            public int From2;
            public int To2;

            public HashSet<int> possibleIndices = new HashSet<int>();

            public bool MaybeValid(int ticketDatum) {
                return ((ticketDatum >= From1 && ticketDatum <= To1) || (ticketDatum >= From2 && ticketDatum <= To2));
            }

            public void InitIndices(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    possibleIndices.Add(i);
                }
            }

            public void eliminateIndices(int[] validTicket, Queue<int> assigned)
            {
                foreach (var i in possibleIndices.ToArray())
                {
                    if (!MaybeValid(validTicket[i])) {
                        possibleIndices.Remove(i);
                        if (possibleIndices.Count == 1)
                        {
                            assigned.Enqueue(possibleIndices.First());
                        }
                    }
                }
            }
        }

        const string testData =
@"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";        
    }
}