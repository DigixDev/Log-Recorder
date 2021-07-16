using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Log_Recorder.DA.Model
{
    public class Project : INotifyPropertyChanged
    {
        #region private variables

        private string _projectNumber,
            _clientName,
            _siteAddress;

        #endregion
        #region properties

        public string ProjectNumber
        {
            get { return _projectNumber; }
            set
            {
                _projectNumber = value;
                OnPropertyChanged("ProjectNumber");
            }
        }

        public string ClientName
        {
            get { return _clientName; }
            set
            {
                _clientName = value;
                OnPropertyChanged("ClientName");
            }
        }
        public string SiteAddress
        {
            get { return _siteAddress; }
            set
            {
                _siteAddress = value;
                OnPropertyChanged("SiteAddress");
            }
        }

        #endregion
        #region collections


        public ObservableCollection<WS.Group> WSGroupList { get; set; }
        
        [XmlIgnore]
        public ObservableCollection<BH.Group> BHGroupList { get; set; }

        [XmlIgnore]
        public ObservableCollection<RBH.Group> RBHGroupList { get; set; }

        [XmlIgnore]
        public ObservableCollection<TP.Group> TPGroupList { get; set; }

        [XmlIgnore]
        public ObservableCollection<DP.Group> DPGroupList { get; set; }

        #endregion
        #region constructor

        public Project()
        {
            MakeEmpty();
        }

        #endregion
        #region public methods

        public void MakeEmpty()
        {
            this.ClientName = this.SiteAddress = String.Empty;
            this.ProjectNumber = String.Empty;
            if (WSGroupList == null)
                WSGroupList = new ObservableCollection<WS.Group>();
            else
            {
                foreach (var item in WSGroupList)
                    item.Dispose();
                WSGroupList.Clear();
            }

            if (BHGroupList == null)
                BHGroupList = new ObservableCollection<BH.Group>();
            else
            {
                foreach (var item in BHGroupList)
                    item.Dispose();
                BHGroupList.Clear();
            }

            if (RBHGroupList == null)
                RBHGroupList = new ObservableCollection<RBH.Group>();
            else
            {
                foreach (var item in RBHGroupList)
                    item.Dispose();
                RBHGroupList.Clear();
            }

            if (TPGroupList == null)
                TPGroupList = new ObservableCollection<TP.Group>();
            else
            {
                foreach (var item in TPGroupList)
                    item.Dispose();
                TPGroupList.Clear();
            }

            if (DPGroupList == null)
                DPGroupList = new ObservableCollection<DP.Group>();
            else
            {
                foreach (var item in DPGroupList)
                    item.Dispose();
                DPGroupList.Clear();
            }
        }

        #endregion
        #region events

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        public bool IsValid
        {
            get
            {
                return true;
            }
        }

        public void RemoveWS(WS.Group group)
        {
            WSGroupList.Remove(group);
            group.Clear();
        }

        public void RemoveBH(BH.Group group)
        {
            BHGroupList.Remove(group);
            group.Clear();
        }

        public void RemoveRBH(RBH.Group group)
        {
            RBHGroupList.Remove(group);
            group.Clear();
        }

        public void RemoveTP(TP.Group group)
        {
            TPGroupList.Remove(group);
            group.Clear();
        }

        public void RemoveDP(DP.Group group)
        {
            DPGroupList.Remove(group);
            group.Clear();
        }
    }
}
