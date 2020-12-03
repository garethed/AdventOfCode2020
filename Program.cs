using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            var days = Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(Day).IsAssignableFrom(t) && t != typeof(Day)).OrderByDescending(d => int.Parse(d.Name.Substring(3)));

            if (args.Length > 0 && args[0].ToLower().Contains("all"))
            {
                foreach (var type in days.Reverse())
                {
                   if (!RunDay(type))
                    {
                        break;
                    }
                }
            }
            else if (args.Length > 0)
            {
                RunDay(days.Where(d => d.Name.Substring(3) == args[0]).First());
            }
            else
            {
                RunDay(days.First());
            }

            

            Utils.WriteLine("** FINISHED **", ConsoleColor.DarkCyan);
            //Console.ReadLine();

        }

        static bool RunDay(Type dayType)
        {
            var day = (Day)dayType.GetConstructor(new Type[0]).Invoke(new object[0]);

            try
            {
                Utils.WriteLine("**** DAY " + day.Index + "****", ConsoleColor.DarkCyan);

                Checkpoint();
                Utils.WriteLine("** TESTS **", ConsoleColor.DarkYellow);
                var test = TestAttribute.TestAnnotatedMethods(day);
                Checkpoint();

                if (test)
                {
                    Utils.WriteLine("** SOLUTIONS **", ConsoleColor.DarkYellow);
                    Utils.Write("Part 1: ", ConsoleColor.White);
                    Utils.WriteLine(day.Part1S(day.Input), ConsoleColor.DarkMagenta);
                    Checkpoint();
                    Utils.Write("Part 2: ", ConsoleColor.White);
                    Utils.WriteLine(day.Part2S(day.Input), ConsoleColor.DarkMagenta);
                    Checkpoint();
                    return true;
                }

            }
            catch (NotImplementedException)
            {
            }

            return false;
        }

        static DateTime timer = DateTime.MaxValue;

        private static void Checkpoint()
        {
            if (timer != DateTime.MaxValue)
            {
                Utils.WriteLine("Completed in " + (DateTime.Now - timer).TotalSeconds.ToString("0.000s"), ConsoleColor.Gray);
            }
            timer = DateTime.Now;
        }
    }
}
