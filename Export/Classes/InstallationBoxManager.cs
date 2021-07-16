using Export.Model;
using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Export.Classes
{
    public class InstallationBoxManager
    {
        public double _startY, _prevDepth;
        private int _activePageIndex;
        public List<InstallationBox> listOfBoxes { get; set; }
        public InstallationBoxManager()
        {
            _startY = 0;
            _activePageIndex = 0;
            _prevDepth = 0;
            listOfBoxes = new List<InstallationBox>();
        }

        public void Add(Installation installation, int maxDepthPerPage)
        {
            InstallationBox box;
            double endY, startY;
            int startPage, endPage;
            startPage = (int)(_prevDepth / maxDepthPerPage);
            //if (_prevDepth > 0 && _prevDepth % maxDepthPerPage == 0)
            //    startPage++;
            endPage = (int)(installation.Depth / maxDepthPerPage);
            if (installation.Depth % maxDepthPerPage == 0)
                endPage--;
            if(startPage==endPage)
            {
                startY = (_prevDepth % maxDepthPerPage) * 100;
                endY = (installation.Depth % maxDepthPerPage) * 100;
                if (endY == 0)
                    endY = maxDepthPerPage * 100;
                box = new InstallationBox(installation, startY, endY, startPage + 1, true);
                listOfBoxes.Add(box);
            }
            else
            {
                startY = (_prevDepth % maxDepthPerPage) * 100;
                endY = maxDepthPerPage * 100;
                box = new InstallationBox(installation, startY, endY, startPage + 1, false);
                listOfBoxes.Add(box);
                startPage++;
                while(endPage-startPage>=1)
                {
                    startY = 0;
                    endY = maxDepthPerPage * 100;
                    box = new InstallationBox(installation, startY, endY, startPage + 1, false);
                    listOfBoxes.Add(box);
                    startPage++;
                }
                startY = 0;
                endY = (installation.Depth % maxDepthPerPage) * 100;
                if (endY == 0)
                    endY = maxDepthPerPage * 100;
                box = new InstallationBox(installation, startY, endY, startPage + 1, true);
                listOfBoxes.Add(box);
                startPage++;
            }
            _prevDepth = installation.Depth;
        }

        public void Clear()
        {
            listOfBoxes.Clear();
        }
    }
}
