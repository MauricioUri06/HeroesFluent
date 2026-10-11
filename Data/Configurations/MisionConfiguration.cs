namespace HeroesFluent.Data.Configurations;
using HeroesFluent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MisionConfiguration : IEntityTypeConfiguration<Mision>
{
    public void Configure(EntityTypeBuilder<Mision> builder)
    {
        //1.- Tabla
        builder.ToTable("Misiones");
        
        //2.- Clave Primaria
        builder.HasKey(m => m.Id);
        
        //3.- Propiedades
        builder.Property(m => m.Titulo)
            .IsRequired()
            .HasMaxLength(150);
        
        builder.Property(m => m.Descripcion)
            .HasMaxLength(500);

        builder.Property(m => m.Dificultad)
            .HasConversion<string>()
            .HasMaxLength(20);
        
        builder.Property(m => m.FechaLimite)
            .HasColumnType("date");

        //4.- Indice
        
        //5.- relacion se configura en HeroeMisionConfiguration

        //6.- Datos Iniciales
        builder.HasData(
            new Mision
        {
            Id = 1,
            Titulo = "Apagar el Incendio",
            Descripcion = "Apagar el Incendio en la 5ta Avenida",
            Dificultad = Dificultad.Baja,
            FechaLimite = new DateTime(2026, 10, 11)
        }, new Mision
        {
            Id = 2,
            Titulo = "Encontrar el escondite de siniestro",
            Descripcion = "Averiguar la ubicacion del escondite del villano siniestro en coastal city",
            Dificultad = Dificultad.Alta,
            FechaLimite = null
        }, new Mision
            {
                Id = 3,
                Titulo = "Evitar el fin del mundo",
                Descripcion = "Detener el meteorito que se acerca a la Tierra",
                Dificultad = Dificultad.Extrema,
                FechaLimite = new DateTime(2027, 1, 31)
            });
    }
}