using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Log_Recorder.Classes
{
    internal class ControlButtons:IDisposable
    {
        private List<TextBox> _textBoxList;
        private Button _button;

        public ControlButtons()
        {
            _textBoxList = new List<TextBox>();
            _button = null;
        }

        public void Add(TextBox textBox)
        {
            _textBoxList.Add(textBox);
            textBox.TextChanged += textBox_TextChanged;
            ControlForm();
        }

        public void ApplyButton(Button button)
        {
            _button = button;
            ControlForm();
        }

        void textBox_TextChanged(object sender, EventArgs e)
        {
            ControlForm();
        }

        private void ControlForm()
        {
            if (_button == null)
                return;
            foreach(var textBox in _textBoxList)
                if (textBox.Text.Length == 0)
                {
                    _button.IsEnabled = false;
                    return;
                }
            _button.IsEnabled = true;
        }

        public void Dispose()
        {
            _textBoxList.Clear();
            _textBoxList = null;
        }
    }
}
