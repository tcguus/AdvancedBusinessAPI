using Microsoft.EntityFrameworkCore;
using AdvancedBusinessAPI.Models;

namespace AdvancedBusinessAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Cliente> Clientes { get; set; }
}