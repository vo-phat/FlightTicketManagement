using System;

namespace GUI.Features.Auth
{
    public static class SessionManager
    {
        public static int AccountId { get; private set; }
        public static string Email { get; private set; }
        public static string Role { get; private set; }

        public static bool IsLoggedIn => AccountId > 0;

        /// <summary>
        /// Ghi thông tin người dùng đang đăng nhập
        /// </summary>
        public static void SetCurrentUser(int accountId, string email, string role)
        {
            AccountId = accountId;
            Email = email;
            Role = role;
        }

        /// <summary>
        /// Xóa thông tin đăng nhập
        /// </summary>
        public static void Clear()
        {
            AccountId = 0;
            Email = null;
            Role = null;
        }
    }
}
