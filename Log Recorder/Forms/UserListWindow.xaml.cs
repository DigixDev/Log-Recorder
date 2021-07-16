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
    /// Interaction logic for UserInfoWindow.xaml
    /// </summary>
    public partial class UserListWindow : Window
    {
        ModelView.UserListModelView _userListModelView;
        public UserListWindow()
        {
            InitializeComponent();
            _userListModelView = new ModelView.UserListModelView();
            this.DataContext = _userListModelView;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            UserInfoWindow win = new UserInfoWindow();
            win.Owner = this;
            if (win.ShowDialog() == true)
                _userListModelView.AddUser(win.UserName, win.UserRoleId);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            UserInfoWindow win = new UserInfoWindow();
            win.Owner = this;
            win.UserName = _userListModelView.SelectedUser.UserName;
            win.UserRoleId = _userListModelView.SelectedUser.UserRoleId.Value;
            if (win.ShowDialog() == true)
            {
                _userListModelView.UpdateUser(win.UserName, win.UserRoleId);
            }
        }
    }
}
