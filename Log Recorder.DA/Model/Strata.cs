using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model
{
    public class Strata
    {
        public double HoleDepth { get; set; }
        public double? WaterStrikes { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
    }
}
