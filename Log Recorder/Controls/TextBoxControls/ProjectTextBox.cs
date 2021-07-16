using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Log_Recorder.Controls.TextBoxControls
{
    public class ProjectTextBox: NumberTextBox
    {
        private const string NoProject = "[No Project]";


        public new int NumberValue
        {
            get { return (int)GetValue(NumberValueProperty); }
            set { SetValue(NumberValueProperty, value); }
        }

        public static readonly DependencyProperty NumberValueProperty = DependencyProperty.Register("NumberValue", typeof(int), typeof(ProjectTextBox), new PropertyMetadata(-1, NumberValueChanged));

        private static void NumberValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProjectTextBox textBox = d as ProjectTextBox;
            if (e.NewValue != null || e.NewValue.ToString() != "-1")
                textBox.Text = ((int)e.NewValue).ToString("N0");
        }

        protected override void OnGotFocus(System.Windows.RoutedEventArgs e)
        {
            this.Foreground = Brushes.Black;

            if (this.Text == NoProject)
                this.Text = String.Empty;
            else if (NumberValue == -1)
                this.Text = String.Empty;
            else
                this.Text = NumberValue.ToString();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(System.Windows.RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (this.Text == String.Empty)
            {
                this.Foreground = Brushes.Gray;
                this.Text = NoProject;
            }
            else
                this.NumberValue=Convert.ToInt32(this.Text);
        }
    }
}
