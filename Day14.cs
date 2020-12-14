using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2020 {

    class Day14 : Day
    {
        long maskAnd = 0;
        long maskOr = 0;
        long maskFloat = 0;
        private Dictionary<long, long> sparseMemory;
        long[] memory = new long[65536];


        [Test(165, testData)]
        public override long Part1(string input)
        {
            maskAnd = 0;
            maskOr = 0;
            maskFloat = 0;

            memory = new long[65536];

            foreach (var l in Utils.splitLines(input))
            {
                switch (l)
                {
                    case string m when m.StartsWith("mask"):
                        updateMask(l.Substring(7));
                        break;
                    default:
                        updateMemory(l);
                        break;

                }
            }

            return memory.Sum();
        }

        private void updateMemory(string l)
        {
            var split = l.Replace("mem[", "").Replace("]", "").Split(" ");
            var newValue = long.Parse(split[2]);
            newValue &= maskAnd;
            newValue |= maskOr;
            memory[int.Parse(split[0])] = newValue;
        }

        private void updateMask(string mask)
        {
            maskAnd = maskOr = maskFloat = 0;

            for (int i = 0; i < mask.Length; i++)
            {
                maskAnd = maskAnd << 1;
                maskOr = maskOr << 1;
                maskFloat = maskFloat << 1;

                switch (mask[i])
                {
                    case '0':
                        // no-op
                        break;
                    case '1':
                        maskAnd += 1;
                        maskOr += 1;
                        break;
                    case 'X':
                        maskAnd += 1;
                        maskFloat += 1;
                        break;
                }

            }
        }


        [Test(4, "mask = XX0000000000000000000000000000000000\nmem[42] = 1")]
        [Test(208, testData2)]
        public override long Part2(string input)
        {
            maskAnd = 0;
            maskOr = 0;
            maskFloat = 0;
            estimatedTotal = 0;
            sparseMemory = new Dictionary<long, long>();

            foreach (var l in Utils.splitLines(input))
            {
                switch (l)
                {
                    case string m when m.StartsWith("mask"):
                        updateMask(l.Substring(7));
                        break;
                    default:
                        updateMemory2(l);
                        break;

                }
            }

            return sparseMemory.Values.Sum();

        }

        long estimatedTotal =0;

        private void updateMemory2(string l)
        {
            var split = l.Replace("mem[", "").Replace("]", "").Split(" ");
            var newValue = long.Parse(split[2]);
            var address = long.Parse(split[0]);
            //address &= maskAnd;
            address |= maskOr;

            updateMemory2(address, 0, newValue);

            estimatedTotal += newValue * (1 << Convert.ToString(maskFloat,2).Count(c => c == '1'));
            //Utils.WriteLine("Actual: " + sparseMemory.Values.Sum() + "; Approx: " + estimatedTotal, ConsoleColor.DarkYellow);
        }

        private void updateMemory2(long newAddress, int bit, long newValue)
        {
            if (bit == 36)
            {
                sparseMemory[newAddress] = newValue;
                return;
            }

            var isFloat = maskFloat & (1L << bit);
            if (isFloat != 0)
            {
                updateMemory2(newAddress | isFloat, bit + 1, newValue);
                updateMemory2(newAddress & ~isFloat, bit + 1, newValue);
            }
            else
            {
                updateMemory2(newAddress, bit + 1, newValue);
            }
        }

        const string testData =
@"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";

        const string testData2 =
@"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1"        ;

    }
}