using Log_Recorder.Classes;
using Log_Recorder.DA.Class;
using Log_Recorder.Forms;
using Log_Recorder.Interface;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shell;

namespace Log_Recorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModelView.ProjectModelView _projectModelView;
        private bool _forceToExit;
        public MainWindow()
        {
            InitializeComponent();
            _forceToExit = false;
            _projectModelView = new ModelView.ProjectModelView();
            DataContext = _projectModelView;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Owner = this;
            if (loginWindow.ShowDialog() == false)
            {
                _forceToExit = true;
                Application.Current.Shutdown();
            }
            InitStatus();
        }

        private void InitProject()
        {
            DA.Class.Global.FileName = String.Empty;
            this.Title = DA.Class.Global.PageTitle;
        }

        private void InitStatus()
        {
            txtCurrentDate.Text = DateTime.Now.ToLongDateString();
            txtCurrentUserName.Text = DA.Class.Global.ActiveUserInfo.UserName;
            txtCurrentUserRoleTitle.Text = DA.Class.Global.ActiveUserInfo.UserRoleTitle;

            statusItemDate.Visibility = System.Windows.Visibility.Visible;
            statusItemUserName.Visibility = System.Windows.Visibility.Visible;
            statusItemUserRole.Visibility = System.Windows.Visibility.Visible;
        }

        private void navCompanyLink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }

        #region menu commands

        private void CommandExitApplication_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CommandExport_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _projectModelView.HasProject();
            e.Handled = true;
        }

        private void CommandOpenProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Log Recorder Project (*.xmp)|*.xmp";
            if (dlg.ShowDialog() == true)
            {
                NewForm();
                _projectModelView.OpenProject(dlg.FileName);
                this.Title = DA.Class.Global.PageTitle;
                MakeTabs();
            }
        }

        private void MakeTabs()
        {
            foreach (var item in _projectModelView.ActiveProject.WSGroupList)
                AddTabItem(item, DA.Class.SheetType.WS);
            foreach (var item in _projectModelView.ActiveProject.BHGroupList)
                AddTabItem(item, DA.Class.SheetType.BH);
            foreach (var item in _projectModelView.ActiveProject.RBHGroupList)
                AddTabItem(item, DA.Class.SheetType.RBH);
            foreach (var item in _projectModelView.ActiveProject.TPGroupList)
                AddTabItem(item, DA.Class.SheetType.TP);
            foreach (var item in _projectModelView.ActiveProject.DPGroupList)
                AddTabItem(item, DA.Class.SheetType.DP);
            if (MainTabControl.Items.Count > 0)
                MainTabControl.SelectedIndex = 0;
        }

        private void CommandNewProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (MainTabControl.Items.Count > 0 )
            {
                if (MessageBox.Show("Delete current project", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
            }
            NewForm();
            this.Title = DA.Class.Global.PageTitle;
        }

        private void NewForm()
        {
            foreach (TabItem tab in MainTabControl.Items)
            {
                tab.Content = null;
                tab.DataContext = null;
            }
            MainTabControl.Items.Clear();
            _projectModelView.NewProject();
        }

        private void CommandSaveProject_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Log Recorder Project (*.xmp)|*.xmp";
            if (String.IsNullOrEmpty( DA.Class.Global.FileName))
                dlg.FileName = String.Format("{0}.xmp", _projectModelView.ActiveProject.ProjectNumber);
            else
                dlg.FileName = DA.Class.Global.FileName;
            if (dlg.ShowDialog() == true)
            {
                _projectModelView.SaveProject(dlg.FileName);
                this.Title = DA.Class.Global.PageTitle;
            }
        }

        private void CommandSaveProject_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _projectModelView.HasProject();
            e.Handled = true;
        }

        private void CommandOpenProjectInfo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProjectWindow win = new ProjectWindow();
            win.Owner = this;
            win.ProjectModelView = _projectModelView;
            win.ShowDialog();
        }

        private void CommandOpenUserList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UserListWindow win = new UserListWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void CommandOpenUserList_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = DA.Class.Global.ActiveUserInfo.IsMainAdmin;
            e.Handled = true;
        }

        private void CommandChangePassword_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChangePassword win = new ChangePassword();
            win.Owner = this;
            win.ShowDialog();
        }

        private void CommandShowStrataPatterns_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Patterns win = new Patterns();
            win.Owner = this;
            win.ShowDialog();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_forceToExit == false)
            {
                if (MessageBox.Show("Do you want to Exit?", "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void CommandDeleteSheet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RemoveCurrentTab();
        }

        private void CommandDeleteSheet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MainTabControl.SelectedIndex == -1 ? false : true);
            e.Handled = true;
        }

        #endregion

        private void AddSheet()
        {
            NewSheet win = new NewSheet();
            object group = null;
            win.Owner = this;
            win.SheetType = DA.Class.SheetType.WS;
            if(win.ShowDialog()==true)
            {
                TabItem item = new TabItem();
                switch(win.SheetType)
                {
                    case DA.Class.SheetType.WS:
                        group = _projectModelView.AddNewSheetWS(win.SheetName);
                        break;
                    case DA.Class.SheetType.BH:
                        group = _projectModelView.AddNewSheetBH(win.SheetName);
                        break;
                    case DA.Class.SheetType.RBH:
                        group = _projectModelView.AddNewSheetRBH(win.SheetName);
                        break;
                    case DA.Class.SheetType.TP:
                        group = _projectModelView.AddNewSheetTP(win.SheetName);
                        break;
                    case DA.Class.SheetType.DP:
                        group = _projectModelView.AddNewSheetDP(win.SheetName);
                        break;
                }
                AddTabItem(group, win.SheetType);
                MainTabControl.SelectedIndex = MainTabControl.Items.Count - 1;
            }
        }

        private void AddTabItem(object obj, int sheetType, int index=-1)
        {
            TabItem item = new TabItem();
            switch (sheetType)
            {
                case DA.Class.SheetType.WS:
                    DA.Model.WS.Group ws = obj as DA.Model.WS.Group;
                    Controls.UserControls.WSContainer wsContainer = new Controls.UserControls.WSContainer();
                    wsContainer.DataContext = ws;
                    wsContainer.SheetType = DA.Class.SheetType.BH;
                    item.Header = ws.GroupSheetData.ExploratoryHoleNo + " - [WS]";
                    item.Content = wsContainer;
                    break;
                case DA.Class.SheetType.BH:
                    DA.Model.BH.Group bh = obj as DA.Model.BH.Group;
                    Controls.UserControls.BHContainer bhContainer = new Controls.UserControls.BHContainer();
                    bhContainer.DataContext = bh;
                    bhContainer.SheetType = DA.Class.SheetType.BH;
                    item.Header = bh.GroupSheetData.ExploratoryHoleNo + " - [BH]";
                    item.Content = bhContainer;
                    break;
                case DA.Class.SheetType.RBH:
                    DA.Model.RBH.Group rbh = obj as DA.Model.RBH.Group;
                    Controls.UserControls.RBHContainer rbhContainer = new Controls.UserControls.RBHContainer();
                    rbhContainer.DataContext = rbh;
                    item.Header = rbh.GroupSheetData.ExploratoryHoleNo + " - [RBH]";
                    item.Content = rbhContainer;
                    break;
                case DA.Class.SheetType.TP:
                    DA.Model.TP.Group tp = obj as DA.Model.TP.Group;
                    Controls.UserControls.TPContainer tpContainer = new Controls.UserControls.TPContainer();
                    tpContainer.DataContext = tp;
                    item.Header = tp.GroupSheetData.ExploratoryHoleNo + " - [TP]";
                    item.Content = tpContainer;
                    break;
                case DA.Class.SheetType.DP:
                    DA.Model.DP.Group dp = obj as DA.Model.DP.Group;
                    Controls.UserControls.DPContainer dpContainer = new Controls.UserControls.DPContainer();
                    dpContainer.DataContext = dp;
                    item.Header = dp.GroupSheetData.ExploratoryHoleNo + " - [DP]";
                    item.Content = dpContainer;
                    break;
            }
            if(index==-1)
            {
            MainTabControl.Items.Add(item);
                MainTabControl.SelectedIndex=MainTabControl.Items.Count-1;
            }
            else
                MainTabControl.Items.Insert(index, item);
        }

        private void RemoveCurrentTab()
        {
            RemoveTab(MainTabControl.SelectedIndex);
        }

        private void RemoveTab(int index)
        {
            TabItem item = MainTabControl.Items[index] as TabItem;
            if (item.Content is Log_Recorder.Controls.UserControls.WSContainer)
            {
                Log_Recorder.Controls.UserControls.WSContainer ws = item.Content as Log_Recorder.Controls.UserControls.WSContainer;
                _projectModelView.ActiveProject.RemoveWS(ws.DataContext as DA.Model.WS.Group);
                ws.DataContext = null;
            }
            else if (item.Content is Log_Recorder.Controls.UserControls.BHContainer)
            {
                Log_Recorder.Controls.UserControls.BHContainer bh = item.Content as Log_Recorder.Controls.UserControls.BHContainer;
                _projectModelView.ActiveProject.RemoveBH(bh.DataContext as DA.Model.BH.Group);
                bh.DataContext = null;
            }
            else if (item.Content is Log_Recorder.Controls.UserControls.RBHContainer)
            {
                Log_Recorder.Controls.UserControls.RBHContainer rbh = item.Content as Log_Recorder.Controls.UserControls.RBHContainer;
                _projectModelView.ActiveProject.RemoveRBH(rbh.DataContext as DA.Model.RBH.Group);
                rbh.DataContext = null;
            }
            else if (item.Content is Log_Recorder.Controls.UserControls.TPContainer)
            {
                Log_Recorder.Controls.UserControls.TPContainer tp = item.Content as Log_Recorder.Controls.UserControls.TPContainer;
                _projectModelView.ActiveProject.RemoveTP(tp.DataContext as DA.Model.TP.Group);
                tp.DataContext = null;
            }
            else if (item.Content is Log_Recorder.Controls.UserControls.DPContainer)
            {
                Log_Recorder.Controls.UserControls.DPContainer dp = item.Content as Log_Recorder.Controls.UserControls.DPContainer;
                _projectModelView.ActiveProject.RemoveDP(dp.DataContext as DA.Model.DP.Group);
                dp.DataContext = null;
            }

            item.Content = null;
            MainTabControl.Items.RemoveAt(index);
        }

        private void CommandExport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<string> holeNoList=new List<string>();
            Forms.Export win = new Forms.Export();
            win.Owner = this;
            foreach (TabItem item in MainTabControl.Items)
                holeNoList.Add(((IRenameable)item.Content).GetSheetName());
            win.HoleNoList = holeNoList;
            if (win.ShowDialog() == true)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Adobe Acrobat PDF File (*.pdf)|*.pdf";
                dlg.FileName = _projectModelView.ActiveProject.ProjectNumber.ToString() + ".pdf";
                if (dlg.ShowDialog() == true)
                {
                    if(win.ExportAll==true)
                        _projectModelView.SaveAsPDF(dlg.FileName);
                    else
                        _projectModelView.SaveAsPDF(dlg.FileName, win.SelectedHoleNo);
                }
            }
        }

        private void CommandAddSheet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddSheet();
        }

        private void CommandRenameSheet_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            RenameSheet win;
            IRenameable uc = ((TabItem)MainTabControl.SelectedItem).Content as IRenameable;
            Debug.Assert(uc == null);
            win = new RenameSheet();
            win.Owner = this;
            win.SheetName = uc.GetSheetName();
            if(win.ShowDialog()==true)
            {
                uc.SetSheetName(win.SheetName);
                ((TabItem)MainTabControl.SelectedItem).Header = win.SheetName + " - [" + uc.GetSheetType() + "]";
            }
        }

        private void CommandRenameSheet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (MainTabControl.Items.Count == 0 ? false : true);
            e.Handled = true;
        }

        private void ChangeType_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetAciveEditableSheetType() > 0)
                e.CanExecute = true;
            else
                e.CanExecute = false;
            e.Handled = true;
        }

        private int GetAciveEditableSheetType()
        {
            if (MainTabControl.SelectedIndex == -1)
                return -1;
            TabItem cur=MainTabControl.Items[MainTabControl.SelectedIndex] as TabItem;
            if(cur==null)
                return -1;
            if (cur.Content is Controls.UserControls.WSContainer)
                return DA.Class.SheetType.WS;
            if (cur.Content is Controls.UserControls.BHContainer)
                return DA.Class.SheetType.BH;
            if (cur.Content is Controls.UserControls.RBHContainer)
                return DA.Class.SheetType.RBH;
            if (cur.Content is Controls.UserControls.TPContainer)
                return DA.Class.SheetType.TP;
            return -1;
        }

        private void ChangeTypeToWS_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetAciveEditableSheetType() == DA.Class.SheetType.WS)
                e.CanExecute = false;
            else
                e.CanExecute = true;
            e.Handled = true;
        }

        private void ChangeTypeToBH_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetAciveEditableSheetType() == DA.Class.SheetType.BH)
                e.CanExecute = false;
            else
                e.CanExecute = true;
            e.Handled = true;
        }

        private void ChangeTypeToRBH_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetAciveEditableSheetType() == DA.Class.SheetType.RBH)
                e.CanExecute = false;
            else
                e.CanExecute = true;
            e.Handled = true;
        }

        private void ChangeTypeToTP_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (GetAciveEditableSheetType() == DA.Class.SheetType.TP)
                e.CanExecute = false;
            else
                e.CanExecute = true;
            e.Handled = true;
        }

        private void ChangeTypeToTP_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem cur = MainTabControl.Items[MainTabControl.SelectedIndex] as TabItem;
            ChangeSheetType(cur, DA.Class.SheetType.TP);
        }

        private void ChangeTypeToRBH_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem cur=MainTabControl.Items[MainTabControl.SelectedIndex] as TabItem;
            ChangeSheetType(cur, DA.Class.SheetType.RBH);
        }

        private void ChangeTypeToBH_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem cur = MainTabControl.Items[MainTabControl.SelectedIndex] as TabItem;
            ChangeSheetType(cur, DA.Class.SheetType.RBH);
        }

        private void ChangeTypeToWS_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem cur = MainTabControl.Items[MainTabControl.SelectedIndex] as TabItem;
            ChangeSheetType(cur, DA.Class.SheetType.RBH);
        }

        private void ChangeSheetType(TabItem cur, int newType)
        {
            int index = MainTabControl.SelectedIndex;
            IChangeable old;
            old = ((UserControl)cur.Content).DataContext as IChangeable;
            switch (newType)
            {
                case DA.Class.SheetType.WS:
                    DA.Model.WS.Group ws = _projectModelView.AddNewSheetWS(old.GetGroupName());
                    ws.CopyAllLists(old.GetStrataList(), old.GetSampleOrTestList(), old.GetWaterLevelList(), old.GetInstallationList(), old.GetRemarks(), old.GetGroupSheetData());
                    AddTabItem(ws, newType);
                    break;
                case DA.Class.SheetType.BH:
                    DA.Model.BH.Group bh = _projectModelView.AddNewSheetBH(old.GetGroupName());
                    bh.CopyAllLists(old.GetStrataList(), old.GetSampleOrTestList(), old.GetWaterLevelList(), old.GetInstallationList(), old.GetRemarks(), old.GetGroupSheetData());
                    AddTabItem(bh, newType);
                    break;
                case DA.Class.SheetType.RBH:
                    DA.Model.RBH.Group rbh = _projectModelView.AddNewSheetRBH(old.GetGroupName());
                    rbh.CopyAllLists(old.GetStrataList(), old.GetSampleOrTestList(), old.GetWaterLevelList(), old.GetInstallationList(), old.GetRemarks(), old.GetGroupSheetData());
                    AddTabItem(rbh, newType);
                    break;
                case DA.Class.SheetType.TP:
                    DA.Model.TP.Group tp = _projectModelView.AddNewSheetTP(old.GetGroupName());
                    tp.CopyAllLists(old.GetStrataList(), null, null, null, old.GetRemarks(), old.GetGroupSheetData());
                    AddTabItem(tp, newType);
                    break;
                default: return;
            }
            RemoveTab(index);
        }


    }
}
