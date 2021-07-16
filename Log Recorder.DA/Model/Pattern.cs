using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model
{
    public class Pattern : IComparable<Pattern>
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public int CompareTo(Pattern other)
        {
            return this.Description.CompareTo(other.Description);
        }
    }
}
