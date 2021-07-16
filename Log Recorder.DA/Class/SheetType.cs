using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Class
{
    public class SheetType
    {
        public const int WS = 1;
        public const int BH = 2;
        public const int RBH = 3;
        public const int TP = 4;
        public const int DP = 5;

        private static Dictionary<int, string> _sheetType = null;
        private static Dictionary<int, string> _sheetFullHeader = null;
        private static Dictionary<int, string> _sheetHeader = null;
        public static Dictionary<int, string> SheetTypes
        {
            get
            {
                if (_sheetType == null)
                    _sheetType = new Dictionary<int, string> { { SheetType.WS, "WS" }, { SheetType.BH, "BH" }, { SheetType.RBH, "RBH" }, { SheetType.TP, "TP" }, { SheetType.DP, "DP" } };
                return _sheetType;
            }
        }
        public static Dictionary<int, string> SheetFullHeader
        {
            get
            {
                if (_sheetFullHeader == null)
                    _sheetFullHeader = new Dictionary<int, string> { { SheetType.WS, "WS - Window/Windowless Sampling Borehole Record" }, { SheetType.BH, "BH - Cable Percussion Borehole Record" }, { SheetType.RBH, "RBH - Rotary Borehole Record" }, { SheetType.TP, "TP - Trial Pit Record" }, { SheetType.DP, "DP - Dynamic Probe" } };
                return _sheetFullHeader;
            }
        }


        public static string GetSheetTitle(int sheetType)
        {
            if (_sheetHeader == null)
                _sheetHeader = new Dictionary<int, string> { { SheetType.WS, "WINDOW/WINDOWLESS SAMPLING BOREHOLE RECORD" }, { SheetType.BH, "CABLE PERCUSSION BOREHOLE RECORD" }, { SheetType.RBH, "ROTARY BOREHOLE RECORD" }, { SheetType.TP, "TRIAL PIT RECORD" }, { SheetType.DP, "DYNAMIC PROBE" } };
            return _sheetHeader[sheetType];
        }
    }
}
