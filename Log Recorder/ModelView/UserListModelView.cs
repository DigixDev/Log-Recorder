using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Forms;

namespace Log_Recorder.ModelView
{
    public class UserListModelView : ModelViewBase
    {
        ICommand _deleteUserCommand = null;

        public ICommand DeleteUserCommand
        {
            get
            {
                if (_deleteUserCommand == null)
                    _deleteUserCommand = new RelayCommands(DeleteSelectedUser);
                return _deleteUserCommand;
            }
        }

        private void DeleteSelectedUser(object obj)
        {
            if(Users.Count==1)
            {
                MessageBox.Show("Userlist can not be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DA.Class.UserRepository.DeleteUser(SelectedUser.UserId) == true)
                Users.Remove(SelectedUser);
        }
        public ObservableCollection<DA.EF.User> Users { get; set; }
        public Dictionary<int, string> UserRoles { get { return DA.Class.UserRole.UserRoles; } }

        public DA.EF.User SelectedUser { get; set; }

        public UserListModelView()
        {
            Users = new ObservableCollection<DA.EF.User>(DA.Class.UserRepository.GetUserList());
        }

        public void AddUser(string userName, byte userRoleId)
        {
            DA.EF.User user = DA.Class.UserRepository.NewUser(userName, userRoleId);
            if (user != null)
                Users.Add(user);
        }

        public void UpdateUser(string userName, byte userRoleId)
        {
            if (DA.Class.UserRepository.UpdateUser(SelectedUser.UserId, userName, userRoleId) == true)
            {
                SelectedUser.UserName = userName;
                SelectedUser.UserRoleId = userRoleId;
                OnPropertyChanged("SelectedUser");
            }
        }
    }
}
