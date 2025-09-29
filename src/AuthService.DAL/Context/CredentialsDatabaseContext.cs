using AuthService.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DAL.Context;

public partial class CredentialsDatabaseContext : DbContext
{
    public CredentialsDatabaseContext()
    {
    }

    public CredentialsDatabaseContext(DbContextOptions<CredentialsDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=CredentialsDatabase");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("Permission_pk");

            entity.ToTable("Permission");

            entity.HasIndex(e => e.PermissionName, "AK_1").IsUnique();

            entity.Property(e => e.PermissionId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Role_pk");

            entity.ToTable("Role");

            entity.HasIndex(e => e.RoleName, "AK_0").IsUnique();

            entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pk");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "AK_2").IsUnique();

            entity.HasIndex(e => e.Username, "AK_3").IsUnique();

            entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.HashedPassword).HasMaxLength(512);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasMany(d => d.Permissions).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_3"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_2"),
                    j =>
                    {
                        j.HasKey("UserId", "PermissionId").HasName("UserPermission_pk");
                        j.ToTable("UserPermission");
                    });

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_0"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("UserRole_pk");
                        j.ToTable("UserRole");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
