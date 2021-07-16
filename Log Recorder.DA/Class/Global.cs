using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Class
{
    public enum UserRoleEnum { User = 1, SubAdmin = 2, MainAdmin = 3 }

    public static class Global
    {
        // Database password is: 4523
        public static string FileName { get; set; }
        public static string PageTitle
        {
            get
            {
                if (FileName == String.Empty)
                    return "Log Software";
                else
                    return "Log Software - [" + FileName + "]";
            }
        }

        public class ActiveUserInfo
        {
            public static int UserID { get; set; }
            public static string UserName { get; set; }
            public static int UserRole { get; set; }
            public static string UserRoleTitle { get { return ((UserRoleEnum)UserRole).ToString(); } }
            public static bool IsMainAdmin { get { return UserRole == Class.UserRole.MainAdmin ? true : false; } }
            public static bool IsSubAdmin { get { return UserRole >= Class.UserRole.SubAdmin ? true : false; } }
        }
    }
}
