using System;
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
        .HasOne(t => t.User)           // Todo punya satu User
        .WithMany(u => u.Todos)        // User punya banyak Todos
        .HasForeignKey(t => t.UserId)  // Key-nya adalah UserId
        .OnDelete(DeleteBehavior.Cascade); // Jika User dihapus, semua Todo-nya ikut terhapus
  }

  public DbSet<Todo> Todos { get; set; }
  public DbSet<User> Users { get; set; }
}
