using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    abstract class Day
    {
        public virtual string Part1S(string input)
        {
            return Part1(input).ToString();
        }
        public virtual string Part2S(string input)
        {
            return Part2(input).ToString();
        }
        
        public virtual int Part1(string input)
        {
            return 666;
        }

        public virtual int Part2(string input)
        {
            return 666;
        }


        public int Index
        {
            get { return int.Parse(this.GetType().Name.Substring(3)); }
        }

        public virtual string Input => Inputs.ForDay(Index);
    }
}
