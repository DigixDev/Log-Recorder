using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Export.Model
{
    public class InstallationBox
    {

        public InstallationBox(Installation installation, double startY, double endY, int curPage, bool isLastPart = true)
        {
            BoxInstallation = installation;
            StartY = startY;
            EndY = endY;
            CurrentPage = curPage;
            IsLastPart = isLastPart;
        }

        public Installation BoxInstallation { get; set; }

        public double StartY { get; set; }

        public double EndY { get; set; }

        public int CurrentPage { get; set; }

        public bool IsLastPart { get; set; }

        public double ImageHeight { get { return EndY - StartY; } }
    }
}
