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

    // ───────────── Nivel 3 — Relaciones y agrupación ─────────────

    // 11.
    public async Task<object?> Consulta11HeroesConEquipoAsync()
    {
        // TODO: cada héroe con el nombre de su equipo ("Sin equipo" si no tiene)
        throw new NotImplementedException();
    }

    // 12.
    public async Task<object?> Consulta12ResumenPorEquipoAsync()
    {
        // TODO: por equipo: nombre, cantidad de héroes y nivel promedio; del promedio más alto al más bajo
        throw new NotImplementedException();
    }

    // 13.
    public async Task<object?> Consulta13MisionesConHeroesAsync()
    {
        // TODO: cada misión con título, dificultad (texto) y héroes asignados con su rol, ordenados por alias
        throw new NotImplementedException();
    }

    // 14.
    public async Task<object?> Consulta14SinMisionesAsync()
    {
        // TODO: héroes sin ninguna misión asignada
        throw new NotImplementedException();
    }

    // 15.
    public async Task<object?> Consulta15FichaAsync(int heroeId)
    {
        // TODO: ficha (alias, nivel, equipo, misiones con rol) con Include y ThenInclude; null si no existe
        throw new NotImplementedException();
    }

    // 16.
    public async Task<object?> Consulta16MisionesAltaOExtremaAsync()
    {
        // TODO: héroes en al menos una misión Alta o Extrema, sin repetir, ordenados por alias
        throw new NotImplementedException();
    }

    // 17.
    public async Task<object?> Consulta17DosOMasMisionesAsync()
    {
        // TODO: alias y cantidad de misiones de los héroes con 2 o más misiones, ordenados por alias
        throw new NotImplementedException();
    }

    // ───────────── Nivel 4 — Fechas, paginación y ranking ─────────────

    // 18.
    public async Task<object?> Consulta18MisionesConFechaAsync()
    {
        // TODO: misiones con fecha límite, de la más próxima a la más lejana (título y fecha)
        throw new NotImplementedException();
    }

    // 19.
    public async Task<object?> Consulta19PaginaAsync(int pagina, int tamano)
    {
        // TODO: página de héroes ordenados por alias + total de páginas (página mínima 1, tamaño de 1 a 50)
        throw new NotImplementedException();
    }

    // 20.
    public async Task<object?> Consulta20Top3Async()
    {
        // TODO: los 3 héroes con mayor poder, cada uno con su cantidad de misiones
        throw new NotImplementedException();
    }
}
