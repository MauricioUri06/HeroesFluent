using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using HeroesFluent.Data;
using HeroesFluent.Models;
using HeroesFluent.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroesFluent.Controllers;

public class HeroesController : Controller
{
    private readonly HeroesDbContext _context;
    private readonly HeroeConsultas _consultas;

    public HeroesController(HeroesDbContext context, HeroeConsultas consultas)
    {
        _context = context;
        _consultas = consultas;
    }

    public async Task<IActionResult> Index()
    {
        await ProbarLinqAsync();

        var heroes = await _context.Heroes.AsNoTracking()
            .Include(h => h.Equipo)
            .Include(h => h.HeroeMisiones).ThenInclude(hm => hm.Mision)
            .OrderBy(h => h.Alias).ToListAsync();
        return View(heroes);
    }

    // Abre /Heroes/Consultas: ejecuta las 20 consultas y muestra cada resultado en la consola.
    public async Task<IActionResult> Consultas()
    {
        await MostrarConsultasAsync();
        return Content("Consultas ejecutadas. Revisa el resultado en la consola.");
    }

    private static readonly JsonSerializerOptions OpcionesJson = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // tildes y ñ legibles
        ReferenceHandler = ReferenceHandler.IgnoreCycles        // evita ciclos Heroe <-> HeroeMision
    };

    private async Task MostrarConsultasAsync()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // El Id cambia cada vez que se vuelve a correr el script de datos de prueba, por eso se busca por alias.
        var idAurora = await _context.Heroes.Where(h => h.Alias == "Aurora").Select(h => h.Id).FirstOrDefaultAsync();

        Console.WriteLine();
        Console.WriteLine("########## CONSULTAS LINQ - HeroesFluent ##########");
        Console.WriteLine();

        // Nivel 1
        await Correr("1. Alias y nivel de todos los héroes, por alias", async () => await _consultas.Consulta01AliasYNivelAsync());
        await Correr("2. Héroes con nivel de poder > 70, de mayor a menor", async () => await _consultas.Consulta02PoderMayorA70Async());
        await Correr("3. Héroes sin equipo, por alias", async () => await _consultas.Consulta03SinEquipoAsync());
        await Correr("4. Alias que contiene \"or\"", async () => await _consultas.Consulta04BuscarPorAliasAsync("or"));
        await Correr("4. (texto vacío) debe devolver lista vacía", async () => await _consultas.Consulta04BuscarPorAliasAsync(""));
        await Correr("5. Héroe con alias \"Aurora\"", async () => await _consultas.Consulta05ObtenerPorAliasAsync("Aurora"));
        await Correr("5. (alias inexistente) debe devolver null", async () => await _consultas.Consulta05ObtenerPorAliasAsync("Inexistente"));
        await Correr("6. Héroes de \"La Paz\", por alias", async () => await _consultas.Consulta06PorCiudadAsync("La Paz"));

        // Nivel 2
        await Correr("7. Total de héroes y nivel promedio", async () => await _consultas.Consulta07TotalYPromedioAsync());
        await Correr("8. ¿Existe alguna misión Extrema?", async () => await _consultas.Consulta08ExisteMisionExtremaAsync());
        await Correr("9. Presupuesto total de los equipos", async () => await _consultas.Consulta09PresupuestoTotalAsync());
        await Correr("10. Héroes bajo el promedio de poder", async () => await _consultas.Consulta10BajoElPromedioAsync());

        // Nivel 3
        await Correr("11. Cada héroe con el nombre de su equipo", async () => await _consultas.Consulta11HeroesConEquipoAsync());
        await Correr("12. Resumen por equipo (cantidad y promedio)", async () => await _consultas.Consulta12ResumenPorEquipoAsync());
        await Correr("13. Cada misión con sus héroes y roles", async () => await _consultas.Consulta13MisionesConHeroesAsync());
        await Correr("14. Héroes sin misiones", async () => await _consultas.Consulta14SinMisionesAsync());
        await Correr($"15. Ficha del héroe Aurora (Id {idAurora})", async () => await _consultas.Consulta15FichaAsync(idAurora));
        await Correr("15. (Id inexistente 9999) debe devolver null", async () => await _consultas.Consulta15FichaAsync(9999));
        await Correr("16. Héroes en misiones Alta o Extrema", async () => await _consultas.Consulta16MisionesAltaOExtremaAsync());
        await Correr("17. Héroes con 2 o más misiones", async () => await _consultas.Consulta17DosOMasMisionesAsync());

        // Nivel 4
        await Correr("18. Misiones con fecha límite, de la más próxima", async () => await _consultas.Consulta18MisionesConFechaAsync());
        await Correr("19. Página 1, tamaño 3", async () => await _consultas.Consulta19PaginaAsync(1, 3));
        await Correr("19. Página 3, tamaño 3 (última)", async () => await _consultas.Consulta19PaginaAsync(3, 3));
        await Correr("19. Página 0, tamaño 100 (debe ajustarse a 1 y 50)", async () => await _consultas.Consulta19PaginaAsync(0, 100));
        await Correr("20. Top 3 héroes por poder con su cantidad de misiones", async () => await _consultas.Consulta20Top3Async());
    }

    // Muestra el resultado de una consulta. Las que aún lanzan NotImplementedException salen como "(pendiente)".
    private static async Task Correr(string titulo, Func<Task<object?>> consulta)
    {
        Console.WriteLine($"===== Consulta {titulo} =====");
        try
        {
            var resultado = await consulta();
            Console.WriteLine(JsonSerializer.Serialize(resultado, OpcionesJson));
        }
        catch (NotImplementedException)
        {
            Console.WriteLine("(pendiente)");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
        Console.WriteLine();
    }

    private async Task ProbarLinqAsync()
    {
        //var consulta = _context.Heroes
        //    .Where(h => h.NivelPoder > 50 )
        //    .OrderByDescending(h => h.NivelPoder)
        //    .Select(h => new
        //    {
        //        h.Alias,
        //        h.NivelPoder,
        //        Equipo = h.Equipo != null ? h.Equipo.Nombre : "Sin equipo",
        //        Misiones = h.HeroeMisiones.Count
        //    });

        // Otros ejemplos para probar:
        //var consulta = _context.Equipos
        //   .Select(e => new { e.Nombre, Heroes = e.Heroes.Count, PoderPromedio = e.Heroes.Average(h => (double?)h.NivelPoder) });
        //var consulta = _context.Misiones
        //    .Where(m => m.Dificultad == Dificultad.Alta || m.Dificultad == Dificultad.Extrema)
        //    .Select(m => new { m.Titulo, m.Dificultad });
         //var consulta = _context.HeroeMisiones
         //   .Where(hm => hm.Rol == "Líder")
         //   .Select(hm => new { hm.Heroe.Alias, hm.Mision.Titulo });
         var consulta = _context.Heroes
            .GroupBy(h => h.Equipo.Nombre)
            .Select(g => new { EquipoId = g.Key, Cantidad = g.Count(), PoderMaximo = g.Max(h => h.NivelPoder) });
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("========== PRUEBA LINQ ==========");
        Console.WriteLine("SQL generado:");
        Console.WriteLine(consulta.ToQueryString());
        Console.WriteLine("---------- Resultado ----------");

        var resultado = await consulta.ToListAsync();

        foreach (var item in resultado)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine($"Total: {resultado.Count}");
        Console.WriteLine("=================================");
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var heroe = await _context.Heroes.AsNoTracking()
            .Include(h => h.Equipo)
            .Include(h => h.HeroeMisiones).ThenInclude(hm => hm.Mision)
            .FirstOrDefaultAsync(h => h.Id == id);
        if (heroe is null)
        {
            return NotFound();
        }

        return View(heroe);
    }
}
