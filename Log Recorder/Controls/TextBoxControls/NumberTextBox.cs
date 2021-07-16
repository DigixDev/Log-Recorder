using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace Log_Recorder.Controls.TextBoxControls
{
    public class NumberTextBox : TextBox
    {
        public int NumberValue 
        {
            get
            {
                if (this.Text == string.Empty)
                    return 0;
                else
                    return Convert.ToInt32(this.Text);
            }
            set { this.Text = value.ToString("N0"); }

        }
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Delete || e.Key == Key.Back || e.Key==Key.Tab)
                base.OnKeyDown(e);
            else
                e.Handled = true;
        }

    }
}
