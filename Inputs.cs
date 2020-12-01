using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AdventOfCode2020
{
    static class Inputs
    {
        public static string ForDay(int day)
        {
            var file = @"C:\Personal\AdventOfCode2020\Inputs\Day" + day + ".txt";

            if (File.Exists(file))
            {
                return File.ReadAllText(file).Trim('\n');
            }
            return "";
        }
    }
}
