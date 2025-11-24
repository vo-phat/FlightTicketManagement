namespace DAO.EF {
    public class Account {
        public int AccountId { get; set; }
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public int FailedAttempts { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
    }

    public class Role {
        public int RoleId { get; set; }
        public string RoleCode { get; set; } = "";
        public string RoleName { get; set; } = "";

        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class AccountRole {
        public int AccountId { get; set; }
        public int RoleId { get; set; }
        public Account Account { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }

    public class Permission {
        public int PermissionId { get; set; }
        public string PermissionCode { get; set; } = "";
        public string PermissionName { get; set; } = "";
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class RolePermission {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public Role Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
