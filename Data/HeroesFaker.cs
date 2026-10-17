using Bogus;
using HeroesFluent.Models;
namespace HeroesFluent.Data;

public class HeroesFaker : Faker<Heroe>
{
    public HeroesFaker(IReadOnlyList<int> listaEquiposId) : base("es")
    {
        RuleFor(h => h.Nombre, f => f.Name.FullName());
        RuleFor(h => h.Alias, f => $" Heroe -{f.Random.Guid():N}");
        RuleFor(h => h.NivelPoder, f => f.Random.Int(1, 100));
        RuleFor(h => h.FechaRegistro, f => f.Date.Past(2, DateTime.UtcNow));
        RuleFor(h => h.EquipoId, f => f.PickRandom<int>(listaEquiposId));
        RuleFor(h => h.Ciudad, f => f.Address.City());
    }
}