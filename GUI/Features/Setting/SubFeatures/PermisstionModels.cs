namespace GUI.Features.Setting.SubFeatures {
    internal record PermissionItem(int PermissionId, string Code, string DisplayName, string Group);
    internal record RoleItem(int RoleId, string Name);
    internal record UserItem(int AccountId, string Email, string FullName);
}