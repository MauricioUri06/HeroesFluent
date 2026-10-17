using HeroesFluent.Data;
using HeroesFluent.Models;
using Microsoft.EntityFrameworkCore;

namespace HeroesFluent.Services;
public class HeroeConsultas
{
    private readonly HeroesDbContext _context;

    public HeroeConsultas(HeroesDbContext context)
    {
        _context = context;
    }
    

    // 1.
    public async Task<object?> Consulta01AliasYNivelAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Select(h => new {h.Alias, h.NivelPoder})
            .OrderBy(h => h.Alias)
            .ToListAsync();
    }

    // 2.
    public async Task<object?> Consulta02PoderMayorA70Async()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Select(h => new {h.Alias, h.NivelPoder})
            .Where(h => h.NivelPoder > 70)
            .OrderByDescending(h => h.NivelPoder)
            .ToListAsync();
    }

    // 3.
    public async Task<object?> Consulta03SinEquipoAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.Equipo == null)
            .OrderBy(h => h.Alias)
            .Select(h => h.Alias)
            .ToListAsync();

    }

    // 4.
    public async Task<object?> Consulta04BuscarPorAliasAsync(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return new List<object>();
        }
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.Alias.Contains(texto))
            .OrderBy(h => h.Alias)
            .Select(h => h.Alias)
            .ToListAsync();
        
    }

    // 5.
    public async Task<object?> Consulta05ObtenerPorAliasAsync(string alias)
    {
        return await _context.Heroes
            .AsNoTracking()
            .FirstOrDefaultAsync(h => h.Alias.Equals(alias));
    }

    // 6.
    public async Task<object?> Consulta06PorCiudadAsync(string ciudad)
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.Ciudad ==  ciudad)
            .OrderBy(h => h.Alias)
            .Select(h => h.Alias)
            .ToListAsync();
    }
    

    // 7.
    public async Task<object?> Consulta07TotalYPromedioAsync()
    {
        var total =  await _context.Heroes.CountAsync();
        var promedio = await _context.Heroes.AverageAsync(h =>  (double?)h.NivelPoder) ?? 0;
        
        return new {Total = total, Promedio = promedio};
    }

    // 8.
    public async Task<object?> Consulta08ExisteMisionExtremaAsync()
    {
        bool existe = await _context.Misiones.AnyAsync(m => m.Dificultad == Dificultad.Extrema);

        return existe;  
    }

    // 9.
    public async Task<object?> Consulta09PresupuestoTotalAsync()
    {
        var total = await _context.Equipos.SumAsync(e => e.Presupuesto);

        return total;
    }

    // 10.
    public async Task<object?> Consulta10BajoElPromedioAsync()
    {
        var promedio = await _context.Heroes.AverageAsync(h => h.NivelPoder);
        
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.NivelPoder < promedio)
            .OrderBy(h => h.NivelPoder)
            .Select(h => new {h.Alias, h.NivelPoder})
            .ToListAsync();
    }
    

    // 11.
    public async Task<object?> Consulta11HeroesConEquipoAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Select(h => new {h.Alias, Equipo = h.Equipo != null ? h.Equipo.Nombre : "Sin Equipo"})
            .ToListAsync();
    }

    // 12.
    public async Task<object?> Consulta12ResumenPorEquipoAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.EquipoId != null)
            .GroupBy(h => h.Equipo!.Nombre)
            .Select(g => new
            {
                Nombre = g.Key,
                Cantidad = g.Count(),
                Promedio = g.Average(g => g.NivelPoder)
            })
            .OrderByDescending(h => h.Promedio)
            .ToListAsync();
    }

    // 13.
    public async Task<object?> Consulta13MisionesConHeroesAsync()
    {
        return await _context.Misiones
            .AsNoTracking()
            .OrderBy(m => m.Id)
            .Select(m => new
            {
                m.Titulo,
                Dificultad = m.Dificultad.ToString(),
                Heroes = m.HeroeMisiones
                    .OrderBy(hm => hm.Heroe.Alias)
                    .Select(hm => new { hm.Heroe.Alias, hm.Rol })
                    .ToList()
            })
            .ToListAsync();
    }

    // 14.
    public async Task<object?> Consulta14SinMisionesAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => !h.HeroeMisiones.Any())
            .OrderBy(h => h.Alias)
            .Select(h => new { h.Alias, h.NivelPoder })
            .ToListAsync();
    }

    // 15.
    public async Task<object?> Consulta15FichaAsync(int heroeId)
    {
        var heroe = await _context.Heroes
            .AsNoTracking()
            .Include(h => h.Equipo)
            .Include(h => h.HeroeMisiones).ThenInclude(hm => hm.Mision)
            .FirstOrDefaultAsync(h => h.Id == heroeId);

        if (heroe is null)
        {
            return null;
        }

        return new
        {
            heroe.Alias,
            Nivel = heroe.NivelPoder,
            Equipo = heroe.Equipo != null ? heroe.Equipo.Nombre : "Sin equipo",
            Misiones = heroe.HeroeMisiones
                .Select(hm => new { hm.Mision.Titulo, hm.Rol })
                .ToList()
        };
    }

    // 16.
    public async Task<object?> Consulta16MisionesAltaOExtremaAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.HeroeMisiones.Any(hm =>
                hm.Mision.Dificultad == Dificultad.Alta ||
                hm.Mision.Dificultad == Dificultad.Extrema))
            .OrderBy(h => h.Alias)
            .Select(h => new { h.Alias, h.NivelPoder })
            .ToListAsync();
    }

    // 17.
    public async Task<object?> Consulta17DosOMasMisionesAsync()
    {
        return await _context.Heroes
            .AsNoTracking()
            .Where(h => h.HeroeMisiones.Count >= 2)
            .OrderBy(h => h.Alias)
            .Select(h => new { h.Alias, CantidadMisiones = h.HeroeMisiones.Count })
            .ToListAsync();
    }

    // ───────────── Nivel 4 — Fechas, paginación y ranking ─────────────

    // 18.
    public async Task<object?> Consulta18MisionesConFechaAsync()
    {
        return await _context.Misiones
            .AsNoTracking()
            .Where(m => m.FechaLimite != null)
            .OrderBy(m => m.FechaLimite)
            .Select(m => new { m.Titulo, m.FechaLimite })
            .ToListAsync();
    }

    // 19.
    public async Task<object?> Consulta19PaginaAsync(int pagina, int tamano)
    {
        pagina = Math.Max(1, pagina);
        tamano = Math.Clamp(tamano, 1, 50);

        var totalHeroes = await _context.Heroes.CountAsync();
        var totalPaginas = (int)Math.Ceiling(totalHeroes / (double)tamano);

        var heroes = await _context.Heroes
            .AsNoTracking()
            .OrderBy(h => h.Alias)
            .Skip((pagina - 1) * tamano)
            .Take(tamano)
            .Select(h => new { h.Alias, h.NivelPoder })
            .ToListAsync();

        return new
        {
            Pagina = pagina,
            Tamano = tamano,
            TotalPaginas = totalPaginas,
            Heroes = heroes
        };
    }

    // 20.
    public async Task<object?> Consulta20Top3Async()
    {
        return await _context.Heroes
            .AsNoTracking()
            .OrderByDescending(h => h.NivelPoder)
            .Take(3)
            .Select(h => new { h.Alias, h.NivelPoder, CantidadMisiones = h.HeroeMisiones.Count })
            .ToListAsync();
    }
}
