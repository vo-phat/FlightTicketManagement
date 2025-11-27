using DAO.EF;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAO.Repositories
{
    public class RolePermissionRepository
    {
        public List<Permission> GetAllPermissions()
        {
            using var db = new Context();
            return db.Permissions.AsNoTracking()
                     .OrderBy(p => p.PermissionCode)
                     .ToList();
        }

        public List<Role> GetAllRoles()
        {
            using var db = new Context();
            return db.Roles.AsNoTracking()
                     .OrderBy(r => r.RoleName)
                     .ToList();
        }

        public List<Account> GetAllAccounts()
        {
            using var db = new Context();
            return db.Accounts.AsNoTracking()
                     .OrderBy(a => a.Email)
                     .ToList();
        }

        public HashSet<int> GetPermissionIdsOfRole(int roleId)
        {
            using var db = new Context();
            return db.RolePermissions.AsNoTracking()
                     .Where(rp => rp.RoleId == roleId)
                     .Select(rp => rp.PermissionId)
                     .ToHashSet();
        }

        public void SavePermissionsForRole(int roleId, IEnumerable<int> permIds)
        {
            using var db = new Context();
            using var tx = db.Database.BeginTransaction();

            var old = db.RolePermissions.Where(rp => rp.RoleId == roleId);
            db.RolePermissions.RemoveRange(old);

            var add = permIds.Distinct()
                             .Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid });
            db.RolePermissions.AddRange(add);

            db.SaveChanges();
            tx.Commit();
        }

        public HashSet<int> GetRoleIdsOfAccount(int accountId)
        {
            using var db = new Context();
            return db.AccountRoles.AsNoTracking()
                     .Where(ar => ar.AccountId == accountId)
                     .Select(ar => ar.RoleId)
                     .ToHashSet();
        }

        public void SaveRolesForAccount(int accountId, IEnumerable<int> roleIds)
        {
            using var db = new Context();
            using var tx = db.Database.BeginTransaction();

            var old = db.AccountRoles.Where(ar => ar.AccountId == accountId);
            db.AccountRoles.RemoveRange(old);

            var add = roleIds.Distinct()
                             .Select(rid => new AccountRole { AccountId = accountId, RoleId = rid });
            db.AccountRoles.AddRange(add);

            db.SaveChanges();
            tx.Commit();
        }

        public HashSet<int> GetEffectivePermissionIdsOfAccount(int accountId)
        {
            using var db = new Context();

            var roleIds = db.AccountRoles
                            .Where(ar => ar.AccountId == accountId)
                            .Select(ar => ar.RoleId)
                            .ToList();
            if (!roleIds.Any()) return new HashSet<int>();

            return db.RolePermissions
                     .Where(rp => roleIds.Contains(rp.RoleId))
                     .Select(rp => rp.PermissionId)
                     .Distinct()
                     .ToHashSet();
        }
    }
}