using AdvancedBusinessAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace AdvancedBusinessAPI.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
  public DbSet<Moto> Motos => Set<Moto>();
  public DbSet<Manutencao> Manutencoes => Set<Manutencao>();
  public DbSet<Usuario> Usuarios => Set<Usuario>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Moto>(e =>
    {
      e.HasKey(x => x.Id);
      e.HasIndex(x => x.Placa).IsUnique();
      e.Property(x => x.Placa).IsRequired().HasMaxLength(8);
      e.Property(x => x.Modelo).IsRequired().HasMaxLength(60);
      e.Property(x => x.Ano).IsRequired();
    });

    modelBuilder.Entity<Manutencao>(e =>
    {
      e.HasKey(x => x.Id);
      e.HasIndex(x => x.MotoId);
      e.HasOne(x => x.Moto).WithMany().HasForeignKey(x => x.MotoId)
        .OnDelete(DeleteBehavior.Cascade);
      e.Property(x => x.Tipo).HasMaxLength(40);
    });

    modelBuilder.Entity<Usuario>(e =>
    {
      e.HasKey(x => x.Id);
      e.HasIndex(x => x.Email).IsUnique();
      e.Property(x => x.Nome).IsRequired().HasMaxLength(100);
      e.Property(x => x.Email).IsRequired().HasMaxLength(160);
    });
  }
}
