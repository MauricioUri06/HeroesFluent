using Microsoft.EntityFrameworkCore;

namespace HeroesFluent.Data;

public class HeroeSeeder
{
    public static async Task InicializarAsync(HeroesDbContext context, int cantidadHeroes)
    {
        if (cantidadHeroes <= 0)
        {
            return;
        }
        var listaEquiposId =  await context.Equipos
            .AsNoTracking()
            .Select(e => e.Id)
            .ToListAsync();
        
        var faker = new HeroesFaker(listaEquiposId);
        
        context.Heroes.AddRange(faker.Generate(cantidadHeroes));
        await context.SaveChangesAsync();
    }
}