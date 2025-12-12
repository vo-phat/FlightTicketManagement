using System;
using System.Collections.Generic;
using System.Linq;
using DAO.EF;
using Microsoft.EntityFrameworkCore;

namespace DAO.Repositories {
    public class RolePermissionRepository {
        // ---------------------------
        // Permissions / Roles / Accounts read
        // ---------------------------
        public List<Permission> GetAllPermissions() {
            using var db = new Context();
            return db.Permissions.AsNoTracking()
                     .OrderBy(p => p.PermissionCode)
                     .ToList();
        }

        public Permission? GetPermissionById(int permissionId) {
            using var db = new Context();
            return db.Permissions.Find(permissionId);
        }

        public List<Role> GetAllRoles() {
            using var db = new Context();
            return db.Roles.AsNoTracking()
                     .OrderBy(r => r.RoleName)
                     .ToList();
        }

        public List<Account> GetAllAccounts() {
            using var db = new Context();
            return db.Accounts.AsNoTracking()
                     .OrderBy(a => a.Email)
                     .ToList();
        }

        // ---------------------------
        // RolePermission mapping
        // ---------------------------
        public HashSet<int> GetPermissionIdsOfRole(int roleId) {
            using var db = new Context();
            return db.RolePermissions.AsNoTracking()
                     .Where(rp => rp.RoleId == roleId)
                     .Select(rp => rp.PermissionId)
                     .ToHashSet();
        }

        /// <summary>
        /// Replace role->permission mappings in a transaction (delete all old, insert new).
        /// Safe: guards null, uses Distinct, and rolls back on exception.
        /// </summary>
        public void SavePermissionsForRole(int roleId, IEnumerable<int>? permIds) {
            using var db = new Context();
            using var tx = db.Database.BeginTransaction();
            try {
                var ids = (permIds ?? Enumerable.Empty<int>()).Distinct().ToList();

                // Remove existing mappings
                var old = db.RolePermissions.Where(rp => rp.RoleId == roleId).ToList();
                if (old.Any()) {
                    db.RolePermissions.RemoveRange(old);
                    db.SaveChanges();
                }

                // Optionally validate permission ids exist (comment/uncomment as desired)
                // var validPermIds = db.Permissions.Where(p => ids.Contains(p.PermissionId)).Select(p => p.PermissionId).ToHashSet();
                // var invalid = ids.Except(validPermIds).ToList();
                // if (invalid.Any()) throw new ArgumentException($"Invalid permissionIds: {string.Join(',', invalid)}");

                // Insert new mappings
                var add = ids.Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid }).ToList();
                if (add.Any()) {
                    db.RolePermissions.AddRange(add);
                    db.SaveChanges();
                }

                tx.Commit();
            } catch {
                try { tx.Rollback(); } catch { /* ignore rollback errors */ }
                throw;
            }
        }

        public void DeletePermissionsByRole(int roleId) {
            using var db = new Context();
            var existing = db.RolePermissions.Where(rp => rp.RoleId == roleId).ToList();
            if (!existing.Any()) return;
            db.RolePermissions.RemoveRange(existing);
            db.SaveChanges();
        }

        // ---------------------------
        // Account <-> Role mapping
        // ---------------------------
        public HashSet<int> GetRoleIdsOfAccount(int accountId) {
            using var db = new Context();
            return db.AccountRoles.AsNoTracking()
                     .Where(ar => ar.AccountId == accountId)
                     .Select(ar => ar.RoleId)
                     .ToHashSet();
        }

        public void SaveRolesForAccount(int accountId, IEnumerable<int>? roleIds) {
            using var db = new Context();
            using var tx = db.Database.BeginTransaction();
            try {
                var ids = (roleIds ?? Enumerable.Empty<int>()).Distinct().ToList();

                var old = db.AccountRoles.Where(ar => ar.AccountId == accountId).ToList();
                if (old.Any()) {
                    db.AccountRoles.RemoveRange(old);
                    db.SaveChanges();
                }

                var add = ids.Select(rid => new AccountRole { AccountId = accountId, RoleId = rid }).ToList();
                if (add.Any()) {
                    db.AccountRoles.AddRange(add);
                    db.SaveChanges();
                }

                tx.Commit();
            } catch {
                try { tx.Rollback(); } catch { /* ignore rollback errors */ }
                throw;
            }
        }

        public HashSet<int> GetEffectivePermissionIdsOfAccount(int accountId) {
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

        // ---------------------------
        // Role CRUD helpers
        // ---------------------------
        public Role? GetRoleById(int roleId) {
            using var db = new Context();
            return db.Roles.Find(roleId);
        }

        public Role? GetRoleByCode(string roleCode) {
            if (string.IsNullOrWhiteSpace(roleCode)) return null;
            using var db = new Context();
            var key = roleCode.Trim().ToLowerInvariant();
            return db.Roles.AsNoTracking().FirstOrDefault(r => r.RoleCode.ToLower() == key);
        }

        public int CreateRole(Role role) {
            if (role == null) throw new ArgumentNullException(nameof(role));
            using var db = new Context();
            db.Roles.Add(role);
            db.SaveChanges();
            return role.RoleId;
        }

        public bool UpdateRole(Role role) {
            if (role == null) throw new ArgumentNullException(nameof(role));
            using var db = new Context();
            var exist = db.Roles.Find(role.RoleId);
            if (exist == null) return false;

            // Update allowed fields (RoleName and RoleCode here; adjust if you want to lock RoleCode)
            exist.RoleName = role.RoleName;
            exist.RoleCode = role.RoleCode;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Delete role if exists and not used by accounts (caller can pre-check CountAccountsForRole).
        /// Returns true if deleted, false otherwise.
        /// </summary>
        public bool DeleteRole(int roleId) {
            using var db = new Context();

            // Safety check: do not delete if any account uses this role
            var hasAccount = db.AccountRoles.Any(ar => ar.RoleId == roleId);
            if (hasAccount) return false;

            var exist = db.Roles.Find(roleId);
            if (exist == null) return false;

            // Explicitly remove role_permissions before removing role (DB has cascade but do explicit for clarity)
            var rp = db.RolePermissions.Where(r => r.RoleId == roleId).ToList();
            if (rp.Any()) {
                db.RolePermissions.RemoveRange(rp);
                db.SaveChanges();
            }

            db.Roles.Remove(exist);
            db.SaveChanges();
            return true;
        }

        public int CountAccountsForRole(int roleId) {
            using var db = new Context();
            return db.AccountRoles.Count(ar => ar.RoleId == roleId);
        }
    }
}
