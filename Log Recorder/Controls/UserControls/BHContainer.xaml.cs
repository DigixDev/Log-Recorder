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
    /// Interaction logic for WSContainer.xaml
    /// </summary>
    public partial class BHContainer : UserControl, IRenameable
    {
        private DA.Model.BH.Group BHGroup
        {
            get
            {
                return (DA.Model.BH.Group)this.DataContext;
            }
        }

        public BHContainer()
        {
            InitializeComponent();
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            Forms.CommantWindow win = new Forms.CommantWindow();
            win.Owner = Application.Current.MainWindow;
            if (win.ShowDialog() == true)
                BHGroup.Remarks.Add(win.Comment);
        }

        private void btnDeleteComment_Click(object sender, RoutedEventArgs e)
        {
            if(CommentList.SelectedIndex>=0)
                BHGroup.Remarks.RemoveAt(CommentList.SelectedIndex);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionViewSource strataViewSource = ((CollectionViewSource)this.FindResource("StrataComboBoxData"));
            strataViewSource.Source = DA.Class.PatternRepository.GetStrataList();

            CollectionViewSource installationViewSource = ((CollectionViewSource)this.FindResource("InstallationComboBoxData"));
            installationViewSource.Source = DA.Class.PatternRepository.GetInstallationList();
        }

        public int SheetType { get; set; }

        public string GetSheetName()
        {
            return BHGroup.GroupSheetData.ExploratoryHoleNo;
        }

        public void SetSheetName(string name)
        {
            BHGroup.GroupSheetData.ExploratoryHoleNo = name;
        }
        
        public string GetSheetType()
        {
            return "BH";
        }
    }
}
