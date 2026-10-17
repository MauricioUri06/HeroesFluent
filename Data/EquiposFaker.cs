using Bogus;
using HeroesFluent.Models;
namespace HeroesFluent.Data;

public class EquiposFaker : Faker<Equipo>
{
    public EquiposFaker() : base("es")
    {
        RuleFor(e => e.Nombre, f =>  $" Heroe -{f.Random.Guid():N}");
        RuleFor(e => e.FechaFundacion, f => f.Date.Past(5, DateTime.UtcNow));
        RuleFor(e => e.Presupuesto, f => f.Random.Decimal(10000, 1000000000));
    }
}