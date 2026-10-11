using HeroesFluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeroesFluent.Data.Configurations;

public class HeroeConfiguration : IEntityTypeConfiguration<Heroe>
{
    public void Configure(EntityTypeBuilder<Heroe> builder)
    {
        //1.- Tabla
        builder.ToTable("Heroes", 
            t => t.HasCheckConstraint("CK_Heroes_NivelPoder", "[NivelPoder] BETWEEN 1 AND 100"));
        
        //2.- Clave Primaria
        builder.HasKey(h => h.Id);
        
        //3.- Propiedades
        builder.Property(h => h.Nombre)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(h => h.Alias)
            .IsRequired()
            .HasMaxLength(60);
        
        builder.Property(h => h.FechaRegistro)
            .HasDefaultValueSql("GETDATE()");
        
        //4.- Indice
        builder.HasIndex(h => h.Alias).IsUnique();

        //5.- Relaciones 
        builder.HasOne(h => h.Equipo)
            .WithMany(e => e.Heroes)
            .HasForeignKey(h => h.EquipoId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

    }
}