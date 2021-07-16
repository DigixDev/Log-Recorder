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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Log_Recorder.Controls.UserControls
{
    /// <summary>
    /// Interaction logic for ListItem.xaml
    /// </summary>
    public partial class PatternListItem : UserControl
    {


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public int Code
        {
            get { return (int)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        public static readonly DependencyProperty CodeProperty = DependencyProperty.Register("Code", typeof(int), typeof(PatternListItem), new PropertyMetadata(-1, CodeValueChanged));

        private static void CodeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PatternListItem obj = d as PatternListItem;
            obj.PreviewImage.Source=new BitmapImage(new Uri(@"pack://application:,,,/Images/Patterns/" + obj.Code.ToString() + ".png"));
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(PatternListItem), new PropertyMetadata(null));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PatternListItem), new PropertyMetadata(String.Empty));


        public PatternListItem()
        {
            InitializeComponent();
        }
    }
}
