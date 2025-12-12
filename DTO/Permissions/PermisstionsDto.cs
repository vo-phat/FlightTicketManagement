namespace DTO.Permissions
{
    public record PermissionItem(int PermissionId, string Code, string DisplayName, string Group);
    public record RoleItem(int RoleId, string Code, string Name) {
        public override string ToString() => Name;
    }
    public record UserItem(int AccountId, string Email, bool IsActive, int FailedAttempts, DateTime CreatedAt);
    public record CreateRoleRequest(string RoleCode, string RoleName);
    public record UpdateRoleRequest(int RoleId, string RoleName);
    public record AssignPermissionsRequest(int RoleId, IEnumerable<int> PermissionIds);
}