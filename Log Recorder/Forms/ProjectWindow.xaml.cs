using Log_Recorder.DA.Class;
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
using System.Windows.Shapes;

namespace Log_Recorder.Forms
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow : Window
    {
        public ModelView.ProjectModelView ProjectModelView { get; set; }
        public ProjectWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            txtProjectNumber.Text = ProjectModelView.ActiveProject.ProjectNumber;
            if (ProjectModelView.ActiveProject.ProjectNumber != String.Empty)
            {
                txtClientName.Text = ProjectModelView.ActiveProject.ClientName;
                //txtGroundLevel.Text = ProjectModelView.ActiveProject.GroundLevel;
                //txtEquipmentType.Text = ProjectModelView.ActiveProject.EquimnetType;
                txtSiteAddress.Text = ProjectModelView.ActiveProject.SiteAddress;
                //txtCheckedBy.Text = ProjectModelView.ActiveProject.CheckedBy;
                //txtLoggedBy.Text = ProjectModelView.ActiveProject.LoggedBy;
                //dpDateCommencted.SelectedDate = ProjectModelView.ActiveProject.DateCommenced;
                //dpDateCompleted.SelectedDate = ProjectModelView.ActiveProject.DateCompleted;
            }
            //else
            //{
            //    txtLoggedBy.Text = txtCheckedBy.Text = Global.ActiveUserInfo.UserName;
            //    dpDateCompleted.SelectedDate = DateTime.Now;
            //}
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            ProjectModelView.ActiveProject.ProjectNumber = txtProjectNumber.Text;
            //ProjectModelView.ActiveProject.GroundLevel = txtGroundLevel.Text;
            ProjectModelView.ActiveProject.ClientName = txtClientName.Text;
            //ProjectModelView.ActiveProject.EquimnetType = txtEquipmentType.Text;
            //ProjectModelView.ActiveProject.DateCommenced = dpDateCommencted.SelectedDate;
            //ProjectModelView.ActiveProject.DateCompleted = dpDateCompleted.SelectedDate;
            //ProjectModelView.ActiveProject.PersonnelName = txtPersonnelName.Text;
            //ProjectModelView.ActiveProject.LoggedBy = txtLoggedBy.Text;
            //ProjectModelView.ActiveProject.CheckedBy = txtCheckedBy.Text;
            ProjectModelView.ActiveProject.SiteAddress = txtSiteAddress.Text;

            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void txtProjectNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnSave.IsEnabled = (txtProjectNumber.Text.Length == 0 ? false : true);
        }
    }
}
