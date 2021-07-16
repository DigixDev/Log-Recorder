using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.Interface
{
    interface IRenameable
    {
        string GetSheetName();
        void SetSheetName(string name);
        string GetSheetType();
    }
}
