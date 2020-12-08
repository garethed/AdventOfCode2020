using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day8 : Day
    {
        [Test(5, testInput)]
        public override int Part1(string input)
        {

            var program = new Program(input);
            program.run();

            return program.acc;
        }


        class Program {
            public string[] raw;
            public Action<Program>[] code;
            public int pc = 0;
            public int acc = 0;

            public Program(string input)
            {
                raw = Utils.splitLines(input).ToArray();
                code = raw.Select(l => parseLine(l)).ToArray();
            }

            private Program() {}

            public Program clone()
            {
                return new Program() { code = (Action<Program>[]) code.Clone(), raw = raw };
            }

            public void run()
            {
                var executionCount = new int[code.Length];

                while (pc >= 0 && pc < code.Length && executionCount[pc] == 0)
                {
                    executionCount[pc]++;
                    code[pc](this);
                }
            }

            public Action<Program> parseLine(string l)
            {
                var parts = l.Split(" ");
                var arg = int.Parse(parts[1]);
                return parts[0] switch {
                    "nop" => (p => p.pc++),
                    "acc" => (p => {p.acc += arg; p.pc++;}),
                    "jmp" => (p => p.pc += arg)
                }; 
                
            }

        }


        [Test(8, testInput)]
        public override int Part2(string input)
        {
            var original = new Program(input);

            for (int i = 0; i < original.code.Length; i++)
            {
                if (original.raw[i].StartsWith("acc"))
                {
                    continue;
                }
                else {
                    var copy = original.clone();
                    copy.code[i] = copy.parseLine(copy.raw[i].Replace("nop", "nop2").Replace("jmp", "nop").Replace("nop2", "jmp"));
                    copy.run();
                    if (copy.pc == copy.code.Length) {
                        return copy.acc;
                    }
                }
            }

            return -1;
        }

        const string testInput = 
@"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6"        ;
    }
}