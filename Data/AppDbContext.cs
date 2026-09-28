using Microsoft.EntityFrameworkCore;

using TaskManagementApi.Models;

namespace TaskManagementApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ------------------------------------------------------------
            // Map table names to match the actual database (lowercase)
            // ------------------------------------------------------------
            modelBuilder.Entity<Role>().ToTable("roles");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<TaskItem>().ToTable("tasks");

            // ------------------------------------------------------------
            // Tasks table: date columns are DATE in SQL Server, not
            // datetime2 (EF's default mapping for DateTime). Without
            // this, EF would send full datetime values and the column
            // types would silently mismatch.
            // ------------------------------------------------------------
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.Property(t => t.create_date)
                    .HasColumnType("date");

                entity.Property(t => t.due_date)
                    .HasColumnType("date");

                entity.Property(t => t.actual_completed_date)
                    .HasColumnType("date");
            });

            // ------------------------------------------------------------
            // User -> Role relationship (many users, one role)
            // ------------------------------------------------------------
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.role_id);
        }
    }
}