using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Log_Recorder.DA.Class
{
    public class UserRepository
    {
        public static List<DA.EF.User> GetUserList()
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                var q = from c in de.Users
                        select c;
                return q.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static bool IsAuthenticated(int userId, string password)
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                var q = (from c in de.Users
                         where c.UserId == userId && c.Password == password
                         select c).SingleOrDefault();
                if (q == null)
                    return false;
                Global.ActiveUserInfo.UserID = q.UserId;
                Global.ActiveUserInfo.UserName = q.UserName;
                Global.ActiveUserInfo.UserRole = q.UserRoleId.Value;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool UpdateUser(int userId, string userName, byte userRoleId)
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                var q = (from c in de.Users
                         where c.UserId == userId
                         select c).Single();
                q.UserName = userName;
                q.UserRoleId = userRoleId;
                de.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static EF.User NewUser(string userName, byte userRoleId)
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                EF.User user = new EF.User() { UserName = userName, UserRoleId = userRoleId, Password = "12345" };
                de.Users.Add(user);
                de.SaveChanges();
                return user;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static bool DeleteUser(int userId)
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                var user = (from c in de.Users
                            where c.UserId == userId
                            select c).SingleOrDefault();
                de.Users.Remove(user);
                de.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static bool ChangePassword(string currentPassword, string newPassword, out string result)
        {
            try
            {
                EF.DataEntities de = new EF.DataEntities();
                var q = (from c in de.Users
                         where c.UserId == Global.ActiveUserInfo.UserID && c.Password == currentPassword
                         select c).SingleOrDefault();
                if(q==null)
                {
                    result = "Current password not matched.";
                    return false;
                }
                else
                {
                    q.Password = newPassword;
                    de.SaveChanges();
                    result = "Password changed";
                    return true;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
    }
}
