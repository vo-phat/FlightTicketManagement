using DAO.Database;
using DTO.Ticket;
using Microsoft.EntityFrameworkCore;

namespace DAO.EF {
    public class Context : DbContext {
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<AccountRole> AccountRoles => Set<AccountRole>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<TicketHistory> TicketsHistory { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                var conn = DatabaseConnection.ConnectionString;
                optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // ACCOUNT
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");
                entity.HasKey(a => a.AccountId);

                entity.Property(a => a.AccountId).HasColumnName("account_id");
                entity.Property(a => a.Email).HasColumnName("email");
                entity.Property(a => a.Password).HasColumnName("password");
                entity.Property(a => a.FailedAttempts).HasColumnName("failed_attempts");
                entity.Property(a => a.IsActive).HasColumnName("is_active");
                entity.Property(a => a.CreatedAt).HasColumnName("created_at");
            });

            // ROLE
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(r => r.RoleId);

                entity.Property(r => r.RoleId).HasColumnName("role_id");
                entity.Property(r => r.RoleCode).HasColumnName("role_code");
                entity.Property(r => r.RoleName).HasColumnName("role_name");
            });

            // ACCOUNT_ROLE
            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.ToTable("account_role");
                entity.HasKey(ar => new { ar.AccountId, ar.RoleId });

                entity.Property(ar => ar.AccountId).HasColumnName("account_id");
                entity.Property(ar => ar.RoleId).HasColumnName("role_id");

                entity.HasOne(ar => ar.Account)
                      .WithMany(a => a.AccountRoles)
                      .HasForeignKey(ar => ar.AccountId);

                entity.HasOne(ar => ar.Role)
                      .WithMany(r => r.AccountRoles)
                      .HasForeignKey(ar => ar.RoleId);
            });

            // PERMISSION
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");
                entity.HasKey(p => p.PermissionId);

                entity.Property(p => p.PermissionId).HasColumnName("permission_id");
                entity.Property(p => p.PermissionCode).HasColumnName("permission_code");
                entity.Property(p => p.PermissionName).HasColumnName("permission_name");
            });

            // ROLE_PERMISSION
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permissions");
                entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

                entity.Property(rp => rp.RoleId).HasColumnName("role_id");
                entity.Property(rp => rp.PermissionId).HasColumnName("permission_id");

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId);
            });
            modelBuilder.Entity<TicketHistory>()
                .HasNoKey()
                .ToTable("ticket_history"); // Tên bảng thật trong DB
            modelBuilder.Entity<Tickets>()
                .HasNoKey()
                .ToTable("tickets"); // Tên bảng thật trong DB
        }
    }
}
