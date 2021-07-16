using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Log_Recorder.Forms
{
    /// <summary>
    /// Interaction logic for StrataPattern.xaml
    /// </summary>
    public partial class Patterns : Window
    {
        private ModelView.PatternModelView _patternModelView;
        public Patterns()
        {
            InitializeComponent();
            _patternModelView = new ModelView.PatternModelView();
            this.DataContext = _patternModelView;
        }

         private void StrataPatternList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (_strataPatternModelView.SelectedStrataPattern != null)
            //    Preview.Source = Export.Properties.Resources._102;
        }

         private void Window_Loaded(object sender, RoutedEventArgs e)
         {
         }
    }
}
