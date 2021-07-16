using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model.DP
{
    public class Row
    {
        public string RowTitle { get; set; }
        public int? V01 { get; set; }
        public int? V02 { get; set; }
        public int? V03 { get; set; }
        public int? V04 { get; set; }
        public int? V05 { get; set; }
        public int? V06 { get; set; }
        public int? V07 { get; set; }
        public int? V08 { get; set; }
        public int? V09 { get; set; }
        public int? V10 { get; set; }
        public string Torque { get; set; }

        public Row()
        {
            Clear();
        }

        public Row(string rowTitle)
        {
            Clear();
            RowTitle = RowTitle;
        }

        public Row(int rowIndex)
        {
            Clear();
            RowTitle = String.Format("{0}-{1}m", rowIndex, rowIndex + 1);
        }

        public int? GetAt(int index)
        {
            switch(index)
            {
                case 1:
                    return V01;
                case 2:
                    return V02;
                case 3:
                    return V03;
                case 4:
                    return V04;
                case 5:
                    return V05;
                case 6:
                    return V06;
                case 7:
                    return V07;
                case 8:
                    return V08;
                case 9:
                    return V09;
                case 10:
                    return V10;
                default:
                    return null;
            }
        }
        internal void Clear()
        {
            V01 = V02 = V03 = V04 = V05 = V06 = V07 = V08 = V09 = V10 = null;
            Torque = String.Empty;
        }
    }
}
