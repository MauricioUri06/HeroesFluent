using Microsoft.EntityFrameworkCore;
using HeroesFluent.Models;

namespace HeroesFluent.Data;

public class HeroesDbContext: DbContext
{
    public HeroesDbContext(DbContextOptions<HeroesDbContext> options) : base(options) {}

    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Mision> Misiones => Set<Mision>();
    public DbSet<Heroe> Heroes => Set<Heroe>();
    public DbSet<HeroeMision> HeroeMisiones => Set<HeroeMision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HeroesDbContext).Assembly);
    }
}