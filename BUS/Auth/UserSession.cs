using DAO.EF;
using DTO.Account;
using DTO.Auth;
using System.Linq;

namespace BUS.Auth {
    public static class UserSession {
        public static AccountDto? CurrentAccount { get; private set; }
        public static int CurrentAccountId => CurrentAccount?.AccountId ?? 0;

        // Role UI-level nhưng định nghĩa ở DTO, nên BUS xài được, GUI cũng xài được
        public static AppRole CurrentAppRole { get; private set; } = AppRole.User;

        public static void SetAccount(AccountDto account) {
            CurrentAccount = account;
            CurrentAppRole = ResolveAppRole(account.AccountId);
        }

        public static void Logout() {
            CurrentAccount = null;
            CurrentAppRole = AppRole.User;
        }

        private static AppRole ResolveAppRole(int accountId) {
            using var db = new Context();

            var codes = db.AccountRoles
                          .Where(ar => ar.AccountId == accountId)
                          .Select(ar => ar.Role.RoleCode)
                          .ToList();

            if (codes.Any(c => c.Equals("ADMIN", System.StringComparison.OrdinalIgnoreCase)))
                return AppRole.Admin;

            if (codes.Any(c => c.Equals("STAFF", System.StringComparison.OrdinalIgnoreCase)))
                return AppRole.Staff;

            return AppRole.User;
        }
    }
}