using Export.Model;
using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Export.Classes
{
    public class StratumBoxManager
    {
        public double _startY, _prevDepth;
        private int _activePageIndex;
        public List<StratumBox> listOfBoxes { get; set; }
        public StratumBoxManager()
        {
            _startY = 0;
            _activePageIndex = 0;
            _prevDepth = 0;
            listOfBoxes = new List<StratumBox>();
        }

        public void Add(Strata strata, double textHeight, int maxDepthPerPage)
        {
            StratumBox box;
            double endY, startY;
            int startPage, endPage;
            if (strata.Code < 100)
                return;
            startPage = (int)(_prevDepth / maxDepthPerPage);
            if (_prevDepth > 0 && _prevDepth % maxDepthPerPage == 0)
                startPage++;
            endPage = (int)(strata.HoleDepth / maxDepthPerPage);
            if (strata.HoleDepth % maxDepthPerPage == 0)
                endPage--;
            if(startPage==endPage)
            {
                startY = (_prevDepth % maxDepthPerPage) * 100;
                endY = (strata.HoleDepth % maxDepthPerPage) * 100;
                if(endY==0)
                    endY = maxDepthPerPage * 100;
                box = new StratumBox(strata, startY, endY, textHeight, startPage + 1, true);
                listOfBoxes.Add(box);
            }
            else
            {
                startY = (_prevDepth % maxDepthPerPage) * 100;
                endY = maxDepthPerPage * 100;
                box = new StratumBox(strata, startY, endY, textHeight, startPage + 1, false);
                listOfBoxes.Add(box);
                startPage++;
                while(endPage-startPage>=1)
                {
                    startY = 0;
                    endY = maxDepthPerPage * 100;
                    box = new StratumBox(strata, startY, endY, textHeight, startPage + 1, false);
                    listOfBoxes.Add(box);
                    startPage++;
                }
                startY = 0;
                endY = (strata.HoleDepth % maxDepthPerPage) * 100;
                if (endY == 0)
                    endY = maxDepthPerPage * 100;
                box = new StratumBox(strata, startY, endY, textHeight, startPage + 1, true);
                listOfBoxes.Add(box);
                startPage++;
            }
            _prevDepth = strata.HoleDepth;
        }

        public void Clear()
        {
            listOfBoxes.Clear();
        }
    }
}
