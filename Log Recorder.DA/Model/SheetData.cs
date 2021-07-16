using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Model
{
    public class SheetData: INotifyPropertyChanged
    {
        #region private variables

        private string  _equipmentType,
            _exploratoryHoleNo,
            _groundLevel,
            _checkedBy,
            _loggedBy;

        private DateTime? _dateCommenced, _dateCompleted;

        #endregion
        #region properties

        public string ExploratoryHoleNo 
        {
            get { return _exploratoryHoleNo; }
            set
            {
                _exploratoryHoleNo = value;
                OnPropertyChanged("ExploratoryHoleNo");
            }
        }

        public string GroundLevel
        {
            get { return _groundLevel; }
            set
            {
                _groundLevel = value;
                OnPropertyChanged("GroundLevel");
            }
        }

        public DateTime? DateCompleted
        {
            get { return _dateCompleted; }
            set
            {
                _dateCompleted = value;
                OnPropertyChanged("DateCompleted");
            }
        }

        public string CheckedBy
        {
            get { return _checkedBy; }
            set
            {
                _checkedBy = value;
                OnPropertyChanged("CheckedBy");
            }
        }

        public string LoggedBy
        {
            get { return _loggedBy; }
            set
            {
                _loggedBy = value;
                OnPropertyChanged("LoggedBy");
            }
        }

        public DateTime? DateCommenced
        {
            get { return _dateCommenced; }
            set
            {
                _dateCommenced = value;
                OnPropertyChanged("DateCommenced");
            }
        }

        public string EquimnetType
        {
            get { return _equipmentType; }
            set
            {
                _equipmentType = value;
                OnPropertyChanged("EquimnetType");
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        internal void Copy(SheetData sheetData)
        {
            this.CheckedBy = sheetData.CheckedBy;
            this.DateCommenced = sheetData.DateCommenced;
            this.DateCompleted = sheetData.DateCompleted;
            this.EquimnetType = sheetData.EquimnetType;
            this.ExploratoryHoleNo = this.ExploratoryHoleNo;
            this.GroundLevel = sheetData.GroundLevel; ;
            this.LoggedBy = sheetData.LoggedBy;
        }
    }
}
