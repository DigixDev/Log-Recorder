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
    public partial class WSContainer : UserControl, IRenameable
    {
        private DA.Model.WS.Group WSGroup
        {
            get
            {
                return (DA.Model.WS.Group)this.DataContext;
            }
        }

        public WSContainer()
        {
            InitializeComponent();
        }

        private void btnAddComment_Click(object sender, RoutedEventArgs e)
        {
            Forms.CommantWindow win = new Forms.CommantWindow();
            win.Owner = Application.Current.MainWindow;
            if (win.ShowDialog() == true)
                WSGroup.Remarks.Add(win.Comment);
        }

        private void btnDeleteComment_Click(object sender, RoutedEventArgs e)
        {
            if(CommentList.SelectedIndex>=0)
                WSGroup.Remarks.RemoveAt(CommentList.SelectedIndex);
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
            return WSGroup.GroupSheetData.ExploratoryHoleNo;
        }

        public void SetSheetName(string name)
        {
            WSGroup.GroupSheetData.ExploratoryHoleNo = name;
        }

        public string GetSheetType()
        {
            return "WS";
        }
    }
}
