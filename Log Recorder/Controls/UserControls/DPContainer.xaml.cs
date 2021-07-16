using Log_Recorder.Interface;
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
    /// Interaction logic for DPContainer.xaml
    /// </summary>
    public partial class DPContainer : UserControl, IRenameable
    {
        private DA.Model.DP.Group DPGroup
        {
            get
            {
                return (DA.Model.DP.Group)this.DataContext;
            }
        }
        public DPContainer()
        {
            InitializeComponent();
        }

        public string GetSheetName()
        {
            return DPGroup.GroupSheetData.ExploratoryHoleNo;
        }

        public void SetSheetName(string name)
        {
            DPGroup.GroupSheetData.ExploratoryHoleNo = name;
        }

        public string GetSheetType()
        {
            return "DP";
        }
    }
}
