using Log_Recorder.DA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace Log_Recorder.DA.Class
{
    public class PatternRepository
    {
        private static List<Model.Pattern> _strata=null;
        private static List<Model.Pattern> _installations = null;
        public static List<Model.Pattern> GetStrataList()
        {
            if (_strata == null)
                LoadPattern();
            return _strata;
        }

        public static List<Model.Pattern> GetInstallationList()
        {
            if (_installations == null)
                LoadPattern();
            return _installations;
        }

        private static void LoadPattern()
        {
            _strata=new List<Pattern>();
            _installations=new List<Pattern>();
            System.IO.StreamReader reader = new System.IO.StreamReader("pattern.xml");
            XmlSerializer ser = new XmlSerializer(typeof(List<Model.Pattern>));
            List<Pattern> list = ser.Deserialize(reader) as List<Model.Pattern>;
            reader.Close();
            foreach(var item in list)
            {
                if (item.Code < 200)
                    _strata.Add(item);
                else
                    _installations.Add(item);
            }
            _installations.Sort();
            _strata.Sort();
        }
    }
}
