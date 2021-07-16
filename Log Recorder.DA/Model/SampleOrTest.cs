using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model
{
    public class SampleOrTest : INotifyPropertyChanged
    {
        private Nullable<int> _r3, _r4, _r5, _r6;
        public string Type { get; set; }
        public double Depth { get; set; }
        public string R1 { get; set; }
        public string R2 { get; set; }
        public Nullable<int> R3 
        {
            get { return _r3; }
            set
            {
                _r3 = value;
                OnValueChanged();
            }
        }
        public int? R4 
        {
            get { return _r4; }
            set
            {
                _r4 = value;
                OnValueChanged();
            }
        }
        public int? R5 
        {
            get { return _r5; }
            set
            {
                _r5 = value;
                OnValueChanged();
            }
        }
        public int? R6 
        {
            get { return _r6; }
            set
            {
                _r6 = value;
                OnValueChanged();
            }
        }
        public int? RN { get; set; }
        public string Comment { get; set; }
        private void OnValueChanged()
        {
            RN = (_r3 ?? 0) + (_r4 ?? 0) + (_r5 ?? 0) + (_r6 ?? 0);
            if (RN == 0)
                RN = null; ;

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("RN"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
