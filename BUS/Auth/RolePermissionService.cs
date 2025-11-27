using BUS.Security;
using BUS.Validation;
using DAO.Repositories;
using DTO.Account;
using DTO.Permissions;
using System.Collections.Generic;
using System.Linq;

namespace BUS.Auth
{
    public class RolePermissionService
    {
        private readonly RolePermissionRepository _repo = new();
        private readonly AccountDao _accountDao = new();

        public List<PermissionItem> GetAllPermissions()
        {
            var entities = _repo.GetAllPermissions();

            // LINQ to Object: map entity -> DTO + Group
            return entities
                .Select(p => new PermissionItem(
                    p.PermissionId,
                    p.PermissionCode,
                    p.PermissionName,
                    MapGroupFromCode(p.PermissionCode)
                ))
                .OrderBy(p => p.Group)
                .ThenBy(p => p.DisplayName)
                .ToList();
        }

        private static string MapGroupFromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return "Misc";
            var prefix = code.Split('.')[0].ToLowerInvariant();
            return prefix switch
            {
                "flights" => "Flights",
                "fare_rules" => "Flights",
                "tickets" => "Tickets",
                "baggage" => "Baggage",
                "catalogs" => "Catalogs",
                "payments" => "Payments",
                "customers" => "Customers",
                "accounts" => "System",
                "system" => "System",
                "notifications" => "Notifications",
                "reports" => "Reports",
                "home" => "Misc",
                _ => "Misc"
            };
        }

        public List<RoleItem> GetAllRoles() =>
            _repo.GetAllRoles()
                 .Select(r => new RoleItem(r.RoleId, r.RoleCode, r.RoleName))
                 .ToList();

        public List<UserItem> GetAllUsers() =>
            _repo.GetAllAccounts()
                 .Select(a => new UserItem(a.AccountId, a.Email, a.IsActive, a.FailedAttempts, a.CreatedAt))
                 .ToList();

        public HashSet<int> GetPermissionIdsOfRole(int roleId)
            => _repo.GetPermissionIdsOfRole(roleId);

        public void SavePermissionsForRole(int roleId, IEnumerable<int> permIds)
            => _repo.SavePermissionsForRole(roleId, permIds);

        public HashSet<int> GetRoleIdsOfAccount(int accountId)
            => _repo.GetRoleIdsOfAccount(accountId);

        public void SaveRolesForAccount(int accountId, IEnumerable<int> roleIds)
            => _repo.SaveRolesForAccount(accountId, roleIds);

        public HashSet<int> GetEffectivePermissionIdsOfAccount(int accountId)
            => _repo.GetEffectivePermissionIdsOfAccount(accountId);
        public int CreateAccountWithRoles(string email, string password, int roleId)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Email không được để trống.");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Mật khẩu không được để trống.");

            if (roleId <= 0)
                throw new Exception("Vai trò không hợp lệ.");

            AccountValidator.ValidateEmail(email);
            AccountValidator.ValidatePassword(password);

            if (_accountDao.ExistsByEmail(email))
                throw new Exception("Email đã tồn tại trong hệ thống.");

            var dto = new AccountDto
            {
                Email = email,
                Password = PasswordHasher.Hash(password),
                FailedAttempts = AccountDto.DEFAULT_FAILED_ATTEMPTS,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            int newId = _accountDao.Create(dto);

            _repo.SaveRolesForAccount(newId, new[] { roleId });

            _accountDao.CreateEmptyProfile(newId);

            return newId;
        }
        public void DeleteAccount(int accountId)
        {
            _repo.SaveRolesForAccount(accountId, Enumerable.Empty<int>());
            _accountDao.LockAccount(accountId);
        }
        public void UpdatePasswordForAccount(int accountId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("Mật khẩu mới không được để trống.");

            AccountValidator.ValidatePassword(newPassword);

            string hash = PasswordHasher.Hash(newPassword);
            _accountDao.UpdatePassword(accountId, hash);
        }

        public void ResetFailedAttempts(int accountId)
        {
            _accountDao.ResetFailedAttemptsToDefault(accountId);
        }

        public void UnlockAccount(int accountId)
        {
            _accountDao.UnlockAccount(accountId);   // gọi xuống DAO
        }

        public HashSet<string> GetEffectivePermissionCodesOfAccount(int accountId)
        {
            // Lấy danh sách id quyền hiệu lực của account
            var idSet = _repo.GetEffectivePermissionIdsOfAccount(accountId);
            if (idSet == null || idSet.Count == 0)
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Lấy toàn bộ permission để map id -> code
            var allPerms = _repo.GetAllPermissions();

            var codes = allPerms
                .Where(p => idSet.Contains(p.PermissionId))
                .Select(p => p.PermissionCode)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return codes;
        }

    }
}