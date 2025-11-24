using System.Collections.Generic;
using System.Linq;
using DAO.Repositories;
using DTO.Permissions;

namespace BUS.Auth {
    public class RolePermissionService {
        private readonly RolePermissionRepository _repo = new();

        public List<PermissionItem> GetAllPermissions() {
            var entities = _repo.GetAllPermissions(); // EF

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
    }
}