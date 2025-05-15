using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
namespace WebHackathon.Utilities
{
    public class Function
    {
        public static string _useremail = string.Empty;
        public static string _username = string.Empty;
        public static int _userid = 0;
        public static int? _userrole = 0;
        public static string _useravatar = string.Empty;

        public static string _message = string.Empty;

        public static string _returnUrl = string.Empty;

        public static bool IsLogin()
        {
            if(string.IsNullOrEmpty(_useremail) || string.IsNullOrEmpty(_useravatar) || string.IsNullOrEmpty(_username) || _userid == 0 || _userrole == 0)
            {
                return false;
            }
            return true;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

    }
}
