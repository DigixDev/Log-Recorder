using Log_Recorder.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnLogOn_Click(object sender, RoutedEventArgs e)
        {
            if (DA.Class.UserRepository.IsAuthenticated((int)cbUserName.SelectedValue, txtPassword.Password) == true)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                txtPrompt.Visibility = System.Windows.Visibility.Visible;
                txtPassword.Focus();
                txtPassword.SelectAll();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void Init()
        {
            Task.Factory.StartNew(new Action(() =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    cbUserName.ItemsSource = DA.Class.UserRepository.GetUserList();
                    cbUserName.SelectedIndex = 0;
                    txtPassword.Focus();
                }));

            }));
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnLogOn.IsEnabled = (txtPassword.Password.Length == 0 ? false : true);
        }
    }
}
