namespace HeroesFluent.Data;

public class EquipoSeeder
{
    public static async Task InicializarAsync(HeroesDbContext context, int cantidadEquipos)
    {
        if (cantidadEquipos <= 0)
        {
            return;
        }

        var faker = new EquiposFaker();
        
        context.Equipos.AddRange(faker.Generate(cantidadEquipos));
        await context.SaveChangesAsync();
    }
}