using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Log_Recorder.ModelView
{
    public class PatternModelView : ModelViewBase
    {
        private DA.Model.Pattern _selectedStrataPattern;

        public List<DA.Model.Pattern> StrataPatterns { get; set; }
        public DA.Model.Pattern SelectedStrataPattern 
        {  
            get{ return _selectedStrataPattern ;}
            set
            {
                int code = value.Code;
                _selectedStrataPattern=value;
                if (value != null)
                    PreviewBox.Source = new BitmapImage(new Uri(@"pack://application:,,,/Images/Patterns/" + value.Code.ToString() + ".bmp"));
            } 
        }

        public PatternModelView()
        {
            StrataPatterns = DA.Class.PatternRepository.GetStrataList();
        }

        public Image PreviewBox { get; set; }
    }
}
