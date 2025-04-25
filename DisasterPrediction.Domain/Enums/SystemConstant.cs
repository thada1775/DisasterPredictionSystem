using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Domain.Enums
{
    public static class SystemConstant
    {
        public static class UserRole
        {
            public const string Admin = "Admin";
            public const string Manager = "Manager";
            public const string User = "User";

            public static string[] GetAllRole()
            {
                return new [] { Admin, Manager, User };
            }
        }

        public static class Project
        {
            public const string View = "Permissions.Project.View";
            public const string Create = "Permissions.Project.Create";
            public const string Edit = "Permissions.Project.Edit";
            public const string Delete = "Permissions.Project.Delete";
        }

        public static class Task
        {
            public const string View = "Permissions.Task.View";
            public const string Create = "Permissions.Task.Create";
            public const string Edit = "Permissions.Task.Edit";
            public const string Delete = "Permissions.Task.Delete";
        }

        public static class User
        {
            public const string AllPermission = "Permissions.User.all";
        }
    }
}
