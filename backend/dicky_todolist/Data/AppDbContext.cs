using Microsoft.EntityFrameworkCore;
using dicky_todolist.Models;

namespace dicky_todolist.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Menegaskan relasi One-to-Many
    modelBuilder.Entity<Todo>()
        .HasQueryFilter(t => t.DeletedAt == null)
        .HasOne(t => t.User)
        .WithMany(u => u.Todos)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    modelBuilder.Entity<User>()
    .HasQueryFilter(u => u.DeletedAt == null);
  }

  public DbSet<Todo> Todos { get; set; }
  public DbSet<User> Users { get; set; }
}
