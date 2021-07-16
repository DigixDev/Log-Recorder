using Log_Recorder.DA.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model.DP
{
    public class Group : IDisposable, IClearGroup
    {
        public SheetData GroupSheetData { get; set; }

        public string ProbeType { get; set; }
        public string ProbeNumber { get; set; }
        public string Operator { get; set; }
        public Row[] Rows { get; set; }

        public Group()
        {
            Rows = new Row[20];
            for (int i = 0; i < 20; i++)
                Rows[i] = new Row(i);
            GroupSheetData = new SheetData();
        }

        public void Clear()
        {
            foreach (var row in Rows)
                row.Clear();
        }

        public void Dispose()
        {
            for (int i = 0; i < 20; i++)
            {
                Rows[i].Clear();
                Rows[i] = null;
            }
            Rows = null;
        }
    }
}
