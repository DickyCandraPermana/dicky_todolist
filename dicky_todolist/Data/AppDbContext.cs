using System;
using Microsoft.EntityFrameworkCore;
using dicky_todolist.Models;

namespace dicky_todolist.Data;

public class AppDbContext : DbContext
{
  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  { }

  public DbSet<Todo> Todos { get; set; }
}
