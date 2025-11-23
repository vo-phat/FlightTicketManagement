using System;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Features.Setting.SubFeatures {
    internal static class PermissionRepository {
        // ===== Permissions =====
        public static List<PermissionItem> GetAllPermissions() => new() {
            new(1,"flights.read","Xem chuyến bay","Flights"),
            new(2,"flights.manage","Quản lý chuyến bay","Flights"),
            new(3,"fare_rules.manage","Quy tắc giá vé","Flights"),
            new(4,"tickets.create_search","Tạo/Tìm đặt chỗ","Tickets"),
            new(5,"tickets.mine","Đặt chỗ của tôi","Tickets"),
            new(6,"tickets.operate","Quản lý vé (check-in/đổi)","Tickets"),
            new(7,"tickets.history","Lịch sử vé","Tickets"),
            new(8,"baggage.checkin","Check-in hành lý / gán tag","Baggage"),
            new(9,"baggage.track","Theo dõi hành lý","Baggage"),
            new(10,"baggage.report","Báo cáo thất lạc","Baggage"),
            new(11,"catalogs.airlines","Hãng hàng không","Catalogs"),
            new(12,"catalogs.aircrafts","Máy bay","Catalogs"),
            new(13,"catalogs.airports","Sân bay","Catalogs"),
            new(14,"catalogs.routes","Tuyến bay","Catalogs"),
            new(15,"catalogs.cabin_classes","Hạng vé","Catalogs"),
            new(16,"catalogs.seats","Ghế máy bay","Catalogs"),
            new(17,"payments.pos","POS / Giao dịch","Payments"),
            new(18,"customers.profiles","Hồ sơ hành khách","Customers"),
            new(19,"accounts.manage","Tài khoản & Quyền","System"),
            new(20,"notifications.read","Thông báo","Notifications"),
            new(21,"reports.view","Báo cáo","Reports"),
            new(22,"system.roles","Vai trò & phân quyền","System"),
            new(23,"home.view","Trang chủ","Misc")
        };

        // ===== Roles =====
        public static List<RoleItem> GetAllRoles() => new() {
            new(1,"Admin"), new(2,"Staff"), new(3,"User")
        };

        public static HashSet<int> GetPermissionIdsOfRole(int roleId) => roleId switch {
            1 => GetAllPermissions().Select(p => p.PermissionId).ToHashSet(),                 // Admin full
            2 => new HashSet<int> { 1, 4, 6, 8, 9, 17, 21, 18, 20, 23 },                               // Staff demo
            3 => new HashSet<int> { 1, 4, 5, 18, 20, 23 },                                         // User demo
            _ => new HashSet<int>()
        };

        public static void SavePermissionsForRole(int roleId, IEnumerable<int> permissionIds) {
            // TODO: TRANSACTION:
            // DELETE FROM Role_Permissions WHERE role_id=@roleId;
            // INSERT INTO Role_Permissions(role_id, permission_id) VALUES...
        }

        // ===== Accounts =====
        public static List<UserItem> GetAllUsers() => new() {
            new(1,"admin@example.com","Administrator"),
            new(2,"staff1@example.com","Staff 1"),
            new(3,"user1@example.com","User 1")
        };

        // Roles of account (User_Role)
        public static HashSet<int> GetRoleIdsOfAccount(int accountId) {
            // TODO: SELECT role_id FROM User_Role WHERE account_id=@accountId
            return accountId switch {
                1 => new HashSet<int> { 1 },     // admin -> Admin
                2 => new HashSet<int> { 2 },     // staff1 -> Staff
                3 => new HashSet<int> { 3 },     // user1 -> User
                _ => new HashSet<int>()
            };
        }

        public static void SaveRolesForAccount(int accountId, IEnumerable<int> roleIds) {
            // TODO: TRANSACTION:
            // DELETE FROM User_Role WHERE account_id=@accountId;
            // INSERT INTO User_Role(account_id, role_id) VALUES ...
        }

        // Optional: direct user overrides (nếu sau này cần)
        public static HashSet<int> GetPermissionIdsOfUserOverride(int accountId) {
            // TODO: SELECT permission_id FROM User_Permissions WHERE account_id=@accountId
            return new HashSet<int>();
        }

        // Effective permissions = Union(Roles) (+/- overrides nếu có dùng)
        public static HashSet<int> GetEffectivePermissionIdsOfAccount(int accountId) {
            var roleIds = GetRoleIdsOfAccount(accountId);
            var union = new HashSet<int>();
            foreach (var rid in roleIds) union.UnionWith(GetPermissionIdsOfRole(rid));

            // nếu dùng override thêm/bớt thì áp dụng ở đây
            // var userOv = GetPermissionIdsOfUserOverride(accountId); union.UnionWith(userOv);
            return union;
        }
    }
}