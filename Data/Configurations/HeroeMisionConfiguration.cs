namespace HeroesFluent.Data.Configurations;
using HeroesFluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class HeroeMisionConfiguration : IEntityTypeConfiguration<HeroeMision>
{
    public void Configure(EntityTypeBuilder<HeroeMision> builder)
    {
        //1.- Tabla
        builder.ToTable("HeroeMisiones");
        
        //2.- Clave Primaria
        builder.HasKey(hm => new {hm.HeroeId, hm.MisionId});

        //3.- Propiedades
        builder.Property(hm => hm.Rol)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(hm => hm.FechaAsignacion)
            .HasDefaultValueSql("GETDATE()");
        //4.- Indice

        //5.- Relaciones
        builder.HasOne(hm => hm.Heroe)
            .WithMany(h => h.HeroeMisiones)
            .HasForeignKey(hm => hm.HeroeId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(hm => hm.Mision)
            .WithMany(m => m.HeroeMisiones)
            .HasForeignKey(hm => hm.MisionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}