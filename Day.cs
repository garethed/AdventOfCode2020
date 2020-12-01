using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    abstract class Day
    {
        public abstract string Part1(string input);
        public abstract string Part2(string input);
        //public abstract dynamic Input { get; }

        public int Index
        {
            get { return int.Parse(this.GetType().Name.Substring(3)); }
        }

        public virtual string Input => Inputs.ForDay(Index);
    }
}
