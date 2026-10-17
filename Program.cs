using HeroesFluent.Data;
using HeroesFluent.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HeroesDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("HeroesFluent")));

builder.Services.AddScoped<HeroeConsultas>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<HeroesDbContext>();
if (context.Heroes.Count() <= 34)
{
    await HeroeSeeder.InicializarAsync(context, 20);
}

if (context.Equipos.Count() < 15)
{
    await EquipoSeeder.InicializarAsync(context, 20);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
