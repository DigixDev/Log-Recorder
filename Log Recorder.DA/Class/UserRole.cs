using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log_Recorder.DA.Class
{
    public class UserRole
    {
        public const int User = 1;
        public const int SubAdmin = 2;
        public const int MainAdmin = 3;

        private static Dictionary<int, string> _userRoles = null;
        public static Dictionary<int, string> UserRoles
        {
            get
            {
                if (_userRoles == null)
                    _userRoles = new Dictionary<int, string> { { User, "User" }, { SubAdmin, "Sub Admin" }, { MainAdmin, "Main Admin" } };
                return _userRoles;

            }
        }
    }
}
