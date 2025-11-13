using Microsoft.EntityFrameworkCore;
using IntuitBack.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace IntuitBack.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Log> Logs => Set<Log>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nombres).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Apellidos).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Cuit).IsRequired().HasMaxLength(20);
            entity.Property(c => c.Email).IsRequired().HasMaxLength(150);
            entity.Property(c => c.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.HasIndex(c => c.Cuit).IsUnique();
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Nivel).HasMaxLength(50).IsRequired();
            entity.Property(l => l.Mensaje).HasMaxLength(500).IsRequired();
            entity.Property(l => l.Detalle).HasMaxLength(4000);
            entity.Property(l => l.FechaCreacion).HasDefaultValueSql("SYSUTCDATETIME()");
        });
    }
}
