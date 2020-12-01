using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    static class Utils
    {
        public static string describe<T>(this T[] array)
        {
            return string.Join(',', array.Select(i => i.ToString()));
        }

        public static void debug(this bool[,] data) {
            Console.WriteLine();
            foreach (var y in Enumerable.Range(0, data.GetLength(1))) {
                foreach (var x in Enumerable.Range(0, data.GetLength(0))) {
                    Console.Write(data[x,y] ? '#' : '.');
                }
                Console.WriteLine();
            }
        }


        public static IEnumerable<T> flatten<T>(this T[,] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                yield return array[i % array.GetLength(0), i / array.GetLength(0)];
            }
        }

        public static IEnumerable<string> splitLines(string input)
        {
            return input.Replace("\r", "").TrimEnd('\n').Split('\n').Select(l => l.Trim());
        }

        public static IEnumerable<string> splitLinesWithoutTrim(string input)
        {
            return input.Replace("\r", "").Split('\n');
        }

        public static bool Test(Func<string, string> method, int[] inputs, int[] outputs)
        {
            return Test(method, inputs.Select(i => i.ToString()).ToArray(), outputs.Select(i => i.ToString()).ToArray());
        }

        public static bool Test<A,B>(Func<A, B> method, A[] inputs, B[] outputs)
        {
            var success = true;

            for (int i = 0; i < inputs.Length; i++)
            {
                success &= Test( method, inputs[i], outputs[i]);

            }

            return success;
        }

        public static IEnumerable<Point> enumerateGrid(int width, int height) 
        {
            foreach (var y in Enumerable.Range(0, height)) 
            {
                foreach (var x in Enumerable.Range(0, width)) 
                {
                    yield return new Point(x,y);
                }
            }
        }

        public static bool Test<A,B>(Func<A,B> method, A input, B output)  {
            return Assert(input, method(input), output);
        }
        public static bool Assert<T>(object input, T actual, T expected)  {

                var success = EqualityComparer<T>.Default.Equals(actual, expected);
                if (success)
                {
                    Write("OK: ", ConsoleColor.DarkGreen);
                }
                else
                {
                    Write("WRONG: ", ConsoleColor.DarkRed);
                    success = false;
                }

                var inputString = input.ToString().Replace("\n", " ").Replace("\r", "");
                if (inputString.Length > 40)
                {
                    inputString = inputString.Substring(0, 40) + "...";
                }

                Write(inputString + " -> ", ConsoleColor.White);

                if (!success)
                {
                    WriteLine(actual.ToString(), ConsoleColor.DarkRed);
                    WriteLine("  Should be " + expected.ToString(), ConsoleColor.DarkRed);
                }
                else
                {
                    WriteLine(actual.ToString(), ConsoleColor.DarkGreen);
                }

                return success;
        }

        static MD5 md5 = System.Security.Cryptography.MD5.Create();


        public static string MD5(string input)
        {
            return BitConverter.ToString(md5.ComputeHash(Encoding.ASCII.GetBytes(input))).Replace("-", "").ToLower();
        }

        public static void WriteLine(string msg, ConsoleColor color)
        {
            Write(msg, color);
            Console.WriteLine();
        }


        public static void Write(string msg, ConsoleColor color)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = old;
        }

        public static void ClearLine()
        {
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorLeft = 0;
        }

        public static void WriteTransient(string msg)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write(msg);
                Console.CursorLeft = 0;
            }
        }

        public static void DumpToFile(StringBuilder sb)
        {
            string temp = System.IO.Path.GetTempFileName().Replace(".tmp", ".txt");
            System.IO.File.WriteAllText(temp, sb.ToString());
            Process.Start(temp);
        }

        public static long gcf(long a, long b)
        {
            while (b != 0L)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long lcm(long a, long b)
        {
            return (a / gcf(a, b)) * b;
        }
    }
}
