using System.Text.RegularExpressions;
using UserService.Services.Contract.Enums;

namespace UserService.Services.Implementation.Utils
{
    public static class UserValidator
    {
        public static bool IsValidName(string userName) => !string.IsNullOrWhiteSpace(userName);
        public static bool IsValidPassword(string password) => !string.IsNullOrWhiteSpace(password);
        public static bool IsValidEmail(string userEmail) => !string.IsNullOrWhiteSpace(userEmail) 
            || !Regex.IsMatch(userEmail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        public static bool IsValidRole(UserRoleCode roleCode) => Enum.GetValues<UserRoleCode>().Contains(roleCode);
    }
}
