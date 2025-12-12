using BUS.Security;
using BUS.Validation;
using DAO.Repositories;
using DTO.Account;
using DTO.Permissions;
using DAO.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS.Auth {
    public class RolePermissionService {
        private readonly RolePermissionRepository _repo = new();
        private readonly AccountDao _accountDao = new();

        // ---------------------------
        // Permissions / Roles / Users read
        // ---------------------------
        public List<PermissionItem> GetAllPermissions() {
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

        private static string MapGroupFromCode(string code) {
            if (string.IsNullOrWhiteSpace(code)) return "Misc";
            var prefix = code.Split('.')[0].ToLowerInvariant();
            return prefix switch {
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

        public RoleItem? GetRoleById(int roleId) {
            var r = _repo.GetRoleById(roleId);
            if (r == null) return null;
            return new RoleItem(r.RoleId, r.RoleCode, r.RoleName);
        }

        public List<UserItem> GetAllUsers() =>
            _repo.GetAllAccounts()
                 .Select(a => new UserItem(a.AccountId, a.Email, a.IsActive, a.FailedAttempts, a.CreatedAt))
                 .ToList();

        public HashSet<int> GetPermissionIdsOfRole(int roleId)
            => _repo.GetPermissionIdsOfRole(roleId);

        // ---------------------------
        // Save permissions with validation & admin protection
        // ---------------------------
        /// <summary>
        /// Lưu ma trận quyền cho 1 role.
        /// Nếu role có role_code == "ADMIN" (case-insensitive) -> sẽ ném Exception (không cho thay đổi).
        /// Nếu permIds chứa id không tồn tại -> ném Exception.
        /// </summary>
        public void SavePermissionsForRole(int roleId, IEnumerable<int>? permIds) {
            // Validate role exists
            var role = _repo.GetRoleById(roleId);
            if (role == null)
                throw new Exception("Vai trò không tồn tại.");

            // Detect admin via role_code
            if (IsAdminRole(role))
                throw new Exception("Không thể thay đổi quyền cho vai trò ADMIN.");

            // Validate permission ids are valid
            var allPermIds = _repo.GetAllPermissions().Select(p => p.PermissionId).ToHashSet();
            var ids = (permIds ?? Enumerable.Empty<int>()).Distinct().ToList();
            var invalid = ids.Where(id => !allPermIds.Contains(id)).ToList();
            if (invalid.Any())
                throw new Exception($"Một số permission id không hợp lệ: {string.Join(',', invalid)}");

            // Save (transaction handled inside repository)
            _repo.SavePermissionsForRole(roleId, ids);
        }

        // ---------------------------
        // Role <-> Account mapping (delegates)
        // ---------------------------
        public HashSet<int> GetRoleIdsOfAccount(int accountId)
            => _repo.GetRoleIdsOfAccount(accountId);

        public void SaveRolesForAccount(int accountId, IEnumerable<int> roleIds)
            => _repo.SaveRolesForAccount(accountId, roleIds);

        public HashSet<int> GetEffectivePermissionIdsOfAccount(int accountId)
            => _repo.GetEffectivePermissionIdsOfAccount(accountId);

        public HashSet<string> GetEffectivePermissionCodesOfAccount(int accountId) {
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

        // ---------------------------
        // Account creation / management (existing)
        // ---------------------------
        public int CreateAccountWithRoles(string email, string password, int roleId) {
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

            var dto = new AccountDto {
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

        public void DeleteAccount(int accountId) {
            _repo.SaveRolesForAccount(accountId, Enumerable.Empty<int>());
            _accountDao.LockAccount(accountId);
        }

        public void UpdatePasswordForAccount(int accountId, string newPassword) {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new Exception("Mật khẩu mới không được để trống.");

            AccountValidator.ValidatePassword(newPassword);

            string hash = PasswordHasher.Hash(newPassword);
            _accountDao.UpdatePassword(accountId, hash);
        }

        public void ResetFailedAttempts(int accountId) {
            _accountDao.ResetFailedAttemptsToDefault(accountId);
        }

        public void UnlockAccount(int accountId) {
            _accountDao.UnlockAccount(accountId);   // gọi xuống DAO
        }

        // ---------------------------
        // Role CRUD: Create / Update / Delete
        // ---------------------------
        public int CreateRole(CreateRoleRequest req) {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (string.IsNullOrWhiteSpace(req.RoleCode)) throw new Exception("RoleCode không được để trống.");
            if (string.IsNullOrWhiteSpace(req.RoleName)) throw new Exception("RoleName không được để trống.");

            // Normalize code
            var code = req.RoleCode.Trim();
            // Check unique code
            var exist = _repo.GetRoleByCode(code);
            if (exist != null)
                throw new Exception("RoleCode đã tồn tại.");

            var entity = new Role {
                RoleCode = code,
                RoleName = req.RoleName.Trim()
            };

            int newId = _repo.CreateRole(entity);
            return newId;
        }

        public bool UpdateRole(UpdateRoleRequest req) {
            if (req == null) throw new ArgumentNullException(nameof(req));
            if (req.RoleId <= 0) throw new Exception("RoleId không hợp lệ.");
            if (string.IsNullOrWhiteSpace(req.RoleName)) throw new Exception("RoleName không được để trống.");

            var exist = _repo.GetRoleById(req.RoleId);
            if (exist == null) throw new Exception("Role không tồn tại.");

            // Protect ADMIN code: do not allow changing role code of ADMIN via this method.
            if (IsAdminRole(exist)) {
                // allow updating display name but keep code unchanged
                exist.RoleName = req.RoleName.Trim();
                // Do not change RoleCode
            } else {
                exist.RoleName = req.RoleName.Trim();
                // we do NOT change RoleCode here - if you want enable, uncomment next line
                // exist.RoleCode = req.RoleCode?.Trim() ?? exist.RoleCode;
            }

            return _repo.UpdateRole(exist);
        }

        public bool DeleteRole(int roleId) {
            if (roleId <= 0) throw new Exception("RoleId không hợp lệ.");

            var role = _repo.GetRoleById(roleId);
            if (role == null) throw new Exception("Role không tồn tại.");

            if (IsAdminRole(role))
                throw new Exception("Không thể xóa vai trò ADMIN.");

            var count = _repo.CountAccountsForRole(roleId);
            if (count > 0)
                throw new Exception("Không thể xóa vai trò vì đang có tài khoản sử dụng.");

            // repository sẽ explicit delete role_permissions and role
            var ok = _repo.DeleteRole(roleId);
            if (!ok) throw new Exception("Xóa vai trò thất bại.");
            return true;
        }

        // ---------------------------
        // Helpers
        // ---------------------------
        private bool IsAdminRole(Role role) {
            if (role == null) return false;
            return !string.IsNullOrEmpty(role.RoleCode) && role.RoleCode.Trim().Equals("ADMIN", StringComparison.OrdinalIgnoreCase);
        }
    }
}
