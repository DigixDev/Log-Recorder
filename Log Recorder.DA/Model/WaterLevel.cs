using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model
{
    public class WaterLevel
    {
        public DateTime Date { get; set; }
        public double? HoleDepth { get; set; }
        public double? CasingDepth { get; set; }
        public string WaterLevelOnStrike { get; set; }
        public string WaterLevelAfter20Mins { get; set; }

        public WaterLevel()
        {
            Date = DateTime.Now;
        }
    }
}
