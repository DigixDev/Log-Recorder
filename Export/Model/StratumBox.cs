using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Export.Model
{
    public class StratumBox
    {

        public StratumBox(Strata strata, double startY, double endY, double textHeight, int curPage, bool isLastPart=true)
        {
            BoxStrata = strata;
            StartImageY = StartTextY = startY;
            EndImageY = endY;
            EndTextY = Math.Max(StartTextY + textHeight, EndImageY);
            TextHeight = textHeight;
            CurrentPage = curPage;
            IsLastPart = isLastPart;
        }

        public Strata BoxStrata { get; set; }

        public double StartTextY { get; set; }

        public double StartImageY { get; set; }

        public double EndImageY { get; set; }

        public double EndTextY { get; set; }

        public int CurrentPage { get; set; }

        public bool IsLastPart { get; set; }

        public double ImageHeight { get { return EndImageY - StartImageY; } }

        public double TextHeight { get; set; }

        public void UpdateTextHeight(ref double prevEndTextY, double textHeight)
        {
            StartTextY = prevEndTextY;
            EndTextY = Math.Max(EndTextY, StartTextY + textHeight);
            prevEndTextY = EndTextY;
        }
    }
}
